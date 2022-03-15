// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.RiskOfRuinaPlugin
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

//Changes to decompiled code:
//(0): The decompiler made the project under .Net Framework. Had to update it to .NET Standard to make certain libraries work.
//(1): Took the boilerplate from the modding discord's first mod guide and slapped all the libraries, etc. into the project files. Seems to have resolved a lot of missing dependencies.
  //(1a) For some reason the boilerplate installs a bunch of packages on your machine under [User]\.nuget . For another, even stranger reason, VS doesn't recognize them by default so you gotta go to Nuget\Package Sources in the settings to point out that this folder exists to VS. If there's a less janky way to get the packages in, I don't know it.


//The decompiler couldn't pull dependencies from the original .dll, so trying to figure out how to make all the "using" lines work is a huge pain in the ass.
//Comments in this part of the code describe the files responsible for each dependency, which are either in RiskofRuinaMod/Modules/Libraries/ or a NuGen package.
using AncientScepter;
using BepInEx; //Apparently VS doesn't think referencing the bepinex .dll from Thunderstore is good enough.
using BepInEx.Bootstrap; //Adding BepInEx as a NG package (from the boilerplate) did it.
using HG; //WTF is HG? (HookGenPatcher?) Don't care it fixed itself lol
using NS_KingKombatArena; //KingKombatArena.dll
using On.RoR2; //MMHOOK_RoR2
using On.RoR2.CharacterSpeech;
using On.RoR2.Networking; //VS thinks this is redundant. I am unsure if this is the case.
using On.RoR2.UI;
using R2API; //R2API.dll
using R2API.Utils;
using RiskOfRuinaMod.Modules; //Self-referential, of course.
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Items;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2; //I am unsure how to make Risk of Rain 2 a reference.
using RoR2.CharacterSpeech;
using RoR2.ContentManagement;
using RoR2.Networking;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine; //Adding UnityEngine as a package seemed to do it.
using UnityEngine.Networking; //Works, now that I have updated to .NET Standard 2.0.
using UnityEngine.SceneManagement; //Apparently does not exist. I am confused.

//We're down from 6000 errors to 3806!!! WE DID IT REDDIT!!!

namespace RiskOfRuinaMod
{
  [BepInDependency]
  [R2APISubmoduleDependency(new string[] {"PrefabAPI", "LanguageAPI", "SoundAPI", "ItemAPI", "ItemDropAPI", "ResourcesAPI"})]
  [NetworkCompatibility]
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
    public static uint argaliaSkinIndex = 1;
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
      RiskOfRuinaPlugin.instance = this;
      Config.ReadConfig();
      Assets.Initialize();
      CameraParams.InitializeParams();
      States.RegisterStates();
      Buffs.RegisterBuffs();
      this.dotCore = new DoTCore();
      Projectiles.RegisterProjectiles();
      Tokens.AddTokens();
      Music.Initialize();
      this.itemManager = new ItemManager();
      this.itemManager.items.Add((RuinaItem) new ArbitersTrophy());
      this.itemManager.items.Add((RuinaItem) new BlackTea());
      this.itemManager.items.Add((RuinaItem) new Prescript());
      this.itemManager.items.Add((RuinaItem) new LiuBadge());
      this.itemManager.items.Add((RuinaItem) new WorkshopAmmo());
      this.itemManager.items.Add((RuinaItem) new WeddingRing());
      this.itemManager.items.Add((RuinaItem) new UdjatMask());
      this.itemManager.items.Add((RuinaItem) new Reverberation());
      ItemManager.instance.AddItems();
      this.itemManager.equips.Add((RuinaEquipment) new BackwardsClock());
      ItemManager.instance.AddEquips();
      ItemDisplays.PopulateDisplays();
      new RedMist().Initialize();
      new AnArbiter().Initialize();
      if (Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
      {
        RiskOfRuinaPlugin.ancientScepterInstalled = true;
        Skills.ScepterSkillSetup("COF");
        this.ScepterSetup();
      }
      if (Chainloader.PluginInfos.ContainsKey("com.Kingpinush.KingKombatArena"))
        RiskOfRuinaPlugin.kombatArenaInstalled = true;
      GameObject gameObject = new GameObject("tmpGo");
      gameObject.AddComponent<NetworkIdentity>();
      RiskOfRuinaPlugin.CentralNetworkObject = PrefabAPI.InstantiateClone(gameObject, "riskOfRuinaNetworkManager");
      Object.Destroy((Object) gameObject);
      RiskOfRuinaPlugin.CentralNetworkObject.AddComponent<RiskOfRuinaNetworkManager>();
      new ContentPacks().Initialize();
      ContentManager.onContentPacksAssigned += new Action<ReadOnlyArray<ReadOnlyContentPack>>(this.LateSetup);
      this.Hook();
    }

