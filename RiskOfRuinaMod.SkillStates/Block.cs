using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{

	internal class Block : BaseSkillState
	{
		public float duration = 0.7f;

		public float invulEnd = 0.35f;

		public float hitBonus = 0.6f;

		public bool invul = false;

		public bool blockOut = false;

		public float damageCounter = 0f;

		public float bonusMult = 1f;

		public float stockBonus = 0.4f;

		public int hits = 0;

		protected TemporaryOverlay iframeOverlay;

		protected RedMistEmotionComponent emotionComponent;

		protected RedMistStatTracker statTracker;

		private float originalHeight;

		private float originalRadius;

		public override void OnEnter()
		{
			emotionComponent = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
			statTracker = ((EntityState)this).get_gameObject().GetComponent<RedMistStatTracker>();
			if (((EntityState)this).get_skillLocator().utility.get_stock() > 1)
			{
				bonusMult += (float)(((EntityState)this).get_skillLocator().utility.get_stock() - 1) * stockBonus;
			}
			AddOverlay(invulEnd);
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
			}
			invul = true;
			((BaseState)this).OnEnter();
			Util.PlaySound("Ruina_Swipe", ((EntityState)this).get_gameObject());
			((EntityState)this).PlayAnimation("FullBody, Override", "BlockIn");
			RiskOfRuinaNetworkManager.ServerOnHit += OnHit;
			CapsuleCollider capsuleCollider = (CapsuleCollider)((EntityState)this).get_characterBody().get_mainHurtBox().get_collider();
			originalHeight = capsuleCollider.height;
			originalRadius = capsuleCollider.radius;
			capsuleCollider.height = originalHeight * 1.5f;
			capsuleCollider.radius = originalRadius * 10f;
		}

		public void OnHit(float damage, GameObject attacker, GameObject victim)
		{
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Expected O, but got Unknown
			if (!(victim == ((EntityState)this).get_gameObject()) || !invul)
			{
				return;
			}
			Util.PlaySound("Play_Defense_Guard", ((EntityState)this).get_gameObject());
			if (!attacker || !(Object)(object)attacker.GetComponent<CharacterBody>() || !((Object)(object)attacker.GetComponent<CharacterBody>() != (Object)(object)((EntityState)this).get_characterBody()))
			{
				return;
			}
			CharacterBody component = attacker.GetComponent<CharacterBody>();
			Vector3 lhs = component.get_footPosition() - ((EntityState)this).get_characterBody().get_footPosition();
			lhs.y = 0f;
			lhs.Normalize();
			Vector3 forward = ((EntityState)this).get_characterDirection().get_forward();
			Vector3 up = ((EntityState)this).get_transform().up;
			Vector3 normalized = Vector3.Cross(up, forward).normalized;
			Vector2 normalized2 = new Vector2(Vector3.Dot(lhs, forward), Vector3.Dot(lhs, normalized)).normalized;
			if (normalized2.x >= 0.5f)
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockHit");
			}
			else if (normalized2.x <= -0.5f)
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockHitBack");
			}
			else if (normalized2.y >= 0.5f)
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockHitRight");
			}
			else if (normalized2.y <= -0.5f)
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockHitLeft");
			}
			else
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockHit");
			}
			invulEnd = ((EntityState)this).get_fixedAge() + hitBonus;
			duration = invulEnd + hitBonus;
			float num = damage;
			if ((bool)attacker && (bool)(Object)(object)attacker.GetComponent<CharacterBody>())
			{
				num = damage;
				if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && (bool)(Object)(object)((EntityState)this).get_characterBody().get_master() && RiskOfRuinaPlugin.KombatIsDueling(((EntityState)this).get_characterBody().get_master()))
				{
					num = damage * 5f;
				}
				damageCounter += num;
				hits++;
			}
			if (((EntityState)this).get_isAuthority())
			{
				EffectData val = new EffectData();
				val.rotation = Util.QuaternionSafeLookRotation(Vector3.zero);
				val.set_origin(((EntityState)this).get_characterBody().get_corePosition());
				EffectManager.SpawnEffect(Assets.blockEffect, val, true);
			}
		}

		public override void FixedUpdate()
		{
			((EntityState)this).FixedUpdate();
			((EntityState)this).get_characterMotor().velocity = Vector3.zero;
			if (((EntityState)this).get_fixedAge() >= invulEnd && invul)
			{
				if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
				{
					((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
				}
				RemoveOverlay();
				invul = false;
				CapsuleCollider capsuleCollider = (CapsuleCollider)((EntityState)this).get_characterBody().get_mainHurtBox().get_collider();
				capsuleCollider.height = 1.5f;
				capsuleCollider.radius = 0.2f;
				if (damageCounter > 0f && ((EntityState)this).get_isAuthority())
				{
					((EntityState)this).outer.SetNextState((EntityState)(object)new BlockCounter
					{
						damageCounter = damageCounter,
						hits = hits,
						bonusMult = bonusMult
					});
				}
			}
			if (damageCounter > 0f && !((EntityState)this).get_inputBank().skill3.down && ((EntityState)this).get_isAuthority())
			{
				((EntityState)this).outer.SetNextState((EntityState)(object)new BlockCounter
				{
					damageCounter = damageCounter,
					hits = hits,
					bonusMult = bonusMult
				});
			}
			if (((EntityState)this).get_fixedAge() >= invulEnd && !blockOut)
			{
				blockOut = true;
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockOut");
			}
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
			RemoveOverlay();
			CapsuleCollider capsuleCollider = (CapsuleCollider)((EntityState)this).get_characterBody().get_mainHurtBox().get_collider();
			capsuleCollider.height = 1.5f;
			capsuleCollider.radius = 0.2f;
			RiskOfRuinaNetworkManager.ServerOnHit -= OnHit;
			if (!blockOut)
			{
				((EntityState)this).PlayAnimation("FullBody, Override", "BlockOut");
			}
			((EntityState)this).OnExit();
		}

		protected void AddOverlay(float duration)
		{
			if (Config.iframeOverlay.Value)
			{
				iframeOverlay = ((Component)(object)((EntityState)this).get_characterBody()).gameObject.AddComponent<TemporaryOverlay>();
				iframeOverlay.duration = duration;
				iframeOverlay.alphaCurve = AnimationCurve.Constant(0f, duration, 0.1f);
				iframeOverlay.animateShaderAlpha = true;
				iframeOverlay.destroyComponentOnEnd = true;
				iframeOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
				iframeOverlay.AddToCharacerModel(((EntityState)this).get_modelLocator().get_modelTransform().GetComponent<CharacterModel>());
			}
		}

		protected void RemoveOverlay()
		{
			if ((bool)(Object)(object)iframeOverlay)
			{
				Object.Destroy((Object)(object)iframeOverlay);
			}
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			((BaseSkillState)this).OnSerialize(writer);
			writer.Write(damageCounter);
			writer.Write(hits);
			writer.Write(invulEnd);
			writer.Write(duration);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			((BaseSkillState)this).OnDeserialize(reader);
			damageCounter = reader.ReadSingle();
			hits = reader.ReadInt32();
			invulEnd = reader.ReadSingle();
			duration = reader.ReadSingle();
		}
	}
}