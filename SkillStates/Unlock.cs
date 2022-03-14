// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.Unlock
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  public class Unlock : BaseSkillState
  {
    public static float procCoefficient = 1f;
    public static float baseDuration = 0.6f;
    public static float force = 800f;
    public static float recoil = 3f;
    public static float range = 256f;
    private float duration;
    private float fireTime;
    private bool hasFired;
    private string muzzleString;
    private TargetTracker tracker;
    private CharacterBody target;
    private bool targetIsValid;
    private int stocks = 0;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.tracker = ((EntityState) this).GetComponent<TargetTracker>();
      this.target = this.tracker.GetTrackingTarget();
      this.stocks = this.activatorSkillSlot.stock;
      this.activatorSkillSlot.stock = 0;
      if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) this.target.healthComponent) && this.target.healthComponent.alive)
      {
        this.targetIsValid = true;
        int num = (int) Util.PlaySound("Play_Binah_Lock_Ready", ((EntityState) this).gameObject);
      }
      else
      {
        this.activatorSkillSlot.stock = this.stocks;
        ((EntityState) this).outer.SetNextStateToMain();
      }
      this.duration = Unlock.baseDuration / ((BaseState) this).attackSpeedStat;
      this.fireTime = 0.2f * this.duration;
      ((EntityState) this).characterBody.SetAimTimer(2f);
      this.muzzleString = "HandR";
    }

    public virtual void OnExit() => ((EntityState) this).OnExit();

    private void Fire()
    {
      if (this.hasFired)
        return;
      this.hasFired = true;
      ((EntityState) this).characterBody.AddSpreadBloom(1.5f);
      EffectManager.SimpleMuzzleFlash(FirePistol2.muzzleEffectPrefab, ((EntityState) this).gameObject, this.muzzleString, false);
      if (this.targetIsValid)
      {
        if (NetworkServer.active)
          this.target.AddTimedBuff(Buffs.strengthBuff, 10f * (float) this.stocks);
        Transform modelTransform = this.target.modelLocator.modelTransform;
        if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) modelTransform))
        {
          TemporaryOverlay temporaryOverlay = ((Component) this.target).gameObject.AddComponent<TemporaryOverlay>();
          temporaryOverlay.duration = 10f * (float) this.stocks;
          temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.0f);
          temporaryOverlay.animateShaderAlpha = true;
          temporaryOverlay.destroyComponentOnEnd = true;
          temporaryOverlay.originalMaterial = Assets.mainAssetBundle.LoadAsset<Material>("matChains");
          temporaryOverlay.AddToCharacerModel(((Component) modelTransform).GetComponent<CharacterModel>());
        }
        if (((EntityState) this).isAuthority)
          EffectManager.SpawnEffect(Assets.unlockEffect, new EffectData()
          {
            rotation = Util.QuaternionSafeLookRotation(Vector3.zero),
            origin = this.target.corePosition
          }, true);
      }
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if ((double) ((EntityState) this).fixedAge >= (double) this.fireTime)
        this.Fire();
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 2;
  }
}
