// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.ScepterCastShockwave
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  public class ScepterCastShockwave : BaseCastChanneledSpellState
  {
    private Vector3 storedPosition;
    private ShakeEmitter shakeEmitter;

    public override void OnEnter()
    {
      this.baseDuration = 0.75f;
      this.baseInterval = 0.0f;
      this.centered = true;
      this.muzzleString = "HandR";
      this.muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
      this.projectilePrefabs.Enqueue(Projectiles.shockwaveScepterPrefab);
      this.castSoundString = "Play_Abiter_Special_Boom";
      this.storedPosition = ((EntityState) this).transform.position;
      base.OnEnter();
    }

    protected override void PlayCastAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "CastShockwave", "Shockwave.playbackRate", 0.25f);

    protected override void Fire()
    {
      if (this.projectilePrefabs.Count <= 0)
        return;
      this.shakeEmitter = ((EntityState) this).gameObject.AddComponent<ShakeEmitter>();
      this.shakeEmitter.amplitudeTimeDecay = true;
      this.shakeEmitter.duration = 1.5f;
      this.shakeEmitter.radius = 200f;
      this.shakeEmitter.scaleShakeRadiusWithLocalScale = false;
      this.shakeEmitter.wave = new Wave()
      {
        amplitude = 0.2f,
        frequency = 20f,
        cycleOffset = 0.0f
      };
      float num1 = 60f;
      float num2 = 50f;
      if (((EntityState) this).isAuthority)
      {
        BlastAttack blastAttack = new BlastAttack()
        {
          attacker = ((EntityState) this).gameObject,
          inflictor = ((EntityState) this).gameObject
        };
        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
        blastAttack.position = ((EntityState) this).transform.position;
        blastAttack.procCoefficient = 0.5f;
        blastAttack.radius = num1;
        blastAttack.baseForce = 8000f;
        blastAttack.bonusForce = Vector3.zero;
        blastAttack.baseDamage = num2 * ((BaseState) this).damageStat;
        blastAttack.falloffModel = (BlastAttack.FalloffModel) 0;
        blastAttack.damageColorIndex = (DamageColorIndex) 0;
        blastAttack.attackerFiltering = (AttackerFiltering) 2;
        blastAttack.crit = ((BaseState) this).RollCrit();
        blastAttack.damageType = (DamageType) 32;
        blastAttack.Fire();
      }
      base.Fire();
    }

    public override void OnExit()
    {
      base.OnExit();
      ((EntityState) this).PlayAnimation("Gesture, Override", "CastShockwaveEnd", "Shockwave.playbackRate", 0.8f);
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      ((EntityState) this).transform.position = this.storedPosition;
      if (!Object.op_Implicit((Object) ((EntityState) this).characterMotor))
        return;
      ((EntityState) this).characterMotor.velocity = Vector3.zero;
    }
  }
}
