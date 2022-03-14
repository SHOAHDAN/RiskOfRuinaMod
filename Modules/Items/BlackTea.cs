// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.BlackTea
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
  internal class BlackTea : RuinaItem
  {
    public ItemDef itemDef;
    public float procChance = 10f;
    public float stackChance = 5f;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaBlackTea";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 0;
      this.itemDef.pickupModelPrefab = Assets.blackTea;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaBlackTea");
      this.itemDef.nameToken = "RUINABLACKTEA_NAME";
      this.itemDef.pickupToken = "RUINABLACKTEA_PICKUP";
      this.itemDef.descriptionToken = "RUINABLACKTEA_DESC";
      this.itemDef.loreToken = "RUINABLACKTEA_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 1 };
      ItemAPI.Add(new CustomItem(this.itemDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup() => GlobalEventManager.OnHitEnemy += new GlobalEventManager.hook_OnHitEnemy((object) this, __methodptr(GlobalEvent_OnHitEnemy));

    private void GlobalEvent_OnHitEnemy(
      GlobalEventManager.orig_OnHitEnemy orig,
      GlobalEventManager self,
      DamageInfo damageInfo,
      GameObject victim)
    {
      GameObject attacker = damageInfo.attacker;
      if (Object.op_Implicit((Object) self) && Object.op_Implicit((Object) attacker))
      {
        CharacterBody component1 = attacker.GetComponent<CharacterBody>();
        CharacterBody component2 = victim.GetComponent<CharacterBody>();
        if (component1.teamComponent.teamIndex != component2.teamComponent.teamIndex)
        {
          CharacterMaster master = component1.master;
          if (Object.op_Implicit((Object) master))
          {
            int itemCount = component1.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName));
            if (itemCount > 0 && Util.CheckRoll((this.procChance + this.stackChance * (float) (itemCount - 1)) * damageInfo.procCoefficient, master) && damageInfo.dotIndex != DoTCore.FairyIndex)
            {
              InflictDotInfo inflictDotInfo = new InflictDotInfo()
              {
                attackerObject = damageInfo.attacker,
                victimObject = victim,
                dotIndex = DoTCore.FairyIndex,
                duration = 10f,
                damageMultiplier = 0.0f
              };
              DotController.InflictDot(ref inflictDotInfo);
            }
          }
        }
      }
      orig.Invoke(self, damageInfo, victim);
    }
  }
}
