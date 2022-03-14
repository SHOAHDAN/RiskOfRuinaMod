// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BaseStates.BaseMeleeAttack
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{
  public class BaseMeleeAttack : BaseSkillState
  {
    public int swingIndex;
    protected string hitboxName = "Sword";
    protected DamageType damageType = (DamageType) 0;
    protected float damageCoefficient = 3.5f;
    protected float procCoefficient = 1f;
    protected float pushForce = 300f;
    protected Vector3 bonusForce = Vector3.zero;
    protected float baseDuration = 1f;
    protected float attackStartTime = 0.2f;
    protected float attackEndTime = 0.4f;
    protected float baseEarlyExitTime = 0.4f;
    protected float hitStopDuration = 0.012f;
    protected float attackRecoil = 0.75f;
    protected float hitHopVelocity = 4f;
    protected bool cancelled = false;
    protected string swingSoundString = "";
    protected string hitSoundString = "";
    protected string muzzleString = "SwingCenter";
    protected GameObject swingEffectPrefab;
    protected GameObject hitEffectPrefab;
    protected NetworkSoundEventIndex impactSound;
    private float earlyExitTime;
    public float duration;
    private bool hasFired;
    private float hitPauseTimer;
    protected OverlapAttack attack;
    protected bool inHitPause;
    private bool hasHopped;
    protected float stopwatch;
    protected Animator animator;
    private BaseState.HitStopCachedState hitStopCachedState;
    private Vector3 storedVelocity;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.duration = this.baseDuration / ((BaseState) this).attackSpeedStat;
      this.earlyExitTime = this.baseEarlyExitTime / ((BaseState) this).attackSpeedStat;
      this.hasFired = false;
      this.animator = ((EntityState) this).GetModelAnimator();
      ((BaseState) this).StartAimMode(0.5f + this.duration, false);
      ((EntityState) this).characterBody.outOfCombatStopwatch = 0.0f;
      this.animator.SetBool("attacking", true);
      HitBoxGroup hitBoxGroup = (HitBoxGroup) null;
      Transform modelTransform = ((EntityState) this).GetModelTransform();
      if (Object.op_Implicit((Object) modelTransform))
        hitBoxGroup = Array.Find<HitBoxGroup>(((Component) modelTransform).GetComponents<HitBoxGroup>(), (Predicate<HitBoxGroup>) (element => element.groupName == this.hitboxName));
      this.PlayAttackAnimation();
      this.attack = new OverlapAttack();
      this.attack.damageType = this.damageType;
      this.attack.attacker = ((EntityState) this).gameObject;
      this.attack.inflictor = ((EntityState) this).gameObject;
      this.attack.teamIndex = ((BaseState) this).GetTeam();
      this.attack.damage = this.damageCoefficient * ((BaseState) this).damageStat;
      this.attack.procCoefficient = this.procCoefficient;
      this.attack.hitEffectPrefab = this.hitEffectPrefab;
      this.attack.forceVector = this.bonusForce;
      this.attack.pushAwayForce = this.pushForce;
      this.attack.hitBoxGroup = hitBoxGroup;
      this.attack.isCrit = ((BaseState) this).RollCrit();
      this.attack.impactSound = this.impactSound;
    }

    protected virtual void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("Gesture, Override", "Slash" + (1 + this.swingIndex).ToString(), "Slash.playbackRate", this.duration, 0.05f);

    public virtual void OnExit()
    {
      if (!this.hasFired && !this.cancelled)
        this.FireAttack();
      ((EntityState) this).OnExit();
      this.animator.SetBool("attacking", false);
    }

    protected virtual void PlaySwingEffect() => EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, ((EntityState) this).gameObject, this.muzzleString, true);

    protected virtual void OnHitEnemyAuthority()
    {
      if (!this.hasHopped)
      {
        if (Object.op_Implicit((Object) ((EntityState) this).characterMotor) && !((EntityState) this).characterMotor.isGrounded && (double) this.hitHopVelocity > 0.0)
          ((BaseState) this).SmallHop(((EntityState) this).characterMotor, this.hitHopVelocity);
        this.hasHopped = true;
      }
      if (this.inHitPause || (double) this.hitStopDuration <= 0.0)
        return;
      this.storedVelocity = ((EntityState) this).characterMotor.velocity;
      this.hitStopCachedState = ((BaseState) this).CreateHitStopCachedState(((EntityState) this).characterMotor, this.animator, "Slash.playbackRate");
      this.hitPauseTimer = this.hitStopDuration / ((BaseState) this).attackSpeedStat;
      this.inHitPause = true;
    }

    protected virtual void FireAttack()
    {
      if (!this.hasFired)
      {
        this.hasFired = true;
        int num = (int) Util.PlayAttackSpeedSound(this.swingSoundString, ((EntityState) this).gameObject, ((BaseState) this).attackSpeedStat);
        if (((EntityState) this).isAuthority)
        {
          this.PlaySwingEffect();
          ((BaseState) this).AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
        }
      }
      if (!((EntityState) this).isAuthority || !this.attack.Fire((List<HurtBox>) null))
        return;
      this.OnHitEnemyAuthority();
    }

    protected virtual void SetNextState()
    {
      int num = this.swingIndex != 0 ? 0 : 1;
      ((EntityState) this).outer.SetNextState((EntityState) new BaseMeleeAttack()
      {
        swingIndex = num
      });
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      this.hitPauseTimer -= Time.fixedDeltaTime;
      if ((double) this.hitPauseTimer <= 0.0 && this.inHitPause)
      {
        ((BaseState) this).ConsumeHitStopCachedState(this.hitStopCachedState, ((EntityState) this).characterMotor, this.animator);
        this.inHitPause = false;
        ((EntityState) this).characterMotor.velocity = this.storedVelocity;
      }
      if (!this.inHitPause)
      {
        this.stopwatch += Time.fixedDeltaTime;
      }
      else
      {
        if (Object.op_Implicit((Object) ((EntityState) this).characterMotor))
          ((EntityState) this).characterMotor.velocity = Vector3.zero;
        if (Object.op_Implicit((Object) this.animator))
          this.animator.SetFloat("Swing.playbackRate", 0.0f);
      }
      if ((double) this.stopwatch >= (double) this.duration * (double) this.attackStartTime && (double) this.stopwatch <= (double) this.duration * (double) this.attackEndTime)
        this.FireAttack();
      if ((double) this.stopwatch >= (double) this.duration - (double) this.earlyExitTime && ((EntityState) this).isAuthority && ((EntityState) this).inputBank.skill1.down)
      {
        if (!this.hasFired)
          this.FireAttack();
        this.SetNextState();
      }
      else
      {
        if ((double) this.stopwatch < (double) this.duration || !((EntityState) this).isAuthority)
          return;
        ((EntityState) this).outer.SetNextStateToMain();
      }
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 1;

    public virtual void OnSerialize(NetworkWriter writer)
    {
      base.OnSerialize(writer);
      writer.Write(this.swingIndex);
    }

    public virtual void OnDeserialize(NetworkReader reader)
    {
      base.OnDeserialize(reader);
      this.swingIndex = reader.ReadInt32();
    }
  }
}
