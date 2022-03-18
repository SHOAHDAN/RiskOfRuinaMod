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
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			chargeEffectPrefab = null;
			startChargeSoundString = "Play_Abiter_Special_Start";
			maxSpellRadius = 60f;
			baseDuration = 1f;
			zooming = true;
			centered = true;
			shakeEmitter = ((EntityState)this).get_gameObject().AddComponent<ShakeEmitter>();
			shakeEmitter.amplitudeTimeDecay = false;
			shakeEmitter.duration = baseDuration / (((BaseState)this).attackSpeedStat / 2f);
			shakeEmitter.radius = 60f;
			shakeEmitter.scaleShakeRadiusWithLocalScale = false;
			shakeEmitter.wave = new Wave
			{
				amplitude = 0.05f,
				frequency = 15f,
				cycleOffset = 0f
			};
			base.OnEnter();
		}

		protected override void PlayChannelAnimation()
		{
			((EntityState)this).PlayAnimation("Gesture, Override", "Channel", "Channel.playbackRate", baseDuration);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		public override void OnExit()
		{
			Object.Destroy((Object)(object)shakeEmitter);
			base.OnExit();
		}

		protected override BaseCastChanneledSpellState GetNextState()
		{
			return new ScepterCastShockwave();
		}
	}
}