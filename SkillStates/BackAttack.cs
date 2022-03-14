// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BackAttack
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
  internal class BackAttack : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      this.attackIndex = 1;
      this.hitboxName = "Back";
      this.damageCoefficient = 1.75f;
      this.baseDuration = 1f;
      this.attackStartTime = 0.4f;
      this.attackEndTime = 0.6f;
      this.baseEarlyExitTime = 0.2f;
      this.hitStopDuration = 0.05f;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitSoundHori.index;
      this.muzzleString = "BasicSwing3";
      this.hitEffectPrefab = Assets.swordHitEffect;
      if (this.attackIndex == 1)
      {
        if (NetworkServer.active)
          ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.AddOverlay(this.baseDuration * this.attackStartTime);
      }
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.slashPrefab;
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "BackSlash", "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack() => base.FireAttack();

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      if ((double) this.stopwatch <= (double) this.duration * (double) this.attackEndTime && !this.inHitPause)
      {
        Vector3 aimDirection = ((EntityState) this).inputBank.aimDirection;
        aimDirection.y = 0.0f;
        ((Vector3) ref aimDirection).Normalize();
        float num = 5f;
        CharacterMotor characterMotor = ((EntityState) this).characterMotor;
        characterMotor.rootMotion = Vector3.op_Subtraction(characterMotor.rootMotion, Vector3.op_Multiply(aimDirection, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / (this.duration * this.attackEndTime)) * Time.fixedDeltaTime));
      }
      if ((double) this.stopwatch <= (double) this.duration * (double) this.attackStartTime)
        return;
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.RemoveOverlay();
    }

    public override void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.RemoveOverlay();
      base.OnExit();
    }
  }
}
