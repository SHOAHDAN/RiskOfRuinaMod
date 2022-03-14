// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOJumpAttack
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOJumpAttack : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      this.attackIndex = 1;
      this.hitboxName = "EGOJump";
      this.damageCoefficient = 1.5f;
      this.baseDuration = 1f;
      this.attackStartTime = 0.35f;
      this.attackEndTime = 0.5f;
      this.baseEarlyExitTime = 0.5f;
      this.hitStopDuration = 0.05f;
      this.swingSoundString = "Ruina_Swipe";
      this.impactSound = Assets.swordHitEGOSoundVert.index;
      this.muzzleString = "Jump";
      this.hitEffectPrefab = Assets.swordHitEffect;
      this.bonusForce = Vector3.op_Multiply(Vector3.up, 3000f);
      base.OnEnter();
      this.swingEffectPrefab = this.statTracker.EGOSlashPrefab;
    }

    protected override void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOJumpSlash", "BaseAttack.playbackRate", this.duration, 0.1f);

    protected override void PlaySwingEffect() => base.PlaySwingEffect();

    protected override void OnHitEnemyAuthority() => base.OnHitEnemyAuthority();

    protected override void FireAttack()
    {
      base.FireAttack();
      if (!((EntityState) this).inputBank.skill1.down || !((EntityState) this).inputBank.jump.down)
        return;
      ((EntityState) this).outer.SetNextState((EntityState) new EGOJumpRisingAttack()
      {
        attackIndex = this.attackIndex,
        inputVector = this.inputVector
      });
    }

    public override void OnExit() => base.OnExit();
  }
}
