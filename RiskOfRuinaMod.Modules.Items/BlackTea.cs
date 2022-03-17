using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items;

internal class BlackTea : RuinaItem
{
	public ItemDef itemDef;

	public float procChance = 10f;

	public float stackChance = 5f;

	internal override ConfigEntry<bool> itemEnabled { get; set; }

	internal override string itemName { get; set; } = "RuinaBlackTea";


	public override void ItemSetup()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		itemDef = ScriptableObject.CreateInstance<ItemDef>();
		((Object)(object)itemDef).name = itemName;
		itemDef.tier = (ItemTier)0;
		itemDef.pickupModelPrefab = Assets.blackTea;
		itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaBlackTea");
		itemDef.nameToken = "RUINABLACKTEA_NAME";
		itemDef.pickupToken = "RUINABLACKTEA_PICKUP";
		itemDef.descriptionToken = "RUINABLACKTEA_DESC";
		itemDef.loreToken = "RUINABLACKTEA_LORE";
		itemDef.tags = (ItemTag[])(object)new ItemTag[1] { (ItemTag)1 };
		ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
		CustomItem item = new CustomItem(itemDef, itemDisplayRules);
		ItemAPI.Add(item);
	}

	public override void HookSetup()
	{
		GlobalEventManager.OnHitEnemy += GlobalEvent_OnHitEnemy;
	}

	private void GlobalEvent_OnHitEnemy(GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		GameObject attacker = damageInfo.attacker;
		if ((bool)(Object)(object)self && (bool)attacker)
		{
			CharacterBody component = attacker.GetComponent<CharacterBody>();
			CharacterBody component2 = victim.GetComponent<CharacterBody>();
			if (component.get_teamComponent().get_teamIndex() != component2.get_teamComponent().get_teamIndex())
			{
				CharacterMaster master = component.get_master();
				if ((bool)(Object)(object)master)
				{
					int itemCount = component.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
					if (itemCount > 0)
					{
						float num = procChance + stackChance * (float)(itemCount - 1);
						if (Util.CheckRoll(num * damageInfo.procCoefficient, master) && damageInfo.dotIndex != DoTCore.FairyIndex)
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
			}
		}
		orig(self, damageInfo, victim);
	}
}
