// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Misc.LockState
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Misc
{
  internal class LockState : BaseState
  {
    public float duration = 5f;
    private List<GameObject> lockedProjectiles;
    private List<LockState.projectileInfo> lockedProjectileInfo;

    public virtual void OnEnter()
    {
      if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) ((EntityState) this).characterBody.master) && RiskOfRuinaPlugin.KombatIsDueling(((EntityState) this).characterBody.master) && (double) this.duration > 3.0)
        this.duration = 3f;
      base.OnEnter();
      this.lockedProjectiles = new List<GameObject>();
      this.lockedProjectileInfo = new List<LockState.projectileInfo>();
      Animator modelAnimator = ((EntityState) this).GetModelAnimator();
      if (Object.op_Implicit((Object) modelAnimator))
        ((Behaviour) modelAnimator).enabled = false;
      if (!Object.op_Implicit((Object) ((EntityState) this).rigidbody) || ((EntityState) this).rigidbody.isKinematic)
        return;
      ((EntityState) this).rigidbody.velocity = Vector3.zero;
      if (Object.op_Implicit((Object) ((EntityState) this).rigidbodyMotor))
        ((EntityState) this).rigidbodyMotor.moveVector = Vector3.zero;
    }

    public virtual void OnExit()
    {
      Animator modelAnimator = ((EntityState) this).GetModelAnimator();
      if (Object.op_Implicit((Object) modelAnimator))
        ((Behaviour) modelAnimator).enabled = true;
      foreach (LockState.projectileInfo projectileInfo in this.lockedProjectileInfo)
      {
        if (Object.op_Implicit((Object) projectileInfo.projectile))
          EntityState.Destroy((Object) projectileInfo.projectile.gameObject);
      }
      ((EntityState) this).OnExit();
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if (Object.op_Implicit((Object) ((EntityState) this).characterMotor))
        ((EntityState) this).characterMotor.velocity = Vector3.zero;
      foreach (Component component1 in Physics.OverlapSphere(((EntityState) this).transform.position, 50f, LayerMask.op_Implicit(((LayerIndex) ref LayerIndex.projectile).mask)))
      {
        ProjectileController component2 = component1.GetComponent<ProjectileController>();
        if (Object.op_Implicit((Object) component2) && Object.op_Implicit((Object) component2.owner) && Object.op_Equality((Object) component2.owner, (Object) ((EntityState) this).gameObject) && !this.lockedProjectiles.Contains(((Component) component2).gameObject))
        {
          this.lockedProjectiles.Add(((Component) component2).gameObject);
          Vector3 vector3_1 = Vector3.zero;
          Vector3 vector3_2 = Vector3.zero;
          Rigidbody component3 = ((Component) component2).GetComponent<Rigidbody>();
          if (Object.op_Implicit((Object) component3) && !component3.isKinematic)
          {
            vector3_1 = component3.velocity;
            if (Object.op_Implicit((Object) ((Component) component2).GetComponent<RigidbodyMotor>()))
              vector3_2 = ((Component) component2).GetComponent<RigidbodyMotor>().moveVector;
          }
          this.lockedProjectileInfo.Add(new LockState.projectileInfo()
          {
            projectile = ((Component) component2).gameObject,
            velocity = vector3_1,
            moveVector = vector3_2,
            position = ((Component) component2).gameObject.transform.position
          });
        }
      }
      foreach (LockState.projectileInfo projectileInfo in this.lockedProjectileInfo)
      {
        if (Object.op_Implicit((Object) projectileInfo.projectile))
        {
          projectileInfo.projectile.transform.position = projectileInfo.position;
          ProjectileController component4 = projectileInfo.projectile.GetComponent<ProjectileController>();
          if (Object.op_Implicit((Object) component4))
          {
            foreach (Collider collider in component4.myColliders)
              collider.enabled = false;
          }
          Rigidbody component5 = projectileInfo.projectile.GetComponent<Rigidbody>();
          if (Object.op_Implicit((Object) component5) && !component5.isKinematic)
          {
            component5.velocity = Vector3.zero;
            if (Object.op_Implicit((Object) projectileInfo.projectile.GetComponent<RigidbodyMotor>()))
              projectileInfo.projectile.GetComponent<RigidbodyMotor>().moveVector = Vector3.zero;
          }
        }
      }
      if ((double) ((EntityState) this).fixedAge < (double) this.duration)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 4;

    private struct projectileInfo
    {
      public GameObject projectile;
      public Vector3 velocity;
      public Vector3 moveVector;
      public Vector3 position;
    }
  }
}
