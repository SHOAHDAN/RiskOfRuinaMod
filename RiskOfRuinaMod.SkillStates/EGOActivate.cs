using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class EGOActivate : BaseSkillState
{
	public static float baseDuration = 1f;

	private float duration;

	private Vector3 storedPosition;

	private Animator modelAnimator;

	public override void OnEnter()
	{
		((BaseState)this).OnEnter();
		duration = baseDuration;
		modelAnimator = ((EntityState)this).GetModelAnimator();
		((EntityState)this).get_characterBody().hideCrosshair = true;
		if ((bool)modelAnimator)
		{
			modelAnimator.SetBool("isMoving", value: false);
			modelAnimator.SetBool("isSprinting", value: false);
		}
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
		}
		EntityStateMachine[] components = ((EntityState)this).get_gameObject().GetComponents<EntityStateMachine>();
		foreach (EntityStateMachine val in components)
		{
			if ((bool)(Object)(object)val)
			{
				if (val.customName == "Weapon")
				{
					val.SetNextStateToMain();
				}
				if (val.customName == "Slide")
				{
					val.SetNextStateToMain();
				}
			}
		}
		RedMistEmotionComponent component = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
		((EntityState)this).PlayAnimation("Gesture, Override", "BufferEmpty");
		((EntityState)this).PlayAnimation("FullBody, Override", "EGOActivate", "EGOActivate.playbackRate", duration);
		Util.PlaySound("Play_Kali_Change", ((EntityState)this).get_gameObject());
		((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.EGOActivateCameraParamsRedMist;
		storedPosition = ((EntityState)this).get_transform().position;
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		((EntityState)this).get_transform().position = storedPosition;
		((EntityState)this).get_characterBody().set_isSprinting(false);
		if ((bool)(Object)(object)((EntityState)this).get_characterMotor())
		{
			((EntityState)this).get_characterMotor().velocity = Vector3.zero;
		}
		if (((EntityState)this).get_isAuthority() && ((EntityState)this).get_fixedAge() >= duration)
		{
			((EntityState)this).outer.SetNextState((EntityState)(object)new EGOActivateOut());
		}
	}

	public override void OnExit()
	{
		((EntityState)this).OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)6;
	}
}
