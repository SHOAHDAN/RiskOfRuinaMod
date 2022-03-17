using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

public class Unlock : BaseSkillState
{
	public static float procCoefficient = 1f;

	public static float baseDuration = 0.6f;

	public static float force = 800f;

	public static float recoil = 3f;

	public static float range = 256f;

	private float duration;

	private float fireTime;

	private bool hasFired;

	private string muzzleString;

	private TargetTracker tracker;

	private CharacterBody target;

	private bool targetIsValid;

	private int stocks = 0;

	public override void OnEnter()
	{
		((BaseState)this).OnEnter();
		tracker = ((EntityState)this).GetComponent<TargetTracker>();
		target = tracker.GetTrackingTarget();
		stocks = ((BaseSkillState)this).get_activatorSkillSlot().get_stock();
		((BaseSkillState)this).get_activatorSkillSlot().set_stock(0);
		if ((bool)(Object)(object)target && (bool)(Object)(object)target.get_healthComponent() && target.get_healthComponent().get_alive())
		{
			targetIsValid = true;
			Util.PlaySound("Play_Binah_Lock_Ready", ((EntityState)this).get_gameObject());
		}
		else
		{
			((BaseSkillState)this).get_activatorSkillSlot().set_stock(stocks);
			((EntityState)this).outer.SetNextStateToMain();
		}
		duration = baseDuration / ((BaseState)this).attackSpeedStat;
		fireTime = 0.2f * duration;
		((EntityState)this).get_characterBody().SetAimTimer(2f);
		muzzleString = "HandR";
	}

	public override void OnExit()
	{
		((EntityState)this).OnExit();
	}

	private void Fire()
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		if (hasFired)
		{
			return;
		}
		hasFired = true;
		((EntityState)this).get_characterBody().AddSpreadBloom(1.5f);
		EffectManager.SimpleMuzzleFlash(FirePistol2.muzzleEffectPrefab, ((EntityState)this).get_gameObject(), muzzleString, false);
		if (targetIsValid)
		{
			if (NetworkServer.get_active())
			{
				target.AddTimedBuff(Buffs.strengthBuff, 10f * (float)stocks);
			}
			Transform modelTransform = target.get_modelLocator().get_modelTransform();
			if ((bool)(Object)(object)target && (bool)modelTransform)
			{
				TemporaryOverlay val = ((Component)(object)target).gameObject.AddComponent<TemporaryOverlay>();
				val.duration = 10f * (float)stocks;
				val.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
				val.animateShaderAlpha = true;
				val.destroyComponentOnEnd = true;
				val.originalMaterial = Assets.mainAssetBundle.LoadAsset<Material>("matChains");
				val.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
			}
			if (((EntityState)this).get_isAuthority())
			{
				EffectData val2 = new EffectData();
				val2.rotation = Util.QuaternionSafeLookRotation(Vector3.zero);
				val2.set_origin(target.get_corePosition());
				EffectManager.SpawnEffect(Assets.unlockEffect, val2, true);
			}
		}
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).get_fixedAge() >= fireTime)
		{
			Fire();
		}
		if (((EntityState)this).get_fixedAge() >= duration && ((EntityState)this).get_isAuthority())
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)2;
	}
}
