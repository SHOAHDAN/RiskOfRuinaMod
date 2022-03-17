using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class JumpRisingAttack : BaseSkillState
{
	public int attackIndex = 1;

	public Vector2 inputVector;

	public float duration;

	protected string swingSoundString = "";

	protected string hitSoundString = "";

	protected string muzzleString = "SwingCenter";

	protected string attackAnimation = "Swing";

	protected GameObject swingEffectPrefab;

	protected GameObject hitEffectPrefab;

	protected float trueMoveSpeed => ((EntityState)this).GetComponent<RedMistStatTracker>().modifiedMoveSpeed;

	public override void OnEnter()
	{
		attackIndex = 1;
		duration = 0.4f;
		swingSoundString = "Ruina_Swipe";
		hitSoundString = "Fairy";
		muzzleString = "SwingLeft";
		swingEffectPrefab = Assets.swordSwingEffect;
		hitEffectPrefab = Assets.swordHitEffect;
		((BaseState)this).OnEnter();
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
		}
		PlayAttackAnimation();
	}

	protected void PlayAttackAnimation()
	{
		((EntityState)this).PlayCrossfade("FullBody, Override", "JumpSlashContinue", "BaseAttack.playbackRate", duration, 0.1f);
	}

	public override void FixedUpdate()
	{
		((EntityState)this).FixedUpdate();
		float num = Mathf.Clamp(0f, 10f, 0.5f * trueMoveSpeed);
		CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
		characterMotor.rootMotion += Vector3.up * (num * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / duration) * Time.fixedDeltaTime);
		((EntityState)this).get_characterMotor().velocity.y = 0f;
		CharacterMotor characterMotor2 = ((EntityState)this).get_characterMotor();
		characterMotor2.set_moveDirection(characterMotor2.get_moveDirection() * 2f);
		if (((EntityState)this).get_fixedAge() >= duration && ((EntityState)this).get_isAuthority())
		{
			((EntityState)this).outer.SetNextStateToMain();
		}
	}

	public override void OnExit()
	{
		if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
		{
			((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
		}
		((EntityState)this).OnExit();
	}
}
