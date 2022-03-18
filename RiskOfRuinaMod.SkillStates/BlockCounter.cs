using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{

	internal class BlockCounter : BaseSkillState
	{
		public float damageCounter = 0f;

		public int hits = 0;

		public float duration = 0.5f;

		public float bonusMult = 1f;

		protected RedMistEmotionComponent emotionComponent;

		protected RedMistStatTracker statTracker;

		protected BlastAttack attack;

		public override void OnEnter()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected O, but got Unknown
			emotionComponent = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
			statTracker = ((EntityState)this).get_gameObject().GetComponent<RedMistStatTracker>();
			if (NetworkServer.get_active())
			{
				((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
			}
			if (((EntityState)this).get_isAuthority())
			{
				attack = new BlastAttack();
				attack.damageType = (DamageType)0;
				attack.procCoefficient = 1f;
				attack.baseForce = 300f;
				attack.bonusForce = Vector3.zero;
				attack.baseDamage = damageCounter * 1.5f * bonusMult;
				attack.crit = ((BaseState)this).RollCrit();
				attack.attacker = ((Component)(object)((EntityState)this).get_characterBody()).gameObject;
				attack.damageColorIndex = (DamageColorIndex)0;
				attack.falloffModel = (FalloffModel)0;
				attack.radius = 15 + Mathf.Clamp(hits, 0, 15);
				attack.inflictor = ((Component)(object)((EntityState)this).get_characterBody()).gameObject;
				attack.position = ((EntityState)this).get_characterBody().get_footPosition();
				attack.procCoefficient = 1f;
				attack.teamIndex = TeamComponent.GetObjectTeam(((Component)(object)((EntityState)this).get_characterBody()).gameObject);
				attack.Fire();
			}
			((BaseState)this).OnEnter();
			Util.PlaySound("Play_Kali_Normal_Hori", ((EntityState)this).get_gameObject());
			((EntityState)this).PlayAnimation("FullBody, Override", "BlockCounter");
			EffectData val = new EffectData();
			val.rotation = Quaternion.identity;
			val.set_origin(((EntityState)this).get_characterBody().get_footPosition());
			EffectManager.SpawnEffect(statTracker.spinPrefab, val, true);
			if (hits > 5)
			{
				Util.PlaySound("Play_Kali_Special_Vert_Fin", ((EntityState)this).get_gameObject());
				val = new EffectData();
				val.rotation = Quaternion.identity;
				val.set_origin(((EntityState)this).get_characterBody().get_footPosition());
				EffectManager.SpawnEffect(statTracker.spinPrefabTwo, val, true);
			}
		}

		public override void FixedUpdate()
		{
			((EntityState)this).FixedUpdate();
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
}