using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates;

internal class EGOBlock : BaseSkillState
{
	public float duration = 1f;

	public float invulStart = 0f;

	public float invulEnd = 0.35f;

	public float bonusMult = 1f;

	public float stockBonus = 0.4f;

	public float hitBonus = 0.6f;

	public bool invul = false;

	public bool blockOut = false;

	public float damageCounter = 0f;

	protected RedMistEmotionComponent emotionComponent;

	protected RedMistStatTracker statTracker;

	private Transform modelTransform;

	private HurtBoxGroup hurtboxGroup;

	private CharacterModel characterModel;

	private ParticleSystem mistEffect;

	private float originalHeight;

	private float originalRadius;

	public override void OnEnter()
	{
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && (bool)(Object)(object)((EntityState)this).get_characterBody().get_master() && RiskOfRuinaPlugin.KombatIsDueling(((EntityState)this).get_characterBody().get_master()))
		{
			duration += 0.2f;
		}
		emotionComponent = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
		statTracker = ((EntityState)this).get_gameObject().GetComponent<RedMistStatTracker>();
		modelTransform = ((EntityState)this).GetModelTransform();
		if ((bool)modelTransform)
		{
			characterModel = modelTransform.GetComponent<CharacterModel>();
			hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
		}
		if ((bool)(Object)(object)characterModel)
		{
			CharacterModel obj = characterModel;
			obj.invisibilityCount++;
		}
		Util.PlaySound("Play_DaeChi", ((EntityState)this).get_gameObject());
		((EntityState)this).PlayAnimation("EGODodge", "EGODodge", "Dodge.playbackRate", invulEnd);
		EffectData val = new EffectData();
		val.rotation = Quaternion.identity;
		val.set_origin(((EntityState)this).get_characterBody().get_corePosition());
		EffectManager.SpawnEffect(statTracker.phaseEffect, val, true);
		ChildLocator modelChildLocator = ((EntityState)this).GetModelChildLocator();
		if ((bool)(Object)(object)modelChildLocator)
		{
			mistEffect = ((EntityState)this).GetComponent<RedMistStatTracker>().mistEffect;
			mistEffect.Play();
		}
		if (NetworkServer.get_active())
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
		}
		invul = true;
		if (((EntityState)this).get_skillLocator().utility.get_stock() > 1)
		{
			bonusMult += (float)(((EntityState)this).get_skillLocator().utility.get_stock() - 1) * stockBonus;
		}
		((BaseState)this).OnEnter();
		RiskOfRuinaNetworkManager.ServerOnHit += OnHit;
		((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.HorizontalSlashCameraParamsRedMist;
		((EntityState)this).get_cameraTargetParams().aimMode = (AimType)2;
		CapsuleCollider capsuleCollider = (CapsuleCollider)((EntityState)this).get_characterBody().get_mainHurtBox().get_collider();
		originalHeight = capsuleCollider.height;
		originalRadius = capsuleCollider.radius;
		capsuleCollider.height = originalHeight * 2f;
		capsuleCollider.radius = originalRadius * 25f;
	}

	public void OnHit(float damage, GameObject attacker, GameObject victim)
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		if (!(victim == ((EntityState)this).get_gameObject()) || !invul)
		{
			return;
		}
		((EntityState)this).PlayAnimation("EGODodge", "EGODodge", "Dodge.playbackRate", hitBonus);
		Util.PlaySound("Play_Defense_Guard", ((EntityState)this).get_gameObject());
		invulEnd = ((EntityState)this).get_fixedAge() + hitBonus;
		duration = invulEnd + hitBonus;
		if ((bool)attacker && (bool)(Object)(object)attacker.GetComponent<CharacterBody>() && (Object)(object)attacker.GetComponent<CharacterBody>() != (Object)(object)((EntityState)this).get_characterBody())
		{
			float num = damage;
			if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && (bool)(Object)(object)((EntityState)this).get_characterBody().get_master() && RiskOfRuinaPlugin.KombatIsDueling(((EntityState)this).get_characterBody().get_master()))
			{
				num = damage * 5f;
			}
			damageCounter += num;
			CharacterBody component = attacker.GetComponent<CharacterBody>();
			if (NetworkServer.get_active())
			{
				DamageInfo val = new DamageInfo
				{
					attacker = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
					inflictor = ((Component)(object)((EntityState)this).get_characterBody()).gameObject,
					crit = ((BaseState)this).RollCrit(),
					damage = (1f + Config.redMistBuffDamage.Value * (float)((EntityState)this).get_characterBody().GetBuffCount(Buffs.RedMistBuff)) * (num * 1.5f * bonusMult),
					position = attacker.transform.position,
					force = Vector3.zero,
					damageType = (DamageType)0,
					damageColorIndex = (DamageColorIndex)0,
					procCoefficient = 1f
				};
				component.get_healthComponent().TakeDamage(val);
				GlobalEventManager.instance.OnHitEnemy(val, attacker);
				GlobalEventManager.instance.OnHitAll(val, attacker);
			}
			if (((EntityState)this).get_isAuthority())
			{
				EffectData val2 = new EffectData();
				val2.rotation = Random.rotation;
				val2.set_origin(component.get_healthComponent().body.get_corePosition());
				EffectManager.SpawnEffect(statTracker.afterimageSlash, val2, true);
				Vector3 vector = component.get_footPosition() - ((EntityState)this).get_characterBody().get_footPosition();
				vector.y = 0f;
				val2 = new EffectData();
				val2.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
				val2.set_origin(((EntityState)this).get_characterBody().get_footPosition() + Random.Range(0f, 2.5f) * vector.normalized);
				EffectManager.SpawnEffect(statTracker.afterimageBlock, val2, true);
			}
		}
	}

	public override void FixedUpdate()
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		((EntityState)this).FixedUpdate();
		if (NetworkServer.get_active() && ((EntityState)this).get_fixedAge() < invulEnd && !((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
		{
			((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
		}
		if (((EntityState)this).get_fixedAge() >= invulEnd && invul)
		{
			if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
			}
			if ((bool)(Object)(object)characterModel)
			{
				CharacterModel obj = characterModel;
				obj.invisibilityCount--;
			}
			if (((EntityState)this).get_isAuthority())
			{
				EffectData val = new EffectData();
				val.rotation = Quaternion.identity;
				val.set_origin(((EntityState)this).get_characterBody().get_corePosition());
				EffectManager.SpawnEffect(statTracker.phaseEffect, val, true);
			}
			mistEffect.Stop();
			invul = false;
			CapsuleCollider capsuleCollider = (CapsuleCollider)((EntityState)this).get_characterBody().get_mainHurtBox().get_collider();
			capsuleCollider.height = 1.5f;
			capsuleCollider.radius = 0.2f;
			if (damageCounter > 0f)
			{
				if (((EntityState)this).get_isAuthority())
				{
					((EntityState)this).outer.SetNextState((EntityState)(object)new EGOBlockCounter
					{
						damageCounter = damageCounter,
						bonusMult = bonusMult
					});
				}
			}
			else
			{
				((EntityState)this).get_cameraTargetParams().cameraParams = CameraParams.defaultCameraParamsRedMist;
				((EntityState)this).get_cameraTargetParams().aimMode = (AimType)0;
			}
		}
		if (damageCounter > 0f && !((EntityState)this).get_inputBank().skill3.down && ((EntityState)this).get_isAuthority())
		{
			((EntityState)this).outer.SetNextState((EntityState)(object)new EGOBlockCounter
			{
				damageCounter = damageCounter,
				bonusMult = bonusMult
			});
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
		CapsuleCollider capsuleCollider = (CapsuleCollider)((EntityState)this).get_characterBody().get_mainHurtBox().get_collider();
		capsuleCollider.height = 1.5f;
		capsuleCollider.radius = 0.2f;
		RiskOfRuinaNetworkManager.ServerOnHit -= OnHit;
		((EntityState)this).OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)6;
	}

	public override void OnSerialize(NetworkWriter writer)
	{
		((BaseSkillState)this).OnSerialize(writer);
		writer.Write(damageCounter);
		writer.Write(invulEnd);
		writer.Write(duration);
	}

	public override void OnDeserialize(NetworkReader reader)
	{
		((BaseSkillState)this).OnDeserialize(reader);
		damageCounter = reader.ReadSingle();
		invulEnd = reader.ReadSingle();
		duration = reader.ReadSingle();
	}
}
