// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.ArbitersTrophy
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using EntityStates;
using On.RoR2;
using R2API;
using RiskOfRuinaMod.Modules.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items
{
  internal class ArbitersTrophy : RuinaItem
  {
    public ItemDef itemDef;
    public float procChance = 1f;
    public float stackChance = 1f;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaArbitersTrophy";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 2;
      this.itemDef.pickupModelPrefab = Assets.arbiterTrophy;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaArbitersTrophy");
      this.itemDef.nameToken = "ARBITERTROPHY_NAME";
      this.itemDef.pickupToken = "ARBITERTROPHY_PICKUP";
      this.itemDef.descriptionToken = "ARBITERTROPHY_DESC";
      this.itemDef.loreToken = "ARBITERTROPHY_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 3 };
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
            if (itemCount > 0 && Util.CheckRoll((this.procChance + this.stackChance * (float) (itemCount - 1)) * damageInfo.procCoefficient, master))
            {
              int buffCount = component2.GetBuffCount(Buffs.lockResistBuff);
              if (buffCount <= 4 && component2.GetBuffCount(Buffs.lockDebuff) == 0)
              {
                if (NetworkServer.active)
                {
                  component2.AddTimedBuff(Buffs.lockDebuff, 5f - (float) buffCount, 1);
                  component2.AddBuff(Buffs.lockResistBuff);
                }
                Transform modelTransform = component2.modelLocator.modelTransform;
                if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) modelTransform))
                {
                  TemporaryOverlay temporaryOverlay = ((Component) component2).gameObject.AddComponent<TemporaryOverlay>();
                  temporaryOverlay.duration = 5f - (float) buffCount;
                  temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.0f);
                  temporaryOverlay.animateShaderAlpha = true;
                  temporaryOverlay.destroyComponentOnEnd = true;
                  temporaryOverlay.originalMaterial = Assets.mainAssetBundle.LoadAsset<Material>("matChains");
                  temporaryOverlay.AddToCharacerModel(((Component) modelTransform).GetComponent<CharacterModel>());
                }
                EntityStateMachine component3 = ((Component) component2).GetComponent<EntityStateMachine>();
                if (Object.op_Inequality((Object) component3, (Object) null))
                {
                  LockState lockState = new LockState()
                  {
                    duration = 5f - (float) buffCount
                  };
                  component3.SetState((EntityState) lockState);
                }
              }
              GameObject gameObject;
              switch (5 - buffCount)
              {
                case 1:
                  gameObject = Assets.lockEffect1s;
                  break;
                case 2:
                  gameObject = Assets.lockEffect2s;
                  break;
                case 3:
                  gameObject = Assets.lockEffect3s;
                  break;
                case 4:
                  gameObject = Assets.lockEffect4s;
                  break;
                case 5:
                  gameObject = Assets.lockEffect5s;
                  break;
                default:
                  gameObject = Assets.lockEffectBreak;
                  break;
              }
              if (Object.op_Implicit((Object) component2.healthComponent) && (double) component2.healthComponent.combinedHealthFraction <= 0.0)
                gameObject = Assets.lockEffectBreak;
              EffectManager.SpawnEffect(gameObject, new EffectData()
              {
                rotation = Util.QuaternionSafeLookRotation(Vector3.zero),
                origin = component2.corePosition
              }, true);
            }
          }
        }
      }
      orig.Invoke(self, damageInfo, victim);
    }
  }
}
