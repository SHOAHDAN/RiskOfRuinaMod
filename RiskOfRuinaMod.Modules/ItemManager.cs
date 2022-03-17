using System.Collections.Generic;
using RiskOfRuinaMod.Modules.Items;

namespace RiskOfRuinaMod.Modules;

internal class ItemManager
{
	public static ItemManager instance;

	public List<RuinaItem> items = new List<RuinaItem>();

	public List<RuinaEquipment> equips = new List<RuinaEquipment>();

	public ItemManager()
	{
		instance = this;
	}

	public void AddItems()
	{
		foreach (RuinaItem item in items)
		{
			item.Init();
		}
	}

	public void AddEquips()
	{
		foreach (RuinaEquipment equip in equips)
		{
			equip.Init();
		}
	}
}
