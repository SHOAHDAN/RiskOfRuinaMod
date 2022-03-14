// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Misc.DetonateFairyOnImpact
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
  [RequireComponent(typeof (ProjectileController))]
  public class DetonateFairyOnImpact : NetworkBehaviour, IProjectileImpactBehavior
  {
    [SyncVar]
    public float radius;
    private ProjectileController controller;

    private void Awake() => this.controller = ((Component) this).GetComponent<ProjectileController>();

    public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
    {
      if (!NetworkServer.active)
        return;
      float radiusSqr = this.radius * this.radius;
      Vector3 position = ((Component) this).transform.position;
      for (TeamIndex teamIndex = (TeamIndex) 0; teamIndex < 4; teamIndex = (TeamIndex) (int) (sbyte) (teamIndex + 1))
      {
        if (teamIndex != this.controller.teamFilter.teamIndex)
          this.FairyBurst((IEnumerable<TeamComponent>) TeamComponent.GetTeamMembers(teamIndex), radiusSqr, position);
      }
    }

    private void FairyBurst(
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
          if (Object.op_Implicit((Object) body))
          {
            DotController dotController = DotController.FindDotController(((Component) body).gameObject);
            if (Object.op_Implicit((Object) dotController) && dotController.HasDotActive(DoTCore.FairyIndex))
            {
              for (int index = 0; index < dotController.dotStackList.Count; ++index)
              {
                DotController.DotStack dotStack = dotController.dotStackList[index];
                if (dotStack.dotIndex == DoTCore.FairyIndex)
                {
                  DamageInfo damageInfo = new DamageInfo()
                  {
                    attacker = dotStack.attackerObject,
                    inflictor = dotStack.attackerObject,
                    crit = dotStack.attackerObject.GetComponent<CharacterBody>().RollCrit(),
                    damage = dotStack.attackerObject.GetComponent<CharacterBody>().damage * 1f,
                    position = body.corePosition,
                    force = Vector3.zero,
                    damageType = (DamageType) 0,
                    damageColorIndex = (DamageColorIndex) 2,
                    dotIndex = DoTCore.FairyIndex,
                    procCoefficient = 0.75f
                  };
                  body.healthComponent.TakeDamage(damageInfo);
                  GlobalEventManager.instance.OnHitEnemy(damageInfo, ((Component) body).gameObject);
                  GlobalEventManager.instance.OnHitAll(damageInfo, ((Component) body).gameObject);
                }
              }
              EffectManager.SpawnEffect(Assets.fairyProcEffect, new EffectData()
              {
                rotation = Util.QuaternionSafeLookRotation(Vector3.zero),
                origin = body.corePosition
              }, false);
            }
          }
        }
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

    public virtual bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
      if (forceAll)
      {
        writer.Write(this.radius);
        return true;
      }
      bool flag = false;
      if (((int) this.syncVarDirtyBits & 1) != 0)
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
        if (((int) reader.ReadPackedUInt32() & 1) == 0)
          return;
        this.radius = reader.ReadSingle();
      }
    }
  }
}
