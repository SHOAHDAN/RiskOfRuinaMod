// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.Dodge
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class Dodge : BaseSkillState
  {
    public Vector3 dodgeVector;
    public float duration = 0.65f;
    public float moveEnd = 0.65f;
    public float invulStart = 0.0f;
    public float invulEnd = 0.4f;
    public float stockBonus = 0.05f;
    public bool invul = false;
    public bool aerial = false;
    protected TemporaryOverlay iframeOverlay;

    public virtual void OnEnter()
    {
      this.dodgeVector = ((EntityState) this).inputBank.moveVector;
      if (!((EntityState) this).characterMotor.isGrounded)
      {
        this.aerial = true;
        ((BaseState) this).SmallHop(((EntityState) this).characterMotor, 10f);
      }
      if (((EntityState) this).skillLocator.utility.stock > 1)
        this.invulEnd += (float) (((EntityState) this).skillLocator.utility.stock - 1) * this.stockBonus;
      this.AddOverlay(this.invulEnd);
      ((BaseState) this).OnEnter();
      int num = (int) Util.PlaySound("Ruina_Swipe", ((EntityState) this).gameObject);
      ((EntityState) this).PlayCrossfade("FullBody, Override", nameof (Dodge), "Dodge.playbackRate", this.duration, 0.1f);
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if (!this.aerial)
      {
        if ((double) ((EntityState) this).fixedAge <= (double) this.moveEnd)
        {
          CharacterMotor characterMotor1 = ((EntityState) this).characterMotor;
          characterMotor1.rootMotion = Vector3.op_Addition(characterMotor1.rootMotion, Vector3.op_Multiply(this.dodgeVector, 3.5f * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / (this.moveEnd * 1.3f)) * Time.fixedDeltaTime));
          ((EntityState) this).characterMotor.velocity.y = 0.0f;
          CharacterMotor characterMotor2 = ((EntityState) this).characterMotor;
          characterMotor2.moveDirection = Vector3.op_Multiply(characterMotor2.moveDirection, 2f);
        }
      }
      else if ((double) ((EntityState) this).fixedAge <= (double) this.moveEnd)
      {
        CharacterMotor characterMotor3 = ((EntityState) this).characterMotor;
        characterMotor3.rootMotion = Vector3.op_Addition(characterMotor3.rootMotion, Vector3.op_Multiply(this.dodgeVector, 2f * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState) this).fixedAge / (this.moveEnd * 1.3f)) * Time.fixedDeltaTime));
        CharacterMotor characterMotor4 = ((EntityState) this).characterMotor;
        characterMotor4.moveDirection = Vector3.op_Multiply(characterMotor4.moveDirection, 2f);
      }
      if (NetworkServer.active && (double) ((EntityState) this).fixedAge >= (double) this.invulStart && !this.invul)
      {
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.invul = true;
      }
      if (NetworkServer.active && (double) ((EntityState) this).fixedAge >= (double) this.invulEnd && this.invul)
      {
        if (((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
          ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.RemoveOverlay();
        this.invul = false;
      }
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.RemoveOverlay();
      ((EntityState) this).OnExit();
    }

    protected void AddOverlay(float duration)
    {
      if (!Config.iframeOverlay.Value)
        return;
      this.iframeOverlay = ((Component) ((EntityState) this).characterBody).gameObject.AddComponent<TemporaryOverlay>();
      this.iframeOverlay.duration = duration;
      this.iframeOverlay.alphaCurve = AnimationCurve.Constant(0.0f, duration, 0.1f);
      this.iframeOverlay.animateShaderAlpha = true;
      this.iframeOverlay.destroyComponentOnEnd = true;
      this.iframeOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
      this.iframeOverlay.AddToCharacerModel(((Component) ((EntityState) this).modelLocator.modelTransform).GetComponent<CharacterModel>());
    }

    protected void RemoveOverlay()
    {
      if (!Object.op_Implicit((Object) this.iframeOverlay))
        return;
      Object.Destroy((Object) this.iframeOverlay);
    }
  }
}
