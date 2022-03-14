// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Misc.ArbiterShockwaveController
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Misc
{
  [RequireComponent(typeof (TeamFilter))]
  public class ArbiterShockwaveController : NetworkBehaviour
  {
    [SyncVar]
    [Tooltip("The area of effect.")]
    public float radius;
    [Tooltip("The buff type to grant")]
    public BuffDef buffDef;
    [Tooltip("The buff duration")]
    public float buffDuration;
    [Tooltip("Barrier amount (based on percentage)")]
    public float barrierAmount;
    [Tooltip("If set, destroys all projectiles in the vicinity.")]
    public bool destroyProjectiles;
    private TeamFilter teamFilter;

    private void Awake() => this.teamFilter = ((Component) this).GetComponent<TeamFilter>();

    private void Start()
    {
      if (NetworkServer.active)
      {
        float radiusSqr = this.radius * this.radius;
        Vector3 position = ((Component) this).transform.position;
        for (TeamIndex teamIndex = (TeamIndex) 0; teamIndex < 4; teamIndex = (TeamIndex) (int) (sbyte) (teamIndex + 1))
        {
          if (teamIndex != this.teamFilter.teamIndex)
            this.HarmTeam((IEnumerable<TeamComponent>) TeamComponent.GetTeamMembers(teamIndex), radiusSqr, position);
        }
        this.HelpTeam((IEnumerable<TeamComponent>) TeamComponent.GetTeamMembers(this.teamFilter.teamIndex), radiusSqr, position);
      }
      if (!this.destroyProjectiles)
        return;
      this.DestroyProjectiles(this.radius * this.radius, ((Component) this).transform.position);
    }

    private void HarmTeam(
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

    private void HelpTeam(
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
          if (Object.op_Implicit((Object) body) && (double) this.barrierAmount != 0.0 && Object.op_Implicit((Object) body.healthComponent))
            body.healthComponent.AddBarrier(this.barrierAmount * body.healthComponent.fullBarrier);
        }
      }
    }

    private void DestroyProjectiles(float radiusSqr, Vector3 currentPosition)
    {
      foreach (Component component1 in Physics.OverlapSphere(currentPosition, radiusSqr, LayerMask.op_Implicit(((LayerIndex) ref LayerIndex.projectile).mask)))
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
