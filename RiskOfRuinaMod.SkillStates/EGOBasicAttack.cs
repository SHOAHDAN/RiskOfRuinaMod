using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	internal class EGOBasicAttack : BaseDirectionalSkill
	{
		public override void OnEnter()
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			if (attackIndex > 3)
			{
				attackIndex = 1;
			}
			hitboxName = "EGOBasic";
			if (attackIndex == 3)
			{
				hitboxName = "EGOBasicThird";
			}
			damageCoefficient = 2f;
			if (attackIndex == 3)
			{
				damageCoefficient = 3f;
			}
			baseDuration = 1.3f;
			attackStartTime = 0.2f;
			attackEndTime = 0.4f;
			baseEarlyExitTime = 0.9f;
			hitStopDuration = 0.05f;
			if (attackIndex == 3)
			{
				pushForce = 600f;
			}
			swingSoundString = "Ruina_Swipe";
			impactSound = Assets.swordHitEGOSoundVert.get_index();
			if (attackIndex == 3)
			{
				impactSound = Assets.swordHitEGOSoundHori.get_index();
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
			swingEffectPrefab = statTracker.EGOSlashPrefab;
		}

		protected override void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "EGOBasicSlash" + attackIndex, "BaseAttack.playbackRate", duration, 0.1f);
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