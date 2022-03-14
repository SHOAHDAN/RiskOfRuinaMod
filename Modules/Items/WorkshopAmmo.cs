// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.WorkshopAmmo
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items
{
  internal class WorkshopAmmo : RuinaItem
  {
    public ItemDef itemDef;
    public float damageIncrease = 0.25f;
    public float stackIncrease = 0.1f;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaWorkshopAmmo";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 1;
      this.itemDef.pickupModelPrefab = Assets.workshopAmmo;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaWorkshopAmmo");
      this.itemDef.nameToken = this.itemName.ToUpper() + "_NAME";
      this.itemDef.pickupToken = this.itemName.ToUpper() + "_PICKUP";
      this.itemDef.descriptionToken = this.itemName.ToUpper() + "_DESC";
      this.itemDef.loreToken = this.itemName.ToUpper() + "_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 1 };
      ItemAPI.Add(new CustomItem(this.itemDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup() => HealthComponent.TakeDamage += new HealthComponent.hook_TakeDamage((object) this, __methodptr(HealthComponent_TakeDamage));

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
        if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) component1) && component1.teamComponent.teamIndex != component2.teamComponent.teamIndex && Object.op_Implicit((Object) component1.master))
        {
          int itemCount = component1.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName));
          if (itemCount > 0)
          {
            float num1 = Vector3.Distance(component1.corePosition, component2.corePosition);
            if ((double) num1 >= 10.0 && NetworkServer.active)
            {
              float num2 = this.damageIncrease + this.stackIncrease * (float) (itemCount - 1);
              float num3 = Mathf.Clamp(Mathf.Lerp(0.0f, num2, (float) (((double) num1 - 10.0) / 100.0)), 0.0f, num2);
              damageInfo.damage += damageInfo.damage * num3;
              damageInfo.damageColorIndex = (DamageColorIndex) 9;
            }
          }
        }
      }
      orig.Invoke(self, damageInfo);
    }
  }
}
