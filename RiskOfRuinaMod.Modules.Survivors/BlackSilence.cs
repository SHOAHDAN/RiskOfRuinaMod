using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using EntityStates;
using On.RoR2.UI;
using RiskOfRuinaMod.Modules.Achievements;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.SkillStates;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfRuinaMod.Modules.Survivors;

internal class BlackSilence : SurvivorBase
{
	public static GameObject blackSilencePrefab;

	public static SkillDef NormalDodge;

	public static SkillDef EGODodge;

	public static SkillDef NormalBlock;

	public static SkillDef EGOBlock;

	public static SkillDef EGOActivate;

	public static SkillDef HorizontalSlash;

	public static SkillDef BasicAttack;

	internal static Material redMistMat = Assets.CreateMaterial("matRedMist", 1f, Color.red, 1f);

	internal static Material mimicryMat = Assets.CreateMaterial("matMimicry", 0f, Color.white, 1f);

	internal static Material coatMat = Assets.CreateMaterial("matCoat", 0f, Color.white, 1f);

	internal static Material coatEGOMat = Assets.mainAssetBundle.LoadAsset<Material>("matCoatEGO");

	private static UnlockableDef masterySkinUnlockableDef;

	internal override string bodyName { get; set; } = "BlackSilence";


	internal override GameObject bodyPrefab { get; set; }

	internal override GameObject displayPrefab { get; set; }

	internal override float sortPosition { get; set; } = 100f;


	internal override ConfigEntry<bool> characterEnabled { get; set; }

	internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
	{
		armor = 20f,
		armorGrowth = 0f,
		bodyName = "BlackSilenceBody",
		bodyNameToken = "COF_BLACKSILENCE_BODY_NAME",
		bodyColor = Color.red,
		characterPortrait = Assets.LoadCharacterIcon("RedMist"),
		crosshair = Assets.LoadCrosshair("SimpleDot"),
		damage = 12f,
		healthGrowth = 33f,
		healthRegen = 1f,
		jumpCount = 2,
		maxHealth = 110f,
		subtitleNameToken = "COF_BLACKSILENCE_BODY_SUBTITLE",
		podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),
		moveSpeed = 7f,
		attackSpeed = 1f
	};


	internal override int mainRendererIndex { get; set; } = 3;


	internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[4]
	{
		new CustomRendererInfo
		{
			childName = "CoatNormal",
			material = coatMat
		},
		new CustomRendererInfo
		{
			childName = "CoatEGO",
			material = coatEGOMat
		},
		new CustomRendererInfo
		{
			childName = "Mimicry",
			material = mimicryMat
		},
		new CustomRendererInfo
		{
			childName = "Model",
			material = redMistMat
		}
	};


	internal override Type characterMainState { get; set; } = typeof(GenericCharacterMain);


	internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }

	internal override List<KeyAssetRuleGroup> itemDisplayRules { get; set; }

	internal override UnlockableDef characterUnlockableDef { get; set; }

	internal override void InitializeCharacter()
	{
		base.InitializeCharacter();
		blackSilencePrefab = bodyPrefab;
		bodyPrefab.AddComponent<TargetTracker>();
		bodyPrefab.AddComponent<RedMistEmotionComponent>();
		bodyPrefab.AddComponent<RedMistStatTracker>();
		HUD.Awake += RedMist.HUDAwake;
	}

	internal override void InitializeUnlockables()
	{
		masterySkinUnlockableDef = Unlockables.AddUnlockable<RedMistMasteryAchievement>(serverTracked: true);
	}

	internal override void InitializeDoppelganger()
	{
		Prefabs.CreateGenericDoppelganger(SurvivorBase.instance.bodyPrefab, bodyName + "MonsterMaster", "Loader");
	}

	internal override void InitializeHitboxes()
	{
		ChildLocator componentInChildren = bodyPrefab.GetComponentInChildren<ChildLocator>();
		GameObject gameObject = ((Component)(object)componentInChildren).gameObject;
		Transform hitboxTransform = componentInChildren.FindChild("BasicHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "Basic");
		hitboxTransform = componentInChildren.FindChild("BasicThirdHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "BasicThird");
		hitboxTransform = componentInChildren.FindChild("ForwardHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "Forward");
		hitboxTransform = componentInChildren.FindChild("BackHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "Back");
		hitboxTransform = componentInChildren.FindChild("JumpHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "Jump");
		hitboxTransform = componentInChildren.FindChild("SideHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "Side");
		hitboxTransform = componentInChildren.FindChild("AirBasicHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "AirBasic");
		hitboxTransform = componentInChildren.FindChild("BlockCounterHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "BlockCounter");
		hitboxTransform = componentInChildren.FindChild("EGOBasicHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOBasic");
		hitboxTransform = componentInChildren.FindChild("EGOBasicThirdHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOBasicThird");
		hitboxTransform = componentInChildren.FindChild("EGOForwardHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOForward");
		hitboxTransform = componentInChildren.FindChild("EGOBackHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOBack");
		hitboxTransform = componentInChildren.FindChild("EGOSideHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOSide");
		hitboxTransform = componentInChildren.FindChild("EGOJumpHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOJump");
		hitboxTransform = componentInChildren.FindChild("EGOAirBasicHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "EGOAirBasic");
		hitboxTransform = componentInChildren.FindChild("HorizontalHitbox");
		Prefabs.SetupHitbox(gameObject, hitboxTransform, "Horizontal");
	}

	internal override void InitializeSkills()
	{
		Skills.CreateSkillFamilies(bodyPrefab);
		string prefix = "COF";
		InitializeSkills(bodyPrefab, prefix);
	}

	public static void InitializeSkills(GameObject bodyPrefab, string prefix)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		SkillLocator component = bodyPrefab.GetComponent<SkillLocator>();
		component.passiveSkill.enabled = true;
		component.passiveSkill.skillNameToken = prefix + "_REDMIST_BODY_PASSIVE_NAME";
		component.passiveSkill.skillDescriptionToken = prefix + "_REDMIST_BODY_PASSIVE_DESCRIPTION";
		component.passiveSkill.icon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistPassiveIcon");
		BasicAttack = Skills.CreatePrimarySkillDef(new SerializableEntityStateType(typeof(BasicStringStart)), "Body", prefix + "_REDMIST_BODY_PRIMARY_LEVELSLASH_NAME", prefix + "_REDMIST_BODY_PRIMARY_LEVELSLASH_DESCRIPTION", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistPrimaryIcon"), agile: false);
		Skills.AddPrimarySkill(bodyPrefab, BasicAttack);
		SkillDefInfo skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "_REDMIST_BODY_SECONDARY_ONRUSH_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_SECONDARY_ONRUSH_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_SECONDARY_ONRUSH_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistSecondaryIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(Onrush));
		skillDefInfo.activationStateMachineName = "Body";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 8f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = true;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)1;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = true;
		skillDefInfo.mustKeyPress = false;
		skillDefInfo.cancelSprintingOnActivation = true;
		skillDefInfo.rechargeStock = 1;
		skillDefInfo.requiredStock = 1;
		skillDefInfo.stockToConsume = 1;
		SkillDef val = Skills.CreateTrackerSkillDef(skillDefInfo);
		Skills.AddSecondarySkills(bodyPrefab, val);
		skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "_REDMIST_BODY_UTILITY_DODGE_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_UTILITY_DODGE_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_UTILITY_DODGE_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistUtilityIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(Dodge));
		skillDefInfo.activationStateMachineName = "Body";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 0f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = false;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)2;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = false;
		skillDefInfo.mustKeyPress = false;
		skillDefInfo.cancelSprintingOnActivation = true;
		skillDefInfo.rechargeStock = 1;
		skillDefInfo.requiredStock = 0;
		skillDefInfo.stockToConsume = 0;
		NormalDodge = Skills.CreateSkillDef(skillDefInfo);
		skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "_REDMIST_BODY_UTILITY_BLOCK_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_UTILITY_BLOCK_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_UTILITY_BLOCK_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistUtilityTwoIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(Block));
		skillDefInfo.activationStateMachineName = "Body";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 0f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = false;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)2;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = true;
		skillDefInfo.mustKeyPress = false;
		skillDefInfo.cancelSprintingOnActivation = true;
		skillDefInfo.rechargeStock = 1;
		skillDefInfo.requiredStock = 0;
		skillDefInfo.stockToConsume = 0;
		NormalBlock = Skills.CreateSkillDef(skillDefInfo);
		Skills.AddUtilitySkills(bodyPrefab, NormalDodge, NormalBlock);
		skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "_REDMIST_BODY_UTILITY_DODGE_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_UTILITY_DODGE_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_UTILITY_DODGE_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistUtilityIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(EGODodge));
		skillDefInfo.activationStateMachineName = "Slide";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 0f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = false;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)2;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = false;
		skillDefInfo.mustKeyPress = false;
		skillDefInfo.cancelSprintingOnActivation = false;
		skillDefInfo.rechargeStock = 1;
		skillDefInfo.requiredStock = 0;
		skillDefInfo.stockToConsume = 0;
		EGODodge = Skills.CreateSkillDef(skillDefInfo);
		Skills.skillDefs.Add(EGODodge);
		skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "_REDMIST_BODY_UTILITY_BLOCK_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_UTILITY_BLOCK_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_UTILITY_BLOCK_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistUtilityTwoIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(EGOBlock));
		skillDefInfo.activationStateMachineName = "Slide";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 0f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = false;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)2;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = true;
		skillDefInfo.mustKeyPress = false;
		skillDefInfo.cancelSprintingOnActivation = false;
		skillDefInfo.rechargeStock = 1;
		skillDefInfo.requiredStock = 0;
		skillDefInfo.stockToConsume = 0;
		EGOBlock = Skills.CreateSkillDef(skillDefInfo);
		Skills.skillDefs.Add(NormalBlock);
		skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "REDMIST_BODY_SPECIAL_EGO_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_SPECIAL_EGO_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_SPECIAL_EGO_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistSpecialIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(EGOActivate));
		skillDefInfo.activationStateMachineName = "Body";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 0f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = false;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)2;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = true;
		skillDefInfo.mustKeyPress = true;
		skillDefInfo.cancelSprintingOnActivation = true;
		skillDefInfo.rechargeStock = 100;
		skillDefInfo.requiredStock = 0;
		skillDefInfo.stockToConsume = 0;
		skillDefInfo.keywordTokens = new string[1] { "KEYWORD_EGO" };
		EGOActivate = Skills.CreateEGOSkillDef(skillDefInfo);
		Skills.AddSpecialSkills(bodyPrefab, EGOActivate);
		skillDefInfo = new SkillDefInfo();
		skillDefInfo.skillName = prefix + "REDMIST_BODY_SPECIAL_HORIZONTAL_NAME";
		skillDefInfo.skillNameToken = prefix + "_REDMIST_BODY_SPECIAL_HORIZONTAL_NAME";
		skillDefInfo.skillDescriptionToken = prefix + "_REDMIST_BODY_SPECIAL_HORIZONTAL_DESCRIPTION";
		skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistSpecialTwoIcon");
		skillDefInfo.activationState = new SerializableEntityStateType(typeof(EGOHorizontal));
		skillDefInfo.activationStateMachineName = "Body";
		skillDefInfo.baseMaxStock = 1;
		skillDefInfo.baseRechargeInterval = 20f;
		skillDefInfo.beginSkillCooldownOnSkillEnd = true;
		skillDefInfo.canceledFromSprinting = false;
		skillDefInfo.forceSprintDuringState = false;
		skillDefInfo.fullRestockOnAssign = true;
		skillDefInfo.interruptPriority = (InterruptPriority)2;
		skillDefInfo.resetCooldownTimerOnUse = false;
		skillDefInfo.isCombatSkill = true;
		skillDefInfo.mustKeyPress = false;
		skillDefInfo.cancelSprintingOnActivation = true;
		skillDefInfo.rechargeStock = 1;
		skillDefInfo.requiredStock = 1;
		skillDefInfo.stockToConsume = 1;
		HorizontalSlash = Skills.CreateSkillDef(skillDefInfo);
		Skills.skillDefs.Add(HorizontalSlash);
	}

	internal override void InitializeSkins()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		GameObject gameObject = bodyPrefab.GetComponentInChildren<ModelLocator>().get_modelTransform().gameObject;
		CharacterModel component = gameObject.GetComponent<CharacterModel>();
		ModelSkinController val = gameObject.AddComponent<ModelSkinController>();
		ChildLocator component2 = gameObject.GetComponent<ChildLocator>();
		SkinnedMeshRenderer mainSkinnedMeshRenderer = component.mainSkinnedMeshRenderer;
		RendererInfo[] baseRendererInfos = component.baseRendererInfos;
		List<SkinDef> list = new List<SkinDef>();
		SkinDef val2 = Skins.CreateSkinDef("COF_REDMIST_BODY_DEFAULT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistMainSkin"), baseRendererInfos, mainSkinnedMeshRenderer, gameObject);
		val2.meshReplacements = (MeshReplacement[])(object)new MeshReplacement[4]
		{
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshRedMistCoat"),
				renderer = baseRendererInfos[0].renderer
			},
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshRedMistCoat"),
				renderer = baseRendererInfos[1].renderer
			},
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshMimicry"),
				renderer = baseRendererInfos[2].renderer
			},
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshRedMist"),
				renderer = baseRendererInfos[3].renderer
			}
		};
		list.Add(val2);
		RendererInfo[] array = (RendererInfo[])(object)new RendererInfo[baseRendererInfos.Length];
		baseRendererInfos.CopyTo(array, 0);
		array[0].defaultMaterial = Assets.CreateMaterial("matArgaliaCoat", 0f, Color.white, 1f);
		array[1].defaultMaterial = Assets.CreateMaterial("matArgaliaCoat", 0f, Color.white, 1f);
		array[2].defaultMaterial = Assets.CreateMaterial("matVibrato", 0.9f, new Color(0f, 0.95f, 1f), 1f);
		array[3].defaultMaterial = Assets.CreateMaterial("matArgalia", 0.9f, new Color(0f, 0.95f, 1f), 1f);
		SkinDef val3 = Skins.CreateSkinDef("COF_REDMIST_BODY_MASTERY_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistSecondSkin"), array, mainSkinnedMeshRenderer, gameObject, masterySkinUnlockableDef);
		val3.meshReplacements = (MeshReplacement[])(object)new MeshReplacement[4]
		{
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArgaliaCoat"),
				renderer = baseRendererInfos[0].renderer
			},
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArgaliaCoat"),
				renderer = baseRendererInfos[1].renderer
			},
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshVibrato"),
				renderer = baseRendererInfos[2].renderer
			},
			new MeshReplacement
			{
				mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArgalia"),
				renderer = baseRendererInfos[3].renderer
			}
		};
		list.Add(val3);
		val.skins = list.ToArray();
	}

	internal static void HUDAwake(HUD.orig_Awake orig, HUD self)
	{
		orig(self);
		EmotionHUD emotionHUD = ((Component)(object)self).gameObject.AddComponent<EmotionHUD>();
		GameObject gameObject = UnityEngine.Object.Instantiate(Assets.mainAssetBundle.LoadAsset<GameObject>("EmotionGauge"), ((Component)(object)self).transform.Find("MainContainer").Find("MainUIArea").Find("SpringCanvas")
			.Find("BottomLeftCluster"));
		gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
		gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
		gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
		gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.2f);
		gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -55f);
		emotionHUD.emotionGauge = gameObject;
		emotionHUD.emotionFill = gameObject.transform.Find("GaugeFill").gameObject.GetComponent<Image>();
	}

	internal override void SetItemDisplays()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_102d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_117d: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1188: Unknown result type (might be due to invalid IL or missing references)
		//IL_1189: Unknown result type (might be due to invalid IL or missing references)
		//IL_118e: Unknown result type (might be due to invalid IL or missing references)
		//IL_119d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1244: Unknown result type (might be due to invalid IL or missing references)
		//IL_1249: Unknown result type (might be due to invalid IL or missing references)
		//IL_124a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1254: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1269: Unknown result type (might be due to invalid IL or missing references)
		//IL_127f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1291: Unknown result type (might be due to invalid IL or missing references)
		//IL_129a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1315: Unknown result type (might be due to invalid IL or missing references)
		//IL_1316: Unknown result type (might be due to invalid IL or missing references)
		//IL_1320: Unknown result type (might be due to invalid IL or missing references)
		//IL_1321: Unknown result type (might be due to invalid IL or missing references)
		//IL_1326: Unknown result type (might be due to invalid IL or missing references)
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_134b: Unknown result type (might be due to invalid IL or missing references)
		//IL_135d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_146a: Unknown result type (might be due to invalid IL or missing references)
		//IL_146f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1470: Unknown result type (might be due to invalid IL or missing references)
		//IL_147a: Unknown result type (might be due to invalid IL or missing references)
		//IL_147b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1480: Unknown result type (might be due to invalid IL or missing references)
		//IL_148f: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1536: Unknown result type (might be due to invalid IL or missing references)
		//IL_153b: Unknown result type (might be due to invalid IL or missing references)
		//IL_153c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1546: Unknown result type (might be due to invalid IL or missing references)
		//IL_1547: Unknown result type (might be due to invalid IL or missing references)
		//IL_154c: Unknown result type (might be due to invalid IL or missing references)
		//IL_155b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1571: Unknown result type (might be due to invalid IL or missing references)
		//IL_1583: Unknown result type (might be due to invalid IL or missing references)
		//IL_158c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1602: Unknown result type (might be due to invalid IL or missing references)
		//IL_1607: Unknown result type (might be due to invalid IL or missing references)
		//IL_1608: Unknown result type (might be due to invalid IL or missing references)
		//IL_1612: Unknown result type (might be due to invalid IL or missing references)
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_1618: Unknown result type (might be due to invalid IL or missing references)
		//IL_1627: Unknown result type (might be due to invalid IL or missing references)
		//IL_163d: Unknown result type (might be due to invalid IL or missing references)
		//IL_164f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1658: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16de: Unknown result type (might be due to invalid IL or missing references)
		//IL_16df: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1709: Unknown result type (might be due to invalid IL or missing references)
		//IL_171b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1724: Unknown result type (might be due to invalid IL or missing references)
		//IL_179a: Unknown result type (might be due to invalid IL or missing references)
		//IL_179f: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1866: Unknown result type (might be due to invalid IL or missing references)
		//IL_186b: Unknown result type (might be due to invalid IL or missing references)
		//IL_186c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1876: Unknown result type (might be due to invalid IL or missing references)
		//IL_1877: Unknown result type (might be due to invalid IL or missing references)
		//IL_187c: Unknown result type (might be due to invalid IL or missing references)
		//IL_188b: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1932: Unknown result type (might be due to invalid IL or missing references)
		//IL_1937: Unknown result type (might be due to invalid IL or missing references)
		//IL_1938: Unknown result type (might be due to invalid IL or missing references)
		//IL_1942: Unknown result type (might be due to invalid IL or missing references)
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_1948: Unknown result type (might be due to invalid IL or missing references)
		//IL_1957: Unknown result type (might be due to invalid IL or missing references)
		//IL_196d: Unknown result type (might be due to invalid IL or missing references)
		//IL_197f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1988: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2007: Unknown result type (might be due to invalid IL or missing references)
		//IL_201d: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2038: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20be: Unknown result type (might be due to invalid IL or missing references)
		//IL_20bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2104: Unknown result type (might be due to invalid IL or missing references)
		//IL_217a: Unknown result type (might be due to invalid IL or missing references)
		//IL_217f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2180: Unknown result type (might be due to invalid IL or missing references)
		//IL_218a: Unknown result type (might be due to invalid IL or missing references)
		//IL_218b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2190: Unknown result type (might be due to invalid IL or missing references)
		//IL_219f: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2246: Unknown result type (might be due to invalid IL or missing references)
		//IL_224b: Unknown result type (might be due to invalid IL or missing references)
		//IL_224c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2256: Unknown result type (might be due to invalid IL or missing references)
		//IL_2257: Unknown result type (might be due to invalid IL or missing references)
		//IL_225c: Unknown result type (might be due to invalid IL or missing references)
		//IL_226b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2281: Unknown result type (might be due to invalid IL or missing references)
		//IL_2293: Unknown result type (might be due to invalid IL or missing references)
		//IL_229c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2312: Unknown result type (might be due to invalid IL or missing references)
		//IL_2317: Unknown result type (might be due to invalid IL or missing references)
		//IL_2318: Unknown result type (might be due to invalid IL or missing references)
		//IL_2322: Unknown result type (might be due to invalid IL or missing references)
		//IL_2323: Unknown result type (might be due to invalid IL or missing references)
		//IL_2328: Unknown result type (might be due to invalid IL or missing references)
		//IL_2337: Unknown result type (might be due to invalid IL or missing references)
		//IL_234d: Unknown result type (might be due to invalid IL or missing references)
		//IL_235f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2368: Unknown result type (might be due to invalid IL or missing references)
		//IL_23de: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2403: Unknown result type (might be due to invalid IL or missing references)
		//IL_2419: Unknown result type (might be due to invalid IL or missing references)
		//IL_242b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2434: Unknown result type (might be due to invalid IL or missing references)
		//IL_24aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_24af: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2500: Unknown result type (might be due to invalid IL or missing references)
		//IL_2576: Unknown result type (might be due to invalid IL or missing references)
		//IL_257b: Unknown result type (might be due to invalid IL or missing references)
		//IL_257c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2586: Unknown result type (might be due to invalid IL or missing references)
		//IL_2587: Unknown result type (might be due to invalid IL or missing references)
		//IL_258c: Unknown result type (might be due to invalid IL or missing references)
		//IL_259b: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2642: Unknown result type (might be due to invalid IL or missing references)
		//IL_2647: Unknown result type (might be due to invalid IL or missing references)
		//IL_2648: Unknown result type (might be due to invalid IL or missing references)
		//IL_2652: Unknown result type (might be due to invalid IL or missing references)
		//IL_2653: Unknown result type (might be due to invalid IL or missing references)
		//IL_2658: Unknown result type (might be due to invalid IL or missing references)
		//IL_2667: Unknown result type (might be due to invalid IL or missing references)
		//IL_267d: Unknown result type (might be due to invalid IL or missing references)
		//IL_268f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2698: Unknown result type (might be due to invalid IL or missing references)
		//IL_270e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2713: Unknown result type (might be due to invalid IL or missing references)
		//IL_2714: Unknown result type (might be due to invalid IL or missing references)
		//IL_271e: Unknown result type (might be due to invalid IL or missing references)
		//IL_271f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2724: Unknown result type (might be due to invalid IL or missing references)
		//IL_2733: Unknown result type (might be due to invalid IL or missing references)
		//IL_2749: Unknown result type (might be due to invalid IL or missing references)
		//IL_275b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2764: Unknown result type (might be due to invalid IL or missing references)
		//IL_27da: Unknown result type (might be due to invalid IL or missing references)
		//IL_27df: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_27eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2815: Unknown result type (might be due to invalid IL or missing references)
		//IL_2827: Unknown result type (might be due to invalid IL or missing references)
		//IL_2830: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2972: Unknown result type (might be due to invalid IL or missing references)
		//IL_2977: Unknown result type (might be due to invalid IL or missing references)
		//IL_2978: Unknown result type (might be due to invalid IL or missing references)
		//IL_2982: Unknown result type (might be due to invalid IL or missing references)
		//IL_2983: Unknown result type (might be due to invalid IL or missing references)
		//IL_2988: Unknown result type (might be due to invalid IL or missing references)
		//IL_2997: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_29bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_29c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b60: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fea: Unknown result type (might be due to invalid IL or missing references)
		//IL_3060: Unknown result type (might be due to invalid IL or missing references)
		//IL_3065: Unknown result type (might be due to invalid IL or missing references)
		//IL_3066: Unknown result type (might be due to invalid IL or missing references)
		//IL_3070: Unknown result type (might be due to invalid IL or missing references)
		//IL_3071: Unknown result type (might be due to invalid IL or missing references)
		//IL_3076: Unknown result type (might be due to invalid IL or missing references)
		//IL_3085: Unknown result type (might be due to invalid IL or missing references)
		//IL_309b: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_312c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3131: Unknown result type (might be due to invalid IL or missing references)
		//IL_3132: Unknown result type (might be due to invalid IL or missing references)
		//IL_313c: Unknown result type (might be due to invalid IL or missing references)
		//IL_313d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3142: Unknown result type (might be due to invalid IL or missing references)
		//IL_3151: Unknown result type (might be due to invalid IL or missing references)
		//IL_3167: Unknown result type (might be due to invalid IL or missing references)
		//IL_3179: Unknown result type (might be due to invalid IL or missing references)
		//IL_3182: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_31fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3208: Unknown result type (might be due to invalid IL or missing references)
		//IL_3209: Unknown result type (might be due to invalid IL or missing references)
		//IL_320e: Unknown result type (might be due to invalid IL or missing references)
		//IL_321d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3233: Unknown result type (might be due to invalid IL or missing references)
		//IL_3245: Unknown result type (might be due to invalid IL or missing references)
		//IL_324e: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32da: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3311: Unknown result type (might be due to invalid IL or missing references)
		//IL_331a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3390: Unknown result type (might be due to invalid IL or missing references)
		//IL_3395: Unknown result type (might be due to invalid IL or missing references)
		//IL_3396: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_33dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_345c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3461: Unknown result type (might be due to invalid IL or missing references)
		//IL_3462: Unknown result type (might be due to invalid IL or missing references)
		//IL_346c: Unknown result type (might be due to invalid IL or missing references)
		//IL_346d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3472: Unknown result type (might be due to invalid IL or missing references)
		//IL_3481: Unknown result type (might be due to invalid IL or missing references)
		//IL_3497: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3528: Unknown result type (might be due to invalid IL or missing references)
		//IL_352d: Unknown result type (might be due to invalid IL or missing references)
		//IL_352e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3538: Unknown result type (might be due to invalid IL or missing references)
		//IL_3539: Unknown result type (might be due to invalid IL or missing references)
		//IL_353e: Unknown result type (might be due to invalid IL or missing references)
		//IL_354d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3563: Unknown result type (might be due to invalid IL or missing references)
		//IL_3575: Unknown result type (might be due to invalid IL or missing references)
		//IL_357e: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3604: Unknown result type (might be due to invalid IL or missing references)
		//IL_3605: Unknown result type (might be due to invalid IL or missing references)
		//IL_360a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3619: Unknown result type (might be due to invalid IL or missing references)
		//IL_362f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3641: Unknown result type (might be due to invalid IL or missing references)
		//IL_364a: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_370d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3716: Unknown result type (might be due to invalid IL or missing references)
		//IL_378c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3791: Unknown result type (might be due to invalid IL or missing references)
		//IL_3792: Unknown result type (might be due to invalid IL or missing references)
		//IL_379c: Unknown result type (might be due to invalid IL or missing references)
		//IL_379d: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3858: Unknown result type (might be due to invalid IL or missing references)
		//IL_385d: Unknown result type (might be due to invalid IL or missing references)
		//IL_385e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3868: Unknown result type (might be due to invalid IL or missing references)
		//IL_3869: Unknown result type (might be due to invalid IL or missing references)
		//IL_386e: Unknown result type (might be due to invalid IL or missing references)
		//IL_387d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3893: Unknown result type (might be due to invalid IL or missing references)
		//IL_38a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_38ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_3924: Unknown result type (might be due to invalid IL or missing references)
		//IL_3929: Unknown result type (might be due to invalid IL or missing references)
		//IL_392a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3934: Unknown result type (might be due to invalid IL or missing references)
		//IL_3935: Unknown result type (might be due to invalid IL or missing references)
		//IL_393a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3949: Unknown result type (might be due to invalid IL or missing references)
		//IL_395f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3971: Unknown result type (might be due to invalid IL or missing references)
		//IL_397a: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d76: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dec: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_3edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f84: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4012: Unknown result type (might be due to invalid IL or missing references)
		//IL_4017: Unknown result type (might be due to invalid IL or missing references)
		//IL_4018: Unknown result type (might be due to invalid IL or missing references)
		//IL_4022: Unknown result type (might be due to invalid IL or missing references)
		//IL_4023: Unknown result type (might be due to invalid IL or missing references)
		//IL_4028: Unknown result type (might be due to invalid IL or missing references)
		//IL_4037: Unknown result type (might be due to invalid IL or missing references)
		//IL_404d: Unknown result type (might be due to invalid IL or missing references)
		//IL_405f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4068: Unknown result type (might be due to invalid IL or missing references)
		//IL_40de: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4103: Unknown result type (might be due to invalid IL or missing references)
		//IL_4119: Unknown result type (might be due to invalid IL or missing references)
		//IL_412b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4134: Unknown result type (might be due to invalid IL or missing references)
		//IL_41aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_41af: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_41bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_41c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4200: Unknown result type (might be due to invalid IL or missing references)
		//IL_4276: Unknown result type (might be due to invalid IL or missing references)
		//IL_427b: Unknown result type (might be due to invalid IL or missing references)
		//IL_427c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4286: Unknown result type (might be due to invalid IL or missing references)
		//IL_4287: Unknown result type (might be due to invalid IL or missing references)
		//IL_428c: Unknown result type (might be due to invalid IL or missing references)
		//IL_429b: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_42cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4342: Unknown result type (might be due to invalid IL or missing references)
		//IL_4347: Unknown result type (might be due to invalid IL or missing references)
		//IL_4348: Unknown result type (might be due to invalid IL or missing references)
		//IL_4352: Unknown result type (might be due to invalid IL or missing references)
		//IL_4353: Unknown result type (might be due to invalid IL or missing references)
		//IL_4358: Unknown result type (might be due to invalid IL or missing references)
		//IL_4367: Unknown result type (might be due to invalid IL or missing references)
		//IL_437d: Unknown result type (might be due to invalid IL or missing references)
		//IL_438f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4398: Unknown result type (might be due to invalid IL or missing references)
		//IL_440e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4413: Unknown result type (might be due to invalid IL or missing references)
		//IL_4414: Unknown result type (might be due to invalid IL or missing references)
		//IL_441e: Unknown result type (might be due to invalid IL or missing references)
		//IL_441f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4424: Unknown result type (might be due to invalid IL or missing references)
		//IL_4433: Unknown result type (might be due to invalid IL or missing references)
		//IL_4449: Unknown result type (might be due to invalid IL or missing references)
		//IL_445b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4464: Unknown result type (might be due to invalid IL or missing references)
		//IL_44da: Unknown result type (might be due to invalid IL or missing references)
		//IL_44df: Unknown result type (might be due to invalid IL or missing references)
		//IL_44e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_44eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_44f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4515: Unknown result type (might be due to invalid IL or missing references)
		//IL_4527: Unknown result type (might be due to invalid IL or missing references)
		//IL_4530: Unknown result type (might be due to invalid IL or missing references)
		//IL_45a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_45ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_45ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_45b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_45b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_45bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_45cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_45fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4672: Unknown result type (might be due to invalid IL or missing references)
		//IL_4677: Unknown result type (might be due to invalid IL or missing references)
		//IL_4678: Unknown result type (might be due to invalid IL or missing references)
		//IL_4682: Unknown result type (might be due to invalid IL or missing references)
		//IL_4683: Unknown result type (might be due to invalid IL or missing references)
		//IL_4688: Unknown result type (might be due to invalid IL or missing references)
		//IL_4697: Unknown result type (might be due to invalid IL or missing references)
		//IL_46ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_46bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_473e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4743: Unknown result type (might be due to invalid IL or missing references)
		//IL_4744: Unknown result type (might be due to invalid IL or missing references)
		//IL_474e: Unknown result type (might be due to invalid IL or missing references)
		//IL_474f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4754: Unknown result type (might be due to invalid IL or missing references)
		//IL_4763: Unknown result type (might be due to invalid IL or missing references)
		//IL_4779: Unknown result type (might be due to invalid IL or missing references)
		//IL_478b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4794: Unknown result type (might be due to invalid IL or missing references)
		//IL_480a: Unknown result type (might be due to invalid IL or missing references)
		//IL_480f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4810: Unknown result type (might be due to invalid IL or missing references)
		//IL_481a: Unknown result type (might be due to invalid IL or missing references)
		//IL_481b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4820: Unknown result type (might be due to invalid IL or missing references)
		//IL_482f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4845: Unknown result type (might be due to invalid IL or missing references)
		//IL_4857: Unknown result type (might be due to invalid IL or missing references)
		//IL_4860: Unknown result type (might be due to invalid IL or missing references)
		//IL_48d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_48db: Unknown result type (might be due to invalid IL or missing references)
		//IL_48dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_48e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_48e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_48ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_48fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4911: Unknown result type (might be due to invalid IL or missing references)
		//IL_4923: Unknown result type (might be due to invalid IL or missing references)
		//IL_492c: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_49c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_49dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_4daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f46: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f83: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5002: Unknown result type (might be due to invalid IL or missing references)
		//IL_5007: Unknown result type (might be due to invalid IL or missing references)
		//IL_5008: Unknown result type (might be due to invalid IL or missing references)
		//IL_5012: Unknown result type (might be due to invalid IL or missing references)
		//IL_5013: Unknown result type (might be due to invalid IL or missing references)
		//IL_5018: Unknown result type (might be due to invalid IL or missing references)
		//IL_5027: Unknown result type (might be due to invalid IL or missing references)
		//IL_503d: Unknown result type (might be due to invalid IL or missing references)
		//IL_504f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5058: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_50dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_50e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_515c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5161: Unknown result type (might be due to invalid IL or missing references)
		//IL_5162: Unknown result type (might be due to invalid IL or missing references)
		//IL_516c: Unknown result type (might be due to invalid IL or missing references)
		//IL_516d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5172: Unknown result type (might be due to invalid IL or missing references)
		//IL_5181: Unknown result type (might be due to invalid IL or missing references)
		//IL_5197: Unknown result type (might be due to invalid IL or missing references)
		//IL_51a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_51b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5228: Unknown result type (might be due to invalid IL or missing references)
		//IL_522d: Unknown result type (might be due to invalid IL or missing references)
		//IL_522e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5237: Unknown result type (might be due to invalid IL or missing references)
		//IL_5240: Unknown result type (might be due to invalid IL or missing references)
		//IL_52b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_52bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_52cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_52db: Unknown result type (might be due to invalid IL or missing references)
		//IL_52f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5303: Unknown result type (might be due to invalid IL or missing references)
		//IL_530c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5382: Unknown result type (might be due to invalid IL or missing references)
		//IL_5387: Unknown result type (might be due to invalid IL or missing references)
		//IL_5388: Unknown result type (might be due to invalid IL or missing references)
		//IL_5392: Unknown result type (might be due to invalid IL or missing references)
		//IL_5393: Unknown result type (might be due to invalid IL or missing references)
		//IL_5398: Unknown result type (might be due to invalid IL or missing references)
		//IL_53a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_53bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_53cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_53d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_544e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5453: Unknown result type (might be due to invalid IL or missing references)
		//IL_5454: Unknown result type (might be due to invalid IL or missing references)
		//IL_545e: Unknown result type (might be due to invalid IL or missing references)
		//IL_545f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5464: Unknown result type (might be due to invalid IL or missing references)
		//IL_5473: Unknown result type (might be due to invalid IL or missing references)
		//IL_5489: Unknown result type (might be due to invalid IL or missing references)
		//IL_549b: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_551a: Unknown result type (might be due to invalid IL or missing references)
		//IL_551f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5520: Unknown result type (might be due to invalid IL or missing references)
		//IL_552a: Unknown result type (might be due to invalid IL or missing references)
		//IL_552b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5530: Unknown result type (might be due to invalid IL or missing references)
		//IL_553f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5555: Unknown result type (might be due to invalid IL or missing references)
		//IL_5567: Unknown result type (might be due to invalid IL or missing references)
		//IL_5570: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_55eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_55f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_55f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_55fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_560b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5621: Unknown result type (might be due to invalid IL or missing references)
		//IL_5633: Unknown result type (might be due to invalid IL or missing references)
		//IL_563c: Unknown result type (might be due to invalid IL or missing references)
		//IL_56b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_56b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_56b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_56ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_56ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5708: Unknown result type (might be due to invalid IL or missing references)
		//IL_577e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5783: Unknown result type (might be due to invalid IL or missing references)
		//IL_5784: Unknown result type (might be due to invalid IL or missing references)
		//IL_578e: Unknown result type (might be due to invalid IL or missing references)
		//IL_578f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5794: Unknown result type (might be due to invalid IL or missing references)
		//IL_57a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_57b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_57cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_57d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_584a: Unknown result type (might be due to invalid IL or missing references)
		//IL_584f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5850: Unknown result type (might be due to invalid IL or missing references)
		//IL_585a: Unknown result type (might be due to invalid IL or missing references)
		//IL_585b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5860: Unknown result type (might be due to invalid IL or missing references)
		//IL_586f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5885: Unknown result type (might be due to invalid IL or missing references)
		//IL_5897: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5916: Unknown result type (might be due to invalid IL or missing references)
		//IL_591b: Unknown result type (might be due to invalid IL or missing references)
		//IL_591c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5926: Unknown result type (might be due to invalid IL or missing references)
		//IL_5927: Unknown result type (might be due to invalid IL or missing references)
		//IL_592c: Unknown result type (might be due to invalid IL or missing references)
		//IL_593b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5951: Unknown result type (might be due to invalid IL or missing references)
		//IL_5963: Unknown result type (might be due to invalid IL or missing references)
		//IL_596c: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_59f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_59f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_59f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_5abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_5def: Unknown result type (might be due to invalid IL or missing references)
		//IL_5df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6042: Unknown result type (might be due to invalid IL or missing references)
		//IL_6047: Unknown result type (might be due to invalid IL or missing references)
		//IL_6048: Unknown result type (might be due to invalid IL or missing references)
		//IL_6052: Unknown result type (might be due to invalid IL or missing references)
		//IL_6053: Unknown result type (might be due to invalid IL or missing references)
		//IL_6058: Unknown result type (might be due to invalid IL or missing references)
		//IL_6067: Unknown result type (might be due to invalid IL or missing references)
		//IL_607d: Unknown result type (might be due to invalid IL or missing references)
		//IL_608f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6098: Unknown result type (might be due to invalid IL or missing references)
		//IL_610e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6113: Unknown result type (might be due to invalid IL or missing references)
		//IL_6114: Unknown result type (might be due to invalid IL or missing references)
		//IL_611e: Unknown result type (might be due to invalid IL or missing references)
		//IL_611f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6124: Unknown result type (might be due to invalid IL or missing references)
		//IL_6133: Unknown result type (might be due to invalid IL or missing references)
		//IL_6149: Unknown result type (might be due to invalid IL or missing references)
		//IL_615b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6164: Unknown result type (might be due to invalid IL or missing references)
		//IL_61da: Unknown result type (might be due to invalid IL or missing references)
		//IL_61df: Unknown result type (might be due to invalid IL or missing references)
		//IL_61e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_61eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_61f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_6215: Unknown result type (might be due to invalid IL or missing references)
		//IL_6227: Unknown result type (might be due to invalid IL or missing references)
		//IL_6230: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_62bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_62cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_62e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_62f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_62fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6372: Unknown result type (might be due to invalid IL or missing references)
		//IL_6377: Unknown result type (might be due to invalid IL or missing references)
		//IL_6378: Unknown result type (might be due to invalid IL or missing references)
		//IL_6382: Unknown result type (might be due to invalid IL or missing references)
		//IL_6383: Unknown result type (might be due to invalid IL or missing references)
		//IL_6388: Unknown result type (might be due to invalid IL or missing references)
		//IL_6397: Unknown result type (might be due to invalid IL or missing references)
		//IL_63ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_63bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_63c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_643e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6443: Unknown result type (might be due to invalid IL or missing references)
		//IL_6444: Unknown result type (might be due to invalid IL or missing references)
		//IL_644e: Unknown result type (might be due to invalid IL or missing references)
		//IL_644f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6454: Unknown result type (might be due to invalid IL or missing references)
		itemDisplayRules = new List<KeyAssetRuleGroup>();
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Jetpack,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
						childName = "Chest",
						localPos = new Vector3(-0.00399f, 0.08923f, -0.18541f),
						localAngles = new Vector3(351.3922f, 359.5073f, 358.4317f),
						localScale = new Vector3(0.2f, 0.2f, 0.2f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.GoldGat,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldGat"),
						childName = "Head",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(2.56457f, 84.09991f, 223.4565f),
						localScale = new Vector3(0.2f, 0.2f, 0.2f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.BFG,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
						childName = "Chest",
						localPos = new Vector3(0.0782f, 0.33674f, -0.00285f),
						localAngles = new Vector3(0f, 0f, 313.6211f),
						localScale = new Vector3(0.3f, 0.3f, 0.3f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.CritGlasses,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
						childName = "Head",
						localPos = new Vector3(0f, 0.18391f, -0.01779f),
						localAngles = new Vector3(270.6798f, -0.00341f, 0.00345f),
						localScale = new Vector3(0.16666f, 0.15727f, 0.15727f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Syringe,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
						childName = "UpperArmR",
						localPos = new Vector3(0.01997f, -0.02756f, 0.00693f),
						localAngles = new Vector3(309.6017f, 26.74006f, 192.8567f),
						localScale = new Vector3(0.1f, 0.1f, 0.1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Behemoth,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
						childName = "Chest",
						localPos = new Vector3(0.15914f, 0.28954f, -0.2313f),
						localAngles = new Vector3(342.1061f, 180f, 0f),
						localScale = new Vector3(0.06017f, 0.06017f, 0.06017f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Missile,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
						childName = "Chest",
						localPos = new Vector3(-0.26159f, 0.37173f, -0.13857f),
						localAngles = new Vector3(0f, 0f, 51.9225f),
						localScale = new Vector3(0.0431f, 0.0431f, 0.0431f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Dagger,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
						childName = "Chest",
						localPos = new Vector3(-0.0553f, 0.2856f, 0.0945f),
						localAngles = new Vector3(334.8839f, 31.5284f, 34.6784f),
						localScale = new Vector3(1.2428f, 1.2428f, 1.2299f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Hoof,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
						childName = "CalfR",
						localPos = new Vector3(-0.00995f, 0.32262f, -0.08719f),
						localAngles = new Vector3(70.62775f, 0.70107f, 1.03991f),
						localScale = new Vector3(0.09833f, 0.08715f, 0.0758f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ChainLightning,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
						childName = "Chest",
						localPos = new Vector3(-0.0011f, -0.0095f, -0.19995f),
						localAngles = new Vector3(0f, 180f, 89.3997f),
						localScale = new Vector3(0.4749f, 0.4749f, 0.4749f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.GhostOnKill,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
						childName = "Head",
						localPos = new Vector3(0f, 0.1343f, -0.00508f),
						localAngles = new Vector3(281.8823f, 180.0002f, 179.9998f),
						localScale = new Vector3(0.54317f, 0.54317f, 0.54317f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Mushroom,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
						childName = "Chest",
						localPos = new Vector3(-0.0139f, 0.21583f, -0.16674f),
						localAngles = new Vector3(0.88879f, 277.6868f, 90.00072f),
						localScale = new Vector3(0.0501f, 0.0501f, 0.0501f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.AttackSpeedOnCrit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
						childName = "Head",
						localPos = new Vector3(0f, 0.11155f, -0.0958f),
						localAngles = new Vector3(285.2229f, 164.0233f, 195.6664f),
						localScale = new Vector3(0.29076f, 0.29076f, 0.29076f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BleedOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
						childName = "CalfL",
						localPos = new Vector3(-0.0533f, 0.24218f, -0.01261f),
						localAngles = new Vector3(277.2209f, 152.5847f, 150.3102f),
						localScale = new Vector3(0.2615f, 0.2615f, 0.2615f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.WardOnLevel,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
						childName = "Pelvis",
						localPos = new Vector3(0f, 0.0817f, -0.0955f),
						localAngles = new Vector3(0f, 0f, 90f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.HealOnCrit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
						childName = "Chest",
						localPos = new Vector3(0.08762f, 0.44194f, -0.14627f),
						localAngles = new Vector3(308.0577f, 353.5334f, 106.1726f),
						localScale = new Vector3(0.07529f, 0.07529f, 0.07529f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.HealWhileSafe,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
						childName = "Chest",
						localPos = new Vector3(0.12272f, 0.2749f, -0.13173f),
						localAngles = new Vector3(289.7526f, 338.3101f, 358.7119f),
						localScale = new Vector3(0.0289f, 0.0289f, 0.0289f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Clover,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
						childName = "Gun",
						localPos = new Vector3(0.0004f, 0.1094f, -0.1329f),
						localAngles = new Vector3(85.6192f, 0.0001f, 179.4897f),
						localScale = new Vector3(0.2749f, 0.2749f, 0.2749f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BarrierOnOverHeal,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
						childName = "LowerArmL",
						localPos = new Vector3(-0.05684f, 0.05248f, -0.0138f),
						localAngles = new Vector3(87.2154f, 101.9521f, 357.0091f),
						localScale = new Vector3(0.2849f, 0.2849f, 0.2849f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.GoldOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
						childName = "Head",
						localPos = new Vector3(0f, 0.04786f, -0.0534f),
						localAngles = new Vector3(276.2473f, 180.0004f, 179.9996f),
						localScale = new Vector3(0.76369f, 0.76369f, 0.76369f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.WarCryOnMultiKill,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
						childName = "UpperArmR",
						localPos = new Vector3(0.05202f, 0.08054f, 0.01918f),
						localAngles = new Vector3(74.69036f, 80.85159f, 19.4037f),
						localScale = new Vector3(0.45128f, 0.45128f, 0.45128f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SprintArmor,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
						childName = "LowerArmR",
						localPos = new Vector3(0.00301f, 0.17958f, 0.01516f),
						localAngles = new Vector3(1.19919f, 54.70515f, 126.3237f),
						localScale = new Vector3(0.17511f, 0.17511f, 0.17511f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.IceRing,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
						childName = "HandR",
						localPos = new Vector3(-0.0068f, 0.08889f, 0.02841f),
						localAngles = new Vector3(274.3965f, 90f, 270f),
						localScale = new Vector3(0.1f, 0.1f, 0.1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.FireRing,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
						childName = "HandR",
						localPos = new Vector3(0.007f, 0.09038f, 0.01348f),
						localAngles = new Vector3(276.5588f, 355.0313f, 5.83008f),
						localScale = new Vector3(0.1f, 0.1f, 0.1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.UtilitySkillMagazine,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.15024f, 0.00316f),
						localAngles = new Vector3(355.8665f, 180f, 180f),
						localScale = new Vector3(0.38391f, 0.38391f, 0.38391f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.JumpBoost,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
						childName = "UpperArmR",
						localPos = new Vector3(-0.08867f, 0.25236f, -0.05454f),
						localAngles = new Vector3(3.35221f, 131.5712f, 163.7028f),
						localScale = new Vector3(0.61867f, 0.5253f, 0.5253f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ArmorReductionOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
						childName = "Head",
						localPos = new Vector3(0.09652f, 0.08089f, 0.0587f),
						localAngles = new Vector3(341.414f, 10.86936f, 95.12854f),
						localScale = new Vector3(0.02584f, 0.02584f, 0.02584f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.NearbyDamageBonus,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
						childName = "Sword",
						localPos = new Vector3(-0.002f, 0.1828f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.1236f, 0.1236f, 0.1236f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ArmorPlate,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
						childName = "UpperArmL",
						localPos = new Vector3(-0.08627f, 0.00747f, -0.02916f),
						localAngles = new Vector3(302.5928f, 295.7042f, 356.4166f),
						localScale = new Vector3(0.1971f, 0.1971f, 0.1971f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.CommandMissile,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
						childName = "Head",
						localPos = new Vector3(0f, -0.04434f, -0.0492f),
						localAngles = new Vector3(43.909f, 4E-05f, 180f),
						localScale = new Vector3(0.26871f, 0.26871f, 0.26871f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Feather,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
						childName = "Chest",
						localPos = new Vector3(-0.05033f, 0.21104f, -0.09953f),
						localAngles = new Vector3(322.391f, 0f, 0f),
						localScale = new Vector3(0.0285f, 0.0285f, 0.0285f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Crowbar,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
						childName = "Head",
						localPos = new Vector3(0f, 0.03227f, -0.0389f),
						localAngles = new Vector3(0f, 0f, 180f),
						localScale = new Vector3(0.13052f, 0.13052f, 0.13052f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.FallBoots,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
						childName = "CalfL",
						localPos = new Vector3(-0.0038f, 0.3729f, -0.0046f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.1485f, 0.1485f, 0.1485f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
						childName = "CalfR",
						localPos = new Vector3(-0.0038f, 0.3729f, -0.0046f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.1485f, 0.1485f, 0.1485f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ExecuteLowHealthElite,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
						childName = "Pelvis",
						localPos = new Vector3(-0.14251f, -0.01864f, 0.00889f),
						localAngles = new Vector3(280.392f, 55.00724f, 37.32348f),
						localScale = new Vector3(0.16115f, 0.16115f, 0.16115f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.EquipmentMagazine,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.NovaOnHeal,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
						childName = "Head",
						localPos = new Vector3(0.02703f, 0.06073f, -0.02988f),
						localAngles = new Vector3(13.41394f, 66.95353f, 8.06389f),
						localScale = new Vector3(0.5349f, 0.5349f, 0.5349f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
						childName = "Head",
						localPos = new Vector3(-0.02703f, 0.06073f, -0.02988f),
						localAngles = new Vector3(9.55873f, 105.6705f, 11.71106f),
						localScale = new Vector3(0.5349f, 0.5349f, -0.5349f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Infusion,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
						childName = "Base",
						localPos = new Vector3(0.08521f, 0.19861f, 0.06679f),
						localAngles = new Vector3(0f, 50.33201f, 0f),
						localScale = new Vector3(0.31127f, 0.31127f, 0.31127f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Medkit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
						childName = "Chest",
						localPos = new Vector3(0.0039f, 0.13044f, 0.04305f),
						localAngles = new Vector3(290f, 180f, 0f),
						localScale = new Vector3(0.01585f, 0.01585f, 0.01585f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Bandolier,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
						childName = "Chest",
						localPos = new Vector3(0.01665f, -0.0014f, 0.02336f),
						localAngles = new Vector3(283.7496f, 269.9772f, 89.84225f),
						localScale = new Vector3(0.24379f, 0.35034f, 0.35034f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BounceNearby,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
						childName = "Chest",
						localPos = new Vector3(-0.17947f, 0.18036f, -0.00389f),
						localAngles = new Vector3(290.3197f, 88.99999f, 0f),
						localScale = new Vector3(0.214f, 0.214f, 0.214f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.IgniteOnKill,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
						childName = "Chest",
						localPos = new Vector3(-0.1039f, 0.32706f, -0.20256f),
						localAngles = new Vector3(78.9426f, 193.0253f, 177.007f),
						localScale = new Vector3(0.3165f, 0.3165f, 0.3165f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.StunChanceOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
						childName = "Base",
						localPos = new Vector3(0.11151f, 0.23955f, 0.00624f),
						localAngles = new Vector3(1.79873f, 180f, 180f),
						localScale = new Vector3(0.41355f, 0.41355f, 0.41355f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Firework,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
						childName = "HandR",
						localPos = new Vector3(0.0086f, 0.0069f, 0.0565f),
						localAngles = new Vector3(283.2718f, 0.33557f, 74.63754f),
						localScale = new Vector3(0.1194f, 0.1194f, 0.1194f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.LunarDagger,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
						childName = "Chest",
						localPos = new Vector3(-0.09689f, 0.4172f, -0.14314f),
						localAngles = new Vector3(350.8387f, 348.7021f, 268.0342f),
						localScale = new Vector3(0.11234f, 0.11234f, 0.11234f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Knurl,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
						childName = "Chest",
						localPos = new Vector3(-0.00777f, 0.11018f, 0.01039f),
						localAngles = new Vector3(78.87074f, 36.6722f, 105.8275f),
						localScale = new Vector3(0.03847f, 0.03847f, 0.03847f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BeetleGland,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetleGland"),
						childName = "Chest",
						localPos = new Vector3(0.10475f, -0.00489f, -0.07403f),
						localAngles = new Vector3(359.9584f, 0.1329f, 39.8304f),
						localScale = new Vector3(0.0553f, 0.0553f, 0.0553f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SprintBonus,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
						childName = "Head",
						localPos = new Vector3(0f, 0.21324f, 0.06803f),
						localAngles = new Vector3(71.27757f, 180.0012f, 71.01811f),
						localScale = new Vector3(0.1655f, 0.1655f, 0.1655f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SecondarySkillMagazine,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
						childName = "Chest",
						localPos = new Vector3(-0.13208f, 0.12727f, 0.10528f),
						localAngles = new Vector3(340.1868f, 55.4414f, 169.2369f),
						localScale = new Vector3(0.0441f, 0.0441f, 0.0441f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.StickyBomb,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
						childName = "Pelvis",
						localPos = new Vector3(0.0594f, 0.05345f, 0.10823f),
						localAngles = new Vector3(8.4958f, 176.5473f, 162.7601f),
						localScale = new Vector3(0.0736f, 0.0736f, 0.0736f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.TreasureCache,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0f, 0f, 0f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BossDamageBonus,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
						childName = "Chest",
						localPos = new Vector3(0.1062f, 0.11162f, 0.09627f),
						localAngles = new Vector3(90f, 41.5689f, 0f),
						localScale = new Vector3(0.2279f, 0.2279f, 0.2279f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SlowOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
						childName = "Pelvis",
						localPos = new Vector3(-0.0074f, 0.076f, -0.0864f),
						localAngles = new Vector3(0f, 23.7651f, 0f),
						localScale = new Vector3(0.0687f, 0.0687f, 0.0687f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ExtraLife,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
						childName = "Chest",
						localPos = new Vector3(-0.12967f, 0.38034f, -0.07946f),
						localAngles = new Vector3(330.14f, 342.5552f, 1.57057f),
						localScale = new Vector3(0.20914f, 0.20914f, 0.20241f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.KillEliteFrenzy,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.21957f, -0.11009f),
						localAngles = new Vector3(278.9036f, 0f, 0f),
						localScale = new Vector3(0.2638f, 0.2638f, 0.2638f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.RepeatHeal,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
						childName = "Chest",
						localPos = new Vector3(0.07487f, 0.30183f, -0.24376f),
						localAngles = new Vector3(297.8014f, 5.34769f, 336.6403f),
						localScale = new Vector3(0.1511f, 0.1511f, 0.1511f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.AutoCastEquipment,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
						childName = "HandR",
						localPos = new Vector3(0.02033f, 0.08756f, -0.02643f),
						localAngles = new Vector3(341.778f, 35.04337f, 99.35478f),
						localScale = new Vector3(0.0722f, 0.0722f, 0.0722f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.IncreaseHealing,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
						childName = "Head",
						localPos = new Vector3(0.07899f, 0.04586f, -0.05757f),
						localAngles = new Vector3(0f, 90f, 0f),
						localScale = new Vector3(0.16854f, 0.16854f, 0.16854f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
						childName = "Head",
						localPos = new Vector3(-0.07899f, 0.04586f, -0.05757f),
						localAngles = new Vector3(0f, 90f, 0f),
						localScale = new Vector3(0.16854f, 0.16854f, -0.16854f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.TitanGoldDuringTP,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
						childName = "Chest",
						localPos = new Vector3(-0.12607f, 0.1224f, -0.1718f),
						localAngles = new Vector3(308.4481f, 225.9465f, 333.6598f),
						localScale = new Vector3(0.1191f, 0.1191f, 0.1191f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SprintWisp,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0f, 0f, 0f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BarrierOnKill,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
						childName = "HandR",
						localPos = new Vector3(0.02152f, 0.09368f, 0.00294f),
						localAngles = new Vector3(270.8865f, 15.31104f, 231.1227f),
						localScale = new Vector3(0.08837f, 0.08837f, 0.08837f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.TPHealingNova,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
						childName = "Chest",
						localPos = new Vector3(0.03903f, 0.20738f, 0.08794f),
						localAngles = new Vector3(328.997f, 26.08873f, 0.92311f),
						localScale = new Vector3(0.13058f, 0.13058f, 0.01305f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.LunarUtilityReplacement,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
						childName = "Chest",
						localPos = new Vector3(0.1135f, 0.23809f, -0.18159f),
						localAngles = new Vector3(53.33944f, 239.3017f, 36.17928f),
						localScale = new Vector3(0.2833f, 0.2833f, 0.2833f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Thorns,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
						childName = "UpperArmR",
						localPos = new Vector3(0.21954f, 0.25103f, 0.17563f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.5f, 0.5f, 0.5f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.LunarPrimaryReplacement,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
						childName = "Head",
						localPos = new Vector3(0.04355f, 0.1722f, -0.01725f),
						localAngles = new Vector3(16.12343f, 180f, 180f),
						localScale = new Vector3(0.15152f, 0.15152f, 0.15152f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.NovaOnLowHealth,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
						childName = "Chest",
						localPos = new Vector3(-0.0484f, 0.10984f, 0.02595f),
						localAngles = new Vector3(316.2306f, 45.1087f, 303.6165f),
						localScale = new Vector3(-0.01575f, -0.01575f, -0.01575f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.LunarTrinket,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
						childName = "LowerArmL",
						localPos = new Vector3(0.0348f, 0.31195f, 0.00935f),
						localAngles = new Vector3(0f, 0f, 90f),
						localScale = new Vector3(1f, 1f, 1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Plant,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
						childName = "Chest",
						localPos = new Vector3(0.17787f, 0.31456f, -0.13987f),
						localAngles = new Vector3(323.3734f, 109.097f, 237.1637f),
						localScale = new Vector3(0.0429f, 0.0429f, 0.0429f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Bear,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.43227f, -0.17999f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.17815f, 0.17815f, 0.18152f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.DeathMark,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
						childName = "Head",
						localPos = new Vector3(0f, 0.14709f, -0.03841f),
						localAngles = new Vector3(2.90161f, 4E-05f, -4E-05f),
						localScale = new Vector3(-0.0375f, -0.0341f, -0.0464f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ExplodeOnDeath,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
						childName = "Pelvis",
						localPos = new Vector3(0.13742f, 0.07936f, 0.01299f),
						localAngles = new Vector3(0.05995f, 358.84f, 8.76334f),
						localScale = new Vector3(0.0283f, 0.0283f, 0.0283f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Seed,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
						childName = "Head",
						localPos = new Vector3(-0.10265f, 0.07407f, -0.0262f),
						localAngles = new Vector3(31.54254f, 236.3528f, 91.58902f),
						localScale = new Vector3(0.01154f, 0.01009f, 0.01009f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SprintOutOfCombat,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
						childName = "Pelvis",
						localPos = new Vector3(0.13093f, -0.05603f, -0.00595f),
						localAngles = new Vector3(359.9796f, 0.38747f, 18.12464f),
						localScale = new Vector3(0.31015f, 0.31015f, 0.31015f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.CooldownOnCrit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySkull"),
						childName = "HandR",
						localPos = new Vector3(0.03084f, 0.09123f, -0.01059f),
						localAngles = new Vector3(85.9473f, 80.3895f, 184.9944f),
						localScale = new Vector3(0.01652f, 0.01652f, 0.01652f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Phasing,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
						childName = "Chest",
						localPos = new Vector3(-0.0063f, 0.14073f, 0.00698f),
						localAngles = new Vector3(90f, 0f, 0f),
						localScale = new Vector3(0.02868f, 0.04732f, 0.03156f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.PersonalShield,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.27486f, 0.05085f),
						localAngles = new Vector3(88.26055f, 232.4472f, 51.9133f),
						localScale = new Vector3(0.1057f, 0.1057f, 0.1057f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ShockNearby,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.30543f, -0.16814f),
						localAngles = new Vector3(285.2071f, 2.45629f, 357.4169f),
						localScale = new Vector3(0.24216f, 0.24216f, 0.24216f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ShieldOnly,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
						childName = "Head",
						localPos = new Vector3(0.05392f, 0.20212f, -0.01851f),
						localAngles = new Vector3(348.1819f, 268.0985f, 0.3896f),
						localScale = new Vector3(0.3521f, 0.3521f, 0.3521f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
						childName = "Head",
						localPos = new Vector3(-0.05153f, 0.20892f, -0.01879f),
						localAngles = new Vector3(11.8181f, 268.0985f, 359.6104f),
						localScale = new Vector3(0.3521f, 0.3521f, -0.3521f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.AlienHead,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
						childName = "Base",
						localPos = new Vector3(0.10939f, 0.13417f, -0.04608f),
						localAngles = new Vector3(284.1171f, 239.5653f, 260.8905f),
						localScale = new Vector3(0.6701f, 0.6701f, 0.6701f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.HeadHunter,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
						childName = "Pelvis",
						localPos = new Vector3(-0.01582f, -0.0147f, -0.0275f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.4769f, 0.15897f, 0.15897f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.EnergizedOnEquipmentUse,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
						childName = "Chest",
						localPos = new Vector3(-0.12219f, 0.22746f, 0.05431f),
						localAngles = new Vector3(319.4921f, 95.44549f, 180.451f),
						localScale = new Vector3(0.2732f, 0.2732f, 0.2732f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.FlatHealth,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
						childName = "Pelvis",
						localPos = new Vector3(-0.1416f, 0.09794f, -0.09843f),
						localAngles = new Vector3(351.9934f, 237.8542f, 86.18441f),
						localScale = new Vector3(0.1245f, 0.1155f, 0.1155f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Tooth,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0f, 0f, 0f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Pearl,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
						childName = "Head",
						localPos = new Vector3(0f, 0.086f, -0.02121f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.2f, 0.2f, 0.2f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.ShinyPearl,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
						childName = "Head",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.2f, 0.2f, 0.2f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BonusGoldPackOnKill,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
						childName = "Base",
						localPos = new Vector3(0.14954f, 0.05886f, 0.01124f),
						localAngles = new Vector3(345.828f, 90.44725f, 85.56704f),
						localScale = new Vector3(0.05585f, 0.05585f, 0.05585f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Squid,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
						childName = "Chest",
						localPos = new Vector3(0.08975f, 0.16036f, -0.16564f),
						localAngles = new Vector3(5.65006f, 75.07253f, 276.3981f),
						localScale = new Vector3(0.02f, 0.02f, 0.02f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Icicle,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
						childName = "Base",
						localPos = new Vector3(0.45946f, 0.68513f, -0.43309f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(1f, 1f, 1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Talisman,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
						childName = "Base",
						localPos = new Vector3(0.2f, 0.95856f, -0.46796f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(1f, 1f, 1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.LaserTurbine,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.00283f, -0.24214f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.2159f, 0.2159f, 0.2159f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.FocusConvergence,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
						childName = "Base",
						localPos = new Vector3(-0.5429f, 0.96699f, -0.33138f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.1f, 0.1f, 0.1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.Incubator,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
						childName = "Chest",
						localPos = new Vector3(-0.08381f, 0.17971f, -0.09568f),
						localAngles = new Vector3(353.0521f, 317.2421f, 69.6292f),
						localScale = new Vector3(0.02609f, 0.02609f, 0.02609f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.FireballsOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
						childName = "Pelvis",
						localPos = new Vector3(0.11671f, 0.06175f, 0.03695f),
						localAngles = new Vector3(57.4526f, 80.80998f, 26.94625f),
						localScale = new Vector3(0.00805f, 0.00805f, 0.00805f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.SiphonOnLowHealth,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0f, 0f, 0f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.BleedOnHitAndExplode,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
						childName = "Head",
						localPos = new Vector3(0.00576f, 0.04279f, 0.0169f),
						localAngles = new Vector3(326.6455f, -1E-05f, 3E-05f),
						localScale = new Vector3(0.0486f, 0.0486f, 0.0486f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.MonstersOnShrineUse,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
						childName = "Chest",
						localPos = new Vector3(0.0022f, 0.084f, 0.066f),
						localAngles = new Vector3(352.4521f, 260.6884f, 341.5106f),
						localScale = new Vector3(-0.01067f, -0.01067f, -0.01067f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Items.RandomDamageZone,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.20552f, 0.03736f),
						localAngles = new Vector3(349.218f, 235.9453f, 0f),
						localScale = new Vector3(0.00365f, 0.00365f, 0.00365f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Fruit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
						childName = "Chest",
						localPos = new Vector3(-0.08468f, 0.06027f, -0.05945f),
						localAngles = new Vector3(355.5085f, 54.78192f, 339.6581f),
						localScale = new Vector3(0.2118f, 0.2118f, 0.2118f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.AffixRed,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.AffixBlue,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[2]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					},
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.AffixWhite,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.AffixPoison,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.AffixHaunted,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.CritOnUse,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
						childName = "Head",
						localPos = new Vector3(0f, 0.24193f, -0.00185f),
						localAngles = new Vector3(272.112f, 180.0012f, 179.9989f),
						localScale = new Vector3(0.13259f, 0.13259f, 0.13259f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.DroneBackup,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
						childName = "Pelvis",
						localPos = new Vector3(0f, 0.1269f, 0f),
						localAngles = new Vector3(0f, 90f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Lightning,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
						childName = "UpperArmR",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.3413f, 0.3413f, 0.3413f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.BurnNearby,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.CrippleWard,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.QuestVolatileBattery,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
						childName = "Chest",
						localPos = new Vector3(0f, 0.2584f, -0.0987f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.2188f, 0.2188f, 0.2188f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.GainArmor,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Recycle,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.FireBallDash,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
						childName = "Pelvis",
						localPos = new Vector3(0f, 0.1269f, 0f),
						localAngles = new Vector3(0f, 90f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Cleanse,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Tonic,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Gateway,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Meteor,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
						childName = "Base",
						localPos = new Vector3(0f, 1.18521f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(1f, 1f, 1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Saw,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerang"),
						childName = "Base",
						localPos = new Vector3(0f, 0.678f, -0.46362f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(1f, 1f, 1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Blackhole,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
						childName = "Base",
						localPos = new Vector3(-0.2f, 0.94981f, -0.46657f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.5f, 0.5f, 0.5f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.Scanner,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.DeathProjectile,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
						childName = "Pelvis",
						localPos = new Vector3(0f, 0.028f, -0.0977f),
						localAngles = new Vector3(0f, 180f, 0f),
						localScale = new Vector3(0.0596f, 0.0596f, 0.0596f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.LifestealOnHit,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
						childName = "Head",
						localPos = new Vector3(0f, 0.07288f, 0.00119f),
						localAngles = new Vector3(274.4367f, 270f, 90f),
						localScale = new Vector3(0.1f, 0.1f, 0.1f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRules.Add(new KeyAssetRuleGroup
		{
			keyAsset = (UnityEngine.Object)(object)Equipment.TeamWarCry,
			displayRuleGroup = new DisplayRuleGroup
			{
				rules = (ItemDisplayRule[])(object)new ItemDisplayRule[1]
				{
					new ItemDisplayRule
					{
						ruleType = (ItemDisplayRuleType)0,
						followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
						childName = "Chest",
						localPos = new Vector3(0f, 0f, 0f),
						localAngles = new Vector3(0f, 0f, 0f),
						localScale = new Vector3(0.01f, 0.01f, 0.01f),
						limbMask = (LimbFlags)0
					}
				}
			}
		});
		itemDisplayRuleSet.GenerateRuntimeValues();
	}

	private static RendererInfo[] SkinRendererInfos(RendererInfo[] defaultRenderers, Material[] materials)
	{
		RendererInfo[] array = (RendererInfo[])(object)new RendererInfo[defaultRenderers.Length];
		defaultRenderers.CopyTo(array, 0);
		array[0].defaultMaterial = materials[0];
		array[1].defaultMaterial = materials[1];
		array[SurvivorBase.instance.mainRendererIndex].defaultMaterial = materials[2];
		return array;
	}
}
