using System;
using System.Collections.Generic;
using EntityStates;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{

	public class BaseMeleeAttack : BaseSkillState
	{
		public int swingIndex;

		protected string hitboxName = "Sword";

		protected DamageType damageType = (DamageType)0;

		protected float damageCoefficient = 3.5f;

		protected float procCoefficient = 1f;

		protected float pushForce = 300f;

		protected Vector3 bonusForce = Vector3.zero;

		protected float baseDuration = 1f;

		protected float attackStartTime = 0.2f;

		protected float attackEndTime = 0.4f;

		protected float baseEarlyExitTime = 0.4f;

		protected float hitStopDuration = 0.012f;

		protected float attackRecoil = 0.75f;

		protected float hitHopVelocity = 4f;

		protected bool cancelled = false;

		protected string swingSoundString = "";

		protected string hitSoundString = "";

		protected string muzzleString = "SwingCenter";

		protected GameObject swingEffectPrefab;

		protected GameObject hitEffectPrefab;

		protected NetworkSoundEventIndex impactSound;

		private float earlyExitTime;

		public float duration;

		private bool hasFired;

		private float hitPauseTimer;

		protected OverlapAttack attack;

		protected bool inHitPause;

		private bool hasHopped;

		protected float stopwatch;

		protected Animator animator;

		private HitStopCachedState hitStopCachedState;

		private Vector3 storedVelocity;

		public override void OnEnter()
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			((BaseState)this).OnEnter();
			duration = baseDuration / ((BaseState)this).attackSpeedStat;
			earlyExitTime = baseEarlyExitTime / ((BaseState)this).attackSpeedStat;
			hasFired = false;
			animator = ((EntityState)this).GetModelAnimator();
			((BaseState)this).StartAimMode(0.5f + duration, false);
			((EntityState)this).get_characterBody().outOfCombatStopwatch = 0f;
			animator.SetBool("attacking", value: true);
			HitBoxGroup hitBoxGroup = null;
			Transform modelTransform = ((EntityState)this).GetModelTransform();
			if ((bool)modelTransform)
			{
				hitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxName);
			}
			PlayAttackAnimation();
			attack = new OverlapAttack();
			attack.damageType = damageType;
			attack.attacker = ((EntityState)this).get_gameObject();
			attack.inflictor = ((EntityState)this).get_gameObject();
			attack.teamIndex = ((BaseState)this).GetTeam();
			attack.damage = damageCoefficient * ((BaseState)this).damageStat;
			attack.procCoefficient = procCoefficient;
			attack.hitEffectPrefab = hitEffectPrefab;
			attack.forceVector = bonusForce;
			attack.pushAwayForce = pushForce;
			attack.hitBoxGroup = hitBoxGroup;
			attack.isCrit = ((BaseState)this).RollCrit();
			attack.impactSound = impactSound;
		}

		protected virtual void PlayAttackAnimation()
		{
			((EntityState)this).PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), "Slash.playbackRate", duration, 0.05f);
		}

		public override void OnExit()
		{
			if (!hasFired && !cancelled)
			{
				FireAttack();
			}
			((EntityState)this).OnExit();
			animator.SetBool("attacking", value: false);
		}

		protected virtual void PlaySwingEffect()
		{
			EffectManager.SimpleMuzzleFlash(swingEffectPrefab, ((EntityState)this).get_gameObject(), muzzleString, true);
		}

		protected virtual void OnHitEnemyAuthority()
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			if (!hasHopped)
			{
				if ((bool)(UnityEngine.Object)(object)((EntityState)this).get_characterMotor() && !((EntityState)this).get_characterMotor().get_isGrounded() && hitHopVelocity > 0f)
				{
					((BaseState)this).SmallHop(((EntityState)this).get_characterMotor(), hitHopVelocity);
				}
				hasHopped = true;
			}
			if (!inHitPause && hitStopDuration > 0f)
			{
				storedVelocity = ((EntityState)this).get_characterMotor().velocity;
				hitStopCachedState = ((BaseState)this).CreateHitStopCachedState(((EntityState)this).get_characterMotor(), animator, "Slash.playbackRate");
				hitPauseTimer = hitStopDuration / ((BaseState)this).attackSpeedStat;
				inHitPause = true;
			}
		}

		protected virtual void FireAttack()
		{
			if (!hasFired)
			{
				hasFired = true;
				Util.PlayAttackSpeedSound(swingSoundString, ((EntityState)this).get_gameObject(), ((BaseState)this).attackSpeedStat);
				if (((EntityState)this).get_isAuthority())
				{
					PlaySwingEffect();
					((BaseState)this).AddRecoil(-1f * attackRecoil, -2f * attackRecoil, -0.5f * attackRecoil, 0.5f * attackRecoil);
				}
			}
			if (((EntityState)this).get_isAuthority() && attack.Fire((List<HurtBox>)null))
			{
				OnHitEnemyAuthority();
			}
		}

		protected virtual void SetNextState()
		{
			int num = ((swingIndex == 0) ? 1 : 0);
			((EntityState)this).outer.SetNextState((EntityState)(object)new BaseMeleeAttack
			{
				swingIndex = num
			});
		}

		public override void FixedUpdate()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			((EntityState)this).FixedUpdate();
			hitPauseTimer -= Time.fixedDeltaTime;
			if (hitPauseTimer <= 0f && inHitPause)
			{
				((BaseState)this).ConsumeHitStopCachedState(hitStopCachedState, ((EntityState)this).get_characterMotor(), animator);
				inHitPause = false;
				((EntityState)this).get_characterMotor().velocity = storedVelocity;
			}
			if (!inHitPause)
			{
				stopwatch += Time.fixedDeltaTime;
			}
			else
			{
				if ((bool)(UnityEngine.Object)(object)((EntityState)this).get_characterMotor())
				{
					((EntityState)this).get_characterMotor().velocity = Vector3.zero;
				}
				if ((bool)animator)
				{
					animator.SetFloat("Swing.playbackRate", 0f);
				}
			}
			if (stopwatch >= duration * attackStartTime && stopwatch <= duration * attackEndTime)
			{
				FireAttack();
			}
			if (stopwatch >= duration - earlyExitTime && ((EntityState)this).get_isAuthority() && ((EntityState)this).get_inputBank().skill1.down)
			{
				if (!hasFired)
				{
					FireAttack();
				}
				SetNextState();
			}
			else if (stopwatch >= duration && ((EntityState)this).get_isAuthority())
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (InterruptPriority)1;
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			((BaseSkillState)this).OnSerialize(writer);
			writer.Write(swingIndex);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			((BaseSkillState)this).OnDeserialize(reader);
			swingIndex = reader.ReadInt32();
		}
	}
}