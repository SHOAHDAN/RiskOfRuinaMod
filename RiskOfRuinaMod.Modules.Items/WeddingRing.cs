using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items;

internal class WeddingRing : RuinaItem
{
	public ItemDef itemDef;

	public float damageIncrease = 0.1f;

	public float stackIncrease = 0.05f;

	public float range = 25f;

	internal override ConfigEntry<bool> itemEnabled { get; set; }

	internal override string itemName { get; set; } = "RuinaWeddingRing";


	public override void ItemSetup()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		itemDef = ScriptableObject.CreateInstance<ItemDef>();
		((Object)(object)itemDef).name = itemName;
		itemDef.tier = (ItemTier)1;
		itemDef.pickupModelPrefab = Assets.weddingRing;
		itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaWeddingRing");
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
		CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
	}

	private void CharacterBody_RecalculateStats(CharacterBody.orig_RecalculateStats orig, CharacterBody self)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		orig(self);
		int count = GetCount(self);
		if (count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(self.get_teamComponent().get_teamIndex()))
		{
			if ((((Component)(object)teamMember).transform.position - self.get_corePosition()).sqrMagnitude <= range * range)
			{
				CharacterBody body = teamMember.get_body();
				if ((bool)(Object)(object)body && (Object)(object)body != (Object)(object)self && (bool)(Object)(object)body.get_inventory())
				{
					num += body.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
				}
			}
		}
		if (num > 0)
		{
			self.set_damage(self.get_damage() + (self.baseDamage + self.levelDamage * (self.get_level() - 1f)) * (damageIncrease + stackIncrease * (float)(count + num - 1)));
		}
	}
}
