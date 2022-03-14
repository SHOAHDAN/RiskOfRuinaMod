// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.FairyCombo
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  public class FairyCombo : BaseMeleeAttack
  {
    private bool firedProjectile = false;

    public override void OnEnter()
    {
      this.hitboxName = "Fairy";
      this.damageType = (DamageType) 0;
      this.damageCoefficient = 0.75f;
      this.procCoefficient = 1f;
      this.pushForce = 300f;
      this.bonusForce = Vector3.zero;
      this.baseDuration = 1f;
      this.attackStartTime = 0.2f;
      this.attackEndTime = 0.4f;
      this.baseEarlyExitTime = 0.2f;
      this.hitStopDuration = 0.012f;
      this.attackRecoil = 0.5f;
      this.hitHopVelocity = 4f;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = (NetworkSoundEventIndex) -1;
      this.muzzleString = this.swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
      this.swingEffectPrefab = Assets.armSwingEffect;
      this.hitEffectPrefab = Assets.fairyHitEffect;
      base.OnEnter();
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("Gesture, Override", "Swipe" + (1 + this.swingIndex).ToString(), "Swipe.playbackRate", this.duration * 0.6f, 0.05f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack()
    {
      base.FireAttack();
      if (!((EntityState) this).isAuthority || this.firedProjectile)
        return;
      this.firedProjectile = true;
      Ray aimRay = ((BaseState) this).GetAimRay();
      if (Object.op_Inequality((Object) Projectiles.fairyLinePrefab, (Object) null))
      {
        FireProjectileInfo fireProjectileInfo = new FireProjectileInfo();
        fireProjectileInfo.projectilePrefab = Projectiles.fairyLinePrefab;
        fireProjectileInfo.position = ((Ray) ref aimRay).origin;
        fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(((Ray) ref aimRay).direction);
        fireProjectileInfo.owner = ((EntityState) this).gameObject;
        fireProjectileInfo.damage = ((BaseState) this).damageStat * 0.75f;
        fireProjectileInfo.force = 0.0f;
        fireProjectileInfo.crit = ((BaseState) this).RollCrit();
        ((FireProjectileInfo) ref fireProjectileInfo).speedOverride = 150f;
        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
      }
    }

    protected override void SetNextState()
    {
      int num = this.swingIndex + 1;
      if (num > 2)
      {
        ((EntityState) this).outer.SetNextStateToMain();
      }
      else
      {
        EntityStateMachine outer = ((EntityState) this).outer;
        FairyCombo fairyCombo = new FairyCombo();
        fairyCombo.swingIndex = num;
        outer.SetNextState((EntityState) fairyCombo);
      }
    }

    public override void OnExit() => base.OnExit();
  }
}
