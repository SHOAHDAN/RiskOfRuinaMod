using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates.BaseStates;

public abstract class BaseThrowSpellState : BaseSkillState
{
	public GameObject projectilePrefab;

	public GameObject muzzleflashEffectPrefab;

	public float baseDuration;

	public float minDamageCoefficient;

	public float maxDamageCoefficient;

	public float force;

	public float selfForce;

	private float duration;

	public float charge;

	public string throwSound;

	private ChildLocator childLocator { get; set; }

	public override void OnEnter()
	{
		((BaseState)this).OnEnter();
		childLocator = ((EntityState)this).GetModelChildLocator();
		duration = baseDuration / ((BaseState)this).attackSpeedStat;
		((EntityState)this).PlayAnimation("Gesture, Override", "CastSpell", "Spell.playbackRate", duration);
		if ((bool)muzzleflashEffectPrefab)
		{
			EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, ((EntityState)this).get_gameObject(), "HandR", false);
		}
		Util.PlaySound(throwSound, ((EntityState)this).get_gameObject());
		Fire();
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).get_isAuthority() && ((EntityState)this).get_fixedAge() >= duration)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override void OnExit()
	{
		((EntityState)this).OnExit();
	}

	private void Fire()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (((EntityState)this).get_isAuthority())
		{
			Ray aimRay = ((BaseState)this).GetAimRay();
			if (projectilePrefab != null)
			{
				float num = Util.Remap(charge, 0f, 1f, minDamageCoefficient, maxDamageCoefficient);
				float num2 = charge * force;
				FireProjectileInfo val = default(FireProjectileInfo);
				val.projectilePrefab = projectilePrefab;
				val.position = childLocator.FindChild("SpearSummon").position;
				val.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
				val.owner = ((EntityState)this).get_gameObject();
				val.damage = ((BaseState)this).damageStat * num;
				val.force = num2;
				val.crit = ((BaseState)this).RollCrit();
				((FireProjectileInfo)(ref val)).set_speedOverride(160f);
				FireProjectileInfo val2 = val;
				ProjectileManager.get_instance().FireProjectile(val2);
			}
			if ((bool)(Object)(object)((EntityState)this).get_characterMotor())
			{
				((EntityState)this).get_characterMotor().ApplyForce(aimRay.direction * ((0f - selfForce) * charge), false, false);
			}
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)2;
	}
}
