using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items
{

	internal class WorkshopAmmo : RuinaItem
	{
		public ItemDef itemDef;

		public float damageIncrease = 0.25f;

		public float stackIncrease = 0.1f;

		internal override ConfigEntry<bool> itemEnabled { get; set; }

		internal override string itemName { get; set; } = "RuinaWorkshopAmmo";


		public override void ItemSetup()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			itemDef = ScriptableObject.CreateInstance<ItemDef>();
			((Object)(object)itemDef).name = itemName;
			itemDef.tier = (ItemTier)1;
			itemDef.pickupModelPrefab = Assets.workshopAmmo;
			itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaWorkshopAmmo");
			itemDef.nameToken = itemName.ToUpper() + "_NAME";
			itemDef.pickupToken = itemName.ToUpper() + "_PICKUP";
			itemDef.descriptionToken = itemName.ToUpper() + "_DESC";
			itemDef.loreToken = itemName.ToUpper() + "_LORE";
			itemDef.tags = (ItemTag[])(object)new ItemTag[1] { (ItemTag)1 };
			ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
			CustomItem item = new CustomItem(itemDef, itemDisplayRules);
			ItemAPI.Add(item);
		}

		public override void HookSetup()
		{
			HealthComponent.TakeDamage += HealthComponent_TakeDamage;
		}

		private void HealthComponent_TakeDamage(HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			GameObject attacker = damageInfo.attacker;
			if ((bool)(Object)(object)self && (bool)attacker)
			{
				CharacterBody component = attacker.GetComponent<CharacterBody>();
				CharacterBody component2 = ((Component)(object)self).GetComponent<CharacterBody>();
				if ((bool)(Object)(object)component2 && (bool)(Object)(object)component && component.get_teamComponent().get_teamIndex() != component2.get_teamComponent().get_teamIndex())
				{
					CharacterMaster master = component.get_master();
					if ((bool)(Object)(object)master)
					{
						int itemCount = component.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
						if (itemCount > 0)
						{
							float num = Vector3.Distance(component.get_corePosition(), component2.get_corePosition());
							if (num >= 10f && NetworkServer.get_active())
							{
								float num2 = damageIncrease + stackIncrease * (float)(itemCount - 1);
								float num3 = Mathf.Clamp(Mathf.Lerp(0f, num2, (num - 10f) / 100f), 0f, num2);
								damageInfo.damage += damageInfo.damage * num3;
								damageInfo.damageColorIndex = (DamageColorIndex)9;
							}
						}
					}
				}
			}
			orig(self, damageInfo);
		}
	}
}