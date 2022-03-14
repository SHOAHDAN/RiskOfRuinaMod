// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.Prescript
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items
{
  internal class Prescript : RuinaItem
  {
    public ItemDef itemDef;
    public float damageIncrease = 0.01f;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaPrescript";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 1;
      this.itemDef.pickupModelPrefab = Assets.prescript;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaPrescript");
      this.itemDef.nameToken = "RUINAPRESCRIPT_NAME";
      this.itemDef.pickupToken = "RUINAPRESCRIPT_PICKUP";
      this.itemDef.descriptionToken = "RUINAPRESCRIPT_DESC";
      this.itemDef.loreToken = "RUINAPRESCRIPT_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 1 };
      ItemAPI.Add(new CustomItem(this.itemDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup() => CharacterBody.RecalculateStats += new CharacterBody.hook_RecalculateStats((object) this, __methodptr(CharacterBody_RecalculateStats));

    private void CharacterBody_RecalculateStats(
      CharacterBody.orig_RecalculateStats orig,
      CharacterBody self)
    {
      orig.Invoke(self);
      int count = this.GetCount(self);
      if (count <= 0)
        return;
      int num = ((IEnumerable<ItemIndex>) self.inventory.itemAcquisitionOrder).Select<ItemIndex, ItemIndex>((Func<ItemIndex, ItemIndex>) (x => x)).Distinct<ItemIndex>().Count<ItemIndex>();
      self.damage += (float) (((double) self.baseDamage + (double) self.levelDamage * ((double) self.level - 1.0)) * ((double) num * ((double) this.damageIncrease * (double) count)));
    }
  }
}
