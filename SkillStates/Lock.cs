// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.Lock
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  public class Lock : BaseSkillState
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

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.tracker = ((EntityState) this).GetComponent<TargetTracker>();
      this.target = this.tracker.GetTrackingTarget();
      if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) this.target.healthComponent) && this.target.healthComponent.alive)
      {
        this.targetIsValid = true;
        int num = (int) Util.PlaySound("Play_Binah_Lock_Ready", ((EntityState) this).gameObject);
      }
      else
      {
        this.activatorSkillSlot.AddOneStock();
        ((EntityState) this).outer.SetNextStateToMain();
      }
      this.duration = Lock.baseDuration / ((BaseState) this).attackSpeedStat;
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
        int buffCount = this.target.GetBuffCount(Buffs.lockResistBuff);
        if (NetworkServer.active)
        {
          DamageInfo damageInfo = new DamageInfo()
          {
            attacker = ((Component) ((EntityState) this).characterBody).gameObject,
            inflictor = ((Component) ((EntityState) this).characterBody).gameObject,
            crit = ((BaseState) this).RollCrit(),
            damage = ((EntityState) this).characterBody.damage * 3f,
            position = this.target.transform.position,
            force = Vector3.zero,
            damageType = (DamageType) 32,
            damageColorIndex = (DamageColorIndex) 0,
            procCoefficient = 1f
          };
          this.target.healthComponent.TakeDamage(damageInfo);
          GlobalEventManager.instance.OnHitEnemy(damageInfo, ((Component) this.target).gameObject);
          GlobalEventManager.instance.OnHitAll(damageInfo, ((Component) this.target).gameObject);
        }
        if (buffCount <= 4 && this.target.GetBuffCount(Buffs.lockDebuff) == 0)
        {
          if (NetworkServer.active)
          {
            this.target.AddTimedBuff(Buffs.lockDebuff, 5f - (float) buffCount, 1);
            this.target.AddBuff(Buffs.lockResistBuff);
          }
          Transform modelTransform = this.target.modelLocator.modelTransform;
          if (Object.op_Implicit((Object) this.target) && Object.op_Implicit((Object) modelTransform))
          {
            TemporaryOverlay temporaryOverlay = ((Component) this.target).gameObject.AddComponent<TemporaryOverlay>();
            temporaryOverlay.duration = 5f - (float) buffCount;
            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.0f);
            temporaryOverlay.animateShaderAlpha = true;
            temporaryOverlay.destroyComponentOnEnd = true;
            temporaryOverlay.originalMaterial = Assets.mainAssetBundle.LoadAsset<Material>("matChains");
            temporaryOverlay.AddToCharacerModel(((Component) modelTransform).GetComponent<CharacterModel>());
          }
          EntityStateMachine component = ((Component) this.target).GetComponent<EntityStateMachine>();
          if (Object.op_Inequality((Object) component, (Object) null))
          {
            LockState lockState = new LockState()
            {
              duration = 5f - (float) buffCount
            };
            component.SetState((EntityState) lockState);
          }
        }
        if (((EntityState) this).isAuthority)
        {
          GameObject gameObject;
          switch (5 - buffCount)
          {
            case 1:
              gameObject = Assets.lockEffect1s;
              break;
            case 2:
              gameObject = Assets.lockEffect2s;
              break;
            case 3:
              gameObject = Assets.lockEffect3s;
              break;
            case 4:
              gameObject = Assets.lockEffect4s;
              break;
            case 5:
              gameObject = Assets.lockEffect5s;
              break;
            default:
              gameObject = Assets.lockEffectBreak;
              break;
          }
          if (Object.op_Implicit((Object) this.target.healthComponent) && (double) this.target.healthComponent.combinedHealthFraction <= 0.0)
            gameObject = Assets.lockEffectBreak;
          EffectManager.SpawnEffect(gameObject, new EffectData()
          {
            rotation = Util.QuaternionSafeLookRotation(Vector3.zero),
            origin = this.target.corePosition
          }, true);
        }
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
