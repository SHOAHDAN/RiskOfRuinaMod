// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.LiuBadge
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items
{
  internal class LiuBadge : RuinaItem
  {
    public ItemDef itemDef;
    public float damageIncrease = 0.1f;
    public float stackIncrease = 0.05f;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaLiuBadge";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 1;
      this.itemDef.pickupModelPrefab = Assets.liuBadge;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaLiuBadge");
      this.itemDef.nameToken = this.itemName.ToUpper() + "_NAME";
      this.itemDef.pickupToken = this.itemName.ToUpper() + "_PICKUP";
      this.itemDef.descriptionToken = this.itemName.ToUpper() + "_DESC";
      this.itemDef.loreToken = this.itemName.ToUpper() + "_LORE";
      this.itemDef.tags = new ItemTag[2]
      {
        (ItemTag) 1,
        (ItemTag) 4
      };
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
      if (RiskOfRuinaPlugin.kombatArenaInstalled)
      {
        if (RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) self.master))
          self.damage += (float) (((double) self.baseDamage + (double) self.levelDamage * ((double) self.level - 1.0)) * ((double) RiskOfRuinaPlugin.KombatDuelsPlayed(self.master) * ((double) this.damageIncrease + (double) this.stackIncrease * (double) (count - 1))));
        else
          self.damage += (float) (((double) self.baseDamage + (double) self.levelDamage * ((double) self.level - 1.0)) * ((double) Run.instance.stageClearCount * ((double) this.damageIncrease + (double) this.stackIncrease * (double) (count - 1))));
      }
      else
        self.damage += (float) (((double) self.baseDamage + (double) self.levelDamage * ((double) self.level - 1.0)) * ((double) Run.instance.stageClearCount * ((double) this.damageIncrease + (double) this.stackIncrease * (double) (count - 1))));
    }
  }
}
