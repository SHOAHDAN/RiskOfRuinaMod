using R2API;
using RiskOfRuinaMod.Modules.Misc;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules
{

	internal static class Projectiles
	{
		internal static GameObject shockwaveSmallPrefab;

		internal static GameObject shockwaveMediumPrefab;

		internal static GameObject shockwaveLargePrefab;

		internal static GameObject shockwaveScepterPrefab;

		internal static GameObject fairyLinePrefab;

		internal static GameObject pillarPrefab;

		internal static GameObject pillarSpearPrefab;

		internal static void RegisterProjectiles()
		{
			CreateShockwaves();
			CreateFairyLine();
			CreatePillar();
			CreatePillarSpear();
			AddProjectile(shockwaveSmallPrefab);
			AddProjectile(shockwaveMediumPrefab);
			AddProjectile(shockwaveLargePrefab);
			AddProjectile(shockwaveScepterPrefab);
			AddProjectile(fairyLinePrefab);
			AddProjectile(pillarPrefab);
			AddProjectile(pillarSpearPrefab);
		}

		internal static void AddProjectile(GameObject projectileToAdd)
		{
			Prefabs.projectilePrefabs.Add(projectileToAdd);
		}

		private static void CreatePillar()
		{
			pillarPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("ArbiterPillar", registerNetwork: true);
			pillarPrefab.transform.localScale = Vector3.one;
			Object.Destroy((Object)(object)pillarPrefab.GetComponent<ProjectileDotZone>());
			ArbiterPillarController arbiterPillarController = pillarPrefab.AddComponent<ArbiterPillarController>();
			arbiterPillarController.Networkradius = 25f;
			arbiterPillarController.freezeProjectiles = true;
			arbiterPillarController.buffDef = Buffs.warpBuff;
			arbiterPillarController.buffDuration = 3f;
			arbiterPillarController.expires = true;
			arbiterPillarController.animateRadius = false;
			arbiterPillarController.interval = 1f;
			arbiterPillarController.expireDuration = 10f;
			Object.Destroy(pillarPrefab.transform.GetChild(0).gameObject);
			GameObject gameObject = Assets.pillarObject.InstantiateClone("PillarEffect", registerNetwork: false);
			gameObject.transform.parent = pillarPrefab.transform;
			gameObject.transform.localPosition = Vector3.zero;
			pillarPrefab.AddComponent<DestroyOnTimer>().duration = 11f;
		}

		private static void CreatePillarSpear()
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			pillarSpearPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile").InstantiateClone("ArbiterPillarSpear", registerNetwork: true);
			pillarSpearPrefab.transform.localScale = Vector3.one;
			DetonateFairyOnImpact detonateFairyOnImpact = pillarSpearPrefab.AddComponent<DetonateFairyOnImpact>();
			detonateFairyOnImpact.Networkradius = 15f;
			GameObject gameObject = Assets.pillarSpear.InstantiateClone("pillarSpearGhost", registerNetwork: false);
			gameObject.AddComponent<ProjectileGhostController>();
			pillarSpearPrefab.transform.localScale *= 2f;
			pillarSpearPrefab.GetComponent<ProjectileController>().ghostPrefab = gameObject;
			pillarSpearPrefab.GetComponent<ProjectileDamage>().damageType = (DamageType)32;
			pillarSpearPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.pillarImpactEffect;
			((ProjectileExplosion)pillarSpearPrefab.GetComponent<ProjectileImpactExplosion>()).blastRadius = 15f;
			pillarSpearPrefab.GetComponent<Rigidbody>().useGravity = false;
			Object.Destroy((Object)(object)pillarSpearPrefab.GetComponent<AntiGravityForce>());
			Object.Destroy((Object)(object)pillarSpearPrefab.GetComponent<ProjectileProximityBeamController>());
		}

		private static void CreateFairyLine()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			fairyLinePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile").InstantiateClone("FairyLine", registerNetwork: true);
			GameObject gameObject = Assets.fairyTrail.InstantiateClone("FairyLineGhost", registerNetwork: false);
			gameObject.AddComponent<ProjectileGhostController>();
			fairyLinePrefab.GetComponent<ProjectileController>().ghostPrefab = gameObject;
			fairyLinePrefab.GetComponent<ProjectileController>().set_shouldPlaySounds(false);
			fairyLinePrefab.GetComponent<ProjectileDamage>().damageType = (DamageType)0;
			fairyLinePrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.fairyExplodeEffect;
			((ProjectileExplosion)fairyLinePrefab.GetComponent<ProjectileImpactExplosion>()).blastRadius = 10f;
			fairyLinePrefab.GetComponent<Rigidbody>().useGravity = false;
			Object.Destroy((Object)(object)fairyLinePrefab.GetComponent<AntiGravityForce>());
			Object.Destroy(fairyLinePrefab.GetComponent<AkEvent>());
			Object.Destroy((Object)(object)fairyLinePrefab.GetComponent<ProjectileProximityBeamController>());
		}

		private static void CreateShockwaves()
		{
			shockwaveSmallPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("ArbiterShockwave", registerNetwork: true);
			shockwaveSmallPrefab.transform.localScale = Vector3.one;
			Object.Destroy((Object)(object)shockwaveSmallPrefab.GetComponent<ProjectileDotZone>());
			shockwaveSmallPrefab.AddComponent<DestroyOnTimer>().duration = 5f;
			ArbiterShockwaveController arbiterShockwaveController = shockwaveSmallPrefab.AddComponent<ArbiterShockwaveController>();
			arbiterShockwaveController.Networkradius = 40f;
			arbiterShockwaveController.barrierAmount = 0.3f;
			arbiterShockwaveController.destroyProjectiles = true;
			arbiterShockwaveController.buffDef = Buffs.feebleDebuff;
			arbiterShockwaveController.buffDuration = 10f;
			Object.Destroy(shockwaveSmallPrefab.transform.GetChild(0).gameObject);
			GameObject gameObject = Assets.shockwaveEffect.InstantiateClone("ShockwaveEffect", registerNetwork: false);
			gameObject.transform.parent = shockwaveSmallPrefab.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one * 40f * 2f;
			shockwaveMediumPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("ArbiterShockwave", registerNetwork: true);
			shockwaveMediumPrefab.transform.localScale = Vector3.one;
			Object.Destroy((Object)(object)shockwaveMediumPrefab.GetComponent<ProjectileDotZone>());
			shockwaveMediumPrefab.AddComponent<DestroyOnTimer>().duration = 5f;
			arbiterShockwaveController = shockwaveMediumPrefab.AddComponent<ArbiterShockwaveController>();
			arbiterShockwaveController.Networkradius = 40f;
			arbiterShockwaveController.barrierAmount = 0.3f;
			arbiterShockwaveController.destroyProjectiles = true;
			arbiterShockwaveController.buffDef = Buffs.feebleDebuff;
			arbiterShockwaveController.buffDuration = 10f;
			Object.Destroy(shockwaveMediumPrefab.transform.GetChild(0).gameObject);
			gameObject = Assets.shockwaveEffect.InstantiateClone("ShockwaveEffect", registerNetwork: false);
			gameObject.transform.parent = shockwaveMediumPrefab.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one * 40f * 2f;
			shockwaveLargePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("ArbiterShockwave", registerNetwork: true);
			shockwaveLargePrefab.transform.localScale = Vector3.one;
			Object.Destroy((Object)(object)shockwaveLargePrefab.GetComponent<ProjectileDotZone>());
			shockwaveLargePrefab.AddComponent<DestroyOnTimer>().duration = 5f;
			arbiterShockwaveController = shockwaveLargePrefab.AddComponent<ArbiterShockwaveController>();
			arbiterShockwaveController.Networkradius = 40f;
			arbiterShockwaveController.barrierAmount = 0.3f;
			arbiterShockwaveController.destroyProjectiles = true;
			arbiterShockwaveController.buffDef = Buffs.feebleDebuff;
			arbiterShockwaveController.buffDuration = 10f;
			Object.Destroy(shockwaveLargePrefab.transform.GetChild(0).gameObject);
			gameObject = Assets.shockwaveEffect.InstantiateClone("ShockwaveEffect", registerNetwork: false);
			gameObject.transform.parent = shockwaveLargePrefab.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one * 40f * 2f;
			shockwaveScepterPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone").InstantiateClone("ArbiterShockwave", registerNetwork: true);
			shockwaveScepterPrefab.transform.localScale = Vector3.one;
			Object.Destroy((Object)(object)shockwaveScepterPrefab.GetComponent<ProjectileDotZone>());
			shockwaveScepterPrefab.AddComponent<DestroyOnTimer>().duration = 5f;
			arbiterShockwaveController = shockwaveScepterPrefab.AddComponent<ArbiterShockwaveController>();
			arbiterShockwaveController.Networkradius = 60f;
			arbiterShockwaveController.barrierAmount = 0.5f;
			arbiterShockwaveController.destroyProjectiles = true;
			arbiterShockwaveController.buffDef = Buffs.feebleDebuff;
			arbiterShockwaveController.buffDuration = 15f;
			Object.Destroy(shockwaveScepterPrefab.transform.GetChild(0).gameObject);
			gameObject = Assets.shockwaveEffect.InstantiateClone("ShockwaveEffect", registerNetwork: false);
			gameObject.transform.parent = shockwaveScepterPrefab.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one * 60f * 2f;
		}

		private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			((ProjectileExplosion)projectileImpactExplosion).blastDamageCoefficient = 1f;
			((ProjectileExplosion)projectileImpactExplosion).blastProcCoefficient = 1f;
			((ProjectileExplosion)projectileImpactExplosion).blastRadius = 1f;
			((ProjectileExplosion)projectileImpactExplosion).bonusBlastForce = Vector3.zero;
			((ProjectileExplosion)projectileImpactExplosion).childrenCount = 0;
			((ProjectileExplosion)projectileImpactExplosion).childrenDamageCoefficient = 0f;
			((ProjectileExplosion)projectileImpactExplosion).childrenProjectilePrefab = null;
			projectileImpactExplosion.destroyOnEnemy = false;
			projectileImpactExplosion.destroyOnWorld = false;
			((ProjectileExplosion)projectileImpactExplosion).explosionSoundString = "";
			((ProjectileExplosion)projectileImpactExplosion).falloffModel = (FalloffModel)0;
			((ProjectileExplosion)projectileImpactExplosion).fireChildren = false;
			projectileImpactExplosion.impactEffect = null;
			projectileImpactExplosion.lifetime = 0f;
			projectileImpactExplosion.lifetimeAfterImpact = 0f;
			projectileImpactExplosion.lifetimeExpiredSoundString = "";
			projectileImpactExplosion.lifetimeRandomOffset = 0f;
			projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
			projectileImpactExplosion.timerAfterImpact = false;
			((Component)(object)projectileImpactExplosion).GetComponent<ProjectileDamage>().damageType = (DamageType)0;
		}

		private static GameObject CreateGhostPrefab(string ghostName)
		{
			GameObject gameObject = Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
			if (!(Object)(object)gameObject.GetComponent<NetworkIdentity>())
			{
				gameObject.AddComponent<NetworkIdentity>();
			}
			if (!(Object)(object)gameObject.GetComponent<ProjectileGhostController>())
			{
				gameObject.AddComponent<ProjectileGhostController>();
			}
			Assets.ConvertAllRenderersToHopooShader(gameObject);
			return gameObject;
		}

		private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
		{
			return Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName).InstantiateClone(newPrefabName);
		}
	}
}