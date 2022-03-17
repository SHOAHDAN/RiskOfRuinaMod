using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates;

public class ScepterCastShockwave : BaseCastChanneledSpellState
{
	private Vector3 storedPosition;

	private ShakeEmitter shakeEmitter;

	public override void OnEnter()
	{
		baseDuration = 0.75f;
		baseInterval = 0f;
		centered = true;
		muzzleString = "HandR";
		muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
		projectilePrefabs.Enqueue(Projectiles.shockwaveScepterPrefab);
		castSoundString = "Play_Abiter_Special_Boom";
		storedPosition = ((EntityState)this).get_transform().position;
		base.OnEnter();
	}

	protected override void PlayCastAnimation()
	{
		((EntityState)this).PlayAnimation("Gesture, Override", "CastShockwave", "Shockwave.playbackRate", 0.25f);
	}

	protected override void Fire()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		if (projectilePrefabs.Count > 0)
		{
			shakeEmitter = ((EntityState)this).get_gameObject().AddComponent<ShakeEmitter>();
			shakeEmitter.amplitudeTimeDecay = true;
			shakeEmitter.duration = 1.5f;
			shakeEmitter.radius = 200f;
			shakeEmitter.scaleShakeRadiusWithLocalScale = false;
			shakeEmitter.wave = new Wave
			{
				amplitude = 0.2f,
				frequency = 20f,
				cycleOffset = 0f
			};
			float radius = 60f;
			float num = 50f;
			if (((EntityState)this).get_isAuthority())
			{
				BlastAttack val = new BlastAttack();
				val.attacker = ((EntityState)this).get_gameObject();
				val.inflictor = ((EntityState)this).get_gameObject();
				val.teamIndex = TeamComponent.GetObjectTeam(val.attacker);
				val.position = ((EntityState)this).get_transform().position;
				val.procCoefficient = 0.5f;
				val.radius = radius;
				val.baseForce = 8000f;
				val.bonusForce = Vector3.zero;
				val.baseDamage = num * ((BaseState)this).damageStat;
				val.falloffModel = (FalloffModel)0;
				val.damageColorIndex = (DamageColorIndex)0;
				val.attackerFiltering = (AttackerFiltering)2;
				val.crit = ((BaseState)this).RollCrit();
				val.damageType = (DamageType)32;
				val.Fire();
			}
			base.Fire();
		}
	}

	public override void OnExit()
	{
		base.OnExit();
		((EntityState)this).PlayAnimation("Gesture, Override", "CastShockwaveEnd", "Shockwave.playbackRate", 0.8f);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		((EntityState)this).get_transform().position = storedPosition;
		if ((bool)(Object)(object)((EntityState)this).get_characterMotor())
		{
			((EntityState)this).get_characterMotor().velocity = Vector3.zero;
		}
	}
}
