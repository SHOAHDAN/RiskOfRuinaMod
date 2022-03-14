// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.Reverberation
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using On.RoR2;
using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Items
{
  internal class Reverberation : RuinaItem
  {
    public ItemDef itemDef;

    internal override ConfigEntry<bool> itemEnabled { get; set; }

    internal override string itemName { get; set; } = "RuinaReverberation";

    public override void ItemSetup()
    {
      this.itemDef = ScriptableObject.CreateInstance<ItemDef>();
      ((Object) this.itemDef).name = this.itemName;
      this.itemDef.tier = (ItemTier) 2;
      this.itemDef.pickupModelPrefab = Assets.reverberation;
      this.itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaReverberation");
      this.itemDef.nameToken = this.itemName.ToUpper() + "_NAME";
      this.itemDef.pickupToken = this.itemName.ToUpper() + "_PICKUP";
      this.itemDef.descriptionToken = this.itemName.ToUpper() + "_DESC";
      this.itemDef.loreToken = this.itemName.ToUpper() + "_LORE";
      this.itemDef.tags = new ItemTag[1]{ (ItemTag) 3 };
      ItemAPI.Add(new CustomItem(this.itemDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup()
    {
      // ISSUE: method pointer
      OverlapAttack.Fire += new OverlapAttack.hook_Fire((object) this, __methodptr(OverlapAttack_Fire));
      // ISSUE: method pointer
      BulletAttack.Fire += new BulletAttack.hook_Fire((object) this, __methodptr(BulletAttack_Fire));
      // ISSUE: method pointer
      BlastAttack.Fire += new BlastAttack.hook_Fire((object) this, __methodptr(BlastAttack_Fire));
    }

    private bool OverlapAttack_Fire(
      OverlapAttack.orig_Fire orig,
      OverlapAttack self,
      List<HurtBox> hitResults)
    {
      GameObject attacker = self.attacker;
      if (Object.op_Implicit((Object) attacker))
      {
        CharacterBody component1 = attacker.GetComponent<CharacterBody>();
        if (Object.op_Implicit((Object) component1) && Object.op_Implicit((Object) component1.master) && Object.op_Implicit((Object) component1.inventory))
        {
          int itemCount = component1.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName));
          if (itemCount > 0)
          {
            Collider[] colliderArray = Physics.OverlapBox(((Component) self.hitBoxGroup.hitBoxes[0]).transform.position, Vector3.op_Multiply(Vector3.op_Division(((Component) self.hitBoxGroup.hitBoxes[0]).transform.localScale, 2f), (float) (1.0 + 0.25 * ((double) itemCount - 1.0))), ((Component) self.hitBoxGroup.hitBoxes[0]).transform.rotation, LayerMask.op_Implicit(((LayerIndex) ref LayerIndex.projectile).mask));
            for (int index = 0; index < colliderArray.Length; ++index)
            {
              ProjectileController component2 = ((Component) colliderArray[index]).GetComponent<ProjectileController>();
              ProjectileDamage component3 = ((Component) colliderArray[index]).GetComponent<ProjectileDamage>();
              if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) component3))
              {
                TeamComponent component4 = component2.owner.GetComponent<TeamComponent>();
                if (Object.op_Implicit((Object) component4) && component4.teamIndex != component1.teamComponent.teamIndex)
                {
                  EffectManager.SpawnEffect(Assets.blockEffect, new EffectData()
                  {
                    origin = ((Component) component2).transform.position
                  }, false);
                  FireProjectileInfo fireProjectileInfo = new FireProjectileInfo();
                  fireProjectileInfo.projectilePrefab = ((Component) component2).gameObject;
                  fireProjectileInfo.position = ((Component) component2).transform.position;
                  ref FireProjectileInfo local = ref fireProjectileInfo;
                  Ray aimRay = component1.inputBank.GetAimRay();
                  Quaternion quaternion = Util.QuaternionSafeLookRotation(((Ray) ref aimRay).direction);
                  local.rotation = quaternion;
                  fireProjectileInfo.owner = attacker;
                  fireProjectileInfo.damage = component3.damage + self.damage;
                  fireProjectileInfo.force = component3.force;
                  fireProjectileInfo.crit = component3.crit;
                  fireProjectileInfo.target = component2.owner;
                  ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                  int num = (int) Util.PlaySound("Play_Defense_Guard", attacker);
                  Object.Destroy((Object) ((Component) component2).gameObject);
                }
              }
            }
          }
        }
      }
      return orig.Invoke(self, hitResults);
    }

    private void BulletAttack_Fire(BulletAttack.orig_Fire orig, BulletAttack self)
    {
      GameObject owner = self.owner;
      if (Object.op_Implicit((Object) owner))
      {
        CharacterBody component1 = owner.GetComponent<CharacterBody>();
        if (Object.op_Implicit((Object) component1) && Object.op_Implicit((Object) component1.master) && Object.op_Implicit((Object) component1.inventory))
        {
          int itemCount = component1.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName));
          RaycastHit raycastHit;
          if (itemCount > 0 && Physics.SphereCast(new Ray(self.origin, self.aimVector), (float) (0.75 * (1.0 + 0.25 * ((double) itemCount - 1.0))), ref raycastHit, self.maxDistance, LayerMask.op_Implicit(((LayerIndex) ref LayerIndex.projectile).mask)))
          {
            ProjectileController component2 = ((Component) ((RaycastHit) ref raycastHit).collider).GetComponent<ProjectileController>();
            ProjectileDamage component3 = ((Component) ((RaycastHit) ref raycastHit).collider).GetComponent<ProjectileDamage>();
            if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) component3))
            {
              TeamComponent component4 = component2.owner.GetComponent<TeamComponent>();
              if (Object.op_Implicit((Object) component4) && component4.teamIndex != component1.teamComponent.teamIndex)
              {
                EffectManager.SpawnEffect(Assets.blockEffect, new EffectData()
                {
                  origin = ((Component) component2).transform.position
                }, false);
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo()
                {
                  projectilePrefab = ((Component) component2).gameObject,
                  position = ((Component) component2).transform.position,
                  rotation = Util.QuaternionSafeLookRotation(self.aimVector),
                  owner = owner,
                  damage = component3.damage + self.damage,
                  force = component3.force,
                  crit = component3.crit
                });
                int num = (int) Util.PlaySound("Play_Defense_Guard", owner);
                Object.Destroy((Object) ((Component) component2).gameObject);
              }
            }
          }
        }
      }
      orig.Invoke(self);
    }

    private BlastAttack.Result BlastAttack_Fire(
      BlastAttack.orig_Fire orig,
      BlastAttack self)
    {
      GameObject attacker = self.attacker;
      if (Object.op_Implicit((Object) attacker))
      {
        CharacterBody component1 = attacker.GetComponent<CharacterBody>();
        if (Object.op_Implicit((Object) component1) && Object.op_Implicit((Object) component1.master) && Object.op_Implicit((Object) component1.inventory))
        {
          int itemCount = component1.inventory.GetItemCount(ItemCatalog.FindItemIndex(this.itemName));
          if (itemCount > 0)
          {
            Collider[] colliderArray = Physics.OverlapSphere(self.position, self.radius * (float) (1.0 + 0.25 * ((double) itemCount - 1.0)), LayerMask.op_Implicit(((LayerIndex) ref LayerIndex.projectile).mask));
            for (int index = 0; index < colliderArray.Length; ++index)
            {
              ProjectileController component2 = ((Component) colliderArray[index]).GetComponent<ProjectileController>();
              ProjectileDamage component3 = ((Component) colliderArray[index]).GetComponent<ProjectileDamage>();
              if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) component3))
              {
                TeamComponent component4 = component2.owner.GetComponent<TeamComponent>();
                if (Object.op_Implicit((Object) component4) && component4.teamIndex != component1.teamComponent.teamIndex)
                {
                  EffectManager.SpawnEffect(Assets.blockEffect, new EffectData()
                  {
                    origin = ((Component) component2).transform.position
                  }, false);
                  FireProjectileInfo fireProjectileInfo = new FireProjectileInfo();
                  fireProjectileInfo.projectilePrefab = ((Component) component2).gameObject;
                  fireProjectileInfo.position = ((Component) component2).transform.position;
                  ref FireProjectileInfo local = ref fireProjectileInfo;
                  Vector3 vector3 = Vector3.op_Subtraction(((Component) component2).transform.position, self.position);
                  Quaternion quaternion = Util.QuaternionSafeLookRotation(((Vector3) ref vector3).normalized);
                  local.rotation = quaternion;
                  fireProjectileInfo.owner = attacker;
                  fireProjectileInfo.damage = component3.damage + self.baseDamage;
                  fireProjectileInfo.force = component3.force;
                  fireProjectileInfo.crit = component3.crit;
                  ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                  int num = (int) Util.PlaySound("Play_Defense_Guard", attacker);
                  Object.Destroy((Object) ((Component) component2).gameObject);
                }
              }
            }
          }
        }
      }
      return orig.Invoke(self);
    }
  }
}
