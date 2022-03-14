// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.RuinaItem
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items
{
  public abstract class RuinaItem
  {
    internal abstract ConfigEntry<bool> itemEnabled { get; set; }

    internal abstract string itemName { get; set; }

    public virtual void Init()
    {
      this.itemEnabled = Config.ItemEnableConfig(this.itemName);
      if (!this.itemEnabled.Value)
        return;
      this.ItemSetup();
      this.HookSetup();
    }

    public abstract void ItemSetup();

    public abstract void HookSetup();

    public int GetCount(CharacterBody character)
    {
      int count = 0;
      if (Object.op_Implicit((Object) character) && Object.op_Implicit((Object) character.inventory))
        count = character.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName));
      return count;
    }
  }
}
