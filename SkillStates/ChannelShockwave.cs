// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.ChannelShockwave
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class ChannelShockwave : BaseChannelSpellState
  {
    private GameObject chargeEffect;

    public override void OnEnter()
    {
      this.chargeEffectPrefab = (GameObject) null;
      this.maxSpellRadius = 40f;
      this.baseDuration = 0.4f;
      this.zooming = true;
      this.centered = true;
      base.OnEnter();
    }

    protected override void PlayChannelAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "Channel", "Channel.playbackRate", this.baseDuration);

    public override void FixedUpdate() => base.FixedUpdate();

    public override void OnExit() => base.OnExit();

    protected override BaseCastChanneledSpellState GetNextState() => (BaseCastChanneledSpellState) new CastShockwave();

    public override InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;
  }
}