    private void LateSetup(ReadOnlyArray<ReadOnlyContentPack> obj)
    {
      SurvivorBase.instance.SetItemDisplays();
      // ISSUE: method pointer
      CharacterBody.RecalculateStats += new CharacterBody.hook_RecalculateStats((object) this, __methodptr(CharacterBody_RecalculateStats));
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void ScepterSetup()
    {
      ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(Skills.ScepterBasicAttack, "RedMistBody", (SkillSlot) 0, 0);
      ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(Skills.scepterShockwaveDef, "ArbiterBody", (SkillSlot) 3, 0);
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool KombatGamemodeActive() => KingKombatArenaMainPlugin.s_GAME_MODE_ACTIVE;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool KombatIsDueling(CharacterMaster masterToCheck) => ((NetworkedGameModeData) ref NetworkedGameModeManager.instance.m_currentNetworkedGameModeData).IsCharacterDueling(masterToCheck);

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool KombatIsCharacter(CharacterMaster masterToCheck) => KingKombatArenaMainPlugin.AccessCurrentKombatArenaInstance().GetAllCurrentCharacterMasters().Contains(masterToCheck);

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool KombatIsWaiting(GameObject bodyToCheck) => KingKombatArenaMainPlugin.IsInWaitingArea(bodyToCheck);

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static int KombatDuelsPlayed(CharacterMaster masterToCheck) => ((ParticipantData) ref ((NetworkedGameModeData) ref NetworkedGameModeManager.instance.m_currentNetworkedGameModeData).AccessParticipantData(masterToCheck)).GetDuelTotal();

    private void Hook()
    {
      if (Config.snapLevel.Value)
      {
        // ISSUE: method pointer
        CharacterBody.OnLevelUp += new CharacterBody.hook_OnLevelUp((object) this, __methodptr(CharacterBody_OnLevelUp));
      }
      // ISSUE: method pointer
      GlobalEventManager.OnHitEnemy += new GlobalEventManager.hook_OnHitEnemy((object) this, __methodptr(GlobalEvent_OnHitEnemy));
      // ISSUE: method pointer
      GlobalEventManager.OnCharacterDeath += new GlobalEventManager.hook_OnCharacterDeath((object) this, __methodptr(GlobalEvent_OnCharacterDeath));
      // ISSUE: method pointer
      BrotherSpeechDriver.DoInitialSightResponse += new BrotherSpeechDriver.hook_DoInitialSightResponse((object) this, __methodptr(BrotherSpeechDriver_DoInitialSightResponse));
      // ISSUE: method pointer
      BrotherSpeechDriver.OnBodyKill += new BrotherSpeechDriver.hook_OnBodyKill((object) this, __methodptr(BrotherSpeechDriver_OnBodyKill));
      // ISSUE: method pointer
      GameNetworkManager.OnClientSceneChanged += new GameNetworkManager.hook_OnClientSceneChanged((object) this, __methodptr(GameNetworkManager_OnClientSceneChanged_Hook));
      // ISSUE: method pointer
      CharacterSelectController.SelectSurvivor += new CharacterSelectController.hook_SelectSurvivor((object) this, __methodptr(OnSurvivorSelected_Hook));
      // ISSUE: method pointer
      CharacterBody.OnSkillActivated += new CharacterBody.hook_OnSkillActivated((object) this, __methodptr(CharacterBody_OnSkillActivated));
      // ISSUE: method pointer
      DotController.onDotInflictedServerGlobal += new DotController.OnDotInflictedServerGlobalDelegate((object) this, __methodptr(DotController_InflictDot));
      // ISSUE: method pointer
      Run.Start += new Run.hook_Start((object) this, __methodptr(Run_Start));
    }

    private void Run_Start(Run.orig_Start orig, Run self)
    {
      if (RoR2Application.isInSinglePlayer)
        ItemDropAPI.RemovePickup(PickupCatalog.FindPickupIndex("RuinaWeddingRing"), false);
      orig.Invoke(self);
    }

    private void CharacterBody_OnSkillActivated(
      CharacterBody.orig_OnSkillActivated orig,
      CharacterBody self,
      GenericSkill skill)
    {
      orig.Invoke(self, skill);
      if (!Object.op_Implicit((Object) self) || !NetworkServer.active)
        return;
      DotController dotController = DotController.FindDotController(((Component) self).gameObject);
      if (Object.op_Implicit((Object) dotController) && dotController.HasDotActive(DoTCore.FairyIndex))
      {
        int num = self.GetBuffCount(Buffs.fairyDebuff) / 3;
        for (int index = 0; index < dotController.dotStackList.Count; ++index)
        {
          DotController.DotStack dotStack = dotController.dotStackList[index];
          if (dotStack.dotIndex == DoTCore.FairyIndex)
          {
            DamageInfo damageInfo;
            if (Object.op_Implicit((Object) dotStack.attackerObject) && Object.op_Implicit((Object) dotStack.attackerObject.GetComponent<CharacterBody>()))
              damageInfo = new DamageInfo()
              {
                attacker = dotStack.attackerObject,
                inflictor = dotStack.attackerObject,
                crit = dotStack.attackerObject.GetComponent<CharacterBody>().RollCrit(),
                damage = dotStack.attackerObject.GetComponent<CharacterBody>().damage * 1f,
                position = self.corePosition,
                force = Vector3.zero,
                damageType = (DamageType) 0,
                damageColorIndex = (DamageColorIndex) 2,
                dotIndex = DoTCore.FairyIndex,
                procCoefficient = 0.75f
              };
            else
              damageInfo = new DamageInfo()
              {
                attacker = dotStack.attackerObject,
                inflictor = dotStack.attackerObject,
                crit = false,
                damage = 1f,
                position = self.corePosition,
                force = Vector3.zero,
                damageType = (DamageType) 0,
                damageColorIndex = (DamageColorIndex) 2,
                dotIndex = DoTCore.FairyIndex,
                procCoefficient = 0.75f
              };
            self.healthComponent.TakeDamage(damageInfo);
            GlobalEventManager.instance.OnHitEnemy(damageInfo, ((Component) self.healthComponent.body).gameObject);
            GlobalEventManager.instance.OnHitAll(damageInfo, ((Component) self.healthComponent.body).gameObject);
          }
        }
        EffectManager.SpawnEffect(Assets.fairyProcEffect, new EffectData()
        {
          rotation = Util.QuaternionSafeLookRotation(Vector3.zero),
          origin = self.corePosition
        }, false);
      }
    }

    private void OnSurvivorSelected_Hook(
      CharacterSelectController.orig_SelectSurvivor orig,
      CharacterSelectController self,
      SurvivorIndex survivor)
    {
      orig.Invoke(self, survivor);
      SurvivorDef survivorDef = SurvivorCatalog.GetSurvivorDef(survivor);
      if (Object.op_Inequality((Object) survivorDef, (Object) null))
      {
        this.IsRedMistSelected = survivorDef.cachedName == "RedMist";
        this.IsArbiterSelected = survivorDef.cachedName == "Arbiter";
        this.IsBlackSilenceSelected = survivorDef.cachedName == "BlackSilence";
        this.IsModCharSelected = this.IsArbiterSelected || this.IsRedMistSelected || this.IsBlackSilenceSelected;
      }
      this.CurrentCharacterNameSelected = survivorDef.cachedName;
    }

    private void GameNetworkManager_OnClientSceneChanged_Hook(
      GameNetworkManager.orig_OnClientSceneChanged orig,
      GameNetworkManager self,
      NetworkConnection conn)
    {
      orig.Invoke(self, conn);
      Scene activeScene = SceneManager.GetActiveScene();
      if (((Scene) ref activeScene).name.Contains("outro") && this.IsModCharSelected)
      {
        this.songOverride = true;
        this.currentOverrideSong = Util.PlaySound("Play_Dark_Fantasy_Studio___Sun_and_Moon", ((Component) this).gameObject);
        ++Music.musicSources;
      }
      else
      {
        if (!this.songOverride)
          return;
        this.songOverride = false;
        AkSoundEngine.StopPlayingID(this.currentOverrideSong);
        --Music.musicSources;
      }
    }

    private void BrotherSpeechDriver_DoInitialSightResponse(
      BrotherSpeechDriver.orig_DoInitialSightResponse orig,
      BrotherSpeechDriver self)
    {
      bool flag = false;
      ReadOnlyCollection<CharacterBody> onlyInstancesList = CharacterBody.readOnlyInstancesList;
      for (int index = 0; index < onlyInstancesList.Count; ++index)
      {
        BodyIndex bodyIndex = onlyInstancesList[index].bodyIndex;
        flag |= bodyIndex == BodyCatalog.FindBodyIndex(RedMist.redMistPrefab);
      }
      if (flag)
      {
        CharacterSpeechController.SpeechInfo[] speechInfoArray = new CharacterSpeechController.SpeechInfo[1]
        {
          new CharacterSpeechController.SpeechInfo()
          {
            duration = 1f,
            maxWait = 4f,
            mustPlay = true,
            priority = 0.0f,
            token = "BROTHER_SEE_REDMIST_1"
          }
        };
        self.SendReponseFromPool(speechInfoArray);
      }
      orig.Invoke(self);
    }

    private void BrotherSpeechDriver_OnBodyKill(
      BrotherSpeechDriver.orig_OnBodyKill orig,
      BrotherSpeechDriver self,
      DamageReport damageReport)
    {
      if (Object.op_Implicit((Object) damageReport.victimBody) && damageReport.victimBodyIndex == BodyCatalog.FindBodyIndex(RedMist.redMistPrefab))
      {
        CharacterSpeechController.SpeechInfo[] speechInfoArray = new CharacterSpeechController.SpeechInfo[1]
        {
          new CharacterSpeechController.SpeechInfo()
          {
            duration = 1f,
            maxWait = 4f,
            mustPlay = true,
            priority = 0.0f,
            token = "BROTHER_KILL_REDMIST_1"
          }
        };
        self.SendReponseFromPool(speechInfoArray);
      }
      orig.Invoke(self, damageReport);
    }

    private void CharacterBody_OnLevelUp(CharacterBody.orig_OnLevelUp orig, CharacterBody self)
    {
      orig.Invoke(self);
      if (!Object.op_Implicit((Object) self) || !this.IsModCharSelected || !self.isPlayerControlled)
        return;
      int num = (int) Util.PlaySound("Ruina_Snap", ((Component) self).gameObject);
    }

    private void CharacterBody_RecalculateStats(
      CharacterBody.orig_RecalculateStats orig,
      CharacterBody self)
    {
      orig.Invoke(self);
      if (!Object.op_Implicit((Object) self))
        return;
      if (self.HasBuff(Buffs.feebleDebuff))
      {
        self.armor *= 0.5f;
        self.damage *= 0.5f;
      }
      if (self.HasBuff(Buffs.warpBuff))
      {
        self.moveSpeed *= 2f;
        self.attackSpeed *= 2f;
      }
      if (self.HasBuff(Buffs.strengthBuff))
      {
        if (self.isPlayerControlled)
        {
          self.moveSpeed *= 1.5f;
          self.attackSpeed *= 1.5f;
          self.regen *= 2f;
          self.armor += 50f;
          self.damage *= 1.5f;
        }
        else
        {
          self.moveSpeed *= 1.5f;
          self.attackSpeed *= 5f;
          self.regen *= 5f;
          self.armor += 100f;
          self.damage *= 5f;
        }
        if (Object.op_Implicit((Object) self.skillLocator))
        {
          if (Object.op_Implicit((Object) self.skillLocator.primary))
            self.skillLocator.primary.cooldownScale -= 0.25f;
          if (Object.op_Implicit((Object) self.skillLocator.secondary))
            self.skillLocator.secondary.cooldownScale -= 0.25f;
          if (Object.op_Implicit((Object) self.skillLocator.utility))
            self.skillLocator.utility.cooldownScale -= 0.25f;
          if (Object.op_Implicit((Object) self.skillLocator.special))
            self.skillLocator.special.cooldownScale -= 0.25f;
        }
      }
      RedMistStatTracker component1 = ((Component) self).GetComponent<RedMistStatTracker>();
      RedMistEmotionComponent component2 = ((Component) self).GetComponent<RedMistEmotionComponent>();
      if (Object.op_Implicit((Object) component1) && Object.op_Implicit((Object) component2))
      {
        float moveSpeed = self.moveSpeed;
        float attackSpeed = self.attackSpeed;
        self.moveSpeed = self.baseMoveSpeed;
        self.attackSpeed = self.baseAttackSpeed;
        float num1 = (moveSpeed - self.baseMoveSpeed) / self.baseMoveSpeed;
        if (self.isSprinting)
          num1 = (float) (((double) moveSpeed - (double) self.baseMoveSpeed * (double) self.sprintingSpeedMultiplier) / ((double) self.baseMoveSpeed * (double) self.sprintingSpeedMultiplier));
        float num2 = (attackSpeed - self.baseAttackSpeed) / self.baseAttackSpeed;
        float num3 = num1 * Config.moveSpeedMult.Value;
        float num4 = num2 * Config.attackSpeedMult.Value;
        float num5 = self.baseDamage + self.levelDamage * (self.level - 1f);
        float num6 = Config.redMistBuffDamage.Value * (float) self.GetBuffCount(Buffs.RedMistBuff) * num5;
        self.damage += num6;
        float num7 = (float) ((double) num3 * (double) num5 + (double) num4 * (double) num5);
        self.damage += num7;
        float num8 = 0.0f;
        float num9 = 0.0f;
        if (Object.op_Implicit((Object) self.inventory) && self.inventory.GetItemCount(RoR2Content.Items.SprintBonus) > 0)
          num8 += Config.sprintSpeedMult.Value * (float) self.inventory.GetItemCount(RoR2Content.Items.SprintBonus);
        if (Object.op_Implicit((Object) self.inventory) && self.inventory.GetItemCount(RoR2Content.Items.SprintOutOfCombat) > 0)
          num9 += Config.sprintSpeedMult.Value * 2f * (float) self.inventory.GetItemCount(RoR2Content.Items.SprintOutOfCombat);
        if (self.HasBuff(Buffs.EGOBuff))
        {
          if (!component2.inEGO && NetworkServer.active)
            self.RemoveBuff(Buffs.EGOBuff);
          self.armor += 50f;
          self.sprintingSpeedMultiplier = 2.2f;
        }
        else
          self.sprintingSpeedMultiplier = 1.5f;
        self.sprintingSpeedMultiplier += num8;
        if (self.outOfCombat)
          self.sprintingSpeedMultiplier += num9;
        if (self.isSprinting)
          self.moveSpeed *= self.sprintingSpeedMultiplier;
      }
    }

    private void GlobalEvent_OnHitEnemy(
      GlobalEventManager.orig_OnHitEnemy orig,
      GlobalEventManager self,
      DamageInfo damageInfo,
      GameObject victim)
    {
      if (NetworkServer.active)
        RiskOfRuinaNetworkManager.OnHit(self, damageInfo, victim);
      GameObject attacker = damageInfo.attacker;
      if (Object.op_Implicit((Object) self) && Object.op_Implicit((Object) attacker))
      {
        CharacterBody component1 = attacker.GetComponent<CharacterBody>();
        CharacterBody component2 = victim.GetComponent<CharacterBody>();
        if (component1.teamComponent.teamIndex != component2.teamComponent.teamIndex)
        {
          if (component1.baseNameToken == "COF_REDMIST_BODY_NAME")
          {
            RedMistStatTracker component3 = ((Component) component1).GetComponent<RedMistStatTracker>();
            RedMistEmotionComponent component4 = ((Component) component1).GetComponent<RedMistEmotionComponent>();
            if (Object.op_Implicit((Object) component3) && Object.op_Implicit((Object) component4))
            {
              float damage = component1.damage;
              float num1 = Mathf.Clamp(damageInfo.damage / damage, 0.0f, 4f);
              float num2 = (float) Run.instance.stageClearCount / ((float) Run.instance.stageClearCount + 1f);
              if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) component1.master))
                num2 = (float) RiskOfRuinaPlugin.KombatDuelsPlayed(component1.master) / ((float) RiskOfRuinaPlugin.KombatDuelsPlayed(component1.master) + 1f);
              float num3 = num1 * Config.emotionRatio.Value;
              float num4 = 1f;
              if (component2.isElite)
                num4 = 1.2f;
              if (component2.isBoss)
                num4 = 1.4f;
              if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive())
                num4 = !Object.op_Implicit((Object) component2.master) || !RiskOfRuinaPlugin.KombatIsCharacter(component2.master) ? 0.75f : 7.5f;
              component4.AddEmotion((num3 + num3 * num2) * num4);
            }
          }
          if (component1.baseNameToken == "COF_ARBITER_BODY_NAME" && damageInfo.dotIndex != DoTCore.FairyIndex && Util.CheckRoll(100f * damageInfo.procCoefficient, component1.master))
          {
            InflictDotInfo inflictDotInfo = new InflictDotInfo()
            {
              attackerObject = damageInfo.attacker,
              victimObject = victim,
              dotIndex = DoTCore.FairyIndex,
              duration = 10f,
              damageMultiplier = 0.0f
            };
            DotController.InflictDot(ref inflictDotInfo);
          }
        }
      }
      orig.Invoke(self, damageInfo, victim);
    }

    private void GlobalEvent_OnCharacterDeath(
      GlobalEventManager.orig_OnCharacterDeath orig,
      GlobalEventManager self,
      DamageReport damageReport)
    {
      if (Object.op_Implicit((Object) damageReport?.attackerBody) && damageReport.attackerBody.baseNameToken == "COF_REDMIST_BODY_NAME")
      {
        RedMistEmotionComponent component = ((Component) damageReport.attackerBody).GetComponent<RedMistEmotionComponent>();
        if (Object.op_Implicit((Object) component) && component.inEGO && NetworkServer.active)
          damageReport.attackerBody.AddBuff(Buffs.RedMistBuff);
        if ((double) damageReport.combinedHealthBeforeDamage - (double) damageReport.damageDealt <= -(double) damageReport.victim.fullHealth)
        {
          EffectManager.SpawnEffect(Assets.mistEffect, new EffectData()
          {
            origin = damageReport.victimBody.corePosition,
            scale = 1f
          }, true);
          if (Object.op_Implicit((Object) damageReport.victimBody.modelLocator) && Object.op_Implicit((Object) ((Component) damageReport.victimBody.modelLocator.modelTransform).GetComponent<CharacterModel>()) && NetworkServer.active)
            RiskOfRuinaNetworkManager.SetInvisible(((Component) damageReport.victimBody).gameObject);
        }
      }
      if (damageReport.victimBody.baseNameToken == "COF_REDMIST_BODY_NAME" || damageReport.victimBody.baseNameToken == "COF_ARBITER_BODY_NAME" || damageReport.victimBody.baseNameToken == "COF_BLACKSILENCE_BODY_NAME")
      {
        EffectManager.SpawnEffect(Assets.pagePoof, new EffectData()
        {
          origin = damageReport.victimBody.corePosition,
          scale = 1f
        }, true);
        if (NetworkServer.active)
          RiskOfRuinaNetworkManager.SetInvisible(((Component) damageReport.victimBody).gameObject);
      }
      orig.Invoke(self, damageReport);
    }

    private void DotController_InflictDot(DotController self, ref InflictDotInfo dotInfo)
    {
      if (dotInfo.dotIndex != DoTCore.FairyIndex)
        return;
      int index = 0;
      for (int count = self.dotStackList.Count; index < count; ++index)
      {
        if (self.dotStackList[index].dotIndex == DoTCore.FairyIndex)
          self.dotStackList[index].timer = Mathf.Max(self.dotStackList[index].timer, dotInfo.duration);
      }
    }
  }
}
