using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates;

public class CastPillar : BaseCastChanneledSpellState
{
	public override void OnEnter()
	{
		baseDuration = 0.5f;
		baseInterval = 0f;
		muzzleString = "HandR";
		muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
		projectilePrefabs.Enqueue(Projectiles.pillarPrefab);
		castSoundString = "Play_Binah_Stone_Ready";
		base.OnEnter();
	}

	protected override void Fire()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		if (projectilePrefabs.Count > 0)
		{
			if (((EntityState)this).get_isAuthority())
			{
				BlastAttack val = new BlastAttack();
				val.attacker = ((EntityState)this).get_gameObject();
				val.inflictor = ((EntityState)this).get_gameObject();
				val.teamIndex = TeamComponent.GetObjectTeam(val.attacker);
				val.position = spellPosition;
				val.procCoefficient = 1f;
				val.radius = 12.5f;
				val.baseForce = 2000f;
				val.bonusForce = Vector3.zero;
				val.baseDamage = 5f * ((BaseState)this).damageStat;
				val.falloffModel = (FalloffModel)2;
				val.damageColorIndex = (DamageColorIndex)0;
				val.attackerFiltering = (AttackerFiltering)2;
				val.crit = ((BaseState)this).RollCrit();
				val.damageType = (DamageType)131072;
				val.Fire();
			}
			base.Fire();
		}
	}

	protected override void PlayCastAnimation()
	{
		((EntityState)this).PlayAnimation("Gesture, Override", "Pillar", "Pillar.playbackRate", baseDuration);
	}
}
