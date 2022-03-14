// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOBlockCounter
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOBlockCounter : BaseSkillState
  {
    public float duration = 0.6f;
    public float blinkDuration = 0.5f;
    public float attackStart = 0.25f;
    public bool invul = false;
    public bool fired = false;
    public float damageCounter = 0.0f;
    public float bonusMult = 1f;
    private Transform modelTransform;
    private CharacterModel characterModel;
    private Animator animator;
    private HurtBoxGroup hurtboxGroup;
    private RedMistStatTracker statTracker;
    private ParticleSystem mistEffect;

    public virtual void OnEnter()
    {
      this.modelTransform = ((EntityState) this).GetModelTransform();
      if (Object.op_Implicit((Object) this.modelTransform))
      {
        this.animator = ((Component) this.modelTransform).GetComponent<Animator>();
        this.characterModel = ((Component) this.modelTransform).GetComponent<CharacterModel>();
        this.hurtboxGroup = ((Component) this.modelTransform).GetComponent<HurtBoxGroup>();
      }
      this.statTracker = ((EntityState) this).GetComponent<RedMistStatTracker>();
      if (Object.op_Implicit((Object) this.characterModel))
        ++this.characterModel.invisibilityCount;
      if (Object.op_Implicit((Object) this.hurtboxGroup))
        ++this.hurtboxGroup.hurtBoxesDeactivatorCounter;
      int num = (int) Util.PlaySound("Play_Claw_Ulti_Move", ((EntityState) this).gameObject);
      ((EntityState) this).PlayAnimation("EGODodge", "EGODodge", "Dodge.playbackRate", this.blinkDuration);
      EffectManager.SpawnEffect(this.statTracker.phaseEffect, new EffectData()
      {
        rotation = Quaternion.identity,
        origin = ((EntityState) this).characterBody.corePosition
      }, true);
      if (Object.op_Implicit((Object) ((EntityState) this).GetModelChildLocator()))
      {
        this.mistEffect = ((EntityState) this).GetComponent<RedMistStatTracker>().mistEffect;
        this.mistEffect.Play();
      }
      this.invul = true;
      ((BaseState) this).OnEnter();
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if ((double) ((EntityState) this).fixedAge >= (double) this.attackStart && !this.fired)
      {
        this.fired = true;
        List<HurtBox> hurtBoxList = new List<HurtBox>();
        SphereSearch sphereSearch = new SphereSearch();
        sphereSearch.mask = ((LayerIndex) ref LayerIndex.entityPrecise).mask;
        sphereSearch.radius = 40f;
        sphereSearch.ClearCandidates();
        sphereSearch.origin = ((EntityState) this).characterBody.corePosition;
        sphereSearch.RefreshCandidates();
        sphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
        TeamMask enemyTeams = TeamMask.GetEnemyTeams(((EntityState) this).teamComponent.teamIndex);
        sphereSearch.FilterCandidatesByHurtBoxTeam(enemyTeams);
        sphereSearch.GetHurtBoxes(hurtBoxList);
        int num = (int) Util.PlaySound("Play_Kali_Special_Vert_Fin", ((EntityState) this).gameObject);
        EffectManager.SpawnEffect(this.statTracker.counterBurst, new EffectData()
        {
          rotation = Quaternion.identity,
          origin = ((EntityState) this).characterBody.footPosition
        }, true);
        foreach (HurtBox target in hurtBoxList)
        {
          if (Object.op_Implicit((Object) target.healthComponent) && Object.op_Implicit((Object) target.healthComponent.body) && Object.op_Inequality((Object) target.healthComponent.body, (Object) ((EntityState) this).characterBody))
            this.DelayedDamage(target);
        }
      }
      if ((double) ((EntityState) this).fixedAge >= (double) this.blinkDuration && this.invul)
      {
        if (Object.op_Implicit((Object) this.characterModel))
          --this.characterModel.invisibilityCount;
        if (Object.op_Implicit((Object) this.hurtboxGroup))
          --this.hurtboxGroup.hurtBoxesDeactivatorCounter;
        EffectManager.SpawnEffect(this.statTracker.phaseEffect, new EffectData()
        {
          rotation = Quaternion.identity,
          origin = ((EntityState) this).characterBody.corePosition
        }, true);
        this.invul = false;
        this.mistEffect.Stop();
      }
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    private void DelayedDamage(HurtBox target)
    {
      if (NetworkServer.active)
      {
        DamageInfo damageInfo = new DamageInfo()
        {
          attacker = ((Component) ((EntityState) this).characterBody).gameObject,
          inflictor = ((Component) ((EntityState) this).characterBody).gameObject,
          crit = ((BaseState) this).RollCrit(),
          damage = (float) ((1.0 + (double) Config.redMistBuffDamage.Value * (double) ((EntityState) this).characterBody.GetBuffCount(Buffs.RedMistBuff)) * ((double) this.damageCounter * 1.5 * (double) this.bonusMult)),
          position = ((Component) target).transform.position,
          force = Vector3.zero,
          damageType = (DamageType) 32,
          damageColorIndex = (DamageColorIndex) 0,
          procCoefficient = 1f
        };
        target.healthComponent.TakeDamage(damageInfo);
        GlobalEventManager.instance.OnHitEnemy(damageInfo, ((Component) target.healthComponent.body).gameObject);
        GlobalEventManager.instance.OnHitAll(damageInfo, ((Component) target.healthComponent.body).gameObject);
      }
      if (!((EntityState) this).isAuthority)
        return;
      EffectManager.SpawnEffect(this.statTracker.afterimageSlash, new EffectData()
      {
        rotation = Random.rotation,
        origin = target.healthComponent.body.corePosition
      }, true);
    }

    public virtual void OnExit()
    {
      this.mistEffect.Stop();
      if (Object.op_Implicit((Object) this.characterModel) && this.characterModel.invisibilityCount > 0)
        --this.characterModel.invisibilityCount;
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 0;
      ((EntityState) this).OnExit();
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;
  }
}
