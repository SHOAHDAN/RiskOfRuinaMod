// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.Onrush
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  public class Onrush : BaseSkillState
  {
    public bool chained = false;
    public int chainNum = 0;
    public bool autoAim = false;
    private float startTime;
    private bool hasFired;
    private TargetTracker tracker;
    private RedMistEmotionComponent emotionComponent;
    private CharacterBody target = (CharacterBody) null;
    private NetworkInstanceId targetID;
    private bool targetIsValid = false;
    protected bool inAir;
    private float lungeDistance;
    private float lungeDuration;
    private float lungeStartTime;
    private float cooldownStartTime;
    private float cooldownDuration;
    private bool firstDash = true;
    private bool lunging = false;
    private bool cooldown = false;
    private Vector3 lungeTarget = Vector3.zero;
    private Vector3 dirToLungeTarget = Vector3.zero;
    private ParticleSystem mistEffect;
    private bool dud = false;
    private float originalTurnSpeed;

    protected float trueMoveSpeed => ((EntityState) this).GetComponent<RedMistStatTracker>().modifiedMoveSpeed;

    protected float trueAttackSpeed => ((EntityState) this).GetComponent<RedMistStatTracker>().modifiedAttackSpeed;

    protected float trueDamage => ((BaseState) this).damageStat;

    public virtual void OnEnter()
    {
      this.tracker = ((EntityState) this).GetComponent<TargetTracker>();
      this.emotionComponent = ((EntityState) this).GetComponent<RedMistEmotionComponent>();
      this.originalTurnSpeed = ((EntityState) this).characterDirection.turnSpeed;
      ((EntityState) this).characterDirection.turnSpeed = 0.0f;
      this.mistEffect = ((EntityState) this).GetComponent<RedMistStatTracker>().mistEffect;
      if (this.autoAim)
      {
        List<HurtBox> source = new List<HurtBox>();
        SphereSearch sphereSearch = new SphereSearch();
        sphereSearch.mask = ((LayerIndex) ref LayerIndex.entityPrecise).mask;
        sphereSearch.radius = this.tracker.maxTrackingDistance;
        sphereSearch.ClearCandidates();
        sphereSearch.origin = ((EntityState) this).characterBody.corePosition;
        sphereSearch.RefreshCandidates();
        sphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
        TeamMask enemyTeams = TeamMask.GetEnemyTeams(((EntityState) this).teamComponent.teamIndex);
        ((TeamMask) ref enemyTeams).RemoveTeam((TeamIndex) 0);
        if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive())
          ((TeamMask) ref enemyTeams).AddTeam((TeamIndex) 0);
        sphereSearch.FilterCandidatesByHurtBoxTeam(enemyTeams);
        sphereSearch.OrderCandidatesByDistance();
        sphereSearch.GetHurtBoxes(source);
        List<HurtBox> list = ((IEnumerable<HurtBox>) ((IEnumerable<HurtBox>) source).OrderBy<HurtBox, float>((Func<HurtBox, float>) (o => o.healthComponent.health))).ToList<HurtBox>();
        if (list.Count > 0)
        {
          foreach (HurtBox hurtBox in list)
          {
            if (Object.op_Implicit((Object) hurtBox.healthComponent) && Object.op_Implicit((Object) hurtBox.healthComponent.body) && !Physics.Linecast(((EntityState) this).characterBody.corePosition, hurtBox.healthComponent.body.corePosition, 2048))
            {
              this.target = hurtBox.healthComponent.body;
              break;
            }
          }
          if (Object.op_Equality((Object) this.target, (Object) null))
          {
            this.dud = true;
            if (((EntityState) this).skillLocator.secondary.stock < ((EntityState) this).skillLocator.secondary.maxStock)
              ((EntityState) this).skillLocator.secondary.AddOneStock();
            if (!((EntityState) this).isAuthority)
              return;
            ((EntityState) this).outer.SetNextStateToMain();
            return;
          }
        }
        else
        {
          this.dud = true;
          if (((EntityState) this).skillLocator.secondary.stock < ((EntityState) this).skillLocator.secondary.maxStock)
            ((EntityState) this).skillLocator.secondary.AddOneStock();
          if (!((EntityState) this).isAuthority)
            return;
          ((EntityState) this).outer.SetNextStateToMain();
          return;
        }
      }
      else if (Object.op_Implicit((Object) this.tracker.GetTrackingTarget()))
        this.target = this.tracker.GetTrackingTarget();
      this.lungeDistance = 10f;
      this.lungeDuration = 0.3f;
      this.startTime = 0.4f;
      this.cooldownDuration = 0.8f;
      if (this.chained)
      {
        ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.HorizontalSlashCameraParamsRedMist;
        ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 2;
        this.startTime = 0.0f;
        this.firstDash = false;
        if (this.chainNum > 5)
          this.lungeDuration = 0.2f;
        if (NetworkServer.active)
          ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
        if (this.emotionComponent.inEGO)
        {
          ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOOnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
          this.mistEffect.Play();
        }
        else
          ((EntityState) this).PlayCrossfade("FullBody, Override", "OnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
      }
      if (((EntityState) this).isAuthority)
        this.TargetSetup();
      ((BaseState) this).OnEnter();
    }

    private void TargetSetup()
    {
      if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) this.target.healthComponent) && this.target.healthComponent.alive)
      {
        this.targetID = ((NetworkBehaviour) this.target).netId;
        this.targetIsValid = true;
        if (this.emotionComponent.inEGO)
        {
          if ((double) this.startTime == 0.0)
            return;
          ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOOnrush", "Onrush.playbackRate", this.startTime, 0.1f);
        }
        else if ((double) this.startTime != 0.0)
          ((EntityState) this).PlayCrossfade("FullBody, Override", nameof (Onrush), "Onrush.playbackRate", this.startTime, 0.1f);
      }
      else
      {
        this.dud = true;
        if (((EntityState) this).skillLocator.secondary.stock < ((EntityState) this).skillLocator.secondary.maxStock)
          ((EntityState) this).skillLocator.secondary.AddOneStock();
        if (!((EntityState) this).isAuthority)
          return;
        ((EntityState) this).outer.SetNextStateToMain();
      }
    }

    public virtual void OnExit()
    {
      this.mistEffect.Stop();
      ((EntityState) this).characterDirection.turnSpeed = this.originalTurnSpeed;
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 0;
      ((EntityState) this).OnExit();
    }

    private void Fire()
    {
      if (this.hasFired || !Object.op_Implicit((Object) this.target) || !Object.op_Implicit((Object) this.target.healthComponent))
        return;
      this.hasFired = true;
      if (this.targetIsValid)
      {
        if (Object.op_Implicit((Object) this.target.healthComponent.body) && ((EntityState) this).isAuthority)
          EffectManager.SpawnEffect(((EntityState) this).GetComponent<RedMistStatTracker>().slashPrefab, new EffectData()
          {
            origin = this.target.healthComponent.body.corePosition,
            scale = 1f,
            rotation = Quaternion.LookRotation(this.dirToLungeTarget)
          }, true);
        if (NetworkServer.active)
        {
          float num = this.trueDamage * 4f;
          DamageInfo damageInfo = new DamageInfo()
          {
            attacker = ((Component) ((EntityState) this).characterBody).gameObject,
            inflictor = ((Component) ((EntityState) this).characterBody).gameObject,
            crit = ((BaseState) this).RollCrit(),
            damage = num,
            position = this.target.transform.position,
            force = Vector3.zero,
            damageType = (DamageType) 0,
            damageColorIndex = (DamageColorIndex) 0,
            procCoefficient = 1f
          };
          this.target.healthComponent.TakeDamage(damageInfo);
          GlobalEventManager.instance.OnHitEnemy(damageInfo, ((Component) this.target.healthComponent.body).gameObject);
          GlobalEventManager.instance.OnHitAll(damageInfo, ((Component) this.target.healthComponent.body).gameObject);
        }
      }
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
              ((EntityState) this).outer.SetNextStateToMain();
            }
            else if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalDodge))
            {
              entityStateMachine.SetNextState((EntityState) new EGODodge());
              ((EntityState) this).outer.SetNextStateToMain();
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
        egoHorizontal.inputVector = Vector2.op_Implicit(Vector3.zero);
        outer.SetNextState((EntityState) egoHorizontal);
      }
      else if (!this.emotionComponent.inEGO && ((EntityState) this).skillLocator.special.CanExecute() && ((EntityState) this).skillLocator.special.stock > 0 && ((EntityState) this).inputBank.skill4.down)
        ((EntityState) this).outer.SetNextState((EntityState) new EGOActivate());
      ((EntityState) this).FixedUpdate();
      if (NetworkServer.active && !this.targetIsValid)
      {
        GameObject localObject = NetworkServer.FindLocalObject(this.targetID);
        if (Object.op_Implicit((Object) localObject) && Object.op_Implicit((Object) localObject.GetComponent<CharacterBody>()))
        {
          this.target = localObject.GetComponent<CharacterBody>();
          this.TargetSetup();
        }
      }
      if (!this.targetIsValid)
        return;
      if (this.hasFired)
      {
        if (this.lunging)
          this.Lunge();
        if (this.cooldown)
          this.Cooldown();
      }
      else if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) this.target.healthComponent) && this.target.healthComponent.alive)
      {
        if ((double) ((EntityState) this).fixedAge >= (double) this.startTime)
          this.Dash();
      }
      else
      {
        if (((EntityState) this).skillLocator.secondary.stock < ((EntityState) this).skillLocator.secondary.maxStock)
          ((EntityState) this).skillLocator.secondary.AddOneStock();
        if ((double) ((EntityState) this).fixedAge >= (double) this.startTime && !this.dud)
        {
          this.hasFired = true;
          this.lunging = false;
          this.cooldown = true;
          this.cooldownStartTime = ((EntityState) this).fixedAge;
          this.cooldownDuration = 0.05f;
        }
        else if (((EntityState) this).isAuthority)
          ((EntityState) this).outer.SetNextStateToMain();
      }
    }

    private void Dash()
    {
      if (this.firstDash)
      {
        if (NetworkServer.active)
          ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
        if (this.emotionComponent.inEGO)
        {
          this.mistEffect.Play();
          int num = (int) Util.PlaySound("Play_Effect_Index_Unlock_Short", ((EntityState) this).gameObject);
          ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOOnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
        }
        else
        {
          int num = (int) Util.PlaySound("Ruina_Swipe", ((EntityState) this).gameObject);
          ((EntityState) this).PlayCrossfade("FullBody, Override", "OnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
        }
        this.firstDash = false;
      }
      if (((EntityState) this).inputBank.jump.down && ((EntityState) this).isAuthority)
        ((EntityState) this).outer.SetNextStateToMain();
      Vector3 corePosition1 = this.target.healthComponent.body.corePosition;
      Vector3 vector3_1 = Vector3.op_Subtraction(((EntityState) this).characterBody.corePosition, this.target.healthComponent.body.corePosition);
      Vector3 vector3_2 = Vector3.op_Multiply(((Vector3) ref vector3_1).normalized, this.lungeDistance);
      vector3_1 = Vector3.op_Subtraction(Vector3.op_Addition(corePosition1, vector3_2), ((EntityState) this).characterBody.corePosition);
      Vector3 normalized = ((Vector3) ref vector3_1).normalized;
      float num1 = 10f;
      if (this.emotionComponent.inEGO)
        num1 = 12f;
      if (this.chainNum > 6)
        num1 = 20f;
      else if (this.chainNum >= 3)
        num1 = 16f;
      CharacterMotor characterMotor = ((EntityState) this).characterMotor;
      characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(normalized, this.trueMoveSpeed), num1), Time.fixedDeltaTime));
      ((EntityState) this).characterMotor.velocity = Vector3.zero;
      ((EntityState) this).characterDirection.forward = normalized;
      if ((double) Vector3.Distance(((EntityState) this).characterBody.corePosition, this.target.healthComponent.body.corePosition) > (double) this.lungeDistance)
        return;
      int num2 = (int) Util.PlaySound("Play_Kali_Special_Cut", ((EntityState) this).gameObject);
      if (this.emotionComponent.inEGO)
        ((EntityState) this).PlayCrossfade("FullBody, Override", "EGOOnrushFinish", "Onrush.playbackRate", this.lungeDuration + this.cooldownDuration, 0.1f);
      else
        ((EntityState) this).PlayCrossfade("FullBody, Override", "OnrushFinish", "Onrush.playbackRate", this.lungeDuration + this.cooldownDuration, 0.1f);
      this.lunging = true;
      Vector3 corePosition2 = this.target.healthComponent.body.corePosition;
      vector3_1 = Vector3.op_Subtraction(((EntityState) this).characterBody.corePosition, this.target.healthComponent.body.corePosition);
      Vector3 vector3_3 = Vector3.op_Multiply(((Vector3) ref vector3_1).normalized, this.lungeDistance);
      this.lungeTarget = Vector3.op_Subtraction(corePosition2, vector3_3);
      vector3_1 = Vector3.op_Subtraction(this.lungeTarget, ((EntityState) this).characterBody.corePosition);
      this.dirToLungeTarget = ((Vector3) ref vector3_1).normalized;
      this.lungeStartTime = ((EntityState) this).fixedAge;
      this.Fire();
    }

    private void Lunge()
    {
      if ((double) ((EntityState) this).fixedAge - (double) this.lungeStartTime < (double) this.lungeDuration)
      {
        CharacterMotor characterMotor = ((EntityState) this).characterMotor;
        characterMotor.rootMotion = Vector3.op_Addition(characterMotor.rootMotion, Vector3.op_Multiply(this.dirToLungeTarget, 20f * FlyUpState.speedCoefficientCurve.Evaluate((((EntityState) this).fixedAge - this.lungeStartTime) / this.lungeDuration) * Time.fixedDeltaTime));
        ((EntityState) this).characterMotor.velocity = Vector3.zero;
        ((EntityState) this).characterDirection.forward = this.dirToLungeTarget;
      }
      else
      {
        if (this.targetIsValid)
        {
          if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) this.target.healthComponent))
          {
            if ((double) this.target.healthComponent.combinedHealthFraction <= 0.0 && ((EntityState) this).skillLocator.secondary.stock < ((EntityState) this).skillLocator.secondary.maxStock)
              ((EntityState) this).skillLocator.secondary.AddOneStock();
          }
          else if (((EntityState) this).skillLocator.secondary.stock < ((EntityState) this).skillLocator.secondary.maxStock)
            ((EntityState) this).skillLocator.secondary.AddOneStock();
        }
        if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
          ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.lunging = false;
        this.cooldown = true;
        this.cooldownStartTime = ((EntityState) this).fixedAge;
      }
    }

    private void Cooldown()
    {
      if ((double) ((EntityState) this).fixedAge - (double) this.cooldownStartTime < (double) this.cooldownDuration)
      {
        this.mistEffect.Stop();
        ((EntityState) this).transform.position = this.lungeTarget;
        ((EntityState) this).characterMotor.velocity = Vector3.zero;
        ((EntityState) this).characterDirection.forward = this.dirToLungeTarget;
        if (((EntityState) this).isAuthority && this.emotionComponent.inEGO && ((EntityState) this).skillLocator.secondary.stock > 0)
        {
          --((EntityState) this).skillLocator.secondary.stock;
          ((EntityState) this).outer.SetNextState((EntityState) new Onrush()
          {
            chained = true,
            chainNum = (this.chainNum + 1),
            autoAim = true
          });
        }
        else
        {
          if ((!((EntityState) this).isAuthority || !((EntityState) this).inputBank.jump.down) && !((EntityState) this).inputBank.skill1.down && !((EntityState) this).inputBank.skill2.down)
            return;
          this.SetNextState();
        }
      }
      else
      {
        if (!((EntityState) this).isAuthority)
          return;
        ((EntityState) this).outer.SetNextStateToMain();
      }
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 2;

    protected virtual void SetNextState()
    {
      if (((EntityState) this).inputBank.skill2.down)
      {
        if (((EntityState) this).skillLocator.secondary.stock <= 0)
          return;
        --((EntityState) this).skillLocator.secondary.stock;
        ((EntityState) this).outer.SetNextState((EntityState) new Onrush()
        {
          chained = true
        });
      }
      else if (((EntityState) this).inputBank.jump.down)
      {
        ((EntityState) this).outer.SetNextStateToMain();
        ((BaseState) this).SmallHop(((EntityState) this).characterMotor, 8f);
      }
      else
        ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnSerialize(NetworkWriter writer)
    {
      base.OnSerialize(writer);
      writer.Write(this.chained);
      writer.Write(this.autoAim);
      writer.Write(this.targetID);
    }

    public virtual void OnDeserialize(NetworkReader reader)
    {
      base.OnDeserialize(reader);
      this.chained = reader.ReadBoolean();
      this.autoAim = reader.ReadBoolean();
      this.targetID = reader.ReadNetworkId();
    }
  }
}
