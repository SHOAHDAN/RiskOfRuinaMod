// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGODodge
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGODodge : BaseSkillState
  {
    public Vector3 dodgeVector;
    public float duration = 0.65f;
    public float blinkDuration = 0.3f;
    public float stockBonus = 0.05f;
    public bool aerial = false;
    public bool invul = false;
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
      if (((EntityState) this).skillLocator.utility.stock > 1)
        this.duration = Mathf.Clamp(this.duration - (float) (((EntityState) this).skillLocator.utility.stock - 1) * this.stockBonus, this.blinkDuration, this.duration);
      if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) ((EntityState) this).characterBody.master) && RiskOfRuinaPlugin.KombatIsDueling(((EntityState) this).characterBody.master))
        this.duration += 0.2f;
      this.statTracker = ((EntityState) this).GetComponent<RedMistStatTracker>();
      this.dodgeVector = ((EntityState) this).inputBank.moveVector;
      this.aerial = !((EntityState) this).characterMotor.isGrounded;
      if (Object.op_Implicit((Object) this.characterModel))
        ++this.characterModel.invisibilityCount;
      if (Object.op_Implicit((Object) this.hurtboxGroup))
        ++this.hurtboxGroup.hurtBoxesDeactivatorCounter;
      int num = (int) Util.PlaySound("Play_Claw_Ulti_Move", ((EntityState) this).gameObject);
      ((EntityState) this).PlayAnimation(nameof (EGODodge), nameof (EGODodge), "Dodge.playbackRate", this.blinkDuration);
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
      if ((double) ((EntityState) this).fixedAge <= (double) this.blinkDuration)
      {
        CharacterMotor characterMotor1 = ((EntityState) this).characterMotor;
        characterMotor1.rootMotion = Vector3.op_Addition(characterMotor1.rootMotion, Vector3.op_Multiply(this.dodgeVector, 40f * Time.fixedDeltaTime));
        CharacterMotor characterMotor2 = ((EntityState) this).characterMotor;
        characterMotor2.moveDirection = Vector3.op_Multiply(characterMotor2.moveDirection, 2f);
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

    public virtual void OnExit()
    {
      this.mistEffect.Stop();
      if (Object.op_Implicit((Object) this.characterModel) && this.characterModel.invisibilityCount > 0)
        --this.characterModel.invisibilityCount;
      ((EntityState) this).OnExit();
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;
  }
}
