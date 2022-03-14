// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.ThrowPillarSpear
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates.Commando.CommandoWeapon;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;

namespace RiskOfRuinaMod.SkillStates
{
  public class ThrowPillarSpear : BaseThrowSpellState
  {
    public override void OnEnter()
    {
      this.baseDuration = 0.8f;
      this.force = 5f;
      this.maxDamageCoefficient = 6f;
      this.minDamageCoefficient = 3f;
      this.muzzleflashEffectPrefab = FirePistol2.muzzleEffectPrefab;
      this.projectilePrefab = Projectiles.pillarSpearPrefab;
      this.selfForce = 0.0f;
      this.throwSound = "Play_Binah_Stone_Fire";
      base.OnEnter();
    }
  }
}
