using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{ X

internal class ChannelShockwave : BaseChannelSpellState
	{
		private GameObject chargeEffect;

		public override void OnEnter()
		{
			chargeEffectPrefab = null;
			maxSpellRadius = 40f;
			baseDuration = 0.4f;
			zooming = true;
			centered = true;
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
			base.OnExit();
		}

		protected override BaseCastChanneledSpellState GetNextState()
		{
			return new CastShockwave();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (InterruptPriority)6;
		}
	}
}