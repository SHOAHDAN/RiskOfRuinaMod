using EntityStates.Commando.CommandoWeapon;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.SkillStates.BaseStates;

namespace RiskOfRuinaMod.SkillStates
{

	public class ThrowPillarSpear : BaseThrowSpellState
	{
		public override void OnEnter()
		{
			baseDuration = 0.8f;
			force = 5f;
			maxDamageCoefficient = 6f;
			minDamageCoefficient = 3f;
			muzzleflashEffectPrefab = FirePistol2.muzzleEffectPrefab;
			projectilePrefab = Projectiles.pillarSpearPrefab;
			selfForce = 0f;
			throwSound = "Play_Binah_Stone_Fire";
			base.OnEnter();
		}
	}
}