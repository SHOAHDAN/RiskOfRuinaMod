// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BasicAttack
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
  internal class BasicAttack : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      if (this.attackIndex > 3)
        this.attackIndex = 1;
      this.hitboxName = "Basic";
      if (this.attackIndex == 3)
        this.hitboxName = "BasicThird";
      this.damageCoefficient = 2f;
      if (this.attackIndex == 3)
        this.damageCoefficient = 3f;
      this.baseDuration = 1.3f;
      this.attackStartTime = 0.2f;
      this.attackEndTime = 0.4f;
      this.baseEarlyExitTime = 0.8f;
      if (this.attackIndex == 3)
        this.baseEarlyExitTime = 0.6f;
      this.hitStopDuration = 0.05f;
      if (this.attackIndex == 3)
        this.pushForce = 600f;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitSoundVert.index;
      if (this.attackIndex == 3)
        this.impactSound = Assets.swordHitSoundHori.index;
      switch (this.attackIndex)
      {
        case 1:
          this.muzzleString = "BasicSwing1";
          break;
        case 2:
          this.muzzleString = "BasicSwing2";
          break;
        case 3:
          this.muzzleString = "BasicSwing3";
          break;
      }
      this.hitEffectPrefab = Assets.swordHitEffect;
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.slashPrefab;
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "BasicSlash" + this.attackIndex.ToString(), "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack()
    {
      base.FireAttack();
      float num = Mathf.Clamp(0.0f, 0.5f, 0.5f * this.trueMoveSpeed);
      CharacterMotor characterMotor = ((EntityState) this).characterMotor;
      characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(((EntityState) this).characterDirection.forward, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / this.duration) * Time.fixedDeltaTime));
    }

    public override void OnExit() => base.OnExit();
  }
}
