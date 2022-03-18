using System.Collections.Generic;
using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{

	internal class EGOBlockCounter : BaseSkillState
	{
		public float duration = 0.6f;

		public float blinkDuration = 0.5f;

		public float attackStart = 0.25f;

		public bool invul = false;

		public bool fired = false;

		public float damageCounter = 0f;

		public float bonusMult = 1f;

		private Transform modelTransform;

		private CharacterModel characterModel;

		private Animator animator;

		private HurtBoxGroup hurtboxGroup;

		private RedMistStatTracker statTracker;

		private ParticleSystem mistEffect;

		public override void OnEnter()
		{
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			modelTransform = ((EntityState)this).GetModelTransform();
			if ((bool)modelTransform)
			{
				animator = modelTransform.GetComponent<Animator>();
				characterModel = modelTransform.GetComponent<CharacterModel>();
				hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
			}
			statTracker = ((EntityState)this).GetComponent<RedMistStatTracker>();
			if ((bool)(Object)(object)characterModel)
			{
				CharacterModel obj = characterModel;
				obj.invisibilityCount++;
			}
			if ((bool)(Object)(object)hurtboxGroup)
			{
				HurtBoxGroup val = hurtboxGroup;
				int hurtBoxesDeactivatorCounter = val.get_hurtBoxesDeactivatorCounter() + 1;
				val.set_hurtBoxesDeactivatorCounter(hurtBoxesDeactivatorCounter);
			}
			Util.PlaySound("Play_Claw_Ulti_Move", ((EntityState)this).get_gameObject());
			((EntityState)this).PlayAnimation("EGODodge", "EGODodge", "Dodge.playbackRate", blinkDuration);
			EffectData val2 = new EffectData();
			val2.rotation = Quaternion.identity;
			val2.set_origin(((EntityState)this).get_characterBody().get_corePosition());
			EffectManager.SpawnEffect(statTracker.phaseEffect, val2, true);
			ChildLocator modelChildLocator = ((EntityState)this).GetModelChildLocator();
			if ((bool)(Object)(object)modelChildLocator)
			{
				mistEffect = ((EntityState)this).GetComponent<RedMistStatTracker>().mistEffect;
				mistEffect.Play();
			}
			invul = true;
			((BaseState)this).OnEnter();
		}

		public override void FixedUpdate()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			((EntityState)this).FixedUpdate();
			if (((EntityState)this).get_fixedAge() >= attackStart && !fired)
			{
				fired = true;
				List<HurtBox> list = new List<HurtBox>();
				SphereSearch val = new SphereSearch();
				val.mask = ((LayerIndex)(ref LayerIndex.entityPrecise)).get_mask();
				val.radius = 40f;
				val.ClearCandidates();
				val.origin = ((EntityState)this).get_characterBody().get_corePosition();
				val.RefreshCandidates();
				val.FilterCandidatesByDistinctHurtBoxEntities();
				TeamMask enemyTeams = TeamMask.GetEnemyTeams(((EntityState)this).get_teamComponent().get_teamIndex());
				val.FilterCandidatesByHurtBoxTeam(enemyTeams);
				val.GetHurtBoxes(list);
				Util.PlaySound("Play_Kali_Special_Vert_Fin", ((EntityState)this).get_gameObject());
				EffectData val2 = new EffectData();
				val2.rotation = Quaternion.identity;
				val2.set_origin(((EntityState)this).get_characterBody().get_footPosition());
				EffectManager.SpawnEffect(statTracker.counterBurst, val2, true);
				foreach (HurtBox item in list)
				{
					if ((bool)(Object)(object)item.healthComponent && (bool)(Object)(object)item.healthComponent.body && (Object)(object)item.healthComponent.body != (Object)(object)((EntityState)this).get_characterBody())
					{
						DelayedDamage(item);
					}
				}
			}
			if (((EntityState)this).get_fixedAge() >= blinkDuration && invul)
			{
				if ((bool)(Object)(object)characterModel)
				{
					CharacterModel obj = characterModel;
					obj.invisibilityCount--;
				}
				if ((bool)(Object)(object)hurtboxGroup)
				{
					HurtBoxGroup val3 = hurtboxGroup;
					int hurtBoxesDeactivatorCounter = val3.get_hurtBoxesDeactivatorCounter() - 1;
					val3.set_hurtBoxesDeactivatorCounter(hurtBoxesDeactivatorCounter);
				}
				EffectData val4 = new EffectData();
				val4.rotation = Quaternion.identity;
				val4.set_origin(((EntityState)this).get_characterBody().get_corePosition());
				EffectManager.SpawnEffect(statTracker.phaseEffect, val4, true);
				invul = false;
				mistEffect.Stop();
			}
			if (((EntityState)this).get_fixedAge() >= duration && ((EntityState)this).get_isAuthority())
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		private void DelayedDamage(HurtBox target)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			if (NetworkServer.get_active())
			{
				DamageInfo val = new DamageInfo
				{
					attacker = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
					inflictor = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
					crit = ((BaseState)this).RollCrit(),
					damage = (1f + Config.redMistBuffDamage.Value * (float)((EntityState)this).get_characterBody().GetBuffCount(Buffs.RedMistBuff)) * (damageCounter * 1.5f * bonusMult),
					position = ((Component)(object)target).transform.position,
					force = Vector3.zero,
					damageType = (DamageType)32,
					damageColorIndex = (DamageColorIndex)0,
					procCoefficient = 1f
				};
				target.healthComponent.TakeDamage(val);
				GlobalEventManager.instance.OnHitEnemy(val, ((Component)(object)target.healthComponent.body).gameObject);
				GlobalEventManager.instance.OnHitAll(val, ((Component)(object)target.healthComponent.body).gameObject);
			}
			if (((EntityState)this).get_isAuthority())
			{
				EffectData val2 = new EffectData();
				val2.rotation = Random.rotation;
				val2.set_origin(target.healthComponent.body.get_corePosition());
				EffectManager.SpawnEffect(statTracker.afterimageSlash, val2, true);
			}
		}

		public override void OnExit()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			mistEffect.Stop();
			if ((bool)(Object)(object)characterModel && characterModel.invisibilityCount > 0)
			{
				CharacterModel obj = characterModel;
				obj.invisibilityCount--;
			}
			((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsRedMist;
			((EntityState)this).get_cameraTargetParams().aimMode = (AimType)0;
			((EntityState)this).OnExit();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (InterruptPriority)6;
		}
	}
}