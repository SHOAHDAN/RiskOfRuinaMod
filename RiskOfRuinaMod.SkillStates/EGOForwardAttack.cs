using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class EGOForwardAttack : BaseDirectionalSkill
{
	public override void OnEnter()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		if (attackIndex > 4)
		{
			attackIndex = 2;
		}
		hitboxName = "EGOForward";
		damageCoefficient = 1.25f;
		damageType = (DamageType)2;
		baseDuration = 1.4f;
		attackStartTime = 0.2f;
		attackEndTime = 0.4f;
		baseEarlyExitTime = 1f;
		hitStopDuration = 0.05f;
		if (attackIndex >= 2)
		{
			baseDuration = 0.9f;
			attackStartTime = 0.1f;
			attackEndTime = 0.3f;
			baseEarlyExitTime = 0.65f;
			hitStopDuration = 0.05f;
			damageCoefficient = 1f;
		}
		swingSoundString = "Ruina_Swipe";
		impactSound = Assets.swordHitEGOSoundStab.get_index();
		switch (attackIndex)
		{
		case 1:
			muzzleString = "Spear1";
			break;
		case 2:
			muzzleString = "Spear1";
			break;
		case 3:
			muzzleString = "EGOSpear3";
			break;
		case 4:
			muzzleString = "EGOSpear4";
			break;
		}
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
		swingEffectPrefab = statTracker.EGOPiercePrefab;
	}

	protected override void PlayAttackAnimation()
	{
		if (attackIndex >= 2)
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "EGOForwardSpear" + attackIndex, "BaseAttack.playbackRate", duration, 0.01f);
		}
		else
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "EGOForwardSpear1", "BaseAttack.playbackRate", duration, 0.1f);
		}
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
		if (!hasFired && attackIndex == 1)
		{
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
			}
			RemoveOverlay();
		}
		base.FireAttack();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!hasFired && !inHitPause)
		{
			Vector3 aimDirection = ((EntityState)this).get_inputBank().get_aimDirection();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			if (attackIndex >= 2)
			{
				float num = 0.25f;
				CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
				characterMotor.rootMotion += aimDirection * (num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / (duration * attackEndTime)) * Time.fixedDeltaTime);
			}
			else
			{
				float num2 = 9f;
				CharacterMotor characterMotor2 = ((EntityState)this).get_characterMotor();
				characterMotor2.rootMotion += aimDirection * (num2 * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / (duration * attackEndTime)) * Time.fixedDeltaTime);
			}
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
