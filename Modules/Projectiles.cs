// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Projectiles
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

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
      Projectiles.CreateShockwaves();
      Projectiles.CreateFairyLine();
      Projectiles.CreatePillar();
      Projectiles.CreatePillarSpear();
      Projectiles.AddProjectile(Projectiles.shockwaveSmallPrefab);
      Projectiles.AddProjectile(Projectiles.shockwaveMediumPrefab);
      Projectiles.AddProjectile(Projectiles.shockwaveLargePrefab);
      Projectiles.AddProjectile(Projectiles.shockwaveScepterPrefab);
      Projectiles.AddProjectile(Projectiles.fairyLinePrefab);
      Projectiles.AddProjectile(Projectiles.pillarPrefab);
      Projectiles.AddProjectile(Projectiles.pillarSpearPrefab);
    }

    internal static void AddProjectile(GameObject projectileToAdd) => Prefabs.projectilePrefabs.Add(projectileToAdd);

    private static void CreatePillar()
    {
      Projectiles.pillarPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "ArbiterPillar", true);
      Projectiles.pillarPrefab.transform.localScale = Vector3.one;
      Object.Destroy((Object) Projectiles.pillarPrefab.GetComponent<ProjectileDotZone>());
      ArbiterPillarController pillarController = Projectiles.pillarPrefab.AddComponent<ArbiterPillarController>();
      pillarController.Networkradius = 25f;
      pillarController.freezeProjectiles = true;
      pillarController.buffDef = Buffs.warpBuff;
      pillarController.buffDuration = 3f;
      pillarController.expires = true;
      pillarController.animateRadius = false;
      pillarController.interval = 1f;
      pillarController.expireDuration = 10f;
      Object.Destroy((Object) ((Component) Projectiles.pillarPrefab.transform.GetChild(0)).gameObject);
      GameObject gameObject = PrefabAPI.InstantiateClone(Assets.pillarObject, "PillarEffect", false);
      gameObject.transform.parent = Projectiles.pillarPrefab.transform;
      gameObject.transform.localPosition = Vector3.zero;
      Projectiles.pillarPrefab.AddComponent<DestroyOnTimer>().duration = 11f;
    }

    private static void CreatePillarSpear()
    {
      Projectiles.pillarSpearPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile"), "ArbiterPillarSpear", true);
      Projectiles.pillarSpearPrefab.transform.localScale = Vector3.one;
      Projectiles.pillarSpearPrefab.AddComponent<DetonateFairyOnImpact>().Networkradius = 15f;
      GameObject gameObject = PrefabAPI.InstantiateClone(Assets.pillarSpear, "pillarSpearGhost", false);
      gameObject.AddComponent<ProjectileGhostController>();
      Transform transform = Projectiles.pillarSpearPrefab.transform;
      transform.localScale = Vector3.op_Multiply(transform.localScale, 2f);
      Projectiles.pillarSpearPrefab.GetComponent<ProjectileController>().ghostPrefab = gameObject;
      Projectiles.pillarSpearPrefab.GetComponent<ProjectileDamage>().damageType = (DamageType) 32;
      Projectiles.pillarSpearPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.pillarImpactEffect;
      ((ProjectileExplosion) Projectiles.pillarSpearPrefab.GetComponent<ProjectileImpactExplosion>()).blastRadius = 15f;
      Projectiles.pillarSpearPrefab.GetComponent<Rigidbody>().useGravity = false;
      Object.Destroy((Object) Projectiles.pillarSpearPrefab.GetComponent<AntiGravityForce>());
      Object.Destroy((Object) Projectiles.pillarSpearPrefab.GetComponent<ProjectileProximityBeamController>());
    }

    private static void CreateFairyLine()
    {
      Projectiles.fairyLinePrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile"), "FairyLine", true);
      GameObject gameObject = PrefabAPI.InstantiateClone(Assets.fairyTrail, "FairyLineGhost", false);
      gameObject.AddComponent<ProjectileGhostController>();
      Projectiles.fairyLinePrefab.GetComponent<ProjectileController>().ghostPrefab = gameObject;
      Projectiles.fairyLinePrefab.GetComponent<ProjectileController>().shouldPlaySounds = false;
      Projectiles.fairyLinePrefab.GetComponent<ProjectileDamage>().damageType = (DamageType) 0;
      Projectiles.fairyLinePrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.fairyExplodeEffect;
      ((ProjectileExplosion) Projectiles.fairyLinePrefab.GetComponent<ProjectileImpactExplosion>()).blastRadius = 10f;
      Projectiles.fairyLinePrefab.GetComponent<Rigidbody>().useGravity = false;
      Object.Destroy((Object) Projectiles.fairyLinePrefab.GetComponent<AntiGravityForce>());
      Object.Destroy((Object) Projectiles.fairyLinePrefab.GetComponent<AkEvent>());
      Object.Destroy((Object) Projectiles.fairyLinePrefab.GetComponent<ProjectileProximityBeamController>());
    }

    private static void CreateShockwaves()
    {
      Projectiles.shockwaveSmallPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "ArbiterShockwave", true);
      Projectiles.shockwaveSmallPrefab.transform.localScale = Vector3.one;
      Object.Destroy((Object) Projectiles.shockwaveSmallPrefab.GetComponent<ProjectileDotZone>());
      Projectiles.shockwaveSmallPrefab.AddComponent<DestroyOnTimer>().duration = 5f;
      ArbiterShockwaveController shockwaveController1 = Projectiles.shockwaveSmallPrefab.AddComponent<ArbiterShockwaveController>();
      shockwaveController1.Networkradius = 40f;
      shockwaveController1.barrierAmount = 0.3f;
      shockwaveController1.destroyProjectiles = true;
      shockwaveController1.buffDef = Buffs.feebleDebuff;
      shockwaveController1.buffDuration = 10f;
      Object.Destroy((Object) ((Component) Projectiles.shockwaveSmallPrefab.transform.GetChild(0)).gameObject);
      GameObject gameObject1 = PrefabAPI.InstantiateClone(Assets.shockwaveEffect, "ShockwaveEffect", false);
      gameObject1.transform.parent = Projectiles.shockwaveSmallPrefab.transform;
      gameObject1.transform.localPosition = Vector3.zero;
      gameObject1.transform.localScale = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.one, 40f), 2f);
      Projectiles.shockwaveMediumPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "ArbiterShockwave", true);
      Projectiles.shockwaveMediumPrefab.transform.localScale = Vector3.one;
      Object.Destroy((Object) Projectiles.shockwaveMediumPrefab.GetComponent<ProjectileDotZone>());
      Projectiles.shockwaveMediumPrefab.AddComponent<DestroyOnTimer>().duration = 5f;
      ArbiterShockwaveController shockwaveController2 = Projectiles.shockwaveMediumPrefab.AddComponent<ArbiterShockwaveController>();
      shockwaveController2.Networkradius = 40f;
      shockwaveController2.barrierAmount = 0.3f;
      shockwaveController2.destroyProjectiles = true;
      shockwaveController2.buffDef = Buffs.feebleDebuff;
      shockwaveController2.buffDuration = 10f;
      Object.Destroy((Object) ((Component) Projectiles.shockwaveMediumPrefab.transform.GetChild(0)).gameObject);
      GameObject gameObject2 = PrefabAPI.InstantiateClone(Assets.shockwaveEffect, "ShockwaveEffect", false);
      gameObject2.transform.parent = Projectiles.shockwaveMediumPrefab.transform;
      gameObject2.transform.localPosition = Vector3.zero;
      gameObject2.transform.localScale = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.one, 40f), 2f);
      Projectiles.shockwaveLargePrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "ArbiterShockwave", true);
      Projectiles.shockwaveLargePrefab.transform.localScale = Vector3.one;
      Object.Destroy((Object) Projectiles.shockwaveLargePrefab.GetComponent<ProjectileDotZone>());
      Projectiles.shockwaveLargePrefab.AddComponent<DestroyOnTimer>().duration = 5f;
      ArbiterShockwaveController shockwaveController3 = Projectiles.shockwaveLargePrefab.AddComponent<ArbiterShockwaveController>();
      shockwaveController3.Networkradius = 40f;
      shockwaveController3.barrierAmount = 0.3f;
      shockwaveController3.destroyProjectiles = true;
      shockwaveController3.buffDef = Buffs.feebleDebuff;
      shockwaveController3.buffDuration = 10f;
      Object.Destroy((Object) ((Component) Projectiles.shockwaveLargePrefab.transform.GetChild(0)).gameObject);
      GameObject gameObject3 = PrefabAPI.InstantiateClone(Assets.shockwaveEffect, "ShockwaveEffect", false);
      gameObject3.transform.parent = Projectiles.shockwaveLargePrefab.transform;
      gameObject3.transform.localPosition = Vector3.zero;
      gameObject3.transform.localScale = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.one, 40f), 2f);
      Projectiles.shockwaveScepterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "ArbiterShockwave", true);
      Projectiles.shockwaveScepterPrefab.transform.localScale = Vector3.one;
      Object.Destroy((Object) Projectiles.shockwaveScepterPrefab.GetComponent<ProjectileDotZone>());
      Projectiles.shockwaveScepterPrefab.AddComponent<DestroyOnTimer>().duration = 5f;
      ArbiterShockwaveController shockwaveController4 = Projectiles.shockwaveScepterPrefab.AddComponent<ArbiterShockwaveController>();
      shockwaveController4.Networkradius = 60f;
      shockwaveController4.barrierAmount = 0.5f;
      shockwaveController4.destroyProjectiles = true;
      shockwaveController4.buffDef = Buffs.feebleDebuff;
      shockwaveController4.buffDuration = 15f;
      Object.Destroy((Object) ((Component) Projectiles.shockwaveScepterPrefab.transform.GetChild(0)).gameObject);
      GameObject gameObject4 = PrefabAPI.InstantiateClone(Assets.shockwaveEffect, "ShockwaveEffect", false);
      gameObject4.transform.parent = Projectiles.shockwaveScepterPrefab.transform;
      gameObject4.transform.localPosition = Vector3.zero;
      gameObject4.transform.localScale = Vector3.op_Multiply(Vector3.op_Multiply(Vector3.one, 60f), 2f);
    }

    private static void InitializeImpactExplosion(
      ProjectileImpactExplosion projectileImpactExplosion)
    {
      ((ProjectileExplosion) projectileImpactExplosion).blastDamageCoefficient = 1f;
      ((ProjectileExplosion) projectileImpactExplosion).blastProcCoefficient = 1f;
      ((ProjectileExplosion) projectileImpactExplosion).blastRadius = 1f;
      ((ProjectileExplosion) projectileImpactExplosion).bonusBlastForce = Vector3.zero;
      ((ProjectileExplosion) projectileImpactExplosion).childrenCount = 0;
      ((ProjectileExplosion) projectileImpactExplosion).childrenDamageCoefficient = 0.0f;
      ((ProjectileExplosion) projectileImpactExplosion).childrenProjectilePrefab = (GameObject) null;
      projectileImpactExplosion.destroyOnEnemy = false;
      projectileImpactExplosion.destroyOnWorld = false;
      ((ProjectileExplosion) projectileImpactExplosion).explosionSoundString = "";
      ((ProjectileExplosion) projectileImpactExplosion).falloffModel = (BlastAttack.FalloffModel) 0;
      ((ProjectileExplosion) projectileImpactExplosion).fireChildren = false;
      projectileImpactExplosion.impactEffect = (GameObject) null;
      projectileImpactExplosion.lifetime = 0.0f;
      projectileImpactExplosion.lifetimeAfterImpact = 0.0f;
      projectileImpactExplosion.lifetimeExpiredSoundString = "";
      projectileImpactExplosion.lifetimeRandomOffset = 0.0f;
      projectileImpactExplosion.offsetForLifetimeExpiredSound = 0.0f;
      projectileImpactExplosion.timerAfterImpact = false;
      ((Component) projectileImpactExplosion).GetComponent<ProjectileDamage>().damageType = (DamageType) 0;
    }

    private static GameObject CreateGhostPrefab(string ghostName)
    {
      GameObject objectToConvert = Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
      if (!Object.op_Implicit((Object) objectToConvert.GetComponent<NetworkIdentity>()))
        objectToConvert.AddComponent<NetworkIdentity>();
      if (!Object.op_Implicit((Object) objectToConvert.GetComponent<ProjectileGhostController>()))
        objectToConvert.AddComponent<ProjectileGhostController>();
      Assets.ConvertAllRenderersToHopooShader(objectToConvert);
      return objectToConvert;
    }

    private static GameObject CloneProjectilePrefab(
      string prefabName,
      string newPrefabName)
    {
      return PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
    }
  }
}
