using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;

namespace RiskOfRuinaMod.SkillStates;

public class BasicStringStart : BaseDirectionalSkill
{
	public override void OnEnter()
	{
		base.OnEnter();
		attackIndex = 0;
	}

	public override void OnExit()
	{
		base.OnExit();
	}

	protected override void FireAttack()
	{
	}

	public override void FixedUpdate()
	{
		if (((EntityState)this).get_isAuthority())
		{
			EvaluateInput();
			SetNextState();
		}
	}

	protected override void SetNextState()
	{
		base.SetNextState();
	}
}
