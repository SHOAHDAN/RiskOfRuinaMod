using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class EGOSideAttack : BaseDirectionalSkill
{
	private float direction = 1f;

	public override void OnEnter()
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		if (attackIndex > 2)
		{
			attackIndex = 1;
		}
		if (inputVector.y > 0.5f)
		{
			direction = 1f;
		}
		else if (inputVector.y < -0.5f)
		{
			direction = -1f;
		}
		hitboxName = "EGOSide";
		damageCoefficient = 1.25f;
		baseDuration = 1.4f;
		attackStartTime = 0.35f;
		attackEndTime = 0.5f;
		baseEarlyExitTime = 0.8f;
		hitStopDuration = 0.05f;
		swingSoundString = "Ruina_Swipe";
		impactSound = Assets.swordHitEGOSoundHori.get_index();
		switch (attackIndex)
		{
		case 1:
			muzzleString = "Side1";
			break;
		case 2:
			muzzleString = "Side2";
			break;
		}
		hitEffectPrefab = Assets.swordHitEffect;
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
		}
		AddOverlay(baseDuration * attackStartTime);
		base.OnEnter();
		swingEffectPrefab = statTracker.EGOSlashPrefab;
	}

	protected override void PlayAttackAnimation()
	{
		((EntityState)this).PlayCrossfade("FullBody, Override", "EGOSideSlash" + attackIndex, "BaseAttack.playbackRate", duration, 0.1f);
	}

	protected override void PlaySwingEffect()
	{
		base.PlaySwingEffect();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!hasFired && !inHitPause)
		{
			Vector3 aimDirection = ((EntityState)this).get_inputBank().get_aimDirection();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			float num = Mathf.Clamp(0f, 3.5f, 0.5f * base.trueMoveSpeed);
			Vector3 normalized = Vector3.Cross(Vector3.up, aimDirection).normalized;
			CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
			characterMotor.rootMotion += direction * (normalized * (num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / (duration * attackEndTime)) * Time.fixedDeltaTime));
		}
	}

	protected override void OnHitEnemyAuthority()
	{
		base.OnHitEnemyAuthority();
	}

	protected override void FireAttack()
	{
		if (!hasFired && NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
		}
		RemoveOverlay();
		base.FireAttack();
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
