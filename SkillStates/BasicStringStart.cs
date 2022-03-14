// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BasicStringStart
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;

namespace RiskOfRuinaMod.SkillStates
{
  public class BasicStringStart : BaseDirectionalSkill
  {
    public override void OnEnter()
    {
      base.OnEnter();
      this.attackIndex = 0;
    }

    public override void OnExit() => base.OnExit();

    protected override void FireAttack()
    {
    }

    public override void FixedUpdate()
    {
      if (!((EntityState) this).isAuthority)
        return;
      this.EvaluateInput();
      this.SetNextState();
    }

    protected override void SetNextState() => base.SetNextState();
  }
}
