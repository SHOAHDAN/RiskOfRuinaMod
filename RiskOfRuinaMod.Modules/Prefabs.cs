using System.Collections.Generic;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Rendering;

namespace RiskOfRuinaMod.Modules;

internal static class Prefabs
{
	private static PhysicMaterial ragdollMaterial;

	internal static List<SurvivorDef> survivorDefinitions = new List<SurvivorDef>();

	internal static List<GameObject> bodyPrefabs = new List<GameObject>();

	internal static List<GameObject> masterPrefabs = new List<GameObject>();

	internal static List<GameObject> projectilePrefabs = new List<GameObject>();

	internal static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string namePrefix, UnlockableDef unlockableDef, float sortPosition)
	{
		string displayNameToken = "COF_" + namePrefix + "_BODY_NAME";
		string descriptionToken = "COF_" + namePrefix + "_BODY_DESCRIPTION";
		string outroFlavorToken = "COF_" + namePrefix + "_BODY_OUTRO_FLAVOR";
		string mainEndingEscapeFailureFlavorToken = "COF_" + namePrefix + "_BODY_OUTRO_FAILURE";
		SurvivorDef val = ScriptableObject.CreateInstance<SurvivorDef>();
		val.bodyPrefab = bodyPrefab;
		val.displayPrefab = displayPrefab;
		val.primaryColor = charColor;
		val.displayNameToken = displayNameToken;
		val.descriptionToken = descriptionToken;
		val.outroFlavorToken = outroFlavorToken;
		val.mainEndingEscapeFailureFlavorToken = mainEndingEscapeFailureFlavorToken;
		val.desiredSortPosition = sortPosition;
		val.unlockableDef = unlockableDef;
		survivorDefinitions.Add(val);
	}

	internal static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string namePrefix)
	{
		RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, namePrefix, null, 100f);
	}

	internal static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string namePrefix, float sortPosition)
	{
		RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, namePrefix, null, sortPosition);
	}

	internal static void RegisterNewSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string namePrefix, UnlockableDef unlockableDef)
	{
		RegisterNewSurvivor(bodyPrefab, displayPrefab, charColor, namePrefix, unlockableDef, 100f);
	}

	internal static GameObject CreateDisplayPrefab(string modelName, GameObject prefab, BodyInfo bodyInfo)
	{
		if (!Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body"))
		{
			return null;
		}
		GameObject gameObject = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body").InstantiateClone(modelName + "Prefab");
		GameObject gameObject2 = CreateModel(gameObject, modelName);
		Transform transform = SetupModel(gameObject, gameObject2.transform, bodyInfo);
		gameObject2.AddComponent<CharacterModel>().baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;
		Assets.ConvertAllRenderersToHopooShader(gameObject2);
		return gameObject2.gameObject;
	}

	internal static GameObject CreatePrefab(string bodyName, string modelName, BodyInfo bodyInfo)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		if (!Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body"))
		{
			return null;
		}
		GameObject gameObject = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body").InstantiateClone(bodyName);
		Transform transform = null;
		GameObject gameObject2 = null;
		if (modelName != "mdl")
		{
			gameObject2 = CreateModel(gameObject, modelName);
			if (gameObject2 == null)
			{
				gameObject2 = ((Component)(object)gameObject.GetComponentInChildren<CharacterModel>()).gameObject;
			}
			transform = SetupModel(gameObject, gameObject2.transform, bodyInfo);
		}
		CharacterBody component = gameObject.GetComponent<CharacterBody>();
		((Object)(object)component).name = bodyInfo.bodyName;
		component.baseNameToken = bodyInfo.bodyNameToken;
		component.subtitleNameToken = bodyInfo.subtitleNameToken;
		component.portraitIcon = bodyInfo.characterPortrait;
		component.crosshairPrefab = bodyInfo.crosshair;
		component.bodyFlags = (BodyFlags)16;
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
		component.aimOriginTransform = transform.Find("AimOrigin");
		component.hullClassification = (HullClassification)0;
		component.preferredPodPrefab = bodyInfo.podPrefab;
		component.isChampion = false;
		component.bodyColor = bodyInfo.bodyColor;
		if (transform != null)
		{
			SetupCharacterDirection(gameObject, transform, gameObject2.transform);
		}
		gameObject.GetComponent<CameraTargetParams>().cameraParams = CameraParams.defaultCameraParamsRedMist;
		if (transform != null)
		{
			SetupModelLocator(gameObject, transform, gameObject2.transform);
		}
		SetupRigidbody(gameObject);
		SetupCapsuleCollider(gameObject);
		SetupMainHurtbox(gameObject, gameObject2);
		SetupFootstepController(gameObject2);
		SetupRagdoll(gameObject2);
		SetupAimAnimator(gameObject, gameObject2);
		bodyPrefabs.Add(gameObject);
		return gameObject;
	}

	internal static void CreateGenericDoppelganger(GameObject bodyPrefab, string masterName, string masterToCopy)
	{
		GameObject gameObject = Resources.Load<GameObject>("Prefabs/CharacterMasters/" + masterToCopy + "MonsterMaster").InstantiateClone(masterName, registerNetwork: true);
		gameObject.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;
		masterPrefabs.Add(gameObject);
	}

	private static Transform SetupModel(GameObject prefab, Transform modelTransform, BodyInfo bodyInfo)
	{
		GameObject gameObject = new GameObject("ModelBase");
		gameObject.transform.parent = prefab.transform;
		gameObject.transform.localPosition = bodyInfo.modelBasePosition;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		GameObject gameObject2 = new GameObject("CameraPivot");
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = bodyInfo.cameraPivotPosition;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = Vector3.one;
		GameObject gameObject3 = new GameObject("AimOrigin");
		gameObject3.transform.parent = gameObject.transform;
		gameObject3.transform.localPosition = bodyInfo.aimOriginPosition;
		gameObject3.transform.localRotation = Quaternion.identity;
		gameObject3.transform.localScale = Vector3.one;
		prefab.GetComponent<CharacterBody>().aimOriginTransform = gameObject3.transform;
		modelTransform.parent = gameObject.transform;
		modelTransform.localPosition = Vector3.zero;
		modelTransform.localRotation = Quaternion.identity;
		return gameObject.transform;
	}

	private static GameObject CreateModel(GameObject main, string modelName)
	{
		Object.DestroyImmediate(main.transform.Find("ModelBase").gameObject);
		Object.DestroyImmediate(main.transform.Find("CameraPivot").gameObject);
		Object.DestroyImmediate(main.transform.Find("AimOrigin").gameObject);
		if (Assets.mainAssetBundle.LoadAsset<GameObject>(modelName) == null)
		{
			Debug.LogError("Trying to load a null model- check to see if the name in your code matches the name of the object in Unity");
			return null;
		}
		return Object.Instantiate(Assets.mainAssetBundle.LoadAsset<GameObject>(modelName));
	}

	internal static void SetupCharacterModel(GameObject prefab, CustomRendererInfo[] rendererInfo, int mainRendererIndex)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		CharacterModel val = prefab.GetComponent<ModelLocator>().get_modelTransform().gameObject.AddComponent<CharacterModel>();
		ChildLocator component = ((Component)(object)val).GetComponent<ChildLocator>();
		val.body = prefab.GetComponent<CharacterBody>();
		if (!(Object)(object)component)
		{
			Debug.LogError("Failed CharacterModel setup: ChildLocator component does not exist on the model");
			return;
		}
		List<RendererInfo> list = new List<RendererInfo>();
		for (int i = 0; i < rendererInfo.Length; i++)
		{
			if (!component.FindChild(rendererInfo[i].childName))
			{
				Debug.LogError("Trying to add a RendererInfo for a renderer that does not exist: " + rendererInfo[i].childName);
				continue;
			}
			Renderer component2 = component.FindChild(rendererInfo[i].childName).GetComponent<Renderer>();
			if ((bool)component2)
			{
				list.Add(new RendererInfo
				{
					renderer = component.FindChild(rendererInfo[i].childName).GetComponent<Renderer>(),
					defaultMaterial = rendererInfo[i].material,
					ignoreOverlays = rendererInfo[i].ignoreOverlays,
					defaultShadowCastingMode = ShadowCastingMode.On
				});
			}
		}
		val.baseRendererInfos = list.ToArray();
		val.autoPopulateLightInfos = true;
		val.invisibilityCount = 0;
		val.temporaryOverlays = new List<TemporaryOverlay>();
		if (mainRendererIndex <= val.baseRendererInfos.Length)
		{
			val.mainSkinnedMeshRenderer = val.baseRendererInfos[mainRendererIndex].renderer.GetComponent<SkinnedMeshRenderer>();
		}
	}

	private static void SetupCharacterDirection(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
	{
		CharacterDirection component = prefab.GetComponent<CharacterDirection>();
		component.targetTransform = modelBaseTransform;
		component.overrideAnimatorForwardTransform = null;
		component.rootMotionAccumulator = null;
		component.modelAnimator = modelTransform.GetComponent<Animator>();
		component.driveFromRootRotation = false;
		component.turnSpeed = 720f;
	}

	private static void SetupModelLocator(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
	{
		ModelLocator component = prefab.GetComponent<ModelLocator>();
		component.set_modelTransform(modelTransform);
		component.modelBaseTransform = modelBaseTransform;
	}

	private static void SetupRigidbody(GameObject prefab)
	{
		Rigidbody component = prefab.GetComponent<Rigidbody>();
		component.mass = 100f;
	}

	private static void SetupCapsuleCollider(GameObject prefab)
	{
		CapsuleCollider component = prefab.GetComponent<CapsuleCollider>();
		component.center = new Vector3(0f, 0f, 0f);
		component.radius = 0.5f;
		component.height = 1.82f;
		component.direction = 1;
	}

	private static void SetupMainHurtbox(GameObject prefab, GameObject model)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		ChildLocator component = model.GetComponent<ChildLocator>();
		if ((bool)component.FindChild("MainHurtbox"))
		{
			HurtBoxGroup val = model.AddComponent<HurtBoxGroup>();
			HurtBox val2 = component.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
			((Component)(object)val2).gameObject.layer = LayerIndex.entityPrecise.intVal;
			val2.healthComponent = prefab.GetComponent<HealthComponent>();
			val2.isBullseye = true;
			val2.damageModifier = (DamageModifier)0;
			val2.hurtBoxGroup = val;
			val2.indexInGroup = 0;
			val.hurtBoxes = (HurtBox[])(object)new HurtBox[1] { val2 };
			val.mainHurtBox = val2;
			val.bullseyeCount = 1;
		}
	}

	private static void SetupFootstepController(GameObject model)
	{
		FootstepHandler val = model.AddComponent<FootstepHandler>();
		val.baseFootstepString = "Play_player_footstep";
		val.sprintFootstepOverrideString = "";
		val.enableFootstepDust = true;
		val.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");
	}

	private static void SetupRagdoll(GameObject model)
	{
		RagdollController component = model.GetComponent<RagdollController>();
		if (!(Object)(object)component)
		{
			return;
		}
		if (ragdollMaterial == null)
		{
			ragdollMaterial = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;
		}
		Transform[] bones = component.bones;
		foreach (Transform transform in bones)
		{
			if ((bool)transform)
			{
				transform.gameObject.layer = LayerIndex.ragdoll.intVal;
				Collider component2 = transform.GetComponent<Collider>();
				if ((bool)component2)
				{
					component2.material = ragdollMaterial;
					component2.sharedMaterial = ragdollMaterial;
				}
			}
		}
	}

	private static void SetupAimAnimator(GameObject prefab, GameObject model)
	{
		AimAnimator val = model.AddComponent<AimAnimator>();
		val.directionComponent = prefab.GetComponent<CharacterDirection>();
		val.pitchRangeMax = 60f;
		val.pitchRangeMin = -60f;
		val.yawRangeMin = -80f;
		val.yawRangeMax = 80f;
		val.pitchGiveupRange = 30f;
		val.yawGiveupRange = 10f;
		val.giveupDuration = 3f;
		val.inputBank = prefab.GetComponent<InputBankTest>();
	}

	internal static void SetupHitbox(GameObject prefab, Transform hitboxTransform, string hitboxName)
	{
		HitBoxGroup val = prefab.AddComponent<HitBoxGroup>();
		HitBox val2 = hitboxTransform.gameObject.AddComponent<HitBox>();
		hitboxTransform.gameObject.layer = LayerIndex.projectile.intVal;
		val.hitBoxes = (HitBox[])(object)new HitBox[1] { val2 };
		val.groupName = hitboxName;
	}

	internal static void SetupHitbox(GameObject prefab, string hitboxName, params Transform[] hitboxTransforms)
	{
		HitBoxGroup val = prefab.AddComponent<HitBoxGroup>();
		List<HitBox> list = new List<HitBox>();
		foreach (Transform transform in hitboxTransforms)
		{
			HitBox item = transform.gameObject.AddComponent<HitBox>();
			transform.gameObject.layer = LayerIndex.projectile.intVal;
			list.Add(item);
		}
		val.hitBoxes = list.ToArray();
		val.groupName = hitboxName;
	}
}
