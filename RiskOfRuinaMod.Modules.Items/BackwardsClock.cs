using System.Collections.Generic;
using BepInEx.Configuration;
using EntityStates;
using On.RoR2;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items
{

	internal class BackwardsClock : RuinaEquipment
	{
		public EquipmentDef equipDef;

		internal override ConfigEntry<bool> equipEnabled { get; set; }

		internal override string equipName { get; set; } = "RuinaBackwardsClock";


		public override void EquipSetup()
		{
			equipDef = ScriptableObject.CreateInstance<EquipmentDef>();
			((Object)(object)equipDef).name = equipName;
			equipDef.appearsInMultiPlayer = true;
			equipDef.appearsInSinglePlayer = false;
			equipDef.isLunar = true;
			equipDef.pickupModelPrefab = Assets.backwardsClock;
			equipDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaBackwardsClock");
			equipDef.nameToken = equipName.ToUpper() + "_NAME";
			equipDef.pickupToken = equipName.ToUpper() + "_PICKUP";
			equipDef.descriptionToken = equipName.ToUpper() + "_DESC";
			equipDef.loreToken = equipName.ToUpper() + "_LORE";
			equipDef.enigmaCompatible = false;
			equipDef.canDrop = true;
			equipDef.cooldown = 0f;
			ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
			CustomEquipment item = new CustomEquipment(equipDef, itemDisplayRules);
			ItemAPI.Add(item);
		}

		public override void HookSetup()
		{
			EquipmentSlot.PerformEquipmentAction += EquipmentSlot_PerformEquipmentAction;
		}

		private bool EquipmentSlot_PerformEquipmentAction(EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentDef equipmentIndex)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Invalid comparison between Unknown and I4
			if ((Object)(object)equipmentIndex == (Object)(object)equipDef)
			{
				if (NetworkServer.get_active())
				{
					foreach (PlayerCharacterMasterController allDeadCharacter in GetAllDeadCharacters())
					{
						GameObject bodyPrefab = BodyCatalog.GetBodyPrefab(allDeadCharacter.get_networkUser().get_NetworkbodyIndexPreference());
						if (bodyPrefab != null)
						{
							allDeadCharacter.get_master().bodyPrefab = bodyPrefab;
						}
						allDeadCharacter.get_master().Respawn(allDeadCharacter.get_master().GetFieldValue<Vector3>("deathFootPosition"), ((Component)(object)allDeadCharacter.get_master()).transform.rotation);
						GameObject gameObject = Resources.Load<GameObject>("Prefabs/Effects/HippoRezEffect");
						EffectData val = new EffectData();
						val.set_origin(allDeadCharacter.get_master().GetBody().get_footPosition());
						val.rotation = ((Component)(object)allDeadCharacter.get_master()).gameObject.transform.rotation;
						EffectManager.SpawnEffect(gameObject, val, true);
					}
					DamageInfo val2 = new DamageInfo
					{
						attacker = null,
						inflictor = null,
						crit = true,
						damage = self.get_characterBody().get_healthComponent().get_combinedHealth() + self.get_characterBody().get_healthComponent().get_fullBarrier(),
						position = ((Component)(object)self).transform.position,
						force = Vector3.zero,
						damageType = (DamageType)262144,
						damageColorIndex = (DamageColorIndex)0,
						procCoefficient = 0f
					};
					self.get_characterBody().get_inventory().SetEquipmentIndex((EquipmentIndex)(-1));
					if (self.get_characterBody().HasBuff(Buffs.HiddenInvincibility))
					{
						self.get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
					}
					self.get_characterBody().get_healthComponent().TakeDamage(val2);
					GlobalEventManager.instance.OnHitAll(val2, ((Component)(object)self).gameObject);
					TeamIndex val3 = (TeamIndex)0;
					while ((int)val3 < 4)
					{
						if (val3 != self.get_characterBody().get_teamComponent().get_teamIndex())
						{
							foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(val3))
							{
								CharacterBody body = teamMember.get_body();
								if ((bool)(Object)(object)body)
								{
									EntityStateMachine component = ((Component)(object)body).GetComponent<EntityStateMachine>();
									if ((Object)(object)component != null)
									{
										StunState state = new StunState
										{
											duration = 5f,
											stunDuration = 5f
										};
										component.SetState((EntityState)(object)state);
									}
								}
							}
						}
						val3 = (TeamIndex)(sbyte)(val3 + 1);
					}
				}
				return true;
			}
			return orig(self, equipmentIndex);
		}

		private List<PlayerCharacterMasterController> GetAllDeadCharacters()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			List<PlayerCharacterMasterController> list = new List<PlayerCharacterMasterController>();
			foreach (PlayerCharacterMasterController instance in PlayerCharacterMasterController.get_instances())
			{
				NetworkUser networkUser = instance.get_networkUser();
				if (instance.get_isConnected() && (networkUser.get_master().IsDeadAndOutOfLivesServer() || networkUser.get_master().bodyPrefab != BodyCatalog.GetBodyPrefab(networkUser.get_NetworkbodyIndexPreference())))
				{
					list.Add(networkUser.get_master().get_playerCharacterMasterController());
				}
			}
			return list;
		}
	}
}