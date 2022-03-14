// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.AirBackFallingAttack
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class AirBackFallingAttack : BaseSkillState
  {
    private float duration = 10f;
    private float cooldown = 0.4f;
    private float landTime = 0.0f;
    private bool landed = false;
    public ShakeEmitter shakeEmitter;
    private float startY = 0.0f;

    protected float trueDamage => ((BaseState) this).damageStat;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      CharacterBody characterBody = ((EntityState) this).characterBody;
      characterBody.bodyFlags = (CharacterBody.BodyFlags) (characterBody.bodyFlags | 1);
      if (((EntityState) this).isAuthority)
      {
        ((EntityState) this).characterMotor.velocity.y = -60f;
        ((EntityState) this).characterMotor.rootMotion.y -= 0.5f;
        ((EntityState) this).characterMotor.velocity.x = 0.0f;
        ((EntityState) this).characterMotor.velocity.z = 0.0f;
      }
      this.startY = ((EntityState) this).characterBody.corePosition.y;
      ((EntityState) this).PlayCrossfade("FullBody, Override", "AirBackSlashContinue", "BaseAttack.playbackRate", this.duration, 0.1f);
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if (this.landed)
      {
        if ((double) ((EntityState) this).fixedAge < (double) this.landTime + (double) this.cooldown || !((EntityState) this).isAuthority)
          return;
        ((EntityState) this).outer.SetNextStateToMain();
      }
      else
      {
        if (((EntityState) this).isAuthority && (double) ((EntityState) this).characterMotor.velocity.y > -100.0)
          ((EntityState) this).characterMotor.velocity.y = -100f;
        if ((double) ((EntityState) this).fixedAge >= (double) this.duration || ((EntityState) this).characterMotor.isGrounded)
        {
          int num1 = (int) Util.PlaySound("Play_Kali_Special_Vert_Fin", ((EntityState) this).gameObject);
          ((EntityState) this).PlayCrossfade("FullBody, Override", "AirBackSlashFinish", "BaseAttack.playbackRate", this.cooldown, 0.1f);
          this.landed = true;
          this.landTime = ((EntityState) this).fixedAge;
          if (((EntityState) this).isAuthority)
          {
            this.shakeEmitter = ((EntityState) this).gameObject.AddComponent<ShakeEmitter>();
            this.shakeEmitter.amplitudeTimeDecay = true;
            this.shakeEmitter.duration = 0.2f;
            this.shakeEmitter.radius = 80f;
            this.shakeEmitter.scaleShakeRadiusWithLocalScale = false;
            this.shakeEmitter.wave = new Wave()
            {
              amplitude = 0.8f,
              frequency = 30f,
              cycleOffset = 0.0f
            };
            float num2 = 8f;
            ((BaseState) this).AddRecoil(-0.4f * num2, -0.8f * num2, -0.3f * num2, 0.3f * num2);
            CharacterMotor characterMotor = ((EntityState) this).characterMotor;
            characterMotor.velocity = Vector3.op_Multiply(characterMotor.velocity, 0.1f);
            CharacterBody characterBody = ((EntityState) this).characterBody;
            characterBody.bodyFlags = (CharacterBody.BodyFlags) (characterBody.bodyFlags - 1);
            EffectManager.SpawnEffect(((EntityState) this).GetComponent<RedMistStatTracker>().groundPoundEffect, new EffectData()
            {
              origin = ((EntityState) this).characterBody.footPosition,
              scale = 1f
            }, true);
            float num3 = Mathf.Clamp((this.startY - ((EntityState) this).characterBody.corePosition.y) / 10f, 1f, 10f);
            Vector3 footPosition = ((EntityState) this).characterBody.footPosition;
            new BlastAttack()
            {
              radius = (10f + num3),
              procCoefficient = 0.8f,
              position = footPosition,
              attacker = ((EntityState) this).gameObject,
              teamIndex = ((EntityState) this).teamComponent.teamIndex,
              crit = ((BaseState) this).RollCrit(),
              baseDamage = (this.trueDamage * 2f * num3),
              damageColorIndex = ((DamageColorIndex) 0),
              falloffModel = ((BlastAttack.FalloffModel) 0),
              attackerFiltering = ((AttackerFiltering) 2),
              damageType = ((DamageType) 0)
            }.Fire();
            ((EntityState) this).characterMotor.velocity.y = 0.0f;
          }
        }
      }
    }

    public virtual void OnExit()
    {
      ((EntityState) this).OnExit();
      Object.Destroy((Object) this.shakeEmitter);
    }
  }
}
