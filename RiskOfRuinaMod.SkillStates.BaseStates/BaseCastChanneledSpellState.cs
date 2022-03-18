using System.Collections.Generic;
using EntityStates;
using RiskOfRuinaMod.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{

	public abstract class BaseCastChanneledSpellState : BaseSkillState
	{
		public Queue<GameObject> projectilePrefabs = new Queue<GameObject>();

		public GameObject muzzleflashEffectPrefab;

		public float baseDuration;

		public Vector3 spellPosition;

		public Quaternion spellRotation;

		public string castSoundString;

		public string muzzleString = "SpellCastEffect";

		public float baseInterval;

		public bool centered = false;

		public GenericSkill chainActivatorSkillSlot;

		protected float overrideDuration;

		private float duration;

		private float interval;

		public float charge;

		private float stopwatch = 0f;

		private float prevAge = 0f;

		private bool valid = true;

		public override void OnEnter()
		{
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			if (spellPosition == Vector3.zero && spellRotation == Quaternion.identity)
			{
				chainActivatorSkillSlot.AddOneStock();
				((EntityState)this).outer.SetNextStateToMain();
				valid = false;
				return;
			}
			((BaseState)this).OnEnter();
			if (overrideDuration == 0f)
			{
				duration = baseDuration / (((BaseState)this).attackSpeedStat / 2f);
			}
			else
			{
				duration = overrideDuration;
			}
			interval = baseInterval / (((BaseState)this).attackSpeedStat / 2f);
			PlayCastAnimation();
			if ((bool)muzzleflashEffectPrefab)
			{
				EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, ((EntityState)this).get_gameObject(), muzzleString, false);
			}
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().AddBuff(Buffs.Slow50);
			}
			if ((bool)(Object)(object)((EntityState)this).get_cameraTargetParams())
			{
				((EntityState)this).get_cameraTargetParams().aimMode = (AimType)2;
			}
			if (muzzleString == "SpellCastEffect")
			{
				ChildLocator modelChildLocator = ((EntityState)this).GetModelChildLocator();
				if ((bool)(Object)(object)modelChildLocator)
				{
					GameObject gameObject = modelChildLocator.FindChild("SpellCastEffect").gameObject;
					gameObject.SetActive(value: false);
					gameObject.SetActive(value: true);
				}
			}
			Fire();
		}

		protected virtual void PlayCastAnimation()
		{
			((EntityState)this).PlayAnimation("Gesture, Override", "CastSpell", "Spell.playbackRate", duration);
		}

		public override void FixedUpdate()
		{
			((EntityState)this).FixedUpdate();
			((EntityState)this).get_characterBody().outOfCombatStopwatch = 0f;
			stopwatch += ((EntityState)this).get_fixedAge() - prevAge;
			prevAge = ((EntityState)this).get_fixedAge();
			if (stopwatch >= interval)
			{
				Fire();
				stopwatch = 0f;
			}
			if (((EntityState)this).get_isAuthority() && ((EntityState)this).get_fixedAge() >= duration)
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		public override void OnExit()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			((EntityState)this).OnExit();
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.Slow50);
			}
			if ((bool)(Object)(object)((EntityState)this).get_cameraTargetParams())
			{
				((EntityState)this).get_cameraTargetParams().aimMode = (AimType)0;
				((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsArbiter;
			}
		}

		protected virtual void Fire()
		{
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			if (projectilePrefabs.Count <= 0)
			{
				return;
			}
			if (castSoundString != "" && valid)
			{
				Util.PlaySound(castSoundString, ((EntityState)this).get_gameObject());
			}
			GameObject gameObject = projectilePrefabs.Dequeue();
			if (!gameObject || !((EntityState)this).get_isAuthority())
			{
				return;
			}
			Ray aimRay = ((BaseState)this).GetAimRay();
			if (gameObject != null)
			{
				Vector3 position = spellPosition;
				Quaternion identity = spellRotation;
				if (centered)
				{
					position = ((EntityState)this).get_transform().position;
					identity = Quaternion.identity;
				}
				FireProjectileInfo val = default(FireProjectileInfo);
				val.projectilePrefab = gameObject;
				val.position = position;
				val.rotation = identity;
				val.owner = ((EntityState)this).get_gameObject();
				val.damage = ((BaseState)this).damageStat;
				val.force = 0f;
				val.crit = ((BaseState)this).RollCrit();
				FireProjectileInfo val2 = val;
				ProjectileManager.get_instance().FireProjectile(val2);
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (InterruptPriority)2;
		}
	}
}