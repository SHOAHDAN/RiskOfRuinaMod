using EntityStates;
using EntityStates.Huntress;
using RiskOfRuinaMod.Modules;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates.BaseStates
{

	public abstract class BaseChannelSpellState : BaseSkillState
	{
		public GameObject chargeEffectPrefab;

		public string chargeSoundString;

		public string startChargeSoundString = "";

		public GameObject crosshairOverridePrefab;

		public float maxSpellRadius;

		public float baseDuration = 3f;

		public Material overrideAreaIndicatorMat;

		public bool zooming = true;

		public bool centered = false;

		public bool line = false;

		private bool hasCharged;

		private GameObject defaultCrosshairPrefab;

		private CharacterCameraParams defaultCameraParams;

		private uint loopSoundInstanceId;

		private float duration { get; set; }

		private Animator animator { get; set; }

		private ChildLocator childLocator { get; set; }

		private GameObject chargeEffectInstance { get; set; }

		protected GameObject areaIndicatorInstance { get; set; }

		protected abstract BaseCastChanneledSpellState GetNextState();

		public override void OnEnter()
		{
			((BaseState)this).OnEnter();
			duration = baseDuration / (((BaseState)this).attackSpeedStat / 2f);
			animator = ((EntityState)this).GetModelAnimator();
			childLocator = ((EntityState)this).GetModelChildLocator();
			if ((bool)(Object)(object)childLocator)
			{
				Transform transform = childLocator.FindChild("HandL");
				if ((bool)transform && (bool)chargeEffectPrefab)
				{
					chargeEffectInstance = Object.Instantiate(chargeEffectPrefab, transform.position, transform.rotation);
					chargeEffectInstance.transform.parent = transform;
					ScaleParticleSystemDuration component = chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
					ObjectScaleCurve component2 = chargeEffectInstance.GetComponent<ObjectScaleCurve>();
					if ((bool)(Object)(object)component)
					{
						component.set_newDuration(duration);
					}
					if ((bool)(Object)(object)component2)
					{
						component2.timeMax = duration;
					}
				}
			}
			PlayChannelAnimation();
			if (startChargeSoundString != "")
			{
				Util.PlaySound(startChargeSoundString, ((EntityState)this).get_gameObject());
			}
			loopSoundInstanceId = Util.PlayAttackSpeedSound(chargeSoundString, ((EntityState)this).get_gameObject(), ((BaseState)this).attackSpeedStat);
			defaultCrosshairPrefab = ((EntityState)this).get_characterBody().crosshairPrefab;
			if ((bool)crosshairOverridePrefab)
			{
				((EntityState)this).get_characterBody().crosshairPrefab = crosshairOverridePrefab;
			}
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().AddBuff(Buffs.Slow50);
			}
			if ((bool)ArrowRain.areaIndicatorPrefab)
			{
				areaIndicatorInstance = Object.Instantiate(ArrowRain.areaIndicatorPrefab);
				if (line)
				{
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
					EntityState.Destroy((Object)gameObject.GetComponent<CapsuleCollider>());
					gameObject.transform.parent = areaIndicatorInstance.transform;
					gameObject.transform.localPosition = new Vector3(0f, 0.35f, 0f);
					gameObject.transform.localScale = new Vector3(0.15f, 0.4f, 0.15f);
					gameObject.GetComponent<MeshRenderer>().material = areaIndicatorInstance.GetComponentInChildren<MeshRenderer>().material;
				}
				areaIndicatorInstance.transform.localScale = Vector3.zero;
				if ((bool)overrideAreaIndicatorMat)
				{
					areaIndicatorInstance.GetComponentInChildren<MeshRenderer>().material = overrideAreaIndicatorMat;
				}
			}
			if (zooming)
			{
				defaultCameraParams = ((EntityState)this).get_cameraTargetParams().cameraParams;
				((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.channelCameraParamsArbiter;
			}
		}

		protected virtual void PlayChannelAnimation()
		{
			((EntityState)this).PlayAnimation("Gesture, Override", "ChannelSpell", "Spell.playbackRate", 0.85f);
		}

		private void UpdateAreaIndicator()
		{
			if (!areaIndicatorInstance)
			{
				return;
			}
			if (centered)
			{
				areaIndicatorInstance.transform.position = ((EntityState)this).get_transform().position;
				areaIndicatorInstance.transform.up = Vector3.up;
				return;
			}
			float num = 128f;
			Ray aimRay = ((BaseState)this).GetAimRay();
			int layerMask = 1 << LayerIndex.world.intVal;
			if (Physics.Raycast(aimRay, out var hitInfo, num, layerMask))
			{
				if (!areaIndicatorInstance.activeSelf)
				{
					areaIndicatorInstance.SetActive(value: true);
				}
				areaIndicatorInstance.transform.position = hitInfo.point;
				areaIndicatorInstance.transform.up = hitInfo.normal;
			}
			else
			{
				if (areaIndicatorInstance.activeSelf)
				{
					areaIndicatorInstance.SetActive(value: false);
				}
				areaIndicatorInstance.transform.position = aimRay.GetPoint(num);
				areaIndicatorInstance.transform.up = -aimRay.direction;
			}
		}

		public override void OnExit()
		{
			if ((bool)crosshairOverridePrefab)
			{
				((EntityState)this).get_characterBody().crosshairPrefab = defaultCrosshairPrefab;
			}
			else
			{
				((EntityState)this).get_characterBody().hideCrosshair = false;
			}
			if ((bool)areaIndicatorInstance)
			{
				EntityState.Destroy((Object)areaIndicatorInstance.gameObject);
			}
			AkSoundEngine.StopPlayingID(loopSoundInstanceId);
			if (!((EntityState)this).outer.get_destroying())
			{
				EndAnimation();
			}
			if (zooming)
			{
				((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsArbiter;
			}
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.Slow50);
			}
			if ((bool)chargeEffectInstance)
			{
				EntityState.Destroy((Object)chargeEffectInstance);
			}
			((EntityState)this).OnExit();
		}

		protected virtual void EndAnimation()
		{
			((EntityState)this).PlayAnimation("Gesture, Override", "BufferEmpty");
		}

		protected float CalcCharge()
		{
			return Mathf.Clamp01(((EntityState)this).get_fixedAge() / duration);
		}

		public override void FixedUpdate()
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			((EntityState)this).FixedUpdate();
			((EntityState)this).get_characterBody().set_isSprinting(false);
			((BaseState)this).StartAimMode(0.5f, false);
			((EntityState)this).get_characterBody().outOfCombatStopwatch = 0f;
			float num = CalcCharge();
			if ((bool)areaIndicatorInstance)
			{
				float num2 = Util.Remap(num, 0f, 1f, 0f, maxSpellRadius);
				areaIndicatorInstance.transform.localScale = new Vector3(num2, num2, num2);
			}
			if (num >= 0.75f && zooming)
			{
				((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.channelFullCameraParamsArbiter;
				((EntityState)this).get_cameraTargetParams().aimMode = (AimType)2;
			}
			if (num >= 1f && !hasCharged)
			{
				hasCharged = true;
			}
			if (((EntityState)this).get_isAuthority() && (bool)(Object)(object)((EntityState)this).get_inputBank() && ((EntityState)this).get_fixedAge() >= 0.2f && ((EntityState)this).get_inputBank().sprint.wasDown)
			{
				((EntityState)this).get_characterBody().set_isSprinting(true);
				if (zooming)
				{
					((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsArbiter;
				}
				RefundCooldown();
				((EntityState)this).outer.SetNextStateToMain();
			}
			else
			{
				if (!((EntityState)this).get_isAuthority() || ((BaseSkillState)this).IsKeyDownAuthority() || !(num >= 1f))
				{
					return;
				}
				BaseCastChanneledSpellState nextState = GetNextState();
				if ((bool)areaIndicatorInstance)
				{
					if (!areaIndicatorInstance.activeSelf)
					{
						nextState.spellPosition = Vector3.zero;
						nextState.spellRotation = Quaternion.identity;
					}
					else
					{
						nextState.spellPosition = areaIndicatorInstance.transform.position;
						nextState.spellRotation = areaIndicatorInstance.transform.rotation;
					}
				}
				else
				{
					nextState.spellPosition = ((EntityState)this).get_transform().position;
					nextState.spellRotation = areaIndicatorInstance.transform.rotation;
				}
				nextState.chainActivatorSkillSlot = ((BaseSkillState)this).get_activatorSkillSlot();
				((EntityState)this).outer.SetNextState((EntityState)(object)nextState);
			}
		}

		private void RefundCooldown()
		{
			((BaseSkillState)this).get_activatorSkillSlot().set_rechargeStopwatch(0.9f * ((BaseSkillState)this).get_activatorSkillSlot().finalRechargeInterval);
		}

		public override void Update()
		{
			((EntityState)this).Update();
			UpdateAreaIndicator();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (InterruptPriority)2;
		}
	}
}