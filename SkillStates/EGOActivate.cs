// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOActivate
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOActivate : BaseSkillState
  {
    public static float baseDuration = 1f;
    private float duration;
    private Vector3 storedPosition;
    private Animator modelAnimator;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.duration = EGOActivate.baseDuration;
      this.modelAnimator = ((EntityState) this).GetModelAnimator();
      ((EntityState) this).characterBody.hideCrosshair = true;
      if (Object.op_Implicit((Object) this.modelAnimator))
      {
        this.modelAnimator.SetBool("isMoving", false);
        this.modelAnimator.SetBool("isSprinting", false);
      }
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      foreach (EntityStateMachine component in ((EntityState) this).gameObject.GetComponents<EntityStateMachine>())
      {
        if (Object.op_Implicit((Object) component))
        {
          if (component.customName == "Weapon")
            component.SetNextStateToMain();
          if (component.customName == "Slide")
            component.SetNextStateToMain();
        }
      }
      ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      ((EntityState) this).PlayAnimation("Gesture, Override", "BufferEmpty");
      ((EntityState) this).PlayAnimation("FullBody, Override", nameof (EGOActivate), "EGOActivate.playbackRate", this.duration);
      int num = (int) Util.PlaySound("Play_Kali_Change", ((EntityState) this).gameObject);
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.EGOActivateCameraParamsRedMist;
      this.storedPosition = ((EntityState) this).transform.position;
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      ((EntityState) this).transform.position = this.storedPosition;
      ((EntityState) this).characterBody.isSprinting = false;
      if (Object.op_Implicit((Object) ((EntityState) this).characterMotor))
        ((EntityState) this).characterMotor.velocity = Vector3.zero;
      if (!((EntityState) this).isAuthority || (double) ((EntityState) this).fixedAge < (double) this.duration)
        return;
      ((EntityState) this).outer.SetNextState((EntityState) new EGOActivateOut());
    }

    public virtual void OnExit() => ((EntityState) this).OnExit();

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;
  }
}
