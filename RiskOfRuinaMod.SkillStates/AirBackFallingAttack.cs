using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{

	internal class AirBackFallingAttack : BaseSkillState
	{
		private float duration = 10f;

		private float cooldown = 0.4f;

		private float landTime = 0f;

		private bool landed = false;

		public ShakeEmitter shakeEmitter;

		private float startY = 0f;

		protected float trueDamage => ((BaseState)this).damageStat;

		public override void OnEnter()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			((BaseState)this).OnEnter();
			CharacterBody characterBody = ((EntityState)this).get_characterBody();
			characterBody.bodyFlags = (BodyFlags)(characterBody.bodyFlags | 1);
			if (((EntityState)this).get_isAuthority())
			{
				((EntityState)this).get_characterMotor().velocity.y = -60f;
				CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
				characterMotor.rootMotion.y = characterMotor.rootMotion.y - 0.5f;
				((EntityState)this).get_characterMotor().velocity.x = 0f;
				((EntityState)this).get_characterMotor().velocity.z = 0f;
			}
			startY = ((EntityState)this).get_characterBody().get_corePosition().y;
			((EntityState)this).PlayCrossfade("FullBody, Override", "AirBackSlashContinue", "BaseAttack.playbackRate", duration, 0.1f);
		}

		public override void FixedUpdate()
		{
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Expected O, but got Unknown
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Expected O, but got Unknown
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			((EntityState)this).FixedUpdate();
			if (landed)
			{
				if (((EntityState)this).get_fixedAge() >= landTime + cooldown && ((EntityState)this).get_isAuthority())
				{
					((EntityState)this).outer.SetNextStateToMain();
				}
				return;
			}
			if (((EntityState)this).get_isAuthority() && ((EntityState)this).get_characterMotor().velocity.y > -100f)
			{
				((EntityState)this).get_characterMotor().velocity.y = -100f;
			}
			if (((EntityState)this).get_fixedAge() >= duration || ((EntityState)this).get_characterMotor().get_isGrounded())
			{
				Util.PlaySound("Play_Kali_Special_Vert_Fin", ((EntityState)this).get_gameObject());
				((EntityState)this).PlayCrossfade("FullBody, Override", "AirBackSlashFinish", "BaseAttack.playbackRate", cooldown, 0.1f);
				landed = true;
				landTime = ((EntityState)this).get_fixedAge();
				if (((EntityState)this).get_isAuthority())
				{
					shakeEmitter = ((EntityState)this).get_gameObject().AddComponent<ShakeEmitter>();
					shakeEmitter.amplitudeTimeDecay = true;
					shakeEmitter.duration = 0.2f;
					shakeEmitter.radius = 80f;
					shakeEmitter.scaleShakeRadiusWithLocalScale = false;
					shakeEmitter.wave = new Wave
					{
						amplitude = 0.8f,
						frequency = 30f,
						cycleOffset = 0f
					};
					float num = 8f;
					((BaseState)this).AddRecoil(-0.4f * num, -0.8f * num, -0.3f * num, 0.3f * num);
					CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
					characterMotor.velocity *= 0.1f;
					CharacterBody characterBody = ((EntityState)this).get_characterBody();
					characterBody.bodyFlags = (BodyFlags)(characterBody.bodyFlags - 1);
					EffectData val = new EffectData();
					val.set_origin(((EntityState)this).get_characterBody().get_footPosition());
					val.scale = 1f;
					EffectManager.SpawnEffect(((EntityState)this).GetComponent<RedMistStatTracker>().groundPoundEffect, val, true);
					float y = ((EntityState)this).get_characterBody().get_corePosition().y;
					float num2 = startY - y;
					float num3 = Mathf.Clamp(num2 / 10f, 1f, 10f);
					Vector3 footPosition = ((EntityState)this).get_characterBody().get_footPosition();
					BlastAttack val2 = new BlastAttack
					{
						radius = 10f + num3,
						procCoefficient = 0.8f,
						position = footPosition,
						attacker = ((EntityState)this).get_gameObject(),
						teamIndex = ((EntityState)this).get_teamComponent().get_teamIndex(),
						crit = ((BaseState)this).RollCrit(),
						baseDamage = trueDamage * 2f * num3,
						damageColorIndex = (DamageColorIndex)0,
						falloffModel = (FalloffModel)0,
						attackerFiltering = (AttackerFiltering)2,
						damageType = (DamageType)0
					};
					val2.Fire();
					((EntityState)this).get_characterMotor().velocity.y = 0f;
				}
			}
		}

		public override void OnExit()
		{
			((EntityState)this).OnExit();
			Object.Destroy((Object)(object)shakeEmitter);
		}
	}
}