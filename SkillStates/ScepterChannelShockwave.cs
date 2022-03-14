// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.ScepterChannelShockwave
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class ScepterChannelShockwave : BaseChannelSpellState
  {
    private GameObject chargeEffect;
    private ShakeEmitter shakeEmitter;

    public override void OnEnter()
    {
      this.chargeEffectPrefab = (GameObject) null;
      this.startChargeSoundString = "Play_Abiter_Special_Start";
      this.maxSpellRadius = 60f;
      this.baseDuration = 1f;
      this.zooming = true;
      this.centered = true;
      this.shakeEmitter = ((EntityState) this).gameObject.AddComponent<ShakeEmitter>();
      this.shakeEmitter.amplitudeTimeDecay = false;
      this.shakeEmitter.duration = this.baseDuration / (((BaseState) this).attackSpeedStat / 2f);
      this.shakeEmitter.radius = 60f;
      this.shakeEmitter.scaleShakeRadiusWithLocalScale = false;
      this.shakeEmitter.wave = new Wave()
      {
        amplitude = 0.05f,
        frequency = 15f,
        cycleOffset = 0.0f
      };
      base.OnEnter();
    }

    protected override void PlayChannelAnimation() => ((EntityState) this).PlayAnimation("Gesture, Override", "Channel", "Channel.playbackRate", this.baseDuration);

    public override void FixedUpdate() => base.FixedUpdate();

    public override void OnExit()
    {
      Object.Destroy((Object) this.shakeEmitter);
      base.OnExit();
    }

    protected override BaseCastChanneledSpellState GetNextState() => (BaseCastChanneledSpellState) new ScepterCastShockwave();
  }
}
