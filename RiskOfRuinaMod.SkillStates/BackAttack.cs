using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class BackAttack : BaseDirectionalSkill
{
	public override void OnEnter()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		attackIndex = 1;
		hitboxName = "Back";
		damageCoefficient = 1.75f;
		baseDuration = 1f;
		attackStartTime = 0.4f;
		attackEndTime = 0.6f;
		baseEarlyExitTime = 0.2f;
		hitStopDuration = 0.05f;
		swingSoundString = "Ruina_Swipe";
		impactSound = Assets.swordHitSoundHori.get_index();
		muzzleString = "BasicSwing3";
		hitEffectPrefab = Assets.swordHitEffect;
		if (attackIndex == 1)
		{
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
			}
			AddOverlay(baseDuration * attackStartTime);
		}
		base.OnEnter();
		swingEffectPrefab = statTracker.slashPrefab;
	}

	protected override void PlayAttackAnimation()
	{
		((EntityState)this).PlayCrossfade("FullBody, Override", "BackSlash", "BaseAttack.playbackRate", duration, 0.1f);
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

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (stopwatch <= duration * attackEndTime && !inHitPause)
		{
			Vector3 aimDirection = ((EntityState)this).get_inputBank().get_aimDirection();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			float num = 5f;
			CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
			characterMotor.rootMotion -= aimDirection * (num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / (duration * attackEndTime)) * Time.fixedDeltaTime);
		}
		if (stopwatch > duration * attackStartTime)
		{
			if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
			}
			RemoveOverlay();
		}
	}

	public override void OnExit()
	{
		if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
		{
			((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
		}
		RemoveOverlay();
		base.OnExit();
	}
}
