// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.UdjatMask
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
  internal class UdjatMask : RuinaItem
  {
    public ItemDef itemDef;
    public float armorIncrease = 5f;
    public float stackIncrease = 5f;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaUdjatMask";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 1;
      this.itemDef.pickupModelPrefab = Assets.udjatMask;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaUdjatMask");
      this.itemDef.nameToken = this.itemName.ToUpper() + "_NAME";
      this.itemDef.pickupToken = this.itemName.ToUpper() + "_PICKUP";
      this.itemDef.descriptionToken = this.itemName.ToUpper() + "_DESC";
      this.itemDef.loreToken = this.itemName.ToUpper() + "_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 2 };
      ItemAPI.Add(new CustomItem(this.itemDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup()
    {
      // ISSUE: method pointer
      HealthComponent.TakeDamage += new HealthComponent.hook_TakeDamage((object) this, __methodptr(HealthComponent_TakeDamage));
      // ISSUE: method pointer
      CharacterBody.RecalculateStats += new CharacterBody.hook_RecalculateStats((object) this, __methodptr(CharacterBody_RecalculateStats));
    }

    private void HealthComponent_TakeDamage(
      HealthComponent.orig_TakeDamage orig,
      HealthComponent self,
      DamageInfo damageInfo)
    {
      GameObject attacker = damageInfo.attacker;
      if (Object.op_Implicit((Object) self) && Object.op_Implicit((Object) attacker))
      {
        CharacterBody component1 = attacker.GetComponent<CharacterBody>();
        CharacterBody component2 = ((Component) self).GetComponent<CharacterBody>();
        if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) component1) && component1.teamComponent.teamIndex != component2.teamComponent.teamIndex && Object.op_Implicit((Object) component2.master) && component2.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName)) > 0 && (double) damageInfo.damage > 0.0)
          component2.AddTimedBuff(Buffs.udjatBuff, 5f);
      }
      orig.Invoke(self, damageInfo);
    }

    private void CharacterBody_RecalculateStats(
      CharacterBody.orig_RecalculateStats orig,
      CharacterBody self)
    {
      orig.Invoke(self);
      int count = this.GetCount(self);
      if (count <= 0 || !Object.op_Implicit((Object) self) || !self.HasBuff(Buffs.udjatBuff))
        return;
      self.armor += (float) self.GetBuffCount(Buffs.udjatBuff) * (this.armorIncrease + this.stackIncrease * (float) (count - 1));
    }
  }
}
