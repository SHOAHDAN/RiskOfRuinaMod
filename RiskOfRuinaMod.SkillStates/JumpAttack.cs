using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	internal class JumpAttack : BaseDirectionalSkill
	{
		public override void OnEnter()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			attackIndex = 1;
			hitboxName = "Jump";
			damageCoefficient = 1.5f;
			baseDuration = 1f;
			attackStartTime = 0.35f;
			attackEndTime = 0.5f;
			baseEarlyExitTime = 0.3f;
			hitStopDuration = 0.05f;
			swingSoundString = "Ruina_Swipe";
			impactSound = Assets.swordHitSoundVert.get_index();
			muzzleString = "Jump";
			hitEffectPrefab = Assets.swordHitEffect;
			bonusForce = Vector3.up * 3000f;
			base.OnEnter();
			swingEffectPrefab = statTracker.slashPrefab;
		}

		protected override void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("FullBody, Override", "JumpSlash", "BaseAttack.playbackRate", duration, 0.1f);
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
				((EntityState)this).outer.SetNextState((EntityState)(object)new JumpRisingAttack
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