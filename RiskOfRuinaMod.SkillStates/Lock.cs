using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{

	public class Lock : BaseSkillState
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

		public override void OnEnter()
		{
			((BaseState)this).OnEnter();
			tracker = ((EntityState)this).GetComponent<TargetTracker>();
			target = tracker.GetTrackingTarget();
			if ((bool)(Object)(object)target && (bool)(Object)(object)target.get_healthComponent() && target.get_healthComponent().get_alive())
			{
				targetIsValid = true;
				Util.PlaySound("Play_Binah_Lock_Ready", ((EntityState)this).get_gameObject());
			}
			else
			{
				((BaseSkillState)this).get_activatorSkillSlot().AddOneStock();
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
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Expected O, but got Unknown
			if (hasFired)
			{
				return;
			}
			hasFired = true;
			((EntityState)this).get_characterBody().AddSpreadBloom(1.5f);
			EffectManager.SimpleMuzzleFlash(FirePistol2.muzzleEffectPrefab, ((EntityState)this).get_gameObject(), muzzleString, false);
			if (!targetIsValid)
			{
				return;
			}
			int buffCount = target.GetBuffCount(Buffs.lockResistBuff);
			if (NetworkServer.get_active())
			{
				DamageInfo val = new DamageInfo
				{
					attacker = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
					inflictor = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
					crit = ((BaseState)this).RollCrit(),
					damage = ((EntityState)this).get_characterBody().get_damage() * 3f,
					position = target.transform.position,
					force = Vector3.zero,
					damageType = (DamageType)32,
					damageColorIndex = (DamageColorIndex)0,
					procCoefficient = 1f
				};
				target.get_healthComponent().TakeDamage(val);
				GlobalEventManager.instance.OnHitEnemy(val, ((Component)(object)target).gameObject);
				GlobalEventManager.instance.OnHitAll(val, ((Component)(object)target).gameObject);
			}
			if (buffCount <= 4 && target.GetBuffCount(Buffs.lockDebuff) == 0)
			{
				if (NetworkServer.get_active())
				{
					target.AddTimedBuff(Buffs.lockDebuff, 5f - (float)buffCount, 1);
					target.AddBuff(Buffs.lockResistBuff);
				}
				Transform modelTransform = target.get_modelLocator().get_modelTransform();
				if ((bool)(Object)(object)target && (bool)modelTransform)
				{
					TemporaryOverlay val2 = ((Component)(object)target).gameObject.AddComponent<TemporaryOverlay>();
					val2.duration = 5f - (float)buffCount;
					val2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
					val2.animateShaderAlpha = true;
					val2.destroyComponentOnEnd = true;
					val2.originalMaterial = Assets.mainAssetBundle.LoadAsset<Material>("matChains");
					val2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
				}
				EntityStateMachine component = ((Component)(object)target).GetComponent<EntityStateMachine>();
				if ((Object)(object)component != null)
				{
					LockState state = new LockState
					{
						duration = 5f - (float)buffCount
					};
					component.SetState((EntityState)(object)state);
				}
			}
			if (((EntityState)this).get_isAuthority())
			{
				int num = 5 - buffCount;
				GameObject gameObject = null;
				gameObject = num switch
				{
					5 => Assets.lockEffect5s,
					4 => Assets.lockEffect4s,
					3 => Assets.lockEffect3s,
					2 => Assets.lockEffect2s,
					1 => Assets.lockEffect1s,
					_ => Assets.lockEffectBreak,
				};
				if ((bool)(Object)(object)target.get_healthComponent() && target.get_healthComponent().get_combinedHealthFraction() <= 0f)
				{
					gameObject = Assets.lockEffectBreak;
				}
				EffectData val3 = new EffectData();
				val3.rotation = Util.QuaternionSafeLookRotation(Vector3.zero);
				val3.set_origin(target.get_corePosition());
				EffectManager.SpawnEffect(gameObject, val3, true);
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
}