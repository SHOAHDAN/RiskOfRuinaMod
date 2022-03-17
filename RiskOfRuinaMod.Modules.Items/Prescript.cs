using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items;

internal class Prescript : RuinaItem
{
	public ItemDef itemDef;

	public float damageIncrease = 0.01f;

	internal override ConfigEntry<bool> itemEnabled { get; set; }

	internal override string itemName { get; set; } = "RuinaPrescript";


	public override void ItemSetup()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		itemDef = ScriptableObject.CreateInstance<ItemDef>();
		((Object)(object)itemDef).name = itemName;
		itemDef.tier = (ItemTier)1;
		itemDef.pickupModelPrefab = Assets.prescript;
		itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaPrescript");
		itemDef.nameToken = "RUINAPRESCRIPT_NAME";
		itemDef.pickupToken = "RUINAPRESCRIPT_PICKUP";
		itemDef.descriptionToken = "RUINAPRESCRIPT_DESC";
		itemDef.loreToken = "RUINAPRESCRIPT_LORE";
		itemDef.tags = (ItemTag[])(object)new ItemTag[1] { (ItemTag)1 };
		ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
		CustomItem item = new CustomItem(itemDef, itemDisplayRules);
		ItemAPI.Add(item);
	}

	public override void HookSetup()
	{
		CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
	}

	private void CharacterBody_RecalculateStats(CharacterBody.orig_RecalculateStats orig, CharacterBody self)
	{
		orig(self);
		int count = GetCount(self);
		if (count > 0)
		{
			List<ItemIndex> itemAcquisitionOrder = self.get_inventory().itemAcquisitionOrder;
			int num = itemAcquisitionOrder.Select((ItemIndex x) => x).Distinct().Count();
			self.set_damage(self.get_damage() + (self.baseDamage + self.levelDamage * (self.get_level() - 1f)) * ((float)num * (damageIncrease * (float)count)));
		}
	}
}
