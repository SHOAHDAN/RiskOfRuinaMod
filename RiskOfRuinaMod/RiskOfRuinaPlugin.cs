using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using AncientScepter;
using BepInEx;
using BepInEx.Bootstrap;
using HG;
using NS_KingKombatArena;
using On.RoR2;
using On.RoR2.CharacterSpeech;
using On.RoR2.Networking;
using On.RoR2.UI;
using R2API;
using R2API.Utils;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Items;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using RoR2.CharacterSpeech;
using RoR2.ContentManagement;
using RoR2.Networking;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace RiskOfRuinaMod
{

	[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
	[R2APISubmoduleDependency(new string[] { "PrefabAPI", "LanguageAPI", "SoundAPI", "ItemAPI", "ItemDropAPI", "ResourcesAPI" })]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
	[BepInPlugin("com.Scoops.RiskOfRuina", "RiskOfRuina", "1.0.6")]
	public class RiskOfRuinaPlugin : BaseUnityPlugin
	{
		public const string MODUID = "com.Scoops.RiskOfRuina";

		public const string MODNAME = "RiskOfRuina";

		public const string MODVERSION = "1.0.6";

		public const string developerPrefix = "COF";

		public static bool DEBUG_MODE = false;

		public static GameObject CentralNetworkObject;

		public static GameObject _centralNetworkObjectSpawned;

		internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

		public static GameObject characterPrefab;

		public static RiskOfRuinaPlugin instance;

		public static bool ancientScepterInstalled = false;

		public static bool kombatArenaInstalled = false;

		public static uint argaliaSkinIndex = 1u;

		public bool IsRedMistSelected = false;

		public bool IsArbiterSelected = false;

		public bool IsBlackSilenceSelected = false;

		public bool IsModCharSelected = false;

		public string CurrentCharacterNameSelected = "";

		public bool songOverride = false;

		public uint currentOverrideSong;

		private DoTCore dotCore;

		private ItemManager itemManager;

		private void Awake()
		{
			instance = this;
			RiskOfRuinaMod.Modules.Config.ReadConfig();
			RiskOfRuinaMod.Modules.Assets.Initialize();
			CameraParams.InitializeParams();
			States.RegisterStates();
			Buffs.RegisterBuffs();
			dotCore = new DoTCore();
			Projectiles.RegisterProjectiles();
			Tokens.AddTokens();
			Music.Initialize();
			itemManager = new ItemManager();
			itemManager.items.Add(new ArbitersTrophy());
			itemManager.items.Add(new BlackTea());
			itemManager.items.Add(new Prescript());
			itemManager.items.Add(new LiuBadge());
			itemManager.items.Add(new WorkshopAmmo());
			itemManager.items.Add(new WeddingRing());
			itemManager.items.Add(new UdjatMask());
			itemManager.items.Add(new Reverberation());
			ItemManager.instance.AddItems();
			itemManager.equips.Add(new BackwardsClock());
			ItemManager.instance.AddEquips();
			ItemDisplays.PopulateDisplays();
			new RedMist().Initialize();
			new AnArbiter().Initialize();
			if (Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
			{
				ancientScepterInstalled = true;
				Skills.ScepterSkillSetup("COF");
				ScepterSetup();
			}
			if (Chainloader.PluginInfos.ContainsKey("com.Kingpinush.KingKombatArena"))
			{
				kombatArenaInstalled = true;
			}
			GameObject gameObject = new GameObject("tmpGo");
			gameObject.AddComponent<NetworkIdentity>();
			CentralNetworkObject = gameObject.InstantiateClone("riskOfRuinaNetworkManager");
			UnityEngine.Object.Destroy(gameObject);
			CentralNetworkObject.AddComponent<RiskOfRuinaNetworkManager>();
			new ContentPacks().Initialize();
			ContentManager.add_onContentPacksAssigned((Action<ReadOnlyArray<ReadOnlyContentPack>>)LateSetup);
			Hook();
		}

		private void LateSetup(ReadOnlyArray<ReadOnlyContentPack> obj)
		{
			SurvivorBase.instance.SetItemDisplays();
			CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private void ScepterSetup()
		{
			ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(Skills.ScepterBasicAttack, "RedMistBody", (SkillSlot)0, 0);
			ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(Skills.scepterShockwaveDef, "ArbiterBody", (SkillSlot)3, 0);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool KombatGamemodeActive()
		{
			return KingKombatArenaMainPlugin.s_GAME_MODE_ACTIVE;
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool KombatIsDueling(CharacterMaster masterToCheck)
		{
			return NetworkedGameModeManager.instance.m_currentNetworkedGameModeData.IsCharacterDueling(masterToCheck);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool KombatIsCharacter(CharacterMaster masterToCheck)
		{
			return KingKombatArenaMainPlugin.AccessCurrentKombatArenaInstance().GetAllCurrentCharacterMasters().Contains(masterToCheck);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool KombatIsWaiting(GameObject bodyToCheck)
		{
			return KingKombatArenaMainPlugin.IsInWaitingArea(bodyToCheck);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static int KombatDuelsPlayed(CharacterMaster masterToCheck)
		{
			return NetworkedGameModeManager.instance.m_currentNetworkedGameModeData.AccessParticipantData(masterToCheck).GetDuelTotal();
		}

		private void Hook()
		{
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			if (RiskOfRuinaMod.Modules.Config.snapLevel.Value)
			{
				CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
			}
			GlobalEventManager.OnHitEnemy += GlobalEvent_OnHitEnemy;
			GlobalEventManager.OnCharacterDeath += GlobalEvent_OnCharacterDeath;
			BrotherSpeechDriver.DoInitialSightResponse += BrotherSpeechDriver_DoInitialSightResponse;
			BrotherSpeechDriver.OnBodyKill += BrotherSpeechDriver_OnBodyKill;
			GameNetworkManager.OnClientSceneChanged += GameNetworkManager_OnClientSceneChanged_Hook;
			CharacterSelectController.SelectSurvivor += OnSurvivorSelected_Hook;
			CharacterBody.OnSkillActivated += CharacterBody_OnSkillActivated;
			DotController.add_onDotInflictedServerGlobal(new OnDotInflictedServerGlobalDelegate(DotController_InflictDot));
			Run.Start += Run_Start;
		}

		private void Run_Start(Run.orig_Start orig, Run self)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (RoR2Application.get_isInSinglePlayer())
			{
				ItemDropAPI.RemovePickup(PickupCatalog.FindPickupIndex("RuinaWeddingRing"), false);
			}
			orig(self);
		}

		private void CharacterBody_OnSkillActivated(CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Expected O, but got Unknown
			orig(self, skill);
			if (!(UnityEngine.Object)(object)self || !NetworkServer.get_active())
			{
				return;
			}
			DotController val = DotController.FindDotController(((Component)(object)self).gameObject);
			if (!(UnityEngine.Object)(object)val || !val.HasDotActive(DoTCore.FairyIndex))
			{
				return;
			}
			int buffCount = self.GetBuffCount(Buffs.fairyDebuff);
			int num = buffCount / 3;
			int num2 = 0;
			for (int i = 0; i < val.dotStackList.Count; i++)
			{
				DotStack val2 = val.dotStackList[i];
				if (val2.dotIndex == DoTCore.FairyIndex)
				{
					DamageInfo val3 = ((!val2.attackerObject || !(UnityEngine.Object)(object)val2.attackerObject.GetComponent<CharacterBody>()) ? new DamageInfo
					{
						attacker = val2.attackerObject,
						inflictor = val2.attackerObject,
						crit = false,
						damage = 1f,
						position = self.get_corePosition(),
						force = Vector3.zero,
						damageType = (DamageType)0,
						damageColorIndex = (DamageColorIndex)2,
						dotIndex = DoTCore.FairyIndex,
						procCoefficient = 0.75f
					} : new DamageInfo
					{
						attacker = val2.attackerObject,
						inflictor = val2.attackerObject,
						crit = val2.attackerObject.GetComponent<CharacterBody>().RollCrit(),
						damage = val2.attackerObject.GetComponent<CharacterBody>().get_damage() * 1f,
						position = self.get_corePosition(),
						force = Vector3.zero,
						damageType = (DamageType)0,
						damageColorIndex = (DamageColorIndex)2,
						dotIndex = DoTCore.FairyIndex,
						procCoefficient = 0.75f
					});
					self.get_healthComponent().TakeDamage(val3);
					GlobalEventManager.instance.OnHitEnemy(val3, ((Component)(object)self.get_healthComponent().body).gameObject);
					GlobalEventManager.instance.OnHitAll(val3, ((Component)(object)self.get_healthComponent().body).gameObject);
				}
			}
			EffectData val4 = new EffectData();
			val4.rotation = Util.QuaternionSafeLookRotation(Vector3.zero);
			val4.set_origin(self.get_corePosition());
			EffectManager.SpawnEffect(RiskOfRuinaMod.Modules.Assets.fairyProcEffect, val4, false);
		}

		private void OnSurvivorSelected_Hook(CharacterSelectController.orig_SelectSurvivor orig, CharacterSelectController self, SurvivorIndex survivor)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			orig(self, survivor);
			SurvivorDef survivorDef = SurvivorCatalog.GetSurvivorDef(survivor);
			if ((UnityEngine.Object)(object)survivorDef != null)
			{
				IsRedMistSelected = survivorDef.get_cachedName() == "RedMist";
				IsArbiterSelected = survivorDef.get_cachedName() == "Arbiter";
				IsBlackSilenceSelected = survivorDef.get_cachedName() == "BlackSilence";
				IsModCharSelected = IsArbiterSelected || IsRedMistSelected || IsBlackSilenceSelected;
			}
			CurrentCharacterNameSelected = survivorDef.get_cachedName();
		}

		private void GameNetworkManager_OnClientSceneChanged_Hook(GameNetworkManager.orig_OnClientSceneChanged orig, GameNetworkManager self, NetworkConnection conn)
		{
			orig(self, conn);
			if (SceneManager.GetActiveScene().name.Contains("outro") && IsModCharSelected)
			{
				songOverride = true;
				currentOverrideSong = Util.PlaySound("Play_Dark_Fantasy_Studio___Sun_and_Moon", base.gameObject);
				Music.musicSources++;
			}
			else if (songOverride)
			{
				songOverride = false;
				AkSoundEngine.StopPlayingID(currentOverrideSong);
				Music.musicSources--;
			}
		}

		private void BrotherSpeechDriver_DoInitialSightResponse(BrotherSpeechDriver.orig_DoInitialSightResponse orig, BrotherSpeechDriver self)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			ReadOnlyCollection<CharacterBody> readOnlyInstancesList = CharacterBody.readOnlyInstancesList;
			for (int i = 0; i < readOnlyInstancesList.Count; i++)
			{
				BodyIndex bodyIndex = readOnlyInstancesList[i].bodyIndex;
				flag |= bodyIndex == BodyCatalog.FindBodyIndex(RedMist.redMistPrefab);
			}
			if (flag)
			{
				SpeechInfo[] array = (SpeechInfo[])(object)new SpeechInfo[1]
				{
				new SpeechInfo
				{
					duration = 1f,
					maxWait = 4f,
					mustPlay = true,
					priority = 0f,
					token = "BROTHER_SEE_REDMIST_1"
				}
				};
				self.SendReponseFromPool(array);
			}
			orig(self);
		}

		private void BrotherSpeechDriver_OnBodyKill(BrotherSpeechDriver.orig_OnBodyKill orig, BrotherSpeechDriver self, DamageReport damageReport)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if ((bool)(UnityEngine.Object)(object)damageReport.victimBody && damageReport.victimBodyIndex == BodyCatalog.FindBodyIndex(RedMist.redMistPrefab))
			{
				SpeechInfo[] array = (SpeechInfo[])(object)new SpeechInfo[1]
				{
				new SpeechInfo
				{
					duration = 1f,
					maxWait = 4f,
					mustPlay = true,
					priority = 0f,
					token = "BROTHER_KILL_REDMIST_1"
				}
				};
				self.SendReponseFromPool(array);
			}
			orig(self, damageReport);
		}

		private void CharacterBody_OnLevelUp(CharacterBody.orig_OnLevelUp orig, CharacterBody self)
		{
			orig(self);
			if ((bool)(UnityEngine.Object)(object)self && IsModCharSelected && self.get_isPlayerControlled())
			{
				Util.PlaySound("Ruina_Snap", ((Component)(object)self).gameObject);
			}
		}

		private void CharacterBody_RecalculateStats(CharacterBody.orig_RecalculateStats orig, CharacterBody self)
		{
			orig(self);
			if (!(UnityEngine.Object)(object)self)
			{
				return;
			}
			if (self.HasBuff(Buffs.feebleDebuff))
			{
				self.set_armor(self.get_armor() * 0.5f);
				self.set_damage(self.get_damage() * 0.5f);
			}
			if (self.HasBuff(Buffs.warpBuff))
			{
				self.set_moveSpeed(self.get_moveSpeed() * 2f);
				self.set_attackSpeed(self.get_attackSpeed() * 2f);
			}
			if (self.HasBuff(Buffs.strengthBuff))
			{
				if (self.get_isPlayerControlled())
				{
					self.set_moveSpeed(self.get_moveSpeed() * 1.5f);
					self.set_attackSpeed(self.get_attackSpeed() * 1.5f);
					self.set_regen(self.get_regen() * 2f);
					self.set_armor(self.get_armor() + 50f);
					self.set_damage(self.get_damage() * 1.5f);
				}
				else
				{
					self.set_moveSpeed(self.get_moveSpeed() * 1.5f);
					self.set_attackSpeed(self.get_attackSpeed() * 5f);
					self.set_regen(self.get_regen() * 5f);
					self.set_armor(self.get_armor() + 100f);
					self.set_damage(self.get_damage() * 5f);
				}
				if ((bool)(UnityEngine.Object)(object)self.get_skillLocator())
				{
					if ((bool)(UnityEngine.Object)(object)self.get_skillLocator().primary)
					{
						GenericSkill primary = self.get_skillLocator().primary;
						primary.set_cooldownScale(primary.get_cooldownScale() - 0.25f);
					}
					if ((bool)(UnityEngine.Object)(object)self.get_skillLocator().secondary)
					{
						GenericSkill secondary = self.get_skillLocator().secondary;
						secondary.set_cooldownScale(secondary.get_cooldownScale() - 0.25f);
					}
					if ((bool)(UnityEngine.Object)(object)self.get_skillLocator().utility)
					{
						GenericSkill utility = self.get_skillLocator().utility;
						utility.set_cooldownScale(utility.get_cooldownScale() - 0.25f);
					}
					if ((bool)(UnityEngine.Object)(object)self.get_skillLocator().special)
					{
						GenericSkill special = self.get_skillLocator().special;
						special.set_cooldownScale(special.get_cooldownScale() - 0.25f);
					}
				}
			}
			RedMistStatTracker component = ((Component)(object)self).GetComponent<RedMistStatTracker>();
			RedMistEmotionComponent component2 = ((Component)(object)self).GetComponent<RedMistEmotionComponent>();
			if (!(UnityEngine.Object)(object)component || !(UnityEngine.Object)(object)component2)
			{
				return;
			}
			float moveSpeed = self.get_moveSpeed();
			float attackSpeed = self.get_attackSpeed();
			self.set_moveSpeed(self.baseMoveSpeed);
			self.set_attackSpeed(self.baseAttackSpeed);
			float num = (moveSpeed - self.baseMoveSpeed) / self.baseMoveSpeed;
			if (self.get_isSprinting())
			{
				num = (moveSpeed - self.baseMoveSpeed * self.sprintingSpeedMultiplier) / (self.baseMoveSpeed * self.sprintingSpeedMultiplier);
			}
			float num2 = (attackSpeed - self.baseAttackSpeed) / self.baseAttackSpeed;
			num *= RiskOfRuinaMod.Modules.Config.moveSpeedMult.Value;
			num2 *= RiskOfRuinaMod.Modules.Config.attackSpeedMult.Value;
			float num3 = self.baseDamage + self.levelDamage * (self.get_level() - 1f);
			float num4 = RiskOfRuinaMod.Modules.Config.redMistBuffDamage.Value * (float)self.GetBuffCount(Buffs.RedMistBuff) * num3;
			self.set_damage(self.get_damage() + num4);
			float num5 = num * num3 + num2 * num3;
			self.set_damage(self.get_damage() + num5);
			float num6 = 0f;
			float num7 = 0f;
			if ((bool)(UnityEngine.Object)(object)self.get_inventory() && self.get_inventory().GetItemCount(Items.SprintBonus) > 0)
			{
				num6 += RiskOfRuinaMod.Modules.Config.sprintSpeedMult.Value * (float)self.get_inventory().GetItemCount(Items.SprintBonus);
			}
			if ((bool)(UnityEngine.Object)(object)self.get_inventory() && self.get_inventory().GetItemCount(Items.SprintOutOfCombat) > 0)
			{
				num7 += RiskOfRuinaMod.Modules.Config.sprintSpeedMult.Value * 2f * (float)self.get_inventory().GetItemCount(Items.SprintOutOfCombat);
			}
			if (self.HasBuff(Buffs.EGOBuff))
			{
				if (!component2.inEGO && NetworkServer.get_active())
				{
					self.RemoveBuff(Buffs.EGOBuff);
				}
				self.set_armor(self.get_armor() + 50f);
				self.sprintingSpeedMultiplier = 2.2f;
			}
			else
			{
				self.sprintingSpeedMultiplier = 1.5f;
			}
			self.sprintingSpeedMultiplier += num6;
			if (self.get_outOfCombat())
			{
				self.sprintingSpeedMultiplier += num7;
			}
			if (self.get_isSprinting())
			{
				self.set_moveSpeed(self.get_moveSpeed() * self.sprintingSpeedMultiplier);
			}
		}

		private void GlobalEvent_OnHitEnemy(GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			if (NetworkServer.get_active())
			{
				RiskOfRuinaNetworkManager.OnHit(self, damageInfo, victim);
			}
			GameObject attacker = damageInfo.attacker;
			if ((bool)(UnityEngine.Object)(object)self && (bool)attacker)
			{
				CharacterBody component = attacker.GetComponent<CharacterBody>();
				CharacterBody component2 = victim.GetComponent<CharacterBody>();
				if (component.get_teamComponent().get_teamIndex() != component2.get_teamComponent().get_teamIndex())
				{
					if (component.baseNameToken == "COF_REDMIST_BODY_NAME")
					{
						RedMistStatTracker component3 = ((Component)(object)component).GetComponent<RedMistStatTracker>();
						RedMistEmotionComponent component4 = ((Component)(object)component).GetComponent<RedMistEmotionComponent>();
						if ((bool)(UnityEngine.Object)(object)component3 && (bool)(UnityEngine.Object)(object)component4)
						{
							float damage = component.get_damage();
							float num = Mathf.Clamp(damageInfo.damage / damage, 0f, 4f);
							float num2 = (float)Run.get_instance().stageClearCount / ((float)Run.get_instance().stageClearCount + 1f);
							if (kombatArenaInstalled && KombatGamemodeActive() && (bool)(UnityEngine.Object)(object)component.get_master())
							{
								num2 = (float)KombatDuelsPlayed(component.get_master()) / ((float)KombatDuelsPlayed(component.get_master()) + 1f);
							}
							float num3 = num * RiskOfRuinaMod.Modules.Config.emotionRatio.Value;
							float num4 = 1f;
							if (component2.get_isElite())
							{
								num4 = 1.2f;
							}
							if (component2.get_isBoss())
							{
								num4 = 1.4f;
							}
							if (kombatArenaInstalled && KombatGamemodeActive())
							{
								num4 = ((!(UnityEngine.Object)(object)component2.get_master() || !KombatIsCharacter(component2.get_master())) ? 0.75f : 7.5f);
							}
							component4.AddEmotion((num3 + num3 * num2) * num4);
						}
					}
					if (component.baseNameToken == "COF_ARBITER_BODY_NAME" && damageInfo.dotIndex != DoTCore.FairyIndex && Util.CheckRoll(100f * damageInfo.procCoefficient, component.get_master()))
					{
						InflictDotInfo val = default(InflictDotInfo);
						val.attackerObject = damageInfo.attacker;
						val.victimObject = victim;
						val.dotIndex = DoTCore.FairyIndex;
						val.duration = 10f;
						val.damageMultiplier = 0f;
						InflictDotInfo val2 = val;
						DotController.InflictDot(ref val2);
					}
				}
			}
			orig(self, damageInfo, victim);
		}

		private void GlobalEvent_OnCharacterDeath(GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			if ((bool)(UnityEngine.Object)(object)damageReport?.attackerBody && damageReport.attackerBody.baseNameToken == "COF_REDMIST_BODY_NAME")
			{
				RedMistEmotionComponent component = ((Component)(object)damageReport.attackerBody).GetComponent<RedMistEmotionComponent>();
				if ((bool)(UnityEngine.Object)(object)component && component.inEGO && NetworkServer.get_active())
				{
					damageReport.attackerBody.AddBuff(Buffs.RedMistBuff);
				}
				if (damageReport.combinedHealthBeforeDamage - damageReport.damageDealt <= 0f - damageReport.victim.get_fullHealth())
				{
					EffectData val = new EffectData();
					val.set_origin(damageReport.victimBody.get_corePosition());
					val.scale = 1f;
					EffectManager.SpawnEffect(RiskOfRuinaMod.Modules.Assets.mistEffect, val, true);
					if ((bool)(UnityEngine.Object)(object)damageReport.victimBody.get_modelLocator() && (bool)(UnityEngine.Object)(object)damageReport.victimBody.get_modelLocator().get_modelTransform().GetComponent<CharacterModel>() && NetworkServer.get_active())
					{
						RiskOfRuinaNetworkManager.SetInvisible(((Component)(object)damageReport.victimBody).gameObject);
					}
				}
			}
			if (damageReport.victimBody.baseNameToken == "COF_REDMIST_BODY_NAME" || damageReport.victimBody.baseNameToken == "COF_ARBITER_BODY_NAME" || damageReport.victimBody.baseNameToken == "COF_BLACKSILENCE_BODY_NAME")
			{
				EffectData val2 = new EffectData();
				val2.set_origin(damageReport.victimBody.get_corePosition());
				val2.scale = 1f;
				EffectManager.SpawnEffect(RiskOfRuinaMod.Modules.Assets.pagePoof, val2, true);
				if (NetworkServer.get_active())
				{
					RiskOfRuinaNetworkManager.SetInvisible(((Component)(object)damageReport.victimBody).gameObject);
				}
			}
			orig(self, damageReport);
		}

		private void DotController_InflictDot(DotController self, ref InflictDotInfo dotInfo)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (dotInfo.dotIndex != DoTCore.FairyIndex)
			{
				return;
			}
			int i = 0;
			for (int count = self.dotStackList.Count; i < count; i++)
			{
				if (self.dotStackList[i].dotIndex == DoTCore.FairyIndex)
				{
					self.dotStackList[i].timer = Mathf.Max(self.dotStackList[i].timer, dotInfo.duration);
				}
			}
		}
	}
}