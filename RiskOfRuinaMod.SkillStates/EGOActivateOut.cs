using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class EGOActivateOut : BaseSkillState
{
	public static float baseDuration = 1f;

	public static float shockwaveRadius = 15f;

	public static float shockwaveForce = 8000f;

	public static float shockwaveBonusForce = 1500f;

	private float duration;

	private RedMistEmotionComponent EGOController;

	private RedMistStatTracker statTracker;

	public override void OnEnter()
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		((BaseState)this).OnEnter();
		duration = baseDuration;
		EGOController = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
		statTracker = ((EntityState)this).get_gameObject().GetComponent<RedMistStatTracker>();
		((EntityState)this).PlayAnimation("FullBody, Override", "BufferEmpty");
		((EntityState)this).PlayAnimation("FullBody, Override", "EGOActivateOut", "EGOActivate.playbackRate", duration);
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.EGOBuff);
		}
		((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.EGOActivateOutCameraParamsRedMist;
		((EntityState)this).get_cameraTargetParams().aimMode = (AimType)2;
		FireShockwave();
		if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalBlock)
		{
			((EntityState)this).get_skillLocator().utility.SetSkillOverride((object)((EntityState)this).get_skillLocator().utility, RedMist.EGOBlock, (SkillOverridePriority)4);
		}
		else if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalDodge)
		{
			((EntityState)this).get_skillLocator().utility.SetSkillOverride((object)((EntityState)this).get_skillLocator().utility, RedMist.EGODodge, (SkillOverridePriority)4);
		}
		((EntityState)this).get_skillLocator().special.SetSkillOverride((object)((EntityState)this).get_skillLocator().special, RedMist.HorizontalSlash, (SkillOverridePriority)4);
	}

	private void FireShockwave()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		Util.PlaySound("Play_Effect_Index_Unlock", ((EntityState)this).get_gameObject());
		EffectData val = new EffectData();
		val.set_origin(((EntityState)this).get_characterBody().get_corePosition());
		val.scale = 1f;
		EffectManager.SpawnEffect(statTracker.EGOActivatePrefab, val, false);
		if (((EntityState)this).get_isAuthority())
		{
			BlastAttack val2 = new BlastAttack();
			val2.attacker = ((EntityState)this).get_gameObject();
			val2.inflictor = ((EntityState)this).get_gameObject();
			val2.teamIndex = TeamComponent.GetObjectTeam(val2.attacker);
			val2.position = ((EntityState)this).get_characterBody().get_corePosition();
			val2.procCoefficient = 0f;
			val2.radius = shockwaveRadius;
			val2.baseForce = shockwaveForce;
			val2.bonusForce = Vector3.up * shockwaveBonusForce;
			val2.baseDamage = 0f;
			val2.falloffModel = (FalloffModel)0;
			val2.damageColorIndex = (DamageColorIndex)3;
			val2.attackerFiltering = (AttackerFiltering)2;
			val2.Fire();
		}
		if ((bool)(Object)(object)EGOController)
		{
			EGOController.EnterEGO();
		}
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		((EntityState)this).get_characterMotor().velocity = Vector3.zero;
		if (((EntityState)this).get_isAuthority() && ((EntityState)this).get_fixedAge() >= duration)
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override void OnExit()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		((EntityState)this).OnExit();
		((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsRedMist;
		((EntityState)this).get_cameraTargetParams().aimMode = (AimType)0;
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
		}
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)4;
	}
}
