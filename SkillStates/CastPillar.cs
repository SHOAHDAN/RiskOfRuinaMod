// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.CastPillar
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
  public class CastPillar : BaseCastChanneledSpellState
  {
    public override void OnEnter()
    {
      this.baseDuration = 0.5f;
      this.baseInterval = 0.0f;
      this.muzzleString = "HandR";
      this.muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
      this.projectilePrefabs.Enqueue(Projectiles.pillarPrefab);
      this.castSoundString = "Play_Binah_Stone_Ready";
      base.OnEnter();
    }

    protected override void Fire()
    {
      if (this.projectilePrefabs.Count <= 0)
        return;
      if (((EntityState) this).isAuthority)
      {
        BlastAttack blastAttack = new BlastAttack()
        {
          attacker = ((EntityState) this).gameObject,
          inflictor = ((EntityState) this).gameObject
        };
        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
        blastAttack.position = this.spellPosition;
        blastAttack.procCoefficient = 1f;
        blastAttack.radius = 12.5f;
        blastAttack.baseForce = 2000f;
        blastAttack.bonusForce = Vector3.zero;
        blastAttack.baseDamage = 5f * ((BaseState) this).damageStat;
        blastAttack.falloffModel = (BlastAttack.FalloffModel) 2;
        blastAttack.damageColorIndex = (DamageColorIndex) 0;
        blastAttack.attackerFiltering = (AttackerFiltering) 2;
        blastAttack.crit = ((BaseState) this).RollCrit();
        blastAttack.damageType = (DamageType) 131072;
        blastAttack.Fire();
      }
      base.Fire();
    }

    protected override void PlayCastAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "Pillar", "Pillar.playbackRate", this.baseDuration);
  }
}
