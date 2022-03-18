using System.Collections.Generic;
using System.Linq;
using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{

	public class Onrush : BaseSkillState
	{
		public bool chained = false;

		public int chainNum = 0;

		public bool autoAim = false;

		private float startTime;

		private bool hasFired;

		private TargetTracker tracker;

		private RedMistEmotionComponent emotionComponent;

		private CharacterBody target = null;

		private NetworkInstanceId targetID;

		private bool targetIsValid = false;

		protected bool inAir;

		private float lungeDistance;

		private float lungeDuration;

		private float lungeStartTime;

		private float cooldownStartTime;

		private float cooldownDuration;

		private bool firstDash = true;

		private bool lunging = false;

		private bool cooldown = false;

		private Vector3 lungeTarget = Vector3.zero;

		private Vector3 dirToLungeTarget = Vector3.zero;

		private ParticleSystem mistEffect;

		private bool dud = false;

		private float originalTurnSpeed;

		protected float trueMoveSpeed => ((EntityState)this).GetComponent<RedMistStatTracker>().modifiedMoveSpeed;

		protected float trueAttackSpeed => ((EntityState)this).GetComponent<RedMistStatTracker>().modifiedAttackSpeed;

		protected float trueDamage => ((BaseState)this).damageStat;

		public override void OnEnter()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			tracker = ((EntityState)this).GetComponent<TargetTracker>();
			emotionComponent = ((EntityState)this).GetComponent<RedMistEmotionComponent>();
			originalTurnSpeed = ((EntityState)this).get_characterDirection().turnSpeed;
			((EntityState)this).get_characterDirection().turnSpeed = 0f;
			mistEffect = ((EntityState)this).GetComponent<RedMistStatTracker>().mistEffect;
			if (autoAim)
			{
				List<HurtBox> list = new List<HurtBox>();
				SphereSearch val = new SphereSearch();
				val.mask = ((LayerIndex)(ref LayerIndex.entityPrecise)).get_mask();
				val.radius = tracker.maxTrackingDistance;
				val.ClearCandidates();
				val.origin = ((EntityState)this).get_characterBody().get_corePosition();
				val.RefreshCandidates();
				val.FilterCandidatesByDistinctHurtBoxEntities();
				TeamMask enemyTeams = TeamMask.GetEnemyTeams(((EntityState)this).get_teamComponent().get_teamIndex());
				((TeamMask)(ref enemyTeams)).RemoveTeam((TeamIndex)0);
				if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive())
				{
					((TeamMask)(ref enemyTeams)).AddTeam((TeamIndex)0);
				}
				val.FilterCandidatesByHurtBoxTeam(enemyTeams);
				val.OrderCandidatesByDistance();
				val.GetHurtBoxes(list);
				List<HurtBox> list2 = list.OrderBy((HurtBox o) => o.healthComponent.health).ToList();
				if (list2.Count <= 0)
				{
					dud = true;
					if (((EntityState)this).get_skillLocator().secondary.get_stock() < ((EntityState)this).get_skillLocator().secondary.get_maxStock())
					{
						((EntityState)this).get_skillLocator().secondary.AddOneStock();
					}
					if (((EntityState)this).get_isAuthority())
					{
						((EntityState)this).outer.SetNextStateToMain();
					}
					return;
				}
				foreach (HurtBox item in list2)
				{
					if ((bool)(Object)(object)item.healthComponent && (bool)(Object)(object)item.healthComponent.body && !Physics.Linecast(((EntityState)this).get_characterBody().get_corePosition(), item.healthComponent.body.get_corePosition(), 2048))
					{
						target = item.healthComponent.body;
						break;
					}
				}
				if ((Object)(object)target == null)
				{
					dud = true;
					if (((EntityState)this).get_skillLocator().secondary.get_stock() < ((EntityState)this).get_skillLocator().secondary.get_maxStock())
					{
						((EntityState)this).get_skillLocator().secondary.AddOneStock();
					}
					if (((EntityState)this).get_isAuthority())
					{
						((EntityState)this).outer.SetNextStateToMain();
					}
					return;
				}
			}
			else if ((bool)(Object)(object)tracker.GetTrackingTarget())
			{
				target = tracker.GetTrackingTarget();
			}
			lungeDistance = 10f;
			lungeDuration = 0.3f;
			startTime = 0.4f;
			cooldownDuration = 0.8f;
			if (chained)
			{
				((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.HorizontalSlashCameraParamsRedMist;
				((EntityState)this).get_cameraTargetParams().aimMode = (AimType)2;
				startTime = 0f;
				firstDash = false;
				if (chainNum > 5)
				{
					lungeDuration = 0.2f;
				}
				if (NetworkServer.get_active())
				{
					((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
				}
				if (emotionComponent.inEGO)
				{
					((EntityState)this).PlayCrossfade("FullBody, Override", "EGOOnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
					mistEffect.Play();
				}
				else
				{
					((EntityState)this).PlayCrossfade("FullBody, Override", "OnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
				}
			}
			if (((EntityState)this).get_isAuthority())
			{
				TargetSetup();
			}
			((BaseState)this).OnEnter();
		}

		private void TargetSetup()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if ((bool)(Object)(object)target && (bool)(Object)(object)target.get_healthComponent() && target.get_healthComponent().get_alive())
			{
				targetID = ((NetworkBehaviour)target).get_netId();
				targetIsValid = true;
				if (emotionComponent.inEGO)
				{
					if (startTime != 0f)
					{
						((EntityState)this).PlayCrossfade("FullBody, Override", "EGOOnrush", "Onrush.playbackRate", startTime, 0.1f);
					}
				}
				else if (startTime != 0f)
				{
					((EntityState)this).PlayCrossfade("FullBody, Override", "Onrush", "Onrush.playbackRate", startTime, 0.1f);
				}
			}
			else
			{
				dud = true;
				if (((EntityState)this).get_skillLocator().secondary.get_stock() < ((EntityState)this).get_skillLocator().secondary.get_maxStock())
				{
					((EntityState)this).get_skillLocator().secondary.AddOneStock();
				}
				if (((EntityState)this).get_isAuthority())
				{
					((EntityState)this).outer.SetNextStateToMain();
				}
			}
		}

		public override void OnExit()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			mistEffect.Stop();
			((EntityState)this).get_characterDirection().turnSpeed = originalTurnSpeed;
			if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
			}
			((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsRedMist;
			((EntityState)this).get_cameraTargetParams().aimMode = (AimType)0;
			((EntityState)this).OnExit();
		}

		private void Fire()
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected O, but got Unknown
			if (hasFired || !(Object)(object)target || !(Object)(object)target.get_healthComponent())
			{
				return;
			}
			hasFired = true;
			if (targetIsValid)
			{
				if ((bool)(Object)(object)target.get_healthComponent().body && ((EntityState)this).get_isAuthority())
				{
					EffectData val = new EffectData();
					val.set_origin(target.get_healthComponent().body.get_corePosition());
					val.scale = 1f;
					val.rotation = Quaternion.LookRotation(dirToLungeTarget);
					EffectManager.SpawnEffect(((EntityState)this).GetComponent<RedMistStatTracker>().slashPrefab, val, true);
				}
				if (NetworkServer.get_active())
				{
					float damage = trueDamage * 4f;
					DamageInfo val2 = new DamageInfo
					{
						attacker = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
						inflictor = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
						crit = ((BaseState)this).RollCrit(),
						damage = damage,
						position = target.transform.position,
						force = Vector3.zero,
						damageType = (DamageType)0,
						damageColorIndex = (DamageColorIndex)0,
						procCoefficient = 1f
					};
					target.get_healthComponent().TakeDamage(val2);
					GlobalEventManager.instance.OnHitEnemy(val2, ((Component)(object)target.get_healthComponent().body).gameObject);
					GlobalEventManager.instance.OnHitAll(val2, ((Component)(object)target.get_healthComponent().body).gameObject);
				}
			}
		}

		public override void FixedUpdate()
		{
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			if (((EntityState)this).get_inputBank().skill3.down)
			{
				if (emotionComponent.inEGO)
				{
					EntityStateMachine val = null;
					EntityStateMachine[] components = ((EntityState)this).get_gameObject().GetComponents<EntityStateMachine>();
					foreach (EntityStateMachine val2 in components)
					{
						if ((bool)(Object)(object)val2 && val2.customName == "Slide")
						{
							val = val2;
						}
					}
					if ((Object)(object)val != null && val.CanInterruptState((InterruptPriority)2))
					{
						if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalBlock)
						{
							val.SetNextState((EntityState)(object)new EGOBlock());
							((EntityState)this).outer.SetNextStateToMain();
						}
						else if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalDodge)
						{
							val.SetNextState((EntityState)(object)new EGODodge());
							((EntityState)this).outer.SetNextStateToMain();
						}
					}
				}
				else
				{
					if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalBlock)
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new Block());
						return;
					}
					if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalDodge)
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new Dodge());
						return;
					}
				}
			}
			else if (emotionComponent.inEGO && ((EntityState)this).get_skillLocator().special.get_stock() > 0 && ((EntityState)this).get_inputBank().skill4.down)
			{
				GenericSkill special = ((EntityState)this).get_skillLocator().special;
				int stock = special.get_stock();
				special.set_stock(stock - 1);
				((EntityState)this).outer.SetNextState((EntityState)(object)new EGOHorizontal
				{
					attackIndex = 1,
					inputVector = Vector3.zero
				});
			}
			else if (!emotionComponent.inEGO && ((EntityState)this).get_skillLocator().special.CanExecute() && ((EntityState)this).get_skillLocator().special.get_stock() > 0 && ((EntityState)this).get_inputBank().skill4.down)
			{
				((EntityState)this).outer.SetNextState((EntityState)(object)new EGOActivate());
			}
			((EntityState)this).FixedUpdate();
			if (NetworkServer.get_active() && !targetIsValid)
			{
				GameObject gameObject = NetworkServer.FindLocalObject(targetID);
				if ((bool)gameObject && (bool)(Object)(object)gameObject.GetComponent<CharacterBody>())
				{
					target = gameObject.GetComponent<CharacterBody>();
					TargetSetup();
				}
			}
			if (!targetIsValid)
			{
				return;
			}
			if (hasFired)
			{
				if (lunging)
				{
					Lunge();
				}
				if (cooldown)
				{
					Cooldown();
				}
				return;
			}
			if ((bool)(Object)(object)target && (bool)(Object)(object)target.get_healthComponent() && target.get_healthComponent().get_alive())
			{
				if (((EntityState)this).get_fixedAge() >= startTime)
				{
					Dash();
				}
				return;
			}
			if (((EntityState)this).get_skillLocator().secondary.get_stock() < ((EntityState)this).get_skillLocator().secondary.get_maxStock())
			{
				((EntityState)this).get_skillLocator().secondary.AddOneStock();
			}
			if (((EntityState)this).get_fixedAge() >= startTime && !dud)
			{
				hasFired = true;
				lunging = false;
				cooldown = true;
				cooldownStartTime = ((EntityState)this).get_fixedAge();
				cooldownDuration = 0.05f;
			}
			else if (((EntityState)this).get_isAuthority())
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		private void Dash()
		{
			if (firstDash)
			{
				if (NetworkServer.get_active())
				{
					((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
				}
				if (emotionComponent.inEGO)
				{
					mistEffect.Play();
					Util.PlaySound("Play_Effect_Index_Unlock_Short", ((EntityState)this).get_gameObject());
					((EntityState)this).PlayCrossfade("FullBody, Override", "EGOOnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
				}
				else
				{
					Util.PlaySound("Ruina_Swipe", ((EntityState)this).get_gameObject());
					((EntityState)this).PlayCrossfade("FullBody, Override", "OnrushContinue", "Onrush.playbackRate", 20f, 0.1f);
				}
				firstDash = false;
			}
			if (((EntityState)this).get_inputBank().jump.down && ((EntityState)this).get_isAuthority())
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
			Vector3 vector = target.get_healthComponent().body.get_corePosition() + (((EntityState)this).get_characterBody().get_corePosition() - target.get_healthComponent().body.get_corePosition()).normalized * lungeDistance;
			Vector3 normalized = (vector - ((EntityState)this).get_characterBody().get_corePosition()).normalized;
			float num = 10f;
			if (emotionComponent.inEGO)
			{
				num = 12f;
			}
			if (chainNum > 6)
			{
				num = 20f;
			}
			else if (chainNum >= 3)
			{
				num = 16f;
			}
			CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
			characterMotor.rootMotion += normalized * trueMoveSpeed * num * Time.fixedDeltaTime;
			((EntityState)this).get_characterMotor().velocity = Vector3.zero;
			((EntityState)this).get_characterDirection().set_forward(normalized);
			float num2 = Vector3.Distance(((EntityState)this).get_characterBody().get_corePosition(), target.get_healthComponent().body.get_corePosition());
			if (num2 <= lungeDistance)
			{
				Util.PlaySound("Play_Kali_Special_Cut", ((EntityState)this).get_gameObject());
				if (emotionComponent.inEGO)
				{
					((EntityState)this).PlayCrossfade("FullBody, Override", "EGOOnrushFinish", "Onrush.playbackRate", lungeDuration + cooldownDuration, 0.1f);
				}
				else
				{
					((EntityState)this).PlayCrossfade("FullBody, Override", "OnrushFinish", "Onrush.playbackRate", lungeDuration + cooldownDuration, 0.1f);
				}
				lunging = true;
				lungeTarget = target.get_healthComponent().body.get_corePosition() - (((EntityState)this).get_characterBody().get_corePosition() - target.get_healthComponent().body.get_corePosition()).normalized * lungeDistance;
				dirToLungeTarget = (lungeTarget - ((EntityState)this).get_characterBody().get_corePosition()).normalized;
				lungeStartTime = ((EntityState)this).get_fixedAge();
				Fire();
			}
		}

		private void Lunge()
		{
			if (((EntityState)this).get_fixedAge() - lungeStartTime < lungeDuration)
			{
				CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
				characterMotor.rootMotion += dirToLungeTarget * (20f * FlyUpState.speedCoefficientCurve.Evaluate((((EntityState)this).get_fixedAge() - lungeStartTime) / lungeDuration) * Time.fixedDeltaTime);
				((EntityState)this).get_characterMotor().velocity = Vector3.zero;
				((EntityState)this).get_characterDirection().set_forward(dirToLungeTarget);
				return;
			}
			if (targetIsValid)
			{
				if ((bool)(Object)(object)target && (bool)(Object)(object)target.get_healthComponent())
				{
					if (target.get_healthComponent().get_combinedHealthFraction() <= 0f && ((EntityState)this).get_skillLocator().secondary.get_stock() < ((EntityState)this).get_skillLocator().secondary.get_maxStock())
					{
						((EntityState)this).get_skillLocator().secondary.AddOneStock();
					}
				}
				else if (((EntityState)this).get_skillLocator().secondary.get_stock() < ((EntityState)this).get_skillLocator().secondary.get_maxStock())
				{
					((EntityState)this).get_skillLocator().secondary.AddOneStock();
				}
			}
			if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
			}
			lunging = false;
			cooldown = true;
			cooldownStartTime = ((EntityState)this).get_fixedAge();
		}

		private void Cooldown()
		{
			if (((EntityState)this).get_fixedAge() - cooldownStartTime < cooldownDuration)
			{
				mistEffect.Stop();
				((EntityState)this).get_transform().position = lungeTarget;
				((EntityState)this).get_characterMotor().velocity = Vector3.zero;
				((EntityState)this).get_characterDirection().set_forward(dirToLungeTarget);
				if (((EntityState)this).get_isAuthority() && emotionComponent.inEGO && ((EntityState)this).get_skillLocator().secondary.get_stock() > 0)
				{
					GenericSkill secondary = ((EntityState)this).get_skillLocator().secondary;
					int stock = secondary.get_stock();
					secondary.set_stock(stock - 1);
					((EntityState)this).outer.SetNextState((EntityState)(object)new Onrush
					{
						chained = true,
						chainNum = chainNum + 1,
						autoAim = true
					});
				}
				else if ((((EntityState)this).get_isAuthority() && ((EntityState)this).get_inputBank().jump.down) || ((EntityState)this).get_inputBank().skill1.down || ((EntityState)this).get_inputBank().skill2.down)
				{
					SetNextState();
				}
			}
			else if (((EntityState)this).get_isAuthority())
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

		protected virtual void SetNextState()
		{
			if (((EntityState)this).get_inputBank().skill2.down)
			{
				if (((EntityState)this).get_skillLocator().secondary.get_stock() > 0)
				{
					GenericSkill secondary = ((EntityState)this).get_skillLocator().secondary;
					int stock = secondary.get_stock();
					secondary.set_stock(stock - 1);
					((EntityState)this).outer.SetNextState((EntityState)(object)new Onrush
					{
						chained = true
					});
				}
			}
			else if (((EntityState)this).get_inputBank().jump.down)
			{
				((EntityState)this).outer.SetNextStateToMain();
				((BaseState)this).SmallHop(((EntityState)this).get_characterMotor(), 8f);
			}
			else
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((BaseSkillState)this).OnSerialize(writer);
			writer.Write(chained);
			writer.Write(autoAim);
			writer.Write(targetID);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			((BaseSkillState)this).OnDeserialize(reader);
			chained = reader.ReadBoolean();
			autoAim = reader.ReadBoolean();
			targetID = reader.ReadNetworkId();
		}
	}

}