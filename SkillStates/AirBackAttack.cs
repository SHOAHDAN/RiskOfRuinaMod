// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.AirBackAttack
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class AirBackAttack : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      this.attackIndex = 1;
      this.hitboxName = "AirBasic";
      this.damageCoefficient = 2f;
      this.baseDuration = 0.25f;
      this.attackStartTime = 0.5f;
      this.attackEndTime = 0.8f;
      this.baseEarlyExitTime = 0.0f;
      this.hitStopDuration = 0.05f;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitSoundVert.index;
      this.muzzleString = "SwingLeft";
      this.hitEffectPrefab = Assets.swordHitEffect;
      this.bonusForce = Vector3.op_Multiply(Vector3.down, 900f);
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.slashPrefab;
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      ((EntityState) this).characterMotor.velocity.y = 0.0f;
      if ((double) this.stopwatch <= (double) this.duration * (double) this.attackEndTime)
        return;
      ((EntityState) this).outer.SetNextState((EntityState) new AirBackFallingAttack());
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "AirBackSlash", "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack() => base.FireAttack();

    public override void OnExit() => base.OnExit();
  }
}
