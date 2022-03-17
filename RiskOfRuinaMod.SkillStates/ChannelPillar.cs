using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;

namespace RiskOfRuinaMod.SkillStates;

internal class ChannelPillar : BaseChannelSpellState
{
	public override void OnEnter()
	{
		chargeEffectPrefab = null;
		maxSpellRadius = 25f;
		baseDuration = 0.25f;
		zooming = false;
		line = true;
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
		return new CastPillar();
	}
}
