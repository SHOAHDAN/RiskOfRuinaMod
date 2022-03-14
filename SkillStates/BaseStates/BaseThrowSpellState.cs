// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BaseStates.BaseThrowSpellState
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{
  public abstract class BaseThrowSpellState : BaseSkillState
  {
    public GameObject projectilePrefab;
    public GameObject muzzleflashEffectPrefab;
    public float baseDuration;
    public float minDamageCoefficient;
    public float maxDamageCoefficient;
    public float force;
    public float selfForce;
    private float duration;
    public float charge;
    public string throwSound;

    private ChildLocator childLocator { get; set; }

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.childLocator = ((EntityState) this).GetModelChildLocator();
      this.duration = this.baseDuration / ((BaseState) this).attackSpeedStat;
      ((EntityState) this).PlayAnimation("Gesture, Override", "CastSpell", "Spell.playbackRate", this.duration);
      if (Object.op_Implicit((Object) this.muzzleflashEffectPrefab))
        EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, ((EntityState) this).gameObject, "HandR", false);
      int num = (int) Util.PlaySound(this.throwSound, ((EntityState) this).gameObject);
      this.Fire();
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if (!((EntityState) this).isAuthority || (double) ((EntityState) this).fixedAge < (double) this.duration)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit() => ((EntityState) this).OnExit();

    private void Fire()
    {
      if (!((EntityState) this).isAuthority)
        return;
      Ray aimRay = ((BaseState) this).GetAimRay();
      if (Object.op_Inequality((Object) this.projectilePrefab, (Object) null))
      {
        float num1 = Util.Remap(this.charge, 0.0f, 1f, this.minDamageCoefficient, this.maxDamageCoefficient);
        float num2 = this.charge * this.force;
        FireProjectileInfo fireProjectileInfo = new FireProjectileInfo();
        fireProjectileInfo.projectilePrefab = this.projectilePrefab;
        fireProjectileInfo.position = this.childLocator.FindChild("SpearSummon").position;
        fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(((Ray) ref aimRay).direction);
        fireProjectileInfo.owner = ((EntityState) this).gameObject;
        fireProjectileInfo.damage = ((BaseState) this).damageStat * num1;
        fireProjectileInfo.force = num2;
        fireProjectileInfo.crit = ((BaseState) this).RollCrit();
        ((FireProjectileInfo) ref fireProjectileInfo).speedOverride = 160f;
        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
      }
      if (Object.op_Implicit((Object) ((EntityState) this).characterMotor))
        ((EntityState) this).characterMotor.ApplyForce(Vector3.op_Multiply(((Ray) ref aimRay).direction, -this.selfForce * this.charge), false, false);
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 2;
  }
}
