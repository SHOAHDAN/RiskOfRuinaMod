// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BaseStates.BaseDirectionalSkill
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using AncientScepter;
using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using RoR2.Audio;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{
  public class BaseDirectionalSkill : BaseSkillState
  {
    protected string hitboxName = "Sword";
    protected DamageType damageType = (DamageType) 0;
    protected float damageCoefficient = 1f;
    protected float procCoefficient = 1f;
    protected float pushForce = 300f;
    protected Vector3 bonusForce = Vector3.zero;
    protected float baseDuration = 1f;
    protected float attackStartTime = 0.2f;
    protected float attackEndTime = 0.4f;
    protected float baseEarlyExitTime = 0.4f;
    protected float hitStopDuration = 0.05f;
    protected float attackRecoil = 0.0f;
    protected float swingHopVelocity = 0.0f;
    protected bool cancelled = false;
    public int attackIndex = 1;
    protected string swingSoundString = "";
    protected string hitSoundString = "";
    protected string muzzleString = "SwingCenter";
    protected string attackAnimation = "Swing";
    protected GameObject swingEffectPrefab;
    protected GameObject hitEffectPrefab;
    protected NetworkSoundEventIndex impactSound;
    protected TemporaryOverlay iframeOverlay;
    private float earlyExitTime;
    public float duration;
    protected bool hasFired;
    private float hitPauseTimer;
    private OverlapAttack attack;
    protected bool inHitPause;
    private bool hasHopped;
    protected float stopwatch;
    protected Animator animator;
    private BaseState.HitStopCachedState hitStopCachedState;
    private Vector3 storedVelocity;
    public Vector2 inputVector;
    protected bool inAir;
    protected RedMistEmotionComponent emotionComponent;
    protected RedMistStatTracker statTracker;

    protected float trueMoveSpeed => ((EntityState) this).GetComponent<RedMistStatTracker>().modifiedMoveSpeed;

    protected float trueAttackSpeed => ((EntityState) this).GetComponent<RedMistStatTracker>().modifiedAttackSpeed;

    protected float trueDamage => ((BaseState) this).damageStat;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.emotionComponent = ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      this.statTracker = ((EntityState) this).gameObject.GetComponent<RedMistStatTracker>();
      this.duration = this.baseDuration / this.trueAttackSpeed;
      this.earlyExitTime = this.baseEarlyExitTime / this.trueAttackSpeed;
      this.hasFired = false;
      this.animator = ((EntityState) this).GetModelAnimator();
      ((BaseState) this).StartAimMode(0.5f + this.duration, false);
      ((EntityState) this).characterBody.outOfCombatStopwatch = 0.0f;
      this.animator.SetBool("attacking", true);
      HitBoxGroup hitBoxGroup = (HitBoxGroup) null;
      Transform modelTransform = ((EntityState) this).GetModelTransform();
      if (Object.op_Implicit((Object) modelTransform))
        hitBoxGroup = Array.Find<HitBoxGroup>(((Component) modelTransform).GetComponents<HitBoxGroup>(), (Predicate<HitBoxGroup>) (element => element.groupName == this.hitboxName));
      if (RiskOfRuinaPlugin.ancientScepterInstalled)
        this.AncientScepterSetup();
      this.PlayAttackAnimation();
      this.attack = new OverlapAttack();
      this.attack.damageType = this.damageType;
      this.attack.attacker = ((EntityState) this).gameObject;
      this.attack.inflictor = ((EntityState) this).gameObject;
      this.attack.teamIndex = ((BaseState) this).GetTeam();
      this.attack.damage = this.damageCoefficient * this.trueDamage;
      this.attack.procCoefficient = this.procCoefficient;
      this.attack.hitEffectPrefab = this.hitEffectPrefab;
      this.attack.forceVector = this.bonusForce;
      this.attack.pushAwayForce = this.pushForce;
      this.attack.hitBoxGroup = hitBoxGroup;
      this.attack.isCrit = ((BaseState) this).RollCrit();
      this.attack.impactSound = this.impactSound;
    }

    protected virtual void PlayAttackAnimation() => ((EntityState) this).PlayCrossfade("Gesture, Override", this.attackAnimation, "BaseAttack.playbackRate", this.duration, 0.05f);

    protected virtual void EvaluateInput()
    {
      Vector3 moveVector = ((EntityState) this).inputBank.moveVector;
      Vector3 aimDirection = ((EntityState) this).inputBank.aimDirection;
      Vector3 vector3_1 = new Vector3(aimDirection.x, 0.0f, aimDirection.z);
      Vector3 normalized1 = ((Vector3) ref vector3_1).normalized;
      Vector3 vector3_2 = Vector3.Cross(((EntityState) this).transform.up, normalized1);
      Vector3 normalized2 = ((Vector3) ref vector3_2).normalized;
      this.inputVector = new Vector2(Vector3.Dot(moveVector, normalized1), Vector3.Dot(moveVector, normalized2));
      this.inAir = !((EntityState) this).characterMotor.isGrounded;
    }

    public virtual void OnExit()
    {
      ((EntityState) this).OnExit();
      this.animator.SetBool("attacking", false);
    }

    protected virtual void PlaySwingEffect() => EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, ((EntityState) this).gameObject, this.muzzleString, true);

    protected virtual void OnHitEnemyAuthority()
    {
      if (this.inHitPause || (double) this.hitStopDuration <= 0.0)
        return;
      this.storedVelocity = ((EntityState) this).characterMotor.velocity;
      this.hitStopCachedState = ((BaseState) this).CreateHitStopCachedState(((EntityState) this).characterMotor, this.animator, "BaseAttack.playbackRate");
      this.hitPauseTimer = this.hitStopDuration / this.trueAttackSpeed;
      this.inHitPause = true;
    }

    protected virtual void FireAttack()
    {
      if (!this.hasFired)
      {
        this.hasFired = true;
        int num = (int) Util.PlayAttackSpeedSound(this.swingSoundString, ((EntityState) this).gameObject, this.trueAttackSpeed);
        if (((EntityState) this).isAuthority)
        {
          this.PlaySwingEffect();
          ((BaseState) this).AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
        }
      }
      if (!((EntityState) this).isAuthority)
        return;
      if (!this.hasHopped)
      {
        if (Object.op_Implicit((Object) ((EntityState) this).characterMotor) && !((EntityState) this).characterMotor.isGrounded && (double) this.swingHopVelocity > 0.0)
          ((BaseState) this).SmallHop(((EntityState) this).characterMotor, this.swingHopVelocity);
        this.hasHopped = true;
      }
      if (this.attack.Fire((List<HurtBox>) null))
        this.OnHitEnemyAuthority();
    }

    public virtual void FixedUpdate()
    {
      if (((EntityState) this).inputBank.skill3.down)
      {
        if (this.emotionComponent.inEGO)
        {
          EntityStateMachine entityStateMachine = (EntityStateMachine) null;
          foreach (EntityStateMachine component in ((EntityState) this).gameObject.GetComponents<EntityStateMachine>())
          {
            if (Object.op_Implicit((Object) component) && component.customName == "Slide")
              entityStateMachine = component;
          }
          if (Object.op_Inequality((Object) entityStateMachine, (Object) null) && entityStateMachine.CanInterruptState((InterruptPriority) 2))
          {
            if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalBlock))
            {
              entityStateMachine.SetNextState((EntityState) new EGOBlock());
              this.storedVelocity = ((EntityState) this).characterMotor.velocity;
              this.hitStopCachedState = ((BaseState) this).CreateHitStopCachedState(((EntityState) this).characterMotor, this.animator, "BaseAttack.playbackRate");
              this.hitPauseTimer = 0.35f;
              this.inHitPause = true;
            }
            else if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalDodge))
            {
              entityStateMachine.SetNextState((EntityState) new EGODodge());
              this.storedVelocity = ((EntityState) this).characterMotor.velocity;
              this.hitStopCachedState = ((BaseState) this).CreateHitStopCachedState(((EntityState) this).characterMotor, this.animator, "BaseAttack.playbackRate");
              this.hitPauseTimer = 0.3f;
              this.inHitPause = true;
            }
          }
        }
        else
        {
          if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalBlock))
          {
            ((EntityState) this).outer.SetNextState((EntityState) new Block());
            return;
          }
          if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalDodge))
          {
            ((EntityState) this).outer.SetNextState((EntityState) new Dodge());
            return;
          }
        }
      }
      else if (this.emotionComponent.inEGO && ((EntityState) this).skillLocator.special.stock > 0 && ((EntityState) this).inputBank.skill4.down)
      {
        --((EntityState) this).skillLocator.special.stock;
        EntityStateMachine outer = ((EntityState) this).outer;
        EGOHorizontal egoHorizontal = new EGOHorizontal();
        egoHorizontal.attackIndex = 1;
        egoHorizontal.inputVector = this.inputVector;
        outer.SetNextState((EntityState) egoHorizontal);
      }
      else if (!this.emotionComponent.inEGO && ((EntityState) this).skillLocator.special.CanExecute() && ((EntityState) this).skillLocator.special.stock > 0 && ((EntityState) this).inputBank.skill4.down)
        ((EntityState) this).outer.SetNextState((EntityState) new EGOActivate());
      else if (((EntityState) this).skillLocator.secondary.stock > 0 && ((EntityState) this).skillLocator.secondary.CanExecute() && ((EntityState) this).inputBank.skill2.down)
      {
        --((EntityState) this).skillLocator.secondary.stock;
        ((EntityState) this).outer.SetNextState((EntityState) new Onrush()
        {
          chained = false
        });
      }
      ((EntityState) this).FixedUpdate();
      this.EvaluateInput();
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
          this.animator.SetFloat("BaseAttack.playbackRate", 0.0f);
      }
      if ((double) this.stopwatch >= (double) this.duration * (double) this.attackStartTime && (double) this.stopwatch <= (double) this.duration * (double) this.attackEndTime)
        this.FireAttack();
      if ((double) this.stopwatch >= (double) this.duration - (double) this.earlyExitTime && ((EntityState) this).isAuthority && Object.op_Implicit((Object) ((EntityState) this).inputBank) && (((EntityState) this).inputBank.skill1.down || ((EntityState) this).inputBank.jump.down || Vector2.op_Inequality(this.inputVector, Vector2.zero) || this.emotionComponent.inEGO && ((EntityState) this).skillLocator.special.stock > 0 && ((EntityState) this).inputBank.skill4.down))
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

    protected virtual void SetNextState()
    {
      if (((EntityState) this).inputBank.skill1.down)
      {
        if (this.inAir)
        {
          if ((double) this.inputVector.x < -0.5)
          {
            if (this.emotionComponent.inEGO)
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOAirBackAttack egoAirBackAttack = new EGOAirBackAttack();
              egoAirBackAttack.attackIndex = 1;
              egoAirBackAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoAirBackAttack);
            }
            else
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              AirBackAttack airBackAttack = new AirBackAttack();
              airBackAttack.attackIndex = 1;
              airBackAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) airBackAttack);
            }
          }
          else if (this.emotionComponent.inEGO)
          {
            if (((object) this).GetType() == typeof (EGOAirBasicAttack))
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOAirBasicAttack egoAirBasicAttack = new EGOAirBasicAttack();
              egoAirBasicAttack.attackIndex = this.attackIndex + 1;
              egoAirBasicAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoAirBasicAttack);
            }
            else
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOAirBasicAttack egoAirBasicAttack = new EGOAirBasicAttack();
              egoAirBasicAttack.attackIndex = 1;
              egoAirBasicAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoAirBasicAttack);
            }
          }
          else if (((object) this).GetType() == typeof (AirBasicAttack))
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            AirBasicAttack airBasicAttack = new AirBasicAttack();
            airBasicAttack.attackIndex = this.attackIndex + 1;
            airBasicAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) airBasicAttack);
          }
          else
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            AirBasicAttack airBasicAttack = new AirBasicAttack();
            airBasicAttack.attackIndex = 1;
            airBasicAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) airBasicAttack);
          }
        }
        else if (((EntityState) this).inputBank.jump.down)
        {
          if (this.emotionComponent.inEGO)
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            EGOJumpAttack egoJumpAttack = new EGOJumpAttack();
            egoJumpAttack.attackIndex = 1;
            egoJumpAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) egoJumpAttack);
          }
          else
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            JumpAttack jumpAttack = new JumpAttack();
            jumpAttack.attackIndex = 1;
            jumpAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) jumpAttack);
          }
        }
        else if ((double) this.inputVector.x > 0.5)
        {
          if (this.emotionComponent.inEGO)
          {
            if (((object) this).GetType() == typeof (EGOForwardAttack))
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOForwardAttack egoForwardAttack = new EGOForwardAttack();
              egoForwardAttack.attackIndex = this.attackIndex + 1;
              egoForwardAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoForwardAttack);
            }
            else
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOForwardAttack egoForwardAttack = new EGOForwardAttack();
              egoForwardAttack.attackIndex = 1;
              egoForwardAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoForwardAttack);
            }
          }
          else if (((object) this).GetType() == typeof (ForwardAttack))
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            ForwardAttack forwardAttack = new ForwardAttack();
            forwardAttack.attackIndex = this.attackIndex + 1;
            forwardAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) forwardAttack);
          }
          else
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            ForwardAttack forwardAttack = new ForwardAttack();
            forwardAttack.attackIndex = 1;
            forwardAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) forwardAttack);
          }
        }
        else if ((double) this.inputVector.x < -0.5)
        {
          if (this.emotionComponent.inEGO)
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            EGOBackAttack egoBackAttack = new EGOBackAttack();
            egoBackAttack.attackIndex = 1;
            egoBackAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) egoBackAttack);
          }
          else
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            BackAttack backAttack = new BackAttack();
            backAttack.attackIndex = 1;
            backAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) backAttack);
          }
        }
        else if ((double) this.inputVector.y > 0.5 || (double) this.inputVector.y < -0.5)
        {
          if (this.emotionComponent.inEGO)
          {
            if (((object) this).GetType() == typeof (EGOSideAttack))
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOSideAttack egoSideAttack = new EGOSideAttack();
              egoSideAttack.attackIndex = this.attackIndex + 1;
              egoSideAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoSideAttack);
            }
            else
            {
              EntityStateMachine outer = ((EntityState) this).outer;
              EGOSideAttack egoSideAttack = new EGOSideAttack();
              egoSideAttack.attackIndex = 1;
              egoSideAttack.inputVector = this.inputVector;
              outer.SetNextState((EntityState) egoSideAttack);
            }
          }
          else if (((object) this).GetType() == typeof (SideAttack))
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            SideAttack sideAttack = new SideAttack();
            sideAttack.attackIndex = this.attackIndex + 1;
            sideAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) sideAttack);
          }
          else
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            SideAttack sideAttack = new SideAttack();
            sideAttack.attackIndex = 1;
            sideAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) sideAttack);
          }
        }
        else if (this.emotionComponent.inEGO)
        {
          if (((object) this).GetType() == typeof (EGOBasicAttack))
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            EGOBasicAttack egoBasicAttack = new EGOBasicAttack();
            egoBasicAttack.attackIndex = this.attackIndex + 1;
            egoBasicAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) egoBasicAttack);
          }
          else
          {
            EntityStateMachine outer = ((EntityState) this).outer;
            EGOBasicAttack egoBasicAttack = new EGOBasicAttack();
            egoBasicAttack.attackIndex = 1;
            egoBasicAttack.inputVector = this.inputVector;
            outer.SetNextState((EntityState) egoBasicAttack);
          }
        }
        else if (((object) this).GetType() == typeof (BasicAttack))
        {
          EntityStateMachine outer = ((EntityState) this).outer;
          BasicAttack basicAttack = new BasicAttack();
          basicAttack.attackIndex = this.attackIndex + 1;
          basicAttack.inputVector = this.inputVector;
          outer.SetNextState((EntityState) basicAttack);
        }
        else
        {
          EntityStateMachine outer = ((EntityState) this).outer;
          BasicAttack basicAttack = new BasicAttack();
          basicAttack.attackIndex = 1;
          basicAttack.inputVector = this.inputVector;
          outer.SetNextState((EntityState) basicAttack);
        }
      }
      else if (((EntityState) this).inputBank.jump.down)
      {
        ((EntityState) this).outer.SetNextStateToMain();
      }
      else
      {
        if (!Vector2.op_Inequality(this.inputVector, Vector2.zero))
          return;
        ((EntityState) this).PlayAnimation("FullBody, Override", "BufferEmpty");
        this.animator.SetBool("isMoving", true);
        ((EntityState) this).outer.SetNextStateToMain();
      }
    }

    protected void AddOverlay(float duration)
    {
      if (!Config.iframeOverlay.Value)
        return;
      this.iframeOverlay = ((Component) ((EntityState) this).characterBody).gameObject.AddComponent<TemporaryOverlay>();
      this.iframeOverlay.duration = duration;
      this.iframeOverlay.alphaCurve = AnimationCurve.Constant(0.0f, duration, 0.1f);
      this.iframeOverlay.animateShaderAlpha = true;
      this.iframeOverlay.destroyComponentOnEnd = true;
      this.iframeOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
      this.iframeOverlay.AddToCharacerModel(((Component) ((EntityState) this).modelLocator.modelTransform).GetComponent<CharacterModel>());
    }

    protected void RemoveOverlay()
    {
      if (!Object.op_Implicit((Object) this.iframeOverlay))
        return;
      Object.Destroy((Object) this.iframeOverlay);
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void AncientScepterSetup()
    {
      if (((EntityState) this).characterBody.inventory.GetItemCount(((ItemBase) ItemBase<AncientScepterItem>.instance).ItemDef) <= 0)
        return;
      this.damageType = (DamageType) 524288;
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 1;

    public virtual void OnSerialize(NetworkWriter writer)
    {
      base.OnSerialize(writer);
      writer.Write(this.inputVector);
      writer.Write(this.attackIndex);
    }

    public virtual void OnDeserialize(NetworkReader reader)
    {
      base.OnDeserialize(reader);
      this.inputVector = reader.ReadVector2();
      this.attackIndex = reader.ReadInt32();
    }
  }
}
