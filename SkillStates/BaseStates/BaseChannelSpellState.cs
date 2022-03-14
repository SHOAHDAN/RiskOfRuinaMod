// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BaseStates.BaseChannelSpellState
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Huntress;
using RiskOfRuinaMod.Modules;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{
  public abstract class BaseChannelSpellState : BaseSkillState
  {
    public GameObject chargeEffectPrefab;
    public string chargeSoundString;
    public string startChargeSoundString = "";
    public GameObject crosshairOverridePrefab;
    public float maxSpellRadius;
    public float baseDuration = 3f;
    public Material overrideAreaIndicatorMat;
    public bool zooming = true;
    public bool centered = false;
    public bool line = false;
    private bool hasCharged;
    private GameObject defaultCrosshairPrefab;
    private CharacterCameraParams defaultCameraParams;
    private uint loopSoundInstanceId;

    protected abstract BaseCastChanneledSpellState GetNextState();

    private float duration { get; set; }

    private Animator animator { get; set; }

    private ChildLocator childLocator { get; set; }

    private GameObject chargeEffectInstance { get; set; }

    protected GameObject areaIndicatorInstance { get; set; }

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.duration = this.baseDuration / (((BaseState) this).attackSpeedStat / 2f);
      this.animator = ((EntityState) this).GetModelAnimator();
      this.childLocator = ((EntityState) this).GetModelChildLocator();
      if (Object.op_Implicit((Object) this.childLocator))
      {
        Transform child = this.childLocator.FindChild("HandL");
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
      this.PlayChannelAnimation();
      if (this.startChargeSoundString != "")
      {
        int num = (int) Util.PlaySound(this.startChargeSoundString, ((EntityState) this).gameObject);
      }
      this.loopSoundInstanceId = Util.PlayAttackSpeedSound(this.chargeSoundString, ((EntityState) this).gameObject, ((BaseState) this).attackSpeedStat);
      this.defaultCrosshairPrefab = ((EntityState) this).characterBody.crosshairPrefab;
      if (Object.op_Implicit((Object) this.crosshairOverridePrefab))
        ((EntityState) this).characterBody.crosshairPrefab = this.crosshairOverridePrefab;
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.Slow50);
      if (Object.op_Implicit((Object) ArrowRain.areaIndicatorPrefab))
      {
        this.areaIndicatorInstance = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
        if (this.line)
        {
          GameObject primitive = GameObject.CreatePrimitive((PrimitiveType) 2);
          EntityState.Destroy((Object) primitive.GetComponent<CapsuleCollider>());
          primitive.transform.parent = this.areaIndicatorInstance.transform;
          primitive.transform.localPosition = new Vector3(0.0f, 0.35f, 0.0f);
          primitive.transform.localScale = new Vector3(0.15f, 0.4f, 0.15f);
          ((Renderer) primitive.GetComponent<MeshRenderer>()).material = ((Renderer) this.areaIndicatorInstance.GetComponentInChildren<MeshRenderer>()).material;
        }
        this.areaIndicatorInstance.transform.localScale = Vector3.zero;
        if (Object.op_Implicit((Object) this.overrideAreaIndicatorMat))
          ((Renderer) this.areaIndicatorInstance.GetComponentInChildren<MeshRenderer>()).material = this.overrideAreaIndicatorMat;
      }
      if (!this.zooming)
        return;
      this.defaultCameraParams = ((EntityState) this).cameraTargetParams.cameraParams;
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.channelCameraParamsArbiter;
    }

    protected virtual void PlayChannelAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "ChannelSpell", "Spell.playbackRate", 0.85f);

    private void UpdateAreaIndicator()
    {
      if (!Object.op_Implicit((Object) this.areaIndicatorInstance))
        return;
      if (this.centered)
      {
        this.areaIndicatorInstance.transform.position = ((EntityState) this).transform.position;
        this.areaIndicatorInstance.transform.up = Vector3.up;
      }
      else
      {
        float num1 = 128f;
        Ray aimRay = ((BaseState) this).GetAimRay();
        int num2 = 1 << LayerIndex.world.intVal;
        RaycastHit raycastHit;
        if (Physics.Raycast(aimRay, ref raycastHit, num1, num2))
        {
          if (!this.areaIndicatorInstance.activeSelf)
            this.areaIndicatorInstance.SetActive(true);
          this.areaIndicatorInstance.transform.position = ((RaycastHit) ref raycastHit).point;
          this.areaIndicatorInstance.transform.up = ((RaycastHit) ref raycastHit).normal;
        }
        else
        {
          if (this.areaIndicatorInstance.activeSelf)
            this.areaIndicatorInstance.SetActive(false);
          this.areaIndicatorInstance.transform.position = ((Ray) ref aimRay).GetPoint(num1);
          this.areaIndicatorInstance.transform.up = Vector3.op_UnaryNegation(((Ray) ref aimRay).direction);
        }
      }
    }

    public virtual void OnExit()
    {
      if (Object.op_Implicit((Object) this.crosshairOverridePrefab))
        ((EntityState) this).characterBody.crosshairPrefab = this.defaultCrosshairPrefab;
      else
        ((EntityState) this).characterBody.hideCrosshair = false;
      if (Object.op_Implicit((Object) this.areaIndicatorInstance))
        EntityState.Destroy((Object) this.areaIndicatorInstance.gameObject);
      AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);
      if (!((EntityState) this).outer.destroying)
        this.EndAnimation();
      if (this.zooming)
        ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsArbiter;
      if (NetworkServer.active)
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
      if (Object.op_Implicit((Object) this.chargeEffectInstance))
        EntityState.Destroy((Object) this.chargeEffectInstance);
      ((EntityState) this).OnExit();
    }

    protected virtual void EndAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "BufferEmpty");

    protected float CalcCharge() => Mathf.Clamp01(((EntityState) this).fixedAge / this.duration);

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      ((EntityState) this).characterBody.isSprinting = false;
      ((BaseState) this).StartAimMode(0.5f, false);
      ((EntityState) this).characterBody.outOfCombatStopwatch = 0.0f;
      float num1 = this.CalcCharge();
      if (Object.op_Implicit((Object) this.areaIndicatorInstance))
      {
        float num2 = Util.Remap(num1, 0.0f, 1f, 0.0f, this.maxSpellRadius);
        this.areaIndicatorInstance.transform.localScale = new Vector3(num2, num2, num2);
      }
      if ((double) num1 >= 0.75 && this.zooming)
      {
        ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.channelFullCameraParamsArbiter;
        ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 2;
      }
      if ((double) num1 >= 1.0 && !this.hasCharged)
        this.hasCharged = true;
      if (((EntityState) this).isAuthority && Object.op_Implicit((Object) ((EntityState) this).inputBank) && (double) ((EntityState) this).fixedAge >= 0.200000002980232 && ((EntityState) this).inputBank.sprint.wasDown)
      {
        ((EntityState) this).characterBody.isSprinting = true;
        if (this.zooming)
          ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsArbiter;
        this.RefundCooldown();
        ((EntityState) this).outer.SetNextStateToMain();
      }
      else
      {
        if (!((EntityState) this).isAuthority || this.IsKeyDownAuthority() || (double) num1 < 1.0)
          return;
        BaseCastChanneledSpellState nextState = this.GetNextState();
        if (Object.op_Implicit((Object) this.areaIndicatorInstance))
        {
          if (!this.areaIndicatorInstance.activeSelf)
          {
            nextState.spellPosition = Vector3.zero;
            nextState.spellRotation = Quaternion.identity;
          }
          else
          {
            nextState.spellPosition = this.areaIndicatorInstance.transform.position;
            nextState.spellRotation = this.areaIndicatorInstance.transform.rotation;
          }
        }
        else
        {
          nextState.spellPosition = ((EntityState) this).transform.position;
          nextState.spellRotation = this.areaIndicatorInstance.transform.rotation;
        }
        nextState.chainActivatorSkillSlot = this.activatorSkillSlot;
        ((EntityState) this).outer.SetNextState((EntityState) nextState);
      }
    }

    private void RefundCooldown() => this.activatorSkillSlot.rechargeStopwatch = 0.9f * this.activatorSkillSlot.finalRechargeInterval;

    public virtual void Update()
    {
      ((EntityState) this).Update();
      this.UpdateAreaIndicator();
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 2;
  }
}
