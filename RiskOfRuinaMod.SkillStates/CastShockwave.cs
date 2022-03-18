using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	public class CastShockwave : BaseCastChanneledSpellState
	{
		private Vector3 storedPosition;

		private int shockwaveNum = 0;

		private ShakeEmitter shakeEmitter;

		public override void OnEnter()
		{
			baseDuration = 3.5f;
			baseInterval = 1.5f;
			centered = true;
			muzzleString = "HandR";
			muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
			projectilePrefabs.Enqueue(Projectiles.shockwaveSmallPrefab);
			projectilePrefabs.Enqueue(Projectiles.shockwaveMediumPrefab);
			projectilePrefabs.Enqueue(Projectiles.shockwaveLargePrefab);
			castSoundString = "Play_Binah_Shockwave";
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
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			if (projectilePrefabs.Count > 0)
			{
				shakeEmitter = ((EntityState)this).get_gameObject().AddComponent<ShakeEmitter>();
				shakeEmitter.amplitudeTimeDecay = true;
				shakeEmitter.duration = 1.5f;
				shakeEmitter.radius = 100f;
				shakeEmitter.scaleShakeRadiusWithLocalScale = false;
				shakeEmitter.wave = new Wave
				{
					amplitude = 0.1f,
					frequency = 10f,
					cycleOffset = 0f
				};
				float radius = 40f;
				if (shockwaveNum == 1)
				{
					radius = 40f;
				}
				if (shockwaveNum == 2)
				{
					radius = 40f;
				}
				float num = 5f;
				if (shockwaveNum == 1)
				{
					num = 10f;
				}
				if (shockwaveNum == 2)
				{
					num = 15f;
				}
				if (((EntityState)this).get_isAuthority())
				{
					BlastAttack val = new BlastAttack();
					val.attacker = ((EntityState)this).get_gameObject();
					val.inflictor = ((EntityState)this).get_gameObject();
					val.teamIndex = TeamComponent.GetObjectTeam(val.attacker);
					val.position = ((EntityState)this).get_transform().position;
					val.procCoefficient = 1f;
					val.radius = radius;
					val.baseForce = 2000f;
					val.bonusForce = Vector3.zero;
					val.baseDamage = num * ((BaseState)this).damageStat;
					val.falloffModel = (FalloffModel)0;
					val.damageColorIndex = (DamageColorIndex)0;
					val.attackerFiltering = (AttackerFiltering)2;
					val.crit = ((BaseState)this).RollCrit();
					val.damageType = (DamageType)32;
					val.Fire();
				}
				shockwaveNum++;
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

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (InterruptPriority)6;
		}
	}
}