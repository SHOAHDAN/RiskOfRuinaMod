using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	internal class EGOJumpAttack : BaseDirectionalSkill
	{
		public override void OnEnter()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			attackIndex = 1;
			hitboxName = "EGOJump";
			damageCoefficient = 1.5f;
			baseDuration = 1f;
			attackStartTime = 0.35f;
			attackEndTime = 0.5f;
			baseEarlyExitTime = 0.5f;
			hitStopDuration = 0.05f;
			swingSoundString = "Ruina_Swipe";
			impactSound = Assets.swordHitEGOSoundVert.get_index();
			muzzleString = "Jump";
			hitEffectPrefab = Assets.swordHitEffect;
			bonusForce = Vector3.up * 3000f;
			base.OnEnter();
			swingEffectPrefab = statTracker.EGOSlashPrefab;
		}

		protected override void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "EGOJumpSlash", "BaseAttack.playbackRate", duration, 0.1f);
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
			if (((EntityState)this).get_inputBank().skill1.down && ((EntityState)this).get_inputBank().jump.down)
			{
				((EntityState)this).outer.SetNextState((EntityState)(object)new EGOJumpRisingAttack
				{
					attackIndex = attackIndex,
					inputVector = inputVector
				});
			}
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}