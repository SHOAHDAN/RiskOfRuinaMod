// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Prefabs
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RiskOfRuinaMod.Modules
{
  internal static class Prefabs
  {
    private static PhysicMaterial ragdollMaterial;
    internal static List<SurvivorDef> survivorDefinitions = new List<SurvivorDef>();
    internal static List<GameObject> bodyPrefabs = new List<GameObject>();
    internal static List<GameObject> masterPrefabs = new List<GameObject>();
    internal static List<GameObject> projectilePrefabs = new List<GameObject>();

    internal static void RegisterNewSurvivor(
      GameObject bodyPrefab,
      GameObject displayPrefab,
      Color charColor,
      string namePrefix,
      UnlockableDef unlockableDef,
      float sortPosition)
    {
      string str1 = "COF_" + namePrefix + "_BODY_NAME";
      string str2 = "COF_" + namePrefix + "_BODY_DESCRIPTION";
      string str3 = "COF_" + namePrefix + "_BODY_OUTRO_FLAVOR";
      string str4 = "COF_" + namePrefix + "_BODY_OUTRO_FAILURE";
      SurvivorDef instance = ScriptableObject.CreateInstance<SurvivorDef>();
      instance.bodyPrefab = bodyPrefab;
      instance.displayPrefab = displayPrefab;
      instance.primaryColor = charColor;
      instance.displayNameToken = str1;
      instance.descriptionToken = str2;
      instance.outroFlavorToken = str3;
      instance.mainEndingEscapeFailureFlavorToken = str4;
      instance.desiredSortPosition = sortPosition;
      instance.unlockableDef = unlockableDef;
      Prefabs.survivorDefinitions.Add(instance);
    }

    internal static void RegisterNewSurvivor(
      GameObject bodyPrefab,
      GameObject displayPrefab,
      Color charColor,
      string namePrefix)
    {
      Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, namePrefix, (UnlockableDef) null, 100f);
    }

    internal static void RegisterNewSurvivor(
      GameObject bodyPrefab,
      GameObject displayPrefab,
      Color charColor,
      string namePrefix,
      float sortPosition)
    {
      Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, namePrefix, (UnlockableDef) null, sortPosition);
    }

    internal static void RegisterNewSurvivor(
      GameObject bodyPrefab,
      GameObject displayPrefab,
      Color charColor,
      string namePrefix,
      UnlockableDef unlockableDef)
    {
      Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, namePrefix, unlockableDef, 100f);
    }

    internal static GameObject CreateDisplayPrefab(
      string modelName,
      GameObject prefab,
      BodyInfo bodyInfo)
    {
      if (!Object.op_Implicit((Object) Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body")))
        return (GameObject) null;
      GameObject gameObject = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body"), modelName + "Prefab");
      GameObject model = Prefabs.CreateModel(gameObject, modelName);
      Prefabs.SetupModel(gameObject, model.transform, bodyInfo);
      model.AddComponent<CharacterModel>().baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;
      Assets.ConvertAllRenderersToHopooShader(model);
      return model.gameObject;
    }

    internal static GameObject CreatePrefab(
      string bodyName,
      string modelName,
      BodyInfo bodyInfo)
    {
      if (!Object.op_Implicit((Object) Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body")))
        return (GameObject) null;
      GameObject prefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body"), bodyName);
      Transform modelBaseTransform = (Transform) null;
      GameObject model = (GameObject) null;
      if (modelName != "mdl")
      {
        model = Prefabs.CreateModel(prefab, modelName);
        if (Object.op_Equality((Object) model, (Object) null))
          model = ((Component) prefab.GetComponentInChildren<CharacterModel>()).gameObject;
        modelBaseTransform = Prefabs.SetupModel(prefab, model.transform, bodyInfo);
      }
      CharacterBody component = prefab.GetComponent<CharacterBody>();
      ((Object) component).name = bodyInfo.bodyName;
      component.baseNameToken = bodyInfo.bodyNameToken;
      component.subtitleNameToken = bodyInfo.subtitleNameToken;
      component.portraitIcon = bodyInfo.characterPortrait;
      component.crosshairPrefab = bodyInfo.crosshair;
      component.bodyFlags = (CharacterBody.BodyFlags) 16;
      component.rootMotionInMainState = false;
      component.baseMaxHealth = bodyInfo.maxHealth;
      component.levelMaxHealth = bodyInfo.healthGrowth;
      component.baseRegen = bodyInfo.healthRegen;
      component.levelRegen = component.baseRegen * 0.2f;
      component.baseMaxShield = bodyInfo.shield;
      component.levelMaxShield = bodyInfo.shieldGrowth;
      component.baseMoveSpeed = bodyInfo.moveSpeed;
      component.levelMoveSpeed = bodyInfo.moveSpeedGrowth;
      component.baseAcceleration = bodyInfo.acceleration;
      component.baseJumpPower = bodyInfo.jumpPower;
      component.levelJumpPower = bodyInfo.jumpPowerGrowth;
      component.baseDamage = bodyInfo.damage;
      component.levelDamage = component.baseDamage * 0.2f;
      component.baseAttackSpeed = bodyInfo.attackSpeed;
      component.levelAttackSpeed = bodyInfo.attackSpeedGrowth;
      component.baseArmor = bodyInfo.armor;
      component.levelArmor = bodyInfo.armorGrowth;
      component.baseCrit = bodyInfo.crit;
      component.levelCrit = bodyInfo.critGrowth;
      component.baseJumpCount = bodyInfo.jumpCount;
      component.sprintingSpeedMultiplier = bodyInfo.sprintSpeedMult;
      component.hideCrosshair = false;
      component.aimOriginTransform = modelBaseTransform.Find("AimOrigin");
      component.hullClassification = (HullClassification) 0;
      component.preferredPodPrefab = bodyInfo.podPrefab;
      component.isChampion = false;
      component.bodyColor = bodyInfo.bodyColor;
      if (Object.op_Inequality((Object) modelBaseTransform, (Object) null))
        Prefabs.SetupCharacterDirection(prefab, modelBaseTransform, model.transform);
      prefab.GetComponent<CameraTargetParams>().cameraParams = CameraParams.defaultCameraParamsRedMist;
      if (Object.op_Inequality((Object) modelBaseTransform, (Object) null))
        Prefabs.SetupModelLocator(prefab, modelBaseTransform, model.transform);
      Prefabs.SetupRigidbody(prefab);
      Prefabs.SetupCapsuleCollider(prefab);
      Prefabs.SetupMainHurtbox(prefab, model);
      Prefabs.SetupFootstepController(model);
      Prefabs.SetupRagdoll(model);
      Prefabs.SetupAimAnimator(prefab, model);
      Prefabs.bodyPrefabs.Add(prefab);
      return prefab;
    }

    internal static void CreateGenericDoppelganger(
      GameObject bodyPrefab,
      string masterName,
      string masterToCopy)
    {
      GameObject gameObject = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/" + masterToCopy + "MonsterMaster"), masterName, true);
      gameObject.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;
      Prefabs.masterPrefabs.Add(gameObject);
    }

    private static Transform SetupModel(
      GameObject prefab,
      Transform modelTransform,
      BodyInfo bodyInfo)
    {
      GameObject gameObject1 = new GameObject("ModelBase");
      gameObject1.transform.parent = prefab.transform;
      gameObject1.transform.localPosition = bodyInfo.modelBasePosition;
      gameObject1.transform.localRotation = Quaternion.identity;
      gameObject1.transform.localScale = new Vector3(1f, 1f, 1f);
      GameObject gameObject2 = new GameObject("CameraPivot")
      {
        transform = {
          parent = gameObject1.transform,
          localPosition = bodyInfo.cameraPivotPosition,
          localRotation = Quaternion.identity,
          localScale = Vector3.one
        }
      };
      prefab.GetComponent<CharacterBody>().aimOriginTransform = new GameObject("AimOrigin")
      {
        transform = {
          parent = gameObject1.transform,
          localPosition = bodyInfo.aimOriginPosition,
          localRotation = Quaternion.identity,
          localScale = Vector3.one
        }
      }.transform;
      modelTransform.parent = gameObject1.transform;
      modelTransform.localPosition = Vector3.zero;
      modelTransform.localRotation = Quaternion.identity;
      return gameObject1.transform;
    }

    private static GameObject CreateModel(GameObject main, string modelName)
    {
      Object.DestroyImmediate((Object) ((Component) main.transform.Find("ModelBase")).gameObject);
      Object.DestroyImmediate((Object) ((Component) main.transform.Find("CameraPivot")).gameObject);
      Object.DestroyImmediate((Object) ((Component) main.transform.Find("AimOrigin")).gameObject);
      if (!Object.op_Equality((Object) Assets.mainAssetBundle.LoadAsset<GameObject>(modelName), (Object) null))
        return Object.Instantiate<GameObject>(Assets.mainAssetBundle.LoadAsset<GameObject>(modelName));
      Debug.LogError((object) "Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
      return (GameObject) null;
    }

    internal static void SetupCharacterModel(
      GameObject prefab,
      CustomRendererInfo[] rendererInfo,
      int mainRendererIndex)
    {
      CharacterModel characterModel = ((Component) prefab.GetComponent<ModelLocator>().modelTransform).gameObject.AddComponent<CharacterModel>();
      ChildLocator component = ((Component) characterModel).GetComponent<ChildLocator>();
      characterModel.body = prefab.GetComponent<CharacterBody>();
      if (!Object.op_Implicit((Object) component))
      {
        Debug.LogError((object) "Failed CharacterModel setup: ChildLocator component does not exist on the model");
      }
      else
      {
        List<CharacterModel.RendererInfo> rendererInfoList = new List<CharacterModel.RendererInfo>();
        for (int index = 0; index < rendererInfo.Length; ++index)
        {
          if (!Object.op_Implicit((Object) component.FindChild(rendererInfo[index].childName)))
            Debug.LogError((object) ("Trying to add a RendererInfo for a renderer that does not exist: " + rendererInfo[index].childName));
          else if (Object.op_Implicit((Object) ((Component) component.FindChild(rendererInfo[index].childName)).GetComponent<Renderer>()))
            rendererInfoList.Add(new CharacterModel.RendererInfo()
            {
              renderer = ((Component) component.FindChild(rendererInfo[index].childName)).GetComponent<Renderer>(),
              defaultMaterial = rendererInfo[index].material,
              ignoreOverlays = rendererInfo[index].ignoreOverlays,
              defaultShadowCastingMode = (ShadowCastingMode) 1
            });
        }
        characterModel.baseRendererInfos = rendererInfoList.ToArray();
        characterModel.autoPopulateLightInfos = true;
        characterModel.invisibilityCount = 0;
        characterModel.temporaryOverlays = new List<TemporaryOverlay>();
        if (mainRendererIndex > characterModel.baseRendererInfos.Length)
          return;
        characterModel.mainSkinnedMeshRenderer = ((Component) characterModel.baseRendererInfos[mainRendererIndex].renderer).GetComponent<SkinnedMeshRenderer>();
      }
    }

    private static void SetupCharacterDirection(
      GameObject prefab,
      Transform modelBaseTransform,
      Transform modelTransform)
    {
      CharacterDirection component = prefab.GetComponent<CharacterDirection>();
      component.targetTransform = modelBaseTransform;
      component.overrideAnimatorForwardTransform = (Transform) null;
      component.rootMotionAccumulator = (RootMotionAccumulator) null;
      component.modelAnimator = ((Component) modelTransform).GetComponent<Animator>();
      component.driveFromRootRotation = false;
      component.turnSpeed = 720f;
    }

    private static void SetupModelLocator(
      GameObject prefab,
      Transform modelBaseTransform,
      Transform modelTransform)
    {
      ModelLocator component = prefab.GetComponent<ModelLocator>();
      component.modelTransform = modelTransform;
      component.modelBaseTransform = modelBaseTransform;
    }

    private static void SetupRigidbody(GameObject prefab) => prefab.GetComponent<Rigidbody>().mass = 100f;

    private static void SetupCapsuleCollider(GameObject prefab)
    {
      CapsuleCollider component = prefab.GetComponent<CapsuleCollider>();
      component.center = new Vector3(0.0f, 0.0f, 0.0f);
      component.radius = 0.5f;
      component.height = 1.82f;
      component.direction = 1;
    }

    private static void SetupMainHurtbox(GameObject prefab, GameObject model)
    {
      ChildLocator component = model.GetComponent<ChildLocator>();
      if (!Object.op_Implicit((Object) component.FindChild("MainHurtbox")))
        return;
      HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();
      HurtBox hurtBox = ((Component) component.FindChild("MainHurtbox")).gameObject.AddComponent<HurtBox>();
      ((Component) hurtBox).gameObject.layer = LayerIndex.entityPrecise.intVal;
      hurtBox.healthComponent = prefab.GetComponent<HealthComponent>();
      hurtBox.isBullseye = true;
      hurtBox.damageModifier = (HurtBox.DamageModifier) 0;
      hurtBox.hurtBoxGroup = hurtBoxGroup;
      hurtBox.indexInGroup = (short) 0;
      hurtBoxGroup.hurtBoxes = new HurtBox[1]{ hurtBox };
      hurtBoxGroup.mainHurtBox = hurtBox;
      hurtBoxGroup.bullseyeCount = 1;
    }

    private static void SetupFootstepController(GameObject model)
    {
      FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
      footstepHandler.baseFootstepString = "Play_player_footstep";
      footstepHandler.sprintFootstepOverrideString = "";
      footstepHandler.enableFootstepDust = true;
      footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");
    }

    private static void SetupRagdoll(GameObject model)
    {
      RagdollController component1 = model.GetComponent<RagdollController>();
      if (!Object.op_Implicit((Object) component1))
        return;
      if (Object.op_Equality((Object) Prefabs.ragdollMaterial, (Object) null))
        Prefabs.ragdollMaterial = ((Component) Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1]).GetComponent<Collider>().material;
      foreach (Transform bone in component1.bones)
      {
        if (Object.op_Implicit((Object) bone))
        {
          ((Component) bone).gameObject.layer = LayerIndex.ragdoll.intVal;
          Collider component2 = ((Component) bone).GetComponent<Collider>();
          if (Object.op_Implicit((Object) component2))
          {
            component2.material = Prefabs.ragdollMaterial;
            component2.sharedMaterial = Prefabs.ragdollMaterial;
          }
        }
      }
    }

    private static void SetupAimAnimator(GameObject prefab, GameObject model)
    {
      AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
      aimAnimator.directionComponent = prefab.GetComponent<CharacterDirection>();
      aimAnimator.pitchRangeMax = 60f;
      aimAnimator.pitchRangeMin = -60f;
      aimAnimator.yawRangeMin = -80f;
      aimAnimator.yawRangeMax = 80f;
      aimAnimator.pitchGiveupRange = 30f;
      aimAnimator.yawGiveupRange = 10f;
      aimAnimator.giveupDuration = 3f;
      aimAnimator.inputBank = prefab.GetComponent<InputBankTest>();
    }

    internal static void SetupHitbox(
      GameObject prefab,
      Transform hitboxTransform,
      string hitboxName)
    {
      HitBoxGroup hitBoxGroup = prefab.AddComponent<HitBoxGroup>();
      HitBox hitBox = ((Component) hitboxTransform).gameObject.AddComponent<HitBox>();
      ((Component) hitboxTransform).gameObject.layer = LayerIndex.projectile.intVal;
      hitBoxGroup.hitBoxes = new HitBox[1]{ hitBox };
      hitBoxGroup.groupName = hitboxName;
    }

    internal static void SetupHitbox(
      GameObject prefab,
      string hitboxName,
      params Transform[] hitboxTransforms)
    {
      HitBoxGroup hitBoxGroup = prefab.AddComponent<HitBoxGroup>();
      List<HitBox> hitBoxList = new List<HitBox>();
      foreach (Transform hitboxTransform in hitboxTransforms)
      {
        HitBox hitBox = ((Component) hitboxTransform).gameObject.AddComponent<HitBox>();
        ((Component) hitboxTransform).gameObject.layer = LayerIndex.projectile.intVal;
        hitBoxList.Add(hitBox);
      }
      hitBoxGroup.hitBoxes = hitBoxList.ToArray();
      hitBoxGroup.groupName = hitboxName;
    }
  }
}
