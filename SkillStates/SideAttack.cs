// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.SideAttack
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
  internal class SideAttack : BaseDirectionalSkill
  {
    private float direction = 1f;

    public override void OnEnter()
    {
      if (this.attackIndex > 2)
        this.attackIndex = 1;
      if ((double) this.inputVector.y > 0.5)
        this.direction = 1f;
      else if ((double) this.inputVector.y < -0.5)
        this.direction = -1f;
      this.hitboxName = "Side";
      this.damageCoefficient = 1.25f;
      this.baseDuration = 1.4f;
      this.attackStartTime = 0.35f;
      this.attackEndTime = 0.5f;
      this.baseEarlyExitTime = 0.6f;
      this.hitStopDuration = 0.05f;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitSoundHori.index;
      switch (this.attackIndex)
      {
        case 1:
          this.muzzleString = "Side1";
          break;
        case 2:
          this.muzzleString = "Side2";
          break;
      }
      this.hitEffectPrefab = Assets.swordHitEffect;
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.AddOverlay(this.baseDuration * this.attackStartTime);
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.slashPrefab;
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "SideSlash" + this.attackIndex.ToString(), "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      if (this.hasFired || this.inHitPause)
        return;
      Vector3 aimDirection = ((EntityState) this).inputBank.aimDirection;
      aimDirection.y = 0.0f;
      ((Vector3) ref aimDirection).Normalize();
      float num = Mathf.Clamp(0.0f, 2.5f, 0.5f * this.trueMoveSpeed);
      Vector3 vector3 = Vector3.Cross(Vector3.up, aimDirection);
      Vector3 normalized = ((Vector3) ref vector3).normalized;
      CharacterMotor characterMotor = ((EntityState) this).characterMotor;
      characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(this.direction, Vector3.op_Multiply(normalized, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / (this.duration * this.attackEndTime)) * Time.fixedDeltaTime)));
    }

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack()
    {
      if (!this.hasFired && NetworkServer.active)
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.RemoveOverlay();
      base.FireAttack();
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
