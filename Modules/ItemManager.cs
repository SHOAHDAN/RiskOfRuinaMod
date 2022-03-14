// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.ItemManager
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RiskOfRuinaMod.Modules.Items;
using System.Collections.Generic;

namespace RiskOfRuinaMod.Modules
{
  internal class ItemManager
  {
    public static ItemManager instance;
    public List<RuinaItem> items = new List<RuinaItem>();
    public List<RuinaEquipment> equips = new List<RuinaEquipment>();

    public ItemManager() => ItemManager.instance = this;

    public void AddItems()
    {
      foreach (RuinaItem ruinaItem in this.items)
        ruinaItem.Init();
    }

    public void AddEquips()
    {
      foreach (RuinaEquipment equip in this.equips)
        equip.Init();
    }
  }
}
