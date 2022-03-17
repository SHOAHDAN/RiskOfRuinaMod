using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates;

internal class EGODodge : BaseSkillState
{
	public Vector3 dodgeVector;

	public float duration = 0.65f;

	public float blinkDuration = 0.3f;

	public float stockBonus = 0.05f;

	public bool aerial = false;

	public bool invul = false;

	private Transform modelTransform;

	private CharacterModel characterModel;

	private Animator animator;

	private HurtBoxGroup hurtboxGroup;

	private RedMistStatTracker statTracker;

	private ParticleSystem mistEffect;

	public override void OnEnter()
	{
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		modelTransform = ((EntityState)this).GetModelTransform();
		if ((bool)modelTransform)
		{
			animator = modelTransform.GetComponent<Animator>();
			characterModel = modelTransform.GetComponent<CharacterModel>();
			hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
		}
		if (((EntityState)this).get_skillLocator().utility.get_stock() > 1)
		{
			float num = (float)(((EntityState)this).get_skillLocator().utility.get_stock() - 1) * stockBonus;
			duration = Mathf.Clamp(duration - num, blinkDuration, duration);
		}
		if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && (bool)(Object)(object)((EntityState)this).get_characterBody().get_master() && RiskOfRuinaPlugin.KombatIsDueling(((EntityState)this).get_characterBody().get_master()))
		{
			duration += 0.2f;
		}
		statTracker = ((EntityState)this).GetComponent<RedMistStatTracker>();
		dodgeVector = ((EntityState)this).get_inputBank().moveVector;
		aerial = !((EntityState)this).get_characterMotor().get_isGrounded();
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
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		((EntityState)this).FixedUpdate();
		if (((EntityState)this).get_fixedAge() <= blinkDuration)
		{
			CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
			characterMotor.rootMotion += dodgeVector * (40f * Time.fixedDeltaTime);
			CharacterMotor characterMotor2 = ((EntityState)this).get_characterMotor();
			characterMotor2.set_moveDirection(characterMotor2.get_moveDirection() * 2f);
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
				HurtBoxGroup val = hurtboxGroup;
				int hurtBoxesDeactivatorCounter = val.get_hurtBoxesDeactivatorCounter() - 1;
				val.set_hurtBoxesDeactivatorCounter(hurtBoxesDeactivatorCounter);
			}
			EffectData val2 = new EffectData();
			val2.rotation = Quaternion.identity;
			val2.set_origin(((EntityState)this).get_characterBody().get_corePosition());
			EffectManager.SpawnEffect(statTracker.phaseEffect, val2, true);
			invul = false;
			mistEffect.Stop();
		}
		if (((EntityState)this).get_fixedAge() >= duration && ((EntityState)this).get_isAuthority())
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override void OnExit()
	{
		mistEffect.Stop();
		if ((bool)(Object)(object)characterModel && characterModel.invisibilityCount > 0)
		{
			CharacterModel obj = characterModel;
			obj.invisibilityCount--;
		}
		((EntityState)this).OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)6;
	}
}
