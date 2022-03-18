using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AncientScepter;
using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{

	public class BaseDirectionalSkill : BaseSkillState
	{
		protected string hitboxName = "Sword";

		protected DamageType damageType = (DamageType)0;

		protected float damageCoefficient = 1f;

		protected float procCoefficient = 1f;

		protected float pushForce = 300f;

		protected Vector3 bonusForce = Vector3.zero;

		protected float baseDuration = 1f;

		protected float attackStartTime = 0.2f;

		protected float attackEndTime = 0.4f;

		protected float baseEarlyExitTime = 0.4f;

		protected float hitStopDuration = 0.05f;

		protected float attackRecoil = 0f;

		protected float swingHopVelocity = 0f;

		protected bool cancelled = false;

		public int attackIndex = 1;

		protected string swingSoundString = "";

		protected string hitSoundString = "";

		protected string muzzleString = "SwingCenter";

		protected string attackAnimation = "Swing";

		protected GameObject swingEffectPrefab;

		protected GameObject hitEffectPrefab;

		protected NetworkSoundEventIndex impactSound;

		protected TemporaryOverlay iframeOverlay;

		private float earlyExitTime;

		public float duration;

		protected bool hasFired;

		private float hitPauseTimer;

		private OverlapAttack attack;

		protected bool inHitPause;

		private bool hasHopped;

		protected float stopwatch;

		protected Animator animator;

		private HitStopCachedState hitStopCachedState;

		private Vector3 storedVelocity;

		public Vector2 inputVector;

		protected bool inAir;

		protected RedMistEmotionComponent emotionComponent;

		protected RedMistStatTracker statTracker;

		protected float trueMoveSpeed => ((EntityState)this).GetComponent<RedMistStatTracker>().modifiedMoveSpeed;

		protected float trueAttackSpeed => ((EntityState)this).GetComponent<RedMistStatTracker>().modifiedAttackSpeed;

		protected float trueDamage => ((BaseState)this).damageStat;

		public override void OnEnter()
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			((BaseState)this).OnEnter();
			emotionComponent = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
			statTracker = ((EntityState)this).get_gameObject().GetComponent<RedMistStatTracker>();
			duration = baseDuration / trueAttackSpeed;
			earlyExitTime = baseEarlyExitTime / trueAttackSpeed;
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
			if (RiskOfRuinaPlugin.ancientScepterInstalled)
			{
				AncientScepterSetup();
			}
			PlayAttackAnimation();
			attack = new OverlapAttack();
			attack.damageType = damageType;
			attack.attacker = ((EntityState)this).get_gameObject();
			attack.inflictor = ((EntityState)this).get_gameObject();
			attack.teamIndex = ((BaseState)this).GetTeam();
			attack.damage = damageCoefficient * trueDamage;
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
			((EntityState)this).PlayCrossfade("Gesture, Override", attackAnimation, "BaseAttack.playbackRate", duration, 0.05f);
		}

		protected virtual void EvaluateInput()
		{
			Vector3 moveVector = ((EntityState)this).get_inputBank().moveVector;
			Vector3 aimDirection = ((EntityState)this).get_inputBank().get_aimDirection();
			Vector3 normalized = new Vector3(aimDirection.x, 0f, aimDirection.z).normalized;
			Vector3 up = ((EntityState)this).get_transform().up;
			Vector3 normalized2 = Vector3.Cross(up, normalized).normalized;
			inputVector = new Vector2(Vector3.Dot(moveVector, normalized), Vector3.Dot(moveVector, normalized2));
			inAir = !((EntityState)this).get_characterMotor().get_isGrounded();
		}

		public override void OnExit()
		{
			((EntityState)this).OnExit();
			animator.SetBool("attacking", value: false);
		}

		protected virtual void PlaySwingEffect()
		{
			EffectManager.SimpleMuzzleFlash(swingEffectPrefab, ((EntityState)this).get_gameObject(), muzzleString, true);
		}

		protected virtual void OnHitEnemyAuthority()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (!inHitPause && hitStopDuration > 0f)
			{
				storedVelocity = ((EntityState)this).get_characterMotor().velocity;
				hitStopCachedState = ((BaseState)this).CreateHitStopCachedState(((EntityState)this).get_characterMotor(), animator, "BaseAttack.playbackRate");
				hitPauseTimer = hitStopDuration / trueAttackSpeed;
				inHitPause = true;
			}
		}

		protected virtual void FireAttack()
		{
			if (!hasFired)
			{
				hasFired = true;
				Util.PlayAttackSpeedSound(swingSoundString, ((EntityState)this).get_gameObject(), trueAttackSpeed);
				if (((EntityState)this).get_isAuthority())
				{
					PlaySwingEffect();
					((BaseState)this).AddRecoil(-1f * attackRecoil, -2f * attackRecoil, -0.5f * attackRecoil, 0.5f * attackRecoil);
				}
			}
			if (!((EntityState)this).get_isAuthority())
			{
				return;
			}
			if (!hasHopped)
			{
				if ((bool)(UnityEngine.Object)(object)((EntityState)this).get_characterMotor() && !((EntityState)this).get_characterMotor().get_isGrounded() && swingHopVelocity > 0f)
				{
					((BaseState)this).SmallHop(((EntityState)this).get_characterMotor(), swingHopVelocity);
				}
				hasHopped = true;
			}
			if (attack.Fire((List<HurtBox>)null))
			{
				OnHitEnemyAuthority();
			}
		}

		public override void FixedUpdate()
		{
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			if (((EntityState)this).get_inputBank().skill3.down)
			{
				if (emotionComponent.inEGO)
				{
					EntityStateMachine val = null;
					EntityStateMachine[] components = ((EntityState)this).get_gameObject().GetComponents<EntityStateMachine>();
					foreach (EntityStateMachine val2 in components)
					{
						if ((bool)(UnityEngine.Object)(object)val2 && val2.customName == "Slide")
						{
							val = val2;
						}
					}
					if ((UnityEngine.Object)(object)val != null && val.CanInterruptState((InterruptPriority)2))
					{
						if ((UnityEngine.Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (UnityEngine.Object)(object)RedMist.NormalBlock)
						{
							val.SetNextState((EntityState)(object)new EGOBlock());
							storedVelocity = ((EntityState)this).get_characterMotor().velocity;
							hitStopCachedState = ((BaseState)this).CreateHitStopCachedState(((EntityState)this).get_characterMotor(), animator, "BaseAttack.playbackRate");
							hitPauseTimer = 0.35f;
							inHitPause = true;
						}
						else if ((UnityEngine.Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (UnityEngine.Object)(object)RedMist.NormalDodge)
						{
							val.SetNextState((EntityState)(object)new EGODodge());
							storedVelocity = ((EntityState)this).get_characterMotor().velocity;
							hitStopCachedState = ((BaseState)this).CreateHitStopCachedState(((EntityState)this).get_characterMotor(), animator, "BaseAttack.playbackRate");
							hitPauseTimer = 0.3f;
							inHitPause = true;
						}
					}
				}
				else
				{
					if ((UnityEngine.Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (UnityEngine.Object)(object)RedMist.NormalBlock)
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new Block());
						return;
					}
					if ((UnityEngine.Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (UnityEngine.Object)(object)RedMist.NormalDodge)
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
					inputVector = inputVector
				});
			}
			else if (!emotionComponent.inEGO && ((EntityState)this).get_skillLocator().special.CanExecute() && ((EntityState)this).get_skillLocator().special.get_stock() > 0 && ((EntityState)this).get_inputBank().skill4.down)
			{
				((EntityState)this).outer.SetNextState((EntityState)(object)new EGOActivate());
			}
			else if (((EntityState)this).get_skillLocator().secondary.get_stock() > 0 && ((EntityState)this).get_skillLocator().secondary.CanExecute() && ((EntityState)this).get_inputBank().skill2.down)
			{
				GenericSkill secondary = ((EntityState)this).get_skillLocator().secondary;
				int stock = secondary.get_stock();
				secondary.set_stock(stock - 1);
				((EntityState)this).outer.SetNextState((EntityState)(object)new Onrush
				{
					chained = false
				});
			}
			((EntityState)this).FixedUpdate();
			EvaluateInput();
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
					animator.SetFloat("BaseAttack.playbackRate", 0f);
				}
			}
			if (stopwatch >= duration * attackStartTime && stopwatch <= duration * attackEndTime)
			{
				FireAttack();
			}
			if (stopwatch >= duration - earlyExitTime && ((EntityState)this).get_isAuthority() && (bool)(UnityEngine.Object)(object)((EntityState)this).get_inputBank() && (((EntityState)this).get_inputBank().skill1.down || ((EntityState)this).get_inputBank().jump.down || inputVector != Vector2.zero || (emotionComponent.inEGO && ((EntityState)this).get_skillLocator().special.get_stock() > 0 && ((EntityState)this).get_inputBank().skill4.down)))
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

		protected virtual void SetNextState()
		{
			if (((EntityState)this).get_inputBank().skill1.down)
			{
				if (inAir)
				{
					if (inputVector.x < -0.5f)
					{
						if (emotionComponent.inEGO)
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOAirBackAttack
							{
								attackIndex = 1,
								inputVector = inputVector
							});
						}
						else
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new AirBackAttack
							{
								attackIndex = 1,
								inputVector = inputVector
							});
						}
					}
					else if (emotionComponent.inEGO)
					{
						if (((object)this).GetType() == typeof(EGOAirBasicAttack))
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOAirBasicAttack
							{
								attackIndex = attackIndex + 1,
								inputVector = inputVector
							});
						}
						else
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOAirBasicAttack
							{
								attackIndex = 1,
								inputVector = inputVector
							});
						}
					}
					else if (((object)this).GetType() == typeof(AirBasicAttack))
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new AirBasicAttack
						{
							attackIndex = attackIndex + 1,
							inputVector = inputVector
						});
					}
					else
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new AirBasicAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
				}
				else if (((EntityState)this).get_inputBank().jump.down)
				{
					if (emotionComponent.inEGO)
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new EGOJumpAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
					else
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new JumpAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
				}
				else if (inputVector.x > 0.5f)
				{
					if (emotionComponent.inEGO)
					{
						if (((object)this).GetType() == typeof(EGOForwardAttack))
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOForwardAttack
							{
								attackIndex = attackIndex + 1,
								inputVector = inputVector
							});
						}
						else
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOForwardAttack
							{
								attackIndex = 1,
								inputVector = inputVector
							});
						}
					}
					else if (((object)this).GetType() == typeof(ForwardAttack))
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new ForwardAttack
						{
							attackIndex = attackIndex + 1,
							inputVector = inputVector
						});
					}
					else
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new ForwardAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
				}
				else if (inputVector.x < -0.5f)
				{
					if (emotionComponent.inEGO)
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new EGOBackAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
					else
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new BackAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
				}
				else if (inputVector.y > 0.5f || inputVector.y < -0.5f)
				{
					if (emotionComponent.inEGO)
					{
						if (((object)this).GetType() == typeof(EGOSideAttack))
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOSideAttack
							{
								attackIndex = attackIndex + 1,
								inputVector = inputVector
							});
						}
						else
						{
							((EntityState)this).outer.SetNextState((EntityState)(object)new EGOSideAttack
							{
								attackIndex = 1,
								inputVector = inputVector
							});
						}
					}
					else if (((object)this).GetType() == typeof(SideAttack))
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new SideAttack
						{
							attackIndex = attackIndex + 1,
							inputVector = inputVector
						});
					}
					else
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new SideAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
				}
				else if (emotionComponent.inEGO)
				{
					if (((object)this).GetType() == typeof(EGOBasicAttack))
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new EGOBasicAttack
						{
							attackIndex = attackIndex + 1,
							inputVector = inputVector
						});
					}
					else
					{
						((EntityState)this).outer.SetNextState((EntityState)(object)new EGOBasicAttack
						{
							attackIndex = 1,
							inputVector = inputVector
						});
					}
				}
				else if (((object)this).GetType() == typeof(BasicAttack))
				{
					((EntityState)this).outer.SetNextState((EntityState)(object)new BasicAttack
					{
						attackIndex = attackIndex + 1,
						inputVector = inputVector
					});
				}
				else
				{
					((EntityState)this).outer.SetNextState((EntityState)(object)new BasicAttack
					{
						attackIndex = 1,
						inputVector = inputVector
					});
				}
			}
			else if (((EntityState)this).get_inputBank().jump.down)
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
			else if (inputVector != Vector2.zero)
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BufferEmpty");
				animator.SetBool("isMoving", value: true);
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		protected void AddOverlay(float duration)
		{
			if (Config.iframeOverlay.Value)
			{
				iframeOverlay = ((Component)(object)((EntityState)this).get_characterBody()).gameObject.AddComponent<TemporaryOverlay>();
				iframeOverlay.duration = duration;
				iframeOverlay.alphaCurve = AnimationCurve.Constant(0f, duration, 0.1f);
				iframeOverlay.animateShaderAlpha = true;
				iframeOverlay.destroyComponentOnEnd = true;
				iframeOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
				iframeOverlay.AddToCharacerModel(((EntityState)this).get_modelLocator().get_modelTransform().GetComponent<CharacterModel>());
			}
		}

		protected void RemoveOverlay()
		{
			if ((bool)(UnityEngine.Object)(object)iframeOverlay)
			{
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)iframeOverlay);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private void AncientScepterSetup()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (((EntityState)this).get_characterBody().get_inventory().GetItemCount(ItemBase<AncientScepterItem>.instance.ItemDef) > 0)
			{
				damageType = (DamageType)524288;
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
			writer.Write(inputVector);
			writer.Write(attackIndex);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			((BaseSkillState)this).OnDeserialize(reader);
			inputVector = reader.ReadVector2();
			attackIndex = reader.ReadInt32();
		}
	}
}