using System.Collections.Generic;
using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items {

	internal class Reverberation : RuinaItem
	{
		public ItemDef itemDef;

		internal override ConfigEntry<bool> itemEnabled { get; set; }

		internal override string itemName { get; set; } = "RuinaReverberation";


		public override void ItemSetup()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			itemDef = ScriptableObject.CreateInstance<ItemDef>();
			((Object)(object)itemDef).name = itemName;
			itemDef.tier = (ItemTier)2;
			itemDef.pickupModelPrefab = Assets.reverberation;
			itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaReverberation");
			itemDef.nameToken = itemName.ToUpper() + "_NAME";
			itemDef.pickupToken = itemName.ToUpper() + "_PICKUP";
			itemDef.descriptionToken = itemName.ToUpper() + "_DESC";
			itemDef.loreToken = itemName.ToUpper() + "_LORE";
			itemDef.tags = (ItemTag[])(object)new ItemTag[1] { (ItemTag)3 };
			ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
			CustomItem item = new CustomItem(itemDef, itemDisplayRules);
			ItemAPI.Add(item);
		}

		public override void HookSetup()
		{
			OverlapAttack.Fire += OverlapAttack_Fire;
			BulletAttack.Fire += BulletAttack_Fire;
			BlastAttack.Fire += BlastAttack_Fire;
		}

		private bool OverlapAttack_Fire(OverlapAttack.orig_Fire orig, OverlapAttack self, List<HurtBox> hitResults)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			GameObject attacker = self.attacker;
			if ((bool)attacker)
			{
				CharacterBody component = attacker.GetComponent<CharacterBody>();
				if ((bool)(Object)(object)component)
				{
					CharacterMaster master = component.get_master();
					if ((bool)(Object)(object)master && (bool)(Object)(object)component.get_inventory())
					{
						int itemCount = component.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
						if (itemCount > 0)
						{
							Vector3 position = ((Component)(object)self.hitBoxGroup.hitBoxes[0]).transform.position;
							Vector3 halfExtents = ((Component)(object)self.hitBoxGroup.hitBoxes[0]).transform.localScale / 2f * (1f + 0.25f * ((float)itemCount - 1f));
							Quaternion rotation = ((Component)(object)self.hitBoxGroup.hitBoxes[0]).transform.rotation;
							Collider[] array = Physics.OverlapBox(position, halfExtents, rotation, ((LayerIndex)(ref LayerIndex.projectile)).get_mask());
							for (int i = 0; i < array.Length; i++)
							{
								ProjectileController component2 = array[i].GetComponent<ProjectileController>();
								ProjectileDamage component3 = array[i].GetComponent<ProjectileDamage>();
								if ((bool)(Object)(object)component2 && (bool)(Object)(object)component3)
								{
									TeamComponent component4 = component2.owner.GetComponent<TeamComponent>();
									if ((bool)(Object)(object)component4 && component4.get_teamIndex() != component.get_teamComponent().get_teamIndex())
									{
										EffectData val = new EffectData();
										val.set_origin(((Component)(object)component2).transform.position);
										EffectManager.SpawnEffect(Assets.blockEffect, val, false);
										FireProjectileInfo val2 = default(FireProjectileInfo);
										val2.projectilePrefab = ((Component)(object)component2).gameObject;
										val2.position = ((Component)(object)component2).transform.position;
										val2.rotation = Util.QuaternionSafeLookRotation(component.get_inputBank().GetAimRay().direction);
										val2.owner = attacker;
										val2.damage = component3.damage + self.damage;
										val2.force = component3.force;
										val2.crit = component3.crit;
										val2.target = component2.owner;
										FireProjectileInfo val3 = val2;
										ProjectileManager.get_instance().FireProjectile(val3);
										Util.PlaySound("Play_Defense_Guard", attacker);
										Object.Destroy(((Component)(object)component2).gameObject);
									}
								}
							}
						}
					}
				}
			}
			return orig(self, hitResults);
		}

		private void BulletAttack_Fire(BulletAttack.orig_Fire orig, BulletAttack self)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			GameObject owner = self.owner;
			if ((bool)owner)
			{
				CharacterBody component = owner.GetComponent<CharacterBody>();
				if ((bool)(Object)(object)component)
				{
					CharacterMaster master = component.get_master();
					if ((bool)(Object)(object)master && (bool)(Object)(object)component.get_inventory())
					{
						int itemCount = component.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
						if (itemCount > 0 && Physics.SphereCast(new Ray(self.origin, self.get_aimVector()), 0.75f * (1f + 0.25f * ((float)itemCount - 1f)), out var hitInfo, self.get_maxDistance(), ((LayerIndex)(ref LayerIndex.projectile)).get_mask()))
						{
							ProjectileController component2 = hitInfo.collider.GetComponent<ProjectileController>();
							ProjectileDamage component3 = hitInfo.collider.GetComponent<ProjectileDamage>();
							if ((bool)(Object)(object)component2 && (bool)(Object)(object)component3)
							{
								TeamComponent component4 = component2.owner.GetComponent<TeamComponent>();
								if ((bool)(Object)(object)component4 && component4.get_teamIndex() != component.get_teamComponent().get_teamIndex())
								{
									EffectData val = new EffectData();
									val.set_origin(((Component)(object)component2).transform.position);
									EffectManager.SpawnEffect(Assets.blockEffect, val, false);
									FireProjectileInfo val2 = default(FireProjectileInfo);
									val2.projectilePrefab = ((Component)(object)component2).gameObject;
									val2.position = ((Component)(object)component2).transform.position;
									val2.rotation = Util.QuaternionSafeLookRotation(self.get_aimVector());
									val2.owner = owner;
									val2.damage = component3.damage + self.damage;
									val2.force = component3.force;
									val2.crit = component3.crit;
									FireProjectileInfo val3 = val2;
									ProjectileManager.get_instance().FireProjectile(val3);
									Util.PlaySound("Play_Defense_Guard", owner);
									Object.Destroy(((Component)(object)component2).gameObject);
								}
							}
						}
					}
				}
			}
			orig(self);
		}

		private Result BlastAttack_Fire(BlastAttack.orig_Fire orig, BlastAttack self)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			GameObject attacker = self.attacker;
			if ((bool)attacker)
			{
				CharacterBody component = attacker.GetComponent<CharacterBody>();
				if ((bool)(Object)(object)component)
				{
					CharacterMaster master = component.get_master();
					if ((bool)(Object)(object)master && (bool)(Object)(object)component.get_inventory())
					{
						int itemCount = component.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
						if (itemCount > 0)
						{
							Collider[] array = Physics.OverlapSphere(self.position, self.radius * (1f + 0.25f * ((float)itemCount - 1f)), ((LayerIndex)(ref LayerIndex.projectile)).get_mask());
							for (int i = 0; i < array.Length; i++)
							{
								ProjectileController component2 = array[i].GetComponent<ProjectileController>();
								ProjectileDamage component3 = array[i].GetComponent<ProjectileDamage>();
								if ((bool)(Object)(object)component2 && (bool)(Object)(object)component3)
								{
									TeamComponent component4 = component2.owner.GetComponent<TeamComponent>();
									if ((bool)(Object)(object)component4 && component4.get_teamIndex() != component.get_teamComponent().get_teamIndex())
									{
										EffectData val = new EffectData();
										val.set_origin(((Component)(object)component2).transform.position);
										EffectManager.SpawnEffect(Assets.blockEffect, val, false);
										FireProjectileInfo val2 = default(FireProjectileInfo);
										val2.projectilePrefab = ((Component)(object)component2).gameObject;
										val2.position = ((Component)(object)component2).transform.position;
										val2.rotation = Util.QuaternionSafeLookRotation((((Component)(object)component2).transform.position - self.position).normalized);
										val2.owner = attacker;
										val2.damage = component3.damage + self.baseDamage;
										val2.force = component3.force;
										val2.crit = component3.crit;
										FireProjectileInfo val3 = val2;
										ProjectileManager.get_instance().FireProjectile(val3);
										Util.PlaySound("Play_Defense_Guard", attacker);
										Object.Destroy(((Component)(object)component2).gameObject);
									}
								}
							}
						}
					}
				}
			}
			return orig(self);
		}
	}
}