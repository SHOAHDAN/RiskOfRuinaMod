using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items;

internal class MoonlightStone : RuinaItem
{
	public ItemDef itemDef;

	internal override ConfigEntry<bool> itemEnabled { get; set; }

	internal override string itemName { get; set; } = "RuinaMoonlightStone";


	public override void ItemSetup()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		itemDef = ScriptableObject.CreateInstance<ItemDef>();
		((Object)(object)itemDef).name = itemName;
		itemDef.tier = (ItemTier)0;
		itemDef.pickupModelPrefab = Assets.moonlightStone;
		itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaMoonlightStone");
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
		CharacterBody.FixedUpdate += ClearBuffs;
	}

	private void ClearBuffs(CharacterBody.orig_FixedUpdate orig, CharacterBody self)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		if (NetworkServer.get_active())
		{
			int count = GetCount(self);
			if (count > 0)
			{
				MoonlightStoneTracker moonlightStoneTracker = ((Component)(object)self).GetComponent<MoonlightStoneTracker>();
				if (!moonlightStoneTracker)
				{
					moonlightStoneTracker = ((Component)(object)self).gameObject.AddComponent<MoonlightStoneTracker>();
				}
				moonlightStoneTracker.timer += Time.deltaTime;
				if (moonlightStoneTracker.timer >= 2f)
				{
					int num = 0;
					DotController val = DotController.FindDotController(((Component)(object)self).gameObject);
					if ((bool)(Object)(object)val)
					{
						int num2 = val.dotStackList.Count - 1;
						while (num2 >= 0 && num < count)
						{
							val.RemoveDotStackAtServer(num2);
							num++;
							num2--;
						}
					}
					int num3 = self.activeBuffsList.Length - 1;
					while (num3 >= 0 && num < count)
					{
						BuffDef buffDef = BuffCatalog.GetBuffDef(self.activeBuffsList[num3]);
						if (buffDef.isDebuff && self.GetBuffCount(buffDef) > 0 && buffDef.get_buffIndex() != BuffCatalog.FindBuffIndex("BanditSkull") && buffDef.get_buffIndex() != BuffCatalog.FindBuffIndex("ElementalRingsCooldown"))
						{
							self.RemoveBuff(self.activeBuffsList[num3]);
							num++;
						}
						num3--;
					}
					moonlightStoneTracker.timer = 0f;
				}
			}
		}
		orig(self);
	}
}
