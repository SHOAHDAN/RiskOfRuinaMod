using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	internal class AirBasicAttack : BaseDirectionalSkill
	{
		public override void OnEnter()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			if (attackIndex > 2)
			{
				attackIndex = 1;
			}
			hitboxName = "AirBasic";
			damageCoefficient = 2f;
			baseDuration = 1f;
			attackStartTime = 0.2f;
			attackEndTime = 0.4f;
			baseEarlyExitTime = 0.4f;
			hitStopDuration = 0.05f;
			swingHopVelocity = 8f;
			pushForce = 0f;
			bonusForce = Vector3.zero;
			swingSoundString = "Ruina_Swipe";
			impactSound = Assets.swordHitSoundHori.get_index();
			switch (attackIndex)
			{
				case 1:
					muzzleString = "Air1";
					break;
				case 2:
					muzzleString = "Air2";
					break;
			}
			hitEffectPrefab = Assets.swordHitEffect;
			base.OnEnter();
			swingEffectPrefab = statTracker.slashPrefab;
		}

		protected override void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "AirBasicSlash" + attackIndex, "BaseAttack.playbackRate", duration, 0.1f);
		}

		protected override void PlaySwingEffect()
		{
			base.PlaySwingEffect();
		}

		protected override void OnHitEnemyAuthority()
		{
			base.OnHitEnemyAuthority();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		protected override void FireAttack()
		{
			base.FireAttack();
			if (inputVector != Vector2.zero && !inHitPause)
			{
				float num = Mathf.Clamp(0f, 2f, 0.5f * base.trueMoveSpeed);
				CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
				characterMotor.rootMotion += ((EntityState)this).get_inputBank().moveVector * (num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / duration) * Time.fixedDeltaTime);
			}
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}