using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates {

	public class FairyCombo : BaseMeleeAttack
	{
		private bool firedProjectile = false;

		public override void OnEnter()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			hitboxName = "Fairy";
			damageType = (DamageType)0;
			damageCoefficient = 0.75f;
			procCoefficient = 1f;
			pushForce = 300f;
			bonusForce = Vector3.zero;
			baseDuration = 1f;
			attackStartTime = 0.2f;
			attackEndTime = 0.4f;
			baseEarlyExitTime = 0.2f;
			hitStopDuration = 0.012f;
			attackRecoil = 0.5f;
			hitHopVelocity = 4f;
			swingSoundString = "Ruina_Swipe";
			impactSound = (NetworkSoundEventIndex)(-1);
			muzzleString = ((swingIndex % 2 == 0) ? "SwingLeft" : "SwingRight");
			swingEffectPrefab = Assets.armSwingEffect;
			hitEffectPrefab = Assets.fairyHitEffect;
			base.OnEnter();
		}

		protected override void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("Gesture, Override", "Swipe" + (1 + swingIndex), "Swipe.playbackRate", duration * 0.6f, 0.05f);
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
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			base.FireAttack();
			if (((EntityState)this).get_isAuthority() && !firedProjectile)
			{
				firedProjectile = true;
				Ray aimRay = ((BaseState)this).GetAimRay();
				if (Projectiles.fairyLinePrefab != null)
				{
					FireProjectileInfo val = default(FireProjectileInfo);
					val.projectilePrefab = Projectiles.fairyLinePrefab;
					val.position = aimRay.origin;
					val.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
					val.owner = ((EntityState)this).get_gameObject();
					val.damage = ((BaseState)this).damageStat * 0.75f;
					val.force = 0f;
					val.crit = ((BaseState)this).RollCrit();
					((FireProjectileInfo)(ref val)).set_speedOverride(150f);
					FireProjectileInfo val2 = val;
					ProjectileManager.get_instance().FireProjectile(val2);
				}
			}
		}

		protected override void SetNextState()
		{
			int num = swingIndex;
			num++;
			if (num > 2)
			{
				((EntityState)this).outer.SetNextStateToMain();
				return;
			}
			((EntityState)this).outer.SetNextState((EntityState)(object)new FairyCombo
			{
				swingIndex = num
			});
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}