// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Misc.ArbiterPillarController
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Misc
{
  [RequireComponent(typeof (TeamFilter))]
  public class ArbiterPillarController : NetworkBehaviour
  {
    [Tooltip("The area of effect.")]
    [SyncVar]
    public float radius;
    [Tooltip("How long between buff pulses in the area of effect.")]
    public float interval = 1f;
    [Tooltip("The child range indicator object. Will be scaled to the radius.")]
    public Transform rangeIndicator;
    [Tooltip("The buff type to grant")]
    public BuffDef buffDef;
    [Tooltip("The buff duration")]
    public float buffDuration;
    [Tooltip("Does the ward disappear over time?")]
    public bool expires;
    [Tooltip("If set, stops all projectiles in the vicinity.")]
    public bool freezeProjectiles;
    public float expireDuration;
    public bool animateRadius;
    public AnimationCurve radiusCoefficientCurve;
    [Tooltip("If set, the ward will give you this amount of time to play removal effects.")]
    public float removalTime;
    private bool needsRemovalTime;
    public string removalSoundString = "";
    public UnityEvent onRemoval;
    private float buffTimer;
    private float rangeIndicatorScaleVelocity;
    private float stopwatch;
    private float calculatedRadius;
    private TeamFilter teamFilter;

    private void Awake() => this.teamFilter = ((Component) this).GetComponent<TeamFilter>();

    private void OnEnable()
    {
      if (!Object.op_Implicit((Object) this.rangeIndicator))
        return;
      ((Component) this.rangeIndicator).gameObject.SetActive(true);
    }

    private void OnDisable()
    {
      if (!Object.op_Implicit((Object) this.rangeIndicator))
        return;
      ((Component) this.rangeIndicator).gameObject.SetActive(false);
    }

    private void Start()
    {
      int num = (int) Util.PlaySound("Play_Binah_Stone_Fire", ((Component) this).gameObject);
      if ((double) this.removalTime > 0.0)
        this.needsRemovalTime = true;
      if (Object.op_Implicit((Object) this.rangeIndicator) && this.expires)
      {
        ScaleParticleSystemDuration component = ((Component) this.rangeIndicator).GetComponent<ScaleParticleSystemDuration>();
        if (Object.op_Implicit((Object) component))
          component.newDuration = this.expireDuration;
      }
      if (!NetworkServer.active)
        return;
      float radiusSqr = this.calculatedRadius * this.calculatedRadius;
      Vector3 position = ((Component) this).transform.position;
      for (TeamIndex teamIndex = (TeamIndex) 0; teamIndex < 4; teamIndex = (TeamIndex) (int) (sbyte) (teamIndex + 1))
        this.BuffTeam((IEnumerable<TeamComponent>) TeamComponent.GetTeamMembers(teamIndex), radiusSqr, position);
    }

    private void Update()
    {
      this.calculatedRadius = this.animateRadius ? this.radius * this.radiusCoefficientCurve.Evaluate(this.stopwatch / this.expireDuration) : this.radius;
      this.stopwatch += Time.deltaTime;
      if (this.expires && NetworkServer.active)
      {
        if (this.needsRemovalTime)
        {
          if ((double) this.stopwatch >= (double) this.expireDuration - (double) this.removalTime)
          {
            this.needsRemovalTime = false;
            int num = (int) Util.PlaySound(this.removalSoundString, ((Component) this).gameObject);
            this.onRemoval.Invoke();
          }
        }
        else if ((double) this.expireDuration <= (double) this.stopwatch)
          Object.Destroy((Object) ((Component) this).gameObject);
      }
      if (!Object.op_Implicit((Object) this.rangeIndicator))
        return;
      float num1 = Mathf.SmoothDamp(this.rangeIndicator.localScale.x, this.calculatedRadius, ref this.rangeIndicatorScaleVelocity, 0.2f);
      this.rangeIndicator.localScale = new Vector3(num1, num1, num1);
    }

    private void FixedUpdate()
    {
      if (NetworkServer.active)
      {
        this.buffTimer -= Time.fixedDeltaTime;
        if ((double) this.buffTimer <= 0.0)
        {
          this.buffTimer = this.interval;
          float radiusSqr = this.calculatedRadius * this.calculatedRadius;
          Vector3 position = ((Component) this).transform.position;
          for (TeamIndex teamIndex = (TeamIndex) 0; teamIndex < 4; teamIndex = (TeamIndex) (int) (sbyte) (teamIndex + 1))
            this.BuffTeam((IEnumerable<TeamComponent>) TeamComponent.GetTeamMembers(teamIndex), radiusSqr, position);
        }
      }
      if (!this.freezeProjectiles)
        return;
      this.FreezeProjectiles(this.calculatedRadius, ((Component) this).transform.position);
    }

    private void BuffTeam(
      IEnumerable<TeamComponent> recipients,
      float radiusSqr,
      Vector3 currentPosition)
    {
      if (!NetworkServer.active)
        return;
      foreach (TeamComponent recipient in recipients)
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) recipient).transform.position, currentPosition);
        if ((double) ((Vector3) ref vector3).sqrMagnitude <= (double) radiusSqr)
        {
          CharacterBody body = recipient.body;
          if (Object.op_Implicit((Object) body) && Object.op_Implicit((Object) this.buffDef))
            body.AddTimedBuff(this.buffDef, this.buffDuration);
        }
      }
    }

    private void FreezeProjectiles(float radius, Vector3 currentPosition)
    {
      foreach (Component component1 in Physics.OverlapSphere(currentPosition, radius, LayerMask.op_Implicit(((LayerIndex) ref LayerIndex.projectile).mask)))
      {
        ProjectileController component2 = component1.GetComponent<ProjectileController>();
        if (Object.op_Implicit((Object) component2))
        {
          TeamComponent component3 = component2.owner.GetComponent<TeamComponent>();
          if (Object.op_Implicit((Object) component3) && component3.teamIndex != this.teamFilter.teamIndex)
          {
            EffectManager.SpawnEffect(Assets.fairyDeleteEffect, new EffectData()
            {
              origin = ((Component) component2).transform.position,
              scale = 4f
            }, false);
            Object.Destroy((Object) ((Component) component2).gameObject);
          }
        }
      }
    }

    public virtual bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
      if (forceAll)
      {
        writer.Write(this.radius);
        return true;
      }
      bool flag = false;
      if ((this.syncVarDirtyBits & 1U) > 0U)
      {
        if (!flag)
        {
          writer.WritePackedUInt32(this.syncVarDirtyBits);
          flag = true;
        }
        writer.Write(this.radius);
      }
      if (!flag)
        writer.WritePackedUInt32(this.syncVarDirtyBits);
      return flag;
    }

    public virtual void OnDeserialize(NetworkReader reader, bool initialState)
    {
      if (initialState)
      {
        this.radius = reader.ReadSingle();
      }
      else
      {
        if ((reader.ReadPackedUInt32() & 1U) <= 0U)
          return;
        this.radius = reader.ReadSingle();
      }
    }

    private void UNetVersion()
    {
    }

    public float Networkradius
    {
      get => this.radius;
      [param: In] set => this.SetSyncVar<float>(value, ref this.radius, 1U);
    }
  }
}
