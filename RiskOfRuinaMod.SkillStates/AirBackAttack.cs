using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates;

internal class AirBackAttack : BaseDirectionalSkill
{
	public override void OnEnter()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		attackIndex = 1;
		hitboxName = "AirBasic";
		damageCoefficient = 2f;
		baseDuration = 0.25f;
		attackStartTime = 0.5f;
		attackEndTime = 0.8f;
		baseEarlyExitTime = 0f;
		hitStopDuration = 0.05f;
		swingSoundString = "Ruina_Swipe";
		impactSound = Assets.swordHitSoundVert.get_index();
		muzzleString = "SwingLeft";
		hitEffectPrefab = Assets.swordHitEffect;
		bonusForce = Vector3.down * 900f;
		base.OnEnter();
		swingEffectPrefab = statTracker.slashPrefab;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		((EntityState)this).get_characterMotor().velocity.y = 0f;
		if (stopwatch > duration * attackEndTime)
		{
			((EntityState)this).outer.SetNextState((EntityState)(object)new AirBackFallingAttack());
		}
	}

	protected override void PlayAttackAnimation()
	{
		((EntityState)this).PlayCrossfade("FullBody, Override", "AirBackSlash", "BaseAttack.playbackRate", duration, 0.1f);
	}

	protected override void PlaySwingEffect()
	{
		base.PlaySwingEffect();
	}

	protected override void OnHitEnemyAuthority()
	{
		base.OnHitEnemyAuthority();
	}

	protected override void FireAttack()
	{
		base.FireAttack();
	}

	public override void OnExit()
	{
		base.OnExit();
	}
}
