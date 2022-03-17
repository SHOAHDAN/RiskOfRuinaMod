using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class EGOHorizontal : BaseDirectionalSkill
{
	private float hopEndTime = 0.5f;

	private float moveEndtime = 1f;

	private bool aerial = false;

	private float originalTurnSpeed;

	private Vector3 savedAimDir;

	private bool hasAimDir;

	public ShakeEmitter shakeEmitter;

	public override void OnEnter()
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		attackIndex = 1;
		hitboxName = "Horizontal";
		damageCoefficient = 20f;
		baseDuration = 2.25f;
		attackStartTime = 0.45f;
		attackEndTime = 0.6f;
		baseEarlyExitTime = 0.5f;
		hitStopDuration = 0.05f;
		swingHopVelocity = 0f;
		bonusForce = ((EntityState)this).get_characterDirection().get_forward() * 4000f + Vector3.up * 2500f;
		procCoefficient = 0.75f;
		swingSoundString = "Play_Kali_Special_Hori_Start";
		impactSound = Assets.swordHitEGOSoundGRHorizontal.get_index();
		muzzleString = "Horizontal";
		hitEffectPrefab = Assets.swordHitEffect;
		aerial = !((EntityState)this).get_characterMotor().get_isGrounded();
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
		}
		base.OnEnter();
		originalTurnSpeed = ((EntityState)this).get_characterDirection().turnSpeed;
		swingEffectPrefab = statTracker.EGOHorizontalPrefab;
		((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.HorizontalSlashCameraParamsRedMist;
		((EntityState)this).get_cameraTargetParams().aimMode = (AimType)2;
	}

	protected override void PlayAttackAnimation()
	{
		((EntityState)this).PlayCrossfade("FullBody, Override", "HorizontalSlash", "BaseAttack.playbackRate", duration, 0.1f);
	}

	protected override void PlaySwingEffect()
	{
		base.PlaySwingEffect();
	}

	protected override void OnHitEnemyAuthority()
	{
		base.OnHitEnemyAuthority();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (stopwatch <= hopEndTime)
		{
			float num = 5f;
			if (aerial)
			{
				num = 2f;
			}
			CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
			characterMotor.rootMotion += Vector3.up * (num * FlyUpState.speedCoefficientCurve.Evaluate(stopwatch / hopEndTime) * Time.fixedDeltaTime);
		}
		if (stopwatch <= moveEndtime)
		{
			float num2 = 5f;
			CharacterMotor characterMotor2 = ((EntityState)this).get_characterMotor();
			characterMotor2.rootMotion += ((EntityState)this).get_inputBank().moveVector * (num2 * FlyUpState.speedCoefficientCurve.Evaluate(stopwatch / moveEndtime) * Time.fixedDeltaTime);
			CharacterMotor characterMotor3 = ((EntityState)this).get_characterMotor();
			characterMotor3.set_moveDirection(characterMotor3.get_moveDirection() * 2f);
			((EntityState)this).get_characterDirection().turnSpeed = 0f;
			((EntityState)this).get_characterDirection().set_forward(((EntityState)this).get_inputBank().get_aimDirection());
		}
		if (stopwatch > moveEndtime)
		{
			if (hasAimDir)
			{
				((EntityState)this).get_characterDirection().set_forward(savedAimDir);
			}
			else
			{
				savedAimDir = ((EntityState)this).get_inputBank().get_aimDirection();
				hasAimDir = true;
			}
		}
		((EntityState)this).get_characterMotor().velocity.y = 0f;
	}

	protected override void FireAttack()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		base.FireAttack();
		shakeEmitter = ((EntityState)this).get_gameObject().AddComponent<ShakeEmitter>();
		shakeEmitter.amplitudeTimeDecay = true;
		shakeEmitter.duration = 0.3f;
		shakeEmitter.radius = 100f;
		shakeEmitter.scaleShakeRadiusWithLocalScale = false;
		shakeEmitter.wave = new Wave
		{
			amplitude = 0.6f,
			frequency = 25f,
			cycleOffset = 0f
		};
	}

	public override void OnExit()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
		{
			((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
		}
		((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsRedMist;
		((EntityState)this).get_cameraTargetParams().aimMode = (AimType)0;
		((EntityState)this).get_characterDirection().turnSpeed = originalTurnSpeed;
		base.OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)6;
	}
}
