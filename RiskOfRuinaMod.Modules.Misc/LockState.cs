using System.Collections.Generic;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Misc;

internal class LockState : BaseState
{
	private struct projectileInfo
	{
		public GameObject projectile;

		public Vector3 velocity;

		public Vector3 moveVector;

		public Vector3 position;
	}

	public float duration = 5f;

	private List<GameObject> lockedProjectiles;

	private List<projectileInfo> lockedProjectileInfo;

	public override void OnEnter()
	{
		if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && (bool)(Object)(object)((EntityState)this).get_characterBody().get_master() && RiskOfRuinaPlugin.KombatIsDueling(((EntityState)this).get_characterBody().get_master()) && duration > 3f)
		{
			duration = 3f;
		}
		((BaseState)this).OnEnter();
		lockedProjectiles = new List<GameObject>();
		lockedProjectileInfo = new List<projectileInfo>();
		Animator modelAnimator = ((EntityState)this).GetModelAnimator();
		if ((bool)modelAnimator)
		{
			modelAnimator.enabled = false;
		}
		if ((bool)((EntityState)this).get_rigidbody() && !((EntityState)this).get_rigidbody().isKinematic)
		{
			((EntityState)this).get_rigidbody().velocity = Vector3.zero;
			if ((bool)(Object)(object)((EntityState)this).get_rigidbodyMotor())
			{
				((EntityState)this).get_rigidbodyMotor().moveVector = Vector3.zero;
			}
		}
	}

	public override void OnExit()
	{
		Animator modelAnimator = ((EntityState)this).GetModelAnimator();
		if ((bool)modelAnimator)
		{
			modelAnimator.enabled = true;
		}
		foreach (projectileInfo item in lockedProjectileInfo)
		{
			if ((bool)item.projectile)
			{
				EntityState.Destroy((Object)item.projectile.gameObject);
			}
		}
		((EntityState)this).OnExit();
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if ((bool)(Object)(object)((EntityState)this).get_characterMotor())
		{
			((EntityState)this).get_characterMotor().velocity = Vector3.zero;
		}
		Collider[] array = Physics.OverlapSphere(((EntityState)this).get_transform().position, 50f, ((LayerIndex)(ref LayerIndex.projectile)).get_mask());
		for (int i = 0; i < array.Length; i++)
		{
			ProjectileController component = array[i].GetComponent<ProjectileController>();
			if (!(Object)(object)component || !component.owner || !(component.owner == ((EntityState)this).get_gameObject()) || lockedProjectiles.Contains(((Component)(object)component).gameObject))
			{
				continue;
			}
			lockedProjectiles.Add(((Component)(object)component).gameObject);
			Vector3 velocity = Vector3.zero;
			Vector3 moveVector = Vector3.zero;
			Rigidbody component2 = ((Component)(object)component).GetComponent<Rigidbody>();
			if ((bool)component2 && !component2.isKinematic)
			{
				velocity = component2.velocity;
				if ((bool)(Object)(object)((Component)(object)component).GetComponent<RigidbodyMotor>())
				{
					moveVector = ((Component)(object)component).GetComponent<RigidbodyMotor>().moveVector;
				}
			}
			projectileInfo projectileInfo = default(projectileInfo);
			projectileInfo.projectile = ((Component)(object)component).gameObject;
			projectileInfo.velocity = velocity;
			projectileInfo.moveVector = moveVector;
			projectileInfo.position = ((Component)(object)component).gameObject.transform.position;
			projectileInfo item = projectileInfo;
			lockedProjectileInfo.Add(item);
		}
		foreach (projectileInfo item2 in lockedProjectileInfo)
		{
			if (!item2.projectile)
			{
				continue;
			}
			item2.projectile.transform.position = item2.position;
			ProjectileController component3 = item2.projectile.GetComponent<ProjectileController>();
			if ((bool)(Object)(object)component3)
			{
				Collider[] myColliders = component3.myColliders;
				foreach (Collider collider in myColliders)
				{
					collider.enabled = false;
				}
			}
			Rigidbody component4 = item2.projectile.GetComponent<Rigidbody>();
			if ((bool)component4 && !component4.isKinematic)
			{
				component4.velocity = Vector3.zero;
				if ((bool)(Object)(object)item2.projectile.GetComponent<RigidbodyMotor>())
				{
					item2.projectile.GetComponent<RigidbodyMotor>().moveVector = Vector3.zero;
				}
			}
		}
		if (((EntityState)this).get_fixedAge() >= duration)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)4;
	}
}
