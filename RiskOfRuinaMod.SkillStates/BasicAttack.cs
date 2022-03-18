using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	internal class BasicAttack : BaseDirectionalSkill
	{
		public override void OnEnter()
		{
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			if (attackIndex > 3)
			{
				attackIndex = 1;
			}
			hitboxName = "Basic";
			if (attackIndex == 3)
			{
				hitboxName = "BasicThird";
			}
			damageCoefficient = 2f;
			if (attackIndex == 3)
			{
				damageCoefficient = 3f;
			}
			baseDuration = 1.3f;
			attackStartTime = 0.2f;
			attackEndTime = 0.4f;
			baseEarlyExitTime = 0.8f;
			if (attackIndex == 3)
			{
				baseEarlyExitTime = 0.6f;
			}
			hitStopDuration = 0.05f;
			if (attackIndex == 3)
			{
				pushForce = 600f;
			}
			swingSoundString = "Ruina_Swipe";
			impactSound = Assets.swordHitSoundVert.get_index();
			if (attackIndex == 3)
			{
				impactSound = Assets.swordHitSoundHori.get_index();
			}
			switch (attackIndex)
			{
				case 1:
					muzzleString = "BasicSwing1";
					break;
				case 2:
					muzzleString = "BasicSwing2";
					break;
				case 3:
					muzzleString = "BasicSwing3";
					break;
			}
			hitEffectPrefab = Assets.swordHitEffect;
			base.OnEnter();
			swingEffectPrefab = statTracker.slashPrefab;
		}

		protected override void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "BasicSlash" + attackIndex, "BaseAttack.playbackRate", duration, 0.1f);
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
			float num = Mathf.Clamp(0f, 0.5f, 0.5f * base.trueMoveSpeed);
			CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
			characterMotor.rootMotion += ((EntityState)this).get_characterDirection().get_forward() * (num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / duration) * Time.fixedDeltaTime);
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}