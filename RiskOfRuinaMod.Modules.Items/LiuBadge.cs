using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items;

internal class LiuBadge : RuinaItem
{
	public ItemDef itemDef;

	public float damageIncrease = 0.1f;

	public float stackIncrease = 0.05f;

	internal override ConfigEntry<bool> itemEnabled { get; set; }

	internal override string itemName { get; set; } = "RuinaLiuBadge";


	public override void ItemSetup()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		itemDef = ScriptableObject.CreateInstance<ItemDef>();
		((Object)(object)itemDef).name = itemName;
		itemDef.tier = (ItemTier)1;
		itemDef.pickupModelPrefab = Assets.liuBadge;
		itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaLiuBadge");
		itemDef.nameToken = itemName.ToUpper() + "_NAME";
		itemDef.pickupToken = itemName.ToUpper() + "_PICKUP";
		itemDef.descriptionToken = itemName.ToUpper() + "_DESC";
		itemDef.loreToken = itemName.ToUpper() + "_LORE";
		itemDef.tags = (ItemTag[])(object)new ItemTag[2]
		{
			(ItemTag)1,
			(ItemTag)4
		};
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
		if (count <= 0)
		{
			return;
		}
		if (RiskOfRuinaPlugin.kombatArenaInstalled)
		{
			if (RiskOfRuinaPlugin.KombatGamemodeActive() && (bool)(Object)(object)self.get_master())
			{
				self.set_damage(self.get_damage() + (self.baseDamage + self.levelDamage * (self.get_level() - 1f)) * ((float)RiskOfRuinaPlugin.KombatDuelsPlayed(self.get_master()) * (damageIncrease + stackIncrease * (float)(count - 1))));
			}
			else
			{
				self.set_damage(self.get_damage() + (self.baseDamage + self.levelDamage * (self.get_level() - 1f)) * ((float)Run.get_instance().stageClearCount * (damageIncrease + stackIncrease * (float)(count - 1))));
			}
		}
		else
		{
			self.set_damage(self.get_damage() + (self.baseDamage + self.levelDamage * (self.get_level() - 1f)) * ((float)Run.get_instance().stageClearCount * (damageIncrease + stackIncrease * (float)(count - 1))));
		}
	}
}
