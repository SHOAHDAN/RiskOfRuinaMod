// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOForwardAttack
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
  internal class EGOForwardAttack : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      if (this.attackIndex > 4)
        this.attackIndex = 2;
      this.hitboxName = "EGOForward";
      this.damageCoefficient = 1.25f;
      this.damageType = (DamageType) 2;
      this.baseDuration = 1.4f;
      this.attackStartTime = 0.2f;
      this.attackEndTime = 0.4f;
      this.baseEarlyExitTime = 1f;
      this.hitStopDuration = 0.05f;
      if (this.attackIndex >= 2)
      {
        this.baseDuration = 0.9f;
        this.attackStartTime = 0.1f;
        this.attackEndTime = 0.3f;
        this.baseEarlyExitTime = 0.65f;
        this.hitStopDuration = 0.05f;
        this.damageCoefficient = 1f;
      }
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitEGOSoundStab.index;
      switch (this.attackIndex)
      {
        case 1:
          this.muzzleString = "Spear1";
          break;
        case 2:
          this.muzzleString = "Spear1";
          break;
        case 3:
          this.muzzleString = "EGOSpear3";
          break;
        case 4:
          this.muzzleString = "EGOSpear4";
          break;
      }
      this.hitEffectPrefab = Assets.swordHitEffect;
      if (this.attackIndex == 1)
      {
        if (NetworkServer.active)
          ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.AddOverlay(this.baseDuration * this.attackStartTime);
      }
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.EGOPiercePrefab;
    }

    protected override void PlayAttackAnimation()
    {
      if (this.attackIndex >= 2)
        ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOForwardSpear" + this.attackIndex.ToString(), "BaseAttack.playbackRate", this.duration, 0.01f);
      else
        ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOForwardSpear1", "BaseAttack.playbackRate", this.duration, 0.1f);
    }

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack()
    {
      if (!this.hasFired && this.attackIndex == 1)
      {
        if (NetworkServer.active)
          ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.RemoveOverlay();
      }
      base.FireAttack();
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      if (this.hasFired || this.inHitPause)
        return;
      Vector3 aimDirection = ((EntityState) this).inputBank.aimDirection;
      aimDirection.y = 0.0f;
      ((Vector3) ref aimDirection).Normalize();
      if (this.attackIndex >= 2)
      {
        float num = 0.25f;
        CharacterMotor characterMotor = ((EntityState) this).characterMotor;
        characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(aimDirection, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / (this.duration * this.attackEndTime)) * Time.fixedDeltaTime));
      }
      else
      {
        float num = 9f;
        CharacterMotor characterMotor = ((EntityState) this).characterMotor;
        characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(aimDirection, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / (this.duration * this.attackEndTime)) * Time.fixedDeltaTime));
      }
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
