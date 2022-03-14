// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BaseStates.BaseChargeSpellState
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{
  public abstract class BaseChargeSpellState : BaseSkillState
  {
    public GameObject chargeEffectPrefab;
    public string chargeSoundString;
    public float baseDuration = 1.5f;
    public float minBloomRadius;
    public float maxBloomRadius;
    public GameObject crosshairOverridePrefab;
    protected static readonly float minChargeDuration = 0.5f;
    private GameObject defaultCrosshairPrefab;
    private uint loopSoundInstanceId;

    protected abstract BaseThrowSpellState GetNextState();

    private float duration { get; set; }

    private Animator animator { get; set; }

    private ChildLocator childLocator { get; set; }

    private GameObject chargeEffectInstance { get; set; }

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.duration = this.baseDuration / ((BaseState) this).attackSpeedStat;
      this.animator = ((EntityState) this).GetModelAnimator();
      this.childLocator = ((EntityState) this).GetModelChildLocator();
      if (Object.op_Implicit((Object) this.childLocator))
      {
        Transform child = this.childLocator.FindChild("HandR");
        if (Object.op_Implicit((Object) child) && Object.op_Implicit((Object) this.chargeEffectPrefab))
        {
          this.chargeEffectInstance = Object.Instantiate<GameObject>(this.chargeEffectPrefab, child.position, child.rotation);
          this.chargeEffectInstance.transform.parent = child;
          ScaleParticleSystemDuration component1 = this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
          ObjectScaleCurve component2 = this.chargeEffectInstance.GetComponent<ObjectScaleCurve>();
          if (Object.op_Implicit((Object) component1))
            component1.newDuration = this.duration;
          if (Object.op_Implicit((Object) component2))
            component2.timeMax = this.duration;
        }
      }
      ((EntityState) this).PlayAnimation("Gesture, Override", "ChannelSpell", "Spell.playbackRate", 0.4f * this.duration);
      this.loopSoundInstanceId = Util.PlayAttackSpeedSound(this.chargeSoundString, ((EntityState) this).gameObject, ((BaseState) this).attackSpeedStat);
      this.defaultCrosshairPrefab = ((EntityState) this).characterBody.crosshairPrefab;
      if (Object.op_Implicit((Object) this.crosshairOverridePrefab))
        ((EntityState) this).characterBody.crosshairPrefab = this.crosshairOverridePrefab;
      ((BaseState) this).StartAimMode(this.duration + 2f, false);
    }

    public virtual void OnExit()
    {
      if (Object.op_Implicit((Object) ((EntityState) this).characterBody))
        ((EntityState) this).characterBody.crosshairPrefab = this.defaultCrosshairPrefab;
      AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);
      if (!((EntityState) this).outer.destroying)
        ((EntityState) this).PlayAnimation("Gesture, Override", "BufferEmpty");
      EntityState.Destroy((Object) this.chargeEffectInstance);
      ((EntityState) this).OnExit();
    }

    protected float CalcCharge() => Mathf.Clamp01(((EntityState) this).fixedAge / this.duration);

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      float num = this.CalcCharge();
      if (!((EntityState) this).isAuthority || (this.IsKeyDownAuthority() || (double) ((EntityState) this).fixedAge < (double) BaseChargeSpellState.minChargeDuration) && (double) ((EntityState) this).fixedAge < (double) this.duration)
        return;
      BaseThrowSpellState nextState = this.GetNextState();
      nextState.charge = num;
      ((EntityState) this).outer.SetNextState((EntityState) nextState);
    }

    public virtual void Update()
    {
      ((EntityState) this).Update();
      ((EntityState) this).characterBody.SetSpreadBloom(Util.Remap(this.CalcCharge(), 0.0f, 1f, this.minBloomRadius, this.maxBloomRadius), true);
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 2;
  }
}
