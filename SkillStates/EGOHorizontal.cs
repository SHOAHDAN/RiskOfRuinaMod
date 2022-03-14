// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOHorizontal
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOHorizontal : BaseDirectionalSkill
  {
    private float hopEndTime = 0.5f;
    private float moveEndtime = 1f;
    private bool aerial = false;
    private float originalTurnSpeed;
    private Vector3 savedAimDir;
    private bool hasAimDir;
    public ShakeEmitter shakeEmitter;

    public override void OnEnter()
    {
      this.attackIndex = 1;
      this.hitboxName = "Horizontal";
      this.damageCoefficient = 20f;
      this.baseDuration = 2.25f;
      this.attackStartTime = 0.45f;
      this.attackEndTime = 0.6f;
      this.baseEarlyExitTime = 0.5f;
      this.hitStopDuration = 0.05f;
      this.swingHopVelocity = 0.0f;
      this.bonusForce = Vector3.op_Addition(Vector3.op_Multiply(((EntityState) this).characterDirection.forward, 4000f), Vector3.op_Multiply(Vector3.up, 2500f));
      this.procCoefficient = 0.75f;
      this.swingSoundString = "Play_Kali_Special_Hori_Start";
      this.impactSound = Assets.swordHitEGOSoundGRHorizontal.index;
      this.muzzleString = "Horizontal";
      this.hitEffectPrefab = Assets.swordHitEffect;
      this.aerial = !((EntityState) this).characterMotor.isGrounded;
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      base.OnEnter();
      this.originalTurnSpeed = ((EntityState) this).characterDirection.turnSpeed;
      this.swingEffectPrefab = this.statTracker.EGOHorizontalPrefab;
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.HorizontalSlashCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 2;
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "HorizontalSlash", "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      if ((double) this.stopwatch <= (double) this.hopEndTime)
      {
        float num = 5f;
        if (this.aerial)
          num = 2f;
        CharacterMotor characterMotor = ((EntityState) this).characterMotor;
        characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(Vector3.up, num * FlyUpState.speedCoefficientCurve.Evaluate(this.stopwatch / this.hopEndTime) * Time.fixedDeltaTime));
      }
      if ((double) this.stopwatch <= (double) this.moveEndtime)
      {
        float num = 5f;
        CharacterMotor characterMotor1 = ((EntityState) this).characterMotor;
        characterMotor1.rootMotion = Vector3.op_Addition(characterMotor1.rootMotion, Vector3.op_Multiply(((EntityState) this).inputBank.moveVector, num * FlyUpState.speedCoefficientCurve.Evaluate(this.stopwatch / this.moveEndtime) * Time.fixedDeltaTime));
        CharacterMotor characterMotor2 = ((EntityState) this).characterMotor;
        characterMotor2.moveDirection = Vector3.op_Multiply(characterMotor2.moveDirection, 2f);
        ((EntityState) this).characterDirection.turnSpeed = 0.0f;
        ((EntityState) this).characterDirection.forward = ((EntityState) this).inputBank.aimDirection;
      }
      if ((double) this.stopwatch > (double) this.moveEndtime)
      {
        if (this.hasAimDir)
        {
          ((EntityState) this).characterDirection.forward = this.savedAimDir;
        }
        else
        {
          this.savedAimDir = ((EntityState) this).inputBank.aimDirection;
          this.hasAimDir = true;
        }
      }
      ((EntityState) this).characterMotor.velocity.y = 0.0f;
    }

    protected override void FireAttack()
    {
      base.FireAttack();
      this.shakeEmitter = ((EntityState) this).gameObject.AddComponent<ShakeEmitter>();
      this.shakeEmitter.amplitudeTimeDecay = true;
      this.shakeEmitter.duration = 0.3f;
      this.shakeEmitter.radius = 100f;
      this.shakeEmitter.scaleShakeRadiusWithLocalScale = false;
      this.shakeEmitter.wave = new Wave()
      {
        amplitude = 0.6f,
        frequency = 25f,
        cycleOffset = 0.0f
      };
    }

    public override void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 0;
      ((EntityState) this).characterDirection.turnSpeed = this.originalTurnSpeed;
      base.OnExit();
    }

    public override InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;
  }
}
