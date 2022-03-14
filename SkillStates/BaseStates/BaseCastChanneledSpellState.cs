// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BaseStates.BaseCastChanneledSpellState
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{
  public abstract class BaseCastChanneledSpellState : BaseSkillState
  {
    public Queue<GameObject> projectilePrefabs = new Queue<GameObject>();
    public GameObject muzzleflashEffectPrefab;
    public float baseDuration;
    public Vector3 spellPosition;
    public Quaternion spellRotation;
    public string castSoundString;
    public string muzzleString = "SpellCastEffect";
    public float baseInterval;
    public bool centered = false;
    public GenericSkill chainActivatorSkillSlot;
    protected float overrideDuration;
    private float duration;
    private float interval;
    public float charge;
    private float stopwatch = 0.0f;
    private float prevAge = 0.0f;
    private bool valid = true;

    public virtual void OnEnter()
    {
      if (Vector3.op_Equality(this.spellPosition, Vector3.zero) && Quaternion.op_Equality(this.spellRotation, Quaternion.identity))
      {
        this.chainActivatorSkillSlot.AddOneStock();
        ((EntityState) this).outer.SetNextStateToMain();
        this.valid = false;
      }
      else
      {
        ((BaseState) this).OnEnter();
        this.duration = (double) this.overrideDuration != 0.0 ? this.overrideDuration : this.baseDuration / (((BaseState) this).attackSpeedStat / 2f);
        this.interval = this.baseInterval / (((BaseState) this).attackSpeedStat / 2f);
        this.PlayCastAnimation();
        if (Object.op_Implicit((Object) this.muzzleflashEffectPrefab))
          EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, ((EntityState) this).gameObject, this.muzzleString, false);
        if (NetworkServer.active)
          ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.Slow50);
        if (Object.op_Implicit((Object) ((EntityState) this).cameraTargetParams))
          ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 2;
        if (this.muzzleString == "SpellCastEffect")
        {
          ChildLocator modelChildLocator = ((EntityState) this).GetModelChildLocator();
          if (Object.op_Implicit((Object) modelChildLocator))
          {
            GameObject gameObject = ((Component) modelChildLocator.FindChild("SpellCastEffect")).gameObject;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
          }
        }
        this.Fire();
      }
    }

    protected virtual void PlayCastAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "CastSpell", "Spell.playbackRate", this.duration);

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      ((EntityState) this).characterBody.outOfCombatStopwatch = 0.0f;
      this.stopwatch += ((EntityState) this).fixedAge - this.prevAge;
      this.prevAge = ((EntityState) this).fixedAge;
      if ((double) this.stopwatch >= (double) this.interval)
      {
        this.Fire();
        this.stopwatch = 0.0f;
      }
      if (!((EntityState) this).isAuthority || (double) ((EntityState) this).fixedAge < (double) this.duration)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      ((EntityState) this).OnExit();
      if (NetworkServer.active)
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
      if (!Object.op_Implicit((Object) ((EntityState) this).cameraTargetParams))
        return;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 0;
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsArbiter;
    }

    protected virtual void Fire()
    {
      if (this.projectilePrefabs.Count <= 0)
        return;
      if (this.castSoundString != "" && this.valid)
      {
        int num = (int) Util.PlaySound(this.castSoundString, ((EntityState) this).gameObject);
      }
      GameObject gameObject = this.projectilePrefabs.Dequeue();
      if (!Object.op_Implicit((Object) gameObject) || !((EntityState) this).isAuthority)
        return;
      ((BaseState) this).GetAimRay();
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        Vector3 vector3 = this.spellPosition;
        Quaternion quaternion = this.spellRotation;
        if (this.centered)
        {
          vector3 = ((EntityState) this).transform.position;
          quaternion = Quaternion.identity;
        }
        ProjectileManager.instance.FireProjectile(new FireProjectileInfo()
        {
          projectilePrefab = gameObject,
          position = vector3,
          rotation = quaternion,
          owner = ((EntityState) this).gameObject,
          damage = ((BaseState) this).damageStat,
          force = 0.0f,
          crit = ((BaseState) this).RollCrit()
        });
      }
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 2;
  }
}
