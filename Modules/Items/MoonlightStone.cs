// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.MoonlightStone
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
  internal class MoonlightStone : RuinaItem
  {
    public ItemDef itemDef;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaMoonlightStone";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 0;
      this.itemDef.pickupModelPrefab = Assets.moonlightStone;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaMoonlightStone");
      this.itemDef.nameToken = this.itemName.ToUpper() + "_NAME";
      this.itemDef.pickupToken = this.itemName.ToUpper() + "_PICKUP";
      this.itemDef.descriptionToken = this.itemName.ToUpper() + "_DESC";
      this.itemDef.loreToken = this.itemName.ToUpper() + "_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 3 };
      ItemAPI.Add(new CustomItem(this.itemDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup() => CharacterBody.FixedUpdate += new CharacterBody.hook_FixedUpdate((object) this, __methodptr(ClearBuffs));

    private void ClearBuffs(CharacterBody.orig_FixedUpdate orig, CharacterBody self)
    {
      if (NetworkServer.active)
      {
        int count = this.GetCount(self);
        if (count > 0)
        {
          MoonlightStoneTracker moonlightStoneTracker = ((Component) self).GetComponent<MoonlightStoneTracker>();
          if (!Object.op_Implicit((Object) moonlightStoneTracker))
            moonlightStoneTracker = ((Component) self).gameObject.AddComponent<MoonlightStoneTracker>();
          moonlightStoneTracker.timer += Time.deltaTime;
          if ((double) moonlightStoneTracker.timer >= 2.0)
          {
            int num = 0;
            DotController dotController = DotController.FindDotController(((Component) self).gameObject);
            if (Object.op_Implicit((Object) dotController))
            {
              for (int index = dotController.dotStackList.Count - 1; index >= 0 && num < count; --index)
              {
                dotController.RemoveDotStackAtServer(index);
                ++num;
              }
            }
            for (int index = self.activeBuffsList.Length - 1; index >= 0 && num < count; --index)
            {
              BuffDef buffDef = BuffCatalog.GetBuffDef((BuffIndex) (int) self.activeBuffsList[index]);
              if (buffDef.isDebuff && self.GetBuffCount(buffDef) > 0 && buffDef.buffIndex != BuffCatalog.FindBuffIndex("BanditSkull") && buffDef.buffIndex != BuffCatalog.FindBuffIndex("ElementalRingsCooldown"))
              {
                self.RemoveBuff((BuffIndex) (int) self.activeBuffsList[index]);
                ++num;
              }
            }
            moonlightStoneTracker.timer = 0.0f;
          }
        }
      }
      orig.Invoke(self);
    }
  }
}
