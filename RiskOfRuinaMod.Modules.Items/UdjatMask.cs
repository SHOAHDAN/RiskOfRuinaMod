using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items
{

	internal class UdjatMask : RuinaItem
	{
		public ItemDef itemDef;

		public float armorIncrease = 5f;

		public float stackIncrease = 5f;

		internal override ConfigEntry<bool> itemEnabled { get; set; }

		internal override string itemName { get; set; } = "RuinaUdjatMask";


		public override void ItemSetup()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			itemDef = ScriptableObject.CreateInstance<ItemDef>();
			((Object)(object)itemDef).name = itemName;
			itemDef.tier = (ItemTier)1;
			itemDef.pickupModelPrefab = Assets.udjatMask;
			itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaUdjatMask");
			itemDef.nameToken = itemName.ToUpper() + "_NAME";
			itemDef.pickupToken = itemName.ToUpper() + "_PICKUP";
			itemDef.descriptionToken = itemName.ToUpper() + "_DESC";
			itemDef.loreToken = itemName.ToUpper() + "_LORE";
			itemDef.tags = (ItemTag[])(object)new ItemTag[1] { (ItemTag)2 };
			ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
			CustomItem item = new CustomItem(itemDef, itemDisplayRules);
			ItemAPI.Add(item);
		}

		public override void HookSetup()
		{
			HealthComponent.TakeDamage += HealthComponent_TakeDamage;
			CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
		}

		private void HealthComponent_TakeDamage(HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			GameObject attacker = damageInfo.attacker;
			if ((bool)(Object)(object)self && (bool)attacker)
			{
				CharacterBody component = attacker.GetComponent<CharacterBody>();
				CharacterBody component2 = ((Component)(object)self).GetComponent<CharacterBody>();
				if ((bool)(Object)(object)component2 && (bool)(Object)(object)component && component.get_teamComponent().get_teamIndex() != component2.get_teamComponent().get_teamIndex())
				{
					CharacterMaster master = component2.get_master();
					if ((bool)(Object)(object)master)
					{
						int itemCount = component2.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
						if (itemCount > 0 && damageInfo.damage > 0f)
						{
							component2.AddTimedBuff(Buffs.udjatBuff, 5f);
						}
					}
				}
			}
			orig(self, damageInfo);
		}

		private void CharacterBody_RecalculateStats(CharacterBody.orig_RecalculateStats orig, CharacterBody self)
		{
			orig(self);
			int count = GetCount(self);
			if (count > 0 && (bool)(Object)(object)self && self.HasBuff(Buffs.udjatBuff))
			{
				self.set_armor(self.get_armor() + (float)self.GetBuffCount(Buffs.udjatBuff) * (armorIncrease + stackIncrease * (float)(count - 1)));
			}
		}
	}
}