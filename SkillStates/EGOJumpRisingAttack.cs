// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOJumpRisingAttack
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOJumpRisingAttack : BaseSkillState
  {
    public int attackIndex = 1;
    public Vector2 inputVector;
    public float duration;
    protected string swingSoundString = "";
    protected string hitSoundString = "";
    protected string muzzleString = "SwingCenter";
    protected string attackAnimation = "Swing";
    protected GameObject swingEffectPrefab;
    protected GameObject hitEffectPrefab;

    protected float trueMoveSpeed => ((EntityState) this).GetComponent<RedMistStatTracker>().modifiedMoveSpeed;

    public virtual void OnEnter()
    {
      this.attackIndex = 1;
      this.duration = 0.4f;
      this.swingSoundString = "Ruina_Swipe";
      this.hitSoundString = "Fairy";
      this.muzzleString = "SwingLeft";
      this.swingEffectPrefab = Assets.EGOSwordSwingEffect;
      this.hitEffectPrefab = Assets.swordHitEffect;
      ((BaseState) this).OnEnter();
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.PlayAttackAnimation();
    }

    protected void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOJumpSlashContinue", "BaseAttack.playbackRate", this.duration, 0.1f);

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      float num = Mathf.Clamp(0.0f, 10f, 0.5f * this.trueMoveSpeed);
      CharacterMotor characterMotor1 = ((EntityState) this).characterMotor;
      characterMotor1.rootMotion = Vector3.op_Addition(characterMotor1.rootMotion, Vector3.op_Multiply(Vector3.up, num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / this.duration) * Time.fixedDeltaTime));
      ((EntityState) this).characterMotor.velocity.y = 0.0f;
      CharacterMotor characterMotor2 = ((EntityState) this).characterMotor;
      characterMotor2.moveDirection = Vector3.op_Multiply(characterMotor2.moveDirection, 2f);
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      ((EntityState) this).OnExit();
    }
  }
}
