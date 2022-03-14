// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.ChargePillarSpear
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  public class ChargePillarSpear : BaseChargeSpellState
  {
    private GameObject chargeEffect;
    private Vector3 originalScale;

    public override void OnEnter()
    {
      this.baseDuration = 2f;
      this.chargeEffectPrefab = (GameObject) null;
      this.chargeSoundString = "Play_Binah_Stone_Ready";
      this.crosshairOverridePrefab = Resources.Load<GameObject>("Prefabs/Crosshair/ToolbotGrenadeLauncherCrosshair");
      this.maxBloomRadius = 0.1f;
      this.minBloomRadius = 1f;
      base.OnEnter();
      ChildLocator modelChildLocator = ((EntityState) this).GetModelChildLocator();
      if (!Object.op_Implicit((Object) modelChildLocator))
        return;
      this.chargeEffect = ((Component) modelChildLocator.FindChild("SpearSummon")).gameObject;
      this.chargeEffect.SetActive(true);
      this.originalScale = this.chargeEffect.transform.localScale;
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      this.chargeEffect.transform.localScale = Vector3.op_Multiply(this.originalScale, 1f + this.CalcCharge());
    }

    public override void OnExit()
    {
      base.OnExit();
      if (!Object.op_Implicit((Object) this.chargeEffect))
        return;
      this.chargeEffect.transform.localScale = this.originalScale;
      this.chargeEffect.SetActive(false);
    }

    protected override BaseThrowSpellState GetNextState() => (BaseThrowSpellState) new ThrowPillarSpear();
  }
}
