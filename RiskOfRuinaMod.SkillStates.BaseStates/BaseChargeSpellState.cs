using EntityStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates.BaseStates;

public abstract class BaseChargeSpellState : BaseSkillState
{
	public GameObject chargeEffectPrefab;

	public string chargeSoundString;

	public float baseDuration = 1.5f;

	public float minBloomRadius;

	public float maxBloomRadius;

	public GameObject crosshairOverridePrefab;

	protected static readonly float minChargeDuration = 0.5f;

	private GameObject defaultCrosshairPrefab;

	private uint loopSoundInstanceId;

	private float duration { get; set; }

	private Animator animator { get; set; }

	private ChildLocator childLocator { get; set; }

	private GameObject chargeEffectInstance { get; set; }

	protected abstract BaseThrowSpellState GetNextState();

	public override void OnEnter()
	{
		((BaseState)this).OnEnter();
		duration = baseDuration / ((BaseState)this).attackSpeedStat;
		animator = ((EntityState)this).GetModelAnimator();
		childLocator = ((EntityState)this).GetModelChildLocator();
		if ((bool)(Object)(object)childLocator)
		{
			Transform transform = childLocator.FindChild("HandR");
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
		((EntityState)this).PlayAnimation("Gesture, Override", "ChannelSpell", "Spell.playbackRate", 0.4f * duration);
		loopSoundInstanceId = Util.PlayAttackSpeedSound(chargeSoundString, ((EntityState)this).get_gameObject(), ((BaseState)this).attackSpeedStat);
		defaultCrosshairPrefab = ((EntityState)this).get_characterBody().crosshairPrefab;
		if ((bool)crosshairOverridePrefab)
		{
			((EntityState)this).get_characterBody().crosshairPrefab = crosshairOverridePrefab;
		}
		((BaseState)this).StartAimMode(duration + 2f, false);
	}

	public override void OnExit()
	{
		if ((bool)(Object)(object)((EntityState)this).get_characterBody())
		{
			((EntityState)this).get_characterBody().crosshairPrefab = defaultCrosshairPrefab;
		}
		AkSoundEngine.StopPlayingID(loopSoundInstanceId);
		if (!((EntityState)this).outer.get_destroying())
		{
			((EntityState)this).PlayAnimation("Gesture, Override", "BufferEmpty");
		}
		EntityState.Destroy((Object)chargeEffectInstance);
		((EntityState)this).OnExit();
	}

	protected float CalcCharge()
	{
		return Mathf.Clamp01(((EntityState)this).get_fixedAge() / duration);
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		float charge = CalcCharge();
		if (((EntityState)this).get_isAuthority() && ((!((BaseSkillState)this).IsKeyDownAuthority() && ((EntityState)this).get_fixedAge() >= minChargeDuration) || ((EntityState)this).get_fixedAge() >= duration))
		{
			BaseThrowSpellState nextState = GetNextState();
			nextState.charge = charge;
			((EntityState)this).outer.SetNextState((EntityState)(object)nextState);
		}
	}

	public override void Update()
	{
		((EntityState)this).Update();
		((EntityState)this).get_characterBody().SetSpreadBloom(Util.Remap(CalcCharge(), 0f, 1f, minBloomRadius, maxBloomRadius), true);
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)2;
	}
}
