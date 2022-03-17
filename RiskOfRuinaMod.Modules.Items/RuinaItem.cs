using BepInEx.Configuration;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items;

public abstract class RuinaItem
{
	internal abstract ConfigEntry<bool> itemEnabled { get; set; }

	internal abstract string itemName { get; set; }

	public virtual void Init()
	{
		itemEnabled = Config.ItemEnableConfig(itemName);
		if (itemEnabled.Value)
		{
			ItemSetup();
			HookSetup();
		}
	}

	public abstract void ItemSetup();

	public abstract void HookSetup();

	public int GetCount(CharacterBody character)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		int result = 0;
		if ((bool)(Object)(object)character && (bool)(Object)(object)character.get_inventory())
		{
			result = character.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
		}
		return result;
	}
}
