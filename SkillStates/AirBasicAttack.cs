// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.AirBasicAttack
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class AirBasicAttack : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      if (this.attackIndex > 2)
        this.attackIndex = 1;
      this.hitboxName = "AirBasic";
      this.damageCoefficient = 2f;
      this.baseDuration = 1f;
      this.attackStartTime = 0.2f;
      this.attackEndTime = 0.4f;
      this.baseEarlyExitTime = 0.4f;
      this.hitStopDuration = 0.05f;
      this.swingHopVelocity = 8f;
      this.pushForce = 0.0f;
      this.bonusForce = Vector3.zero;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitSoundHori.index;
      switch (this.attackIndex)
      {
        case 1:
          this.muzzleString = "Air1";
          break;
        case 2:
          this.muzzleString = "Air2";
          break;
      }
      this.hitEffectPrefab = Assets.swordHitEffect;
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.slashPrefab;
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "AirBasicSlash" + this.attackIndex.ToString(), "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    public override void FixedUpdate() => base.FixedUpdate();

    protected override void FireAttack()
    {
      base.FireAttack();
      if (!Vector2.op_Inequality(this.inputVector, Vector2.zero) || this.inHitPause)
        return;
      float num = Mathf.Clamp(0.0f, 2f, 0.5f * this.trueMoveSpeed);
      CharacterMotor characterMotor = ((EntityState) this).characterMotor;
      characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(((EntityState) this).inputBank.moveVector, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / this.duration) * Time.fixedDeltaTime));
    }

    public override void OnExit() => base.OnExit();
  }
}
