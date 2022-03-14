// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Survivors.AnArbiter
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using AncientScepter;
using BepInEx.Configuration;
using EntityStates;
using RiskOfRuinaMod.Modules.Achievements;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Survivors
{
  internal class AnArbiter : SurvivorBase
  {
    internal static Material arbiterMat = Assets.CreateMaterial("matArbiter", 0.8f, Color.yellow);
    private static UnlockableDef masterySkinUnlockableDef;

    internal override string bodyName { get; set; } = "Arbiter";

    internal override GameObject bodyPrefab { get; set; }

    internal override GameObject displayPrefab { get; set; }

    internal override float sortPosition { get; set; } = 100f;

    internal override ConfigEntry<bool> characterEnabled { get; set; }

    internal override BodyInfo bodyInfo { get; set; } = new BodyInfo()
    {
      armor = 30f,
      armorGrowth = 0.0f,
      bodyName = "ArbiterBody",
      bodyNameToken = "COF_ARBITER_BODY_NAME",
      bodyColor = Color.grey,
      characterPortrait = Assets.LoadCharacterIcon("Arbiter"),
      crosshair = Assets.LoadCrosshair("SimpleDot"),
      damage = 10f,
      healthGrowth = 33f,
      healthRegen = 1f,
      jumpCount = 1,
      maxHealth = 110f,
      subtitleNameToken = "COF_ARBITER_BODY_SUBTITLE",
      podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
    };

    internal override int mainRendererIndex { get; set; } = 0;

    internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[1]
    {
      new CustomRendererInfo()
      {
        childName = "Model",
        material = AnArbiter.arbiterMat
      }
    };

    internal override Type characterMainState { get; set; } = typeof (GenericCharacterMain);

    internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }

    internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

    internal override UnlockableDef characterUnlockableDef { get; set; }

    internal override void InitializeCharacter()
    {
      base.InitializeCharacter();
      this.bodyPrefab.AddComponent<TargetTracker>().maxTrackingDistance = 40f;
    }

    internal override void InitializeUnlockables() => AnArbiter.masterySkinUnlockableDef = Unlockables.AddUnlockable<ArbiterMasteryAchievement>(true);

    internal override void InitializeDoppelganger() => Prefabs.CreateGenericDoppelganger(SurvivorBase.instance.bodyPrefab, this.bodyName + "MonsterMaster", "Mage");

    internal override void InitializeHitboxes()
    {
      ChildLocator componentInChildren = this.bodyPrefab.GetComponentInChildren<ChildLocator>();
      Prefabs.SetupHitbox(((Component) componentInChildren).gameObject, componentInChildren.FindChild("FairyHitbox"), "Fairy");
    }

    internal override void InitializeSkills()
    {
      RiskOfRuinaMod.Modules.Skills.CreateSkillFamilies(this.bodyPrefab);
      string str = "COF";
      SkillLocator component = this.bodyPrefab.GetComponent<SkillLocator>();
      component.passiveSkill.enabled = true;
      component.passiveSkill.skillNameToken = str + "_ARBITER_BODY_PASSIVE_NAME";
      component.passiveSkill.skillDescriptionToken = str + "_ARBITER_BODY_PASSIVE_DESCRIPTION";
      component.passiveSkill.icon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterPassiveIcon");
      component.passiveSkill.keywordToken = "KEYWORD_FAIRY";
      RiskOfRuinaMod.Modules.Skills.AddPrimarySkill(this.bodyPrefab, RiskOfRuinaMod.Modules.Skills.CreatePrimarySkillDef(new SerializableEntityStateType(typeof (FairyCombo)), "Weapon", str + "_ARBITER_BODY_PRIMARY_FAIRY_NAME", str + "_ARBITER_BODY_PRIMARY_FAIRY_DESCRIPTION", Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterPrimaryIcon"), false));
      SkillDef trackerSkillDef = RiskOfRuinaMod.Modules.Skills.CreateTrackerSkillDef(new SkillDefInfo()
      {
        skillName = str + "_ARBITER_BODY_SECONDARY_LOCK_NAME",
        skillNameToken = str + "_ARBITER_BODY_SECONDARY_LOCK_NAME",
        skillDescriptionToken = str + "_ARBITER_BODY_SECONDARY_LOCK_DESCRIPTION",
        skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterSecondaryIcon"),
        activationState = new SerializableEntityStateType(typeof (Lock)),
        activationStateMachineName = "Slide",
        baseMaxStock = 1,
        baseRechargeInterval = 12f,
        beginSkillCooldownOnSkillEnd = false,
        canceledFromSprinting = false,
        forceSprintDuringState = false,
        fullRestockOnAssign = true,
        interruptPriority = (InterruptPriority) 1,
        resetCooldownTimerOnUse = false,
        isCombatSkill = true,
        mustKeyPress = false,
        cancelSprintingOnActivation = true,
        rechargeStock = 1,
        requiredStock = 1,
        stockToConsume = 1,
        keywordTokens = new string[1]{ "KEYWORD_LOCK" }
      });
      RiskOfRuinaMod.Modules.Skills.unlockSkillDef = RiskOfRuinaMod.Modules.Skills.CreateTrackerSkillDef(new SkillDefInfo()
      {
        skillName = str + "_ARBITER_BODY_SECONDARY_UNLOCK_NAME",
        skillNameToken = str + "_ARBITER_BODY_SECONDARY_UNLOCK_NAME",
        skillDescriptionToken = str + "_ARBITER_BODY_SECONDARY_UNLOCK_DESCRIPTION",
        skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterSecondaryTwoIcon"),
        activationState = new SerializableEntityStateType(typeof (Unlock)),
        activationStateMachineName = "Slide",
        baseMaxStock = 1,
        baseRechargeInterval = 25f,
        beginSkillCooldownOnSkillEnd = false,
        canceledFromSprinting = false,
        forceSprintDuringState = false,
        fullRestockOnAssign = true,
        interruptPriority = (InterruptPriority) 1,
        resetCooldownTimerOnUse = false,
        isCombatSkill = true,
        mustKeyPress = false,
        cancelSprintingOnActivation = true,
        rechargeStock = 1,
        requiredStock = 1,
        stockToConsume = 0,
        keywordTokens = new string[1]{ "KEYWORD_UNLOCK" }
      });
      RiskOfRuinaMod.Modules.Skills.AddSecondarySkills(this.bodyPrefab, trackerSkillDef, RiskOfRuinaMod.Modules.Skills.unlockSkillDef);
      RiskOfRuinaMod.Modules.Skills.AddUtilitySkills(this.bodyPrefab, RiskOfRuinaMod.Modules.Skills.CreateSkillDef(new SkillDefInfo()
      {
        skillName = str + "_ARBITER_BODY_UTILITY_PILLARS_NAME",
        skillNameToken = str + "_ARBITER_BODY_UTILITY_PILLARS_NAME",
        skillDescriptionToken = str + "_ARBITER_BODY_UTILITY_PILLARS_DESCRIPTION",
        skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterUtilityIcon"),
        activationState = new SerializableEntityStateType(typeof (ChannelPillar)),
        activationStateMachineName = "Weapon",
        baseMaxStock = 1,
        baseRechargeInterval = 16f,
        beginSkillCooldownOnSkillEnd = true,
        canceledFromSprinting = false,
        forceSprintDuringState = false,
        fullRestockOnAssign = true,
        interruptPriority = (InterruptPriority) 1,
        resetCooldownTimerOnUse = false,
        isCombatSkill = true,
        mustKeyPress = false,
        cancelSprintingOnActivation = true,
        rechargeStock = 1,
        requiredStock = 1,
        stockToConsume = 1
      }), RiskOfRuinaMod.Modules.Skills.CreateSkillDef(new SkillDefInfo()
      {
        skillName = str + "_ARBITER_BODY_UTILITY_PILLARSSPEAR_NAME",
        skillNameToken = str + "_ARBITER_BODY_UTILITY_PILLARSSPEAR_NAME",
        skillDescriptionToken = str + "_ARBITER_BODY_UTILITY_PILLARSSPEAR_DESCRIPTION",
        skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterUtilityTwoIcon"),
        activationState = new SerializableEntityStateType(typeof (ChargePillarSpear)),
        activationStateMachineName = "Weapon",
        baseMaxStock = 1,
        baseRechargeInterval = 7f,
        beginSkillCooldownOnSkillEnd = true,
        canceledFromSprinting = false,
        forceSprintDuringState = false,
        fullRestockOnAssign = true,
        interruptPriority = (InterruptPriority) 1,
        resetCooldownTimerOnUse = false,
        isCombatSkill = true,
        mustKeyPress = false,
        cancelSprintingOnActivation = true,
        rechargeStock = 1,
        requiredStock = 1,
        stockToConsume = 1,
        keywordTokens = new string[1]{ "KEYWORD_FAIRY" }
      }));
      RiskOfRuinaMod.Modules.Skills.AddSpecialSkills(this.bodyPrefab, RiskOfRuinaMod.Modules.Skills.CreateSkillDef(new SkillDefInfo()
      {
        skillName = str + "_ARBITER_BODY_SPECIAL_SHOCKWAVE_NAME",
        skillNameToken = str + "_ARBITER_BODY_SPECIAL_SHOCKWAVE_NAME",
        skillDescriptionToken = str + "_ARBITER_BODY_SPECIAL_SHOCKWAVE_DESCRIPTION",
        skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterSpecialIcon"),
        activationState = new SerializableEntityStateType(typeof (ChannelShockwave)),
        activationStateMachineName = "Weapon",
        baseMaxStock = 5,
        baseRechargeInterval = 10f,
        beginSkillCooldownOnSkillEnd = true,
        canceledFromSprinting = false,
        forceSprintDuringState = false,
        fullRestockOnAssign = true,
        interruptPriority = (InterruptPriority) 1,
        resetCooldownTimerOnUse = false,
        isCombatSkill = true,
        mustKeyPress = false,
        cancelSprintingOnActivation = true,
        rechargeStock = 1,
        requiredStock = 5,
        stockToConsume = 5,
        keywordTokens = new string[1]{ "KEYWORD_FEEBLE" }
      }));
    }

    internal override void InitializeSkins()
    {
      GameObject gameObject = ((Component) this.bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform).gameObject;
      CharacterModel component = gameObject.GetComponent<CharacterModel>();
      ModelSkinController modelSkinController = gameObject.AddComponent<ModelSkinController>();
      gameObject.GetComponent<ChildLocator>();
      SkinnedMeshRenderer skinnedMeshRenderer = component.mainSkinnedMeshRenderer;
      CharacterModel.RendererInfo[] baseRendererInfos = component.baseRendererInfos;
      List<SkinDef> skinDefList = new List<SkinDef>();
      SkinDef skinDef1 = Skins.CreateSkinDef("COF_ARBITER_BODY_DEFAULT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterMainSkin"), baseRendererInfos, skinnedMeshRenderer, gameObject);
      skinDef1.meshReplacements = new SkinDef.MeshReplacement[1]
      {
        new SkinDef.MeshReplacement()
        {
          mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArbiter"),
          renderer = baseRendererInfos[SurvivorBase.instance.mainRendererIndex].renderer
        }
      };
      skinDefList.Add(skinDef1);
      CharacterModel.RendererInfo[] rendererInfos1 = new CharacterModel.RendererInfo[baseRendererInfos.Length];
      baseRendererInfos.CopyTo((Array) rendererInfos1, 0);
      rendererInfos1[0].defaultMaterial = Assets.CreateMaterial("matSkinTwo", 0.8f, new Color(1f, 2.40566f, 0.0f));
      SkinDef skinDef2 = Skins.CreateSkinDef("COF_ARBITER_BODY_SECOND_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterSecondSkin"), rendererInfos1, skinnedMeshRenderer, gameObject);
      skinDef2.meshReplacements = new SkinDef.MeshReplacement[1]
      {
        new SkinDef.MeshReplacement()
        {
          mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArbiter"),
          renderer = baseRendererInfos[SurvivorBase.instance.mainRendererIndex].renderer
        }
      };
      skinDefList.Add(skinDef2);
      CharacterModel.RendererInfo[] rendererInfos2 = new CharacterModel.RendererInfo[baseRendererInfos.Length];
      baseRendererInfos.CopyTo((Array) rendererInfos2, 0);
      rendererInfos2[0].defaultMaterial = Assets.CreateMaterial("matSkinThree", 0.8f, new Color(3.109756f, 3.109756f, 3.109756f));
      SkinDef skinDef3 = Skins.CreateSkinDef("COF_ARBITER_BODY_THIRD_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterThirdSkin"), rendererInfos2, skinnedMeshRenderer, gameObject);
      skinDef3.meshReplacements = new SkinDef.MeshReplacement[1]
      {
        new SkinDef.MeshReplacement()
        {
          mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArbiter"),
          renderer = baseRendererInfos[SurvivorBase.instance.mainRendererIndex].renderer
        }
      };
      skinDefList.Add(skinDef3);
      CharacterModel.RendererInfo[] rendererInfos3 = new CharacterModel.RendererInfo[baseRendererInfos.Length];
      baseRendererInfos.CopyTo((Array) rendererInfos3, 0);
      rendererInfos3[0].defaultMaterial = Assets.CreateMaterial("matArbiterMastery", 1f, Color.yellow, 1f);
      SkinDef skinDef4 = Skins.CreateSkinDef("COF_ARBITER_BODY_MASTERY_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterMasterySkin"), rendererInfos3, skinnedMeshRenderer, gameObject, AnArbiter.masterySkinUnlockableDef);
      skinDef4.meshReplacements = new SkinDef.MeshReplacement[1]
      {
        new SkinDef.MeshReplacement()
        {
          mesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshArbiterMastery"),
          renderer = baseRendererInfos[0].renderer
        }
      };
      skinDefList.Add(skinDef4);
      modelSkinController.skins = skinDefList.ToArray();
    }

    internal override void SetItemDisplays()
    {
      this.itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules1 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup1 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup1.keyAsset = (Object) RoR2Content.Equipment.Jetpack;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local1 = ref keyAssetRuleGroup1;
      DisplayRuleGroup displayRuleGroup1 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup1).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
          childName = "Chest",
          localPos = new Vector3(-0.00399f, 0.08923f, -0.18541f),
          localAngles = new Vector3(351.3922f, 359.5073f, 358.4317f),
          localScale = new Vector3(0.2f, 0.2f, 0.2f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup2 = displayRuleGroup1;
      local1.displayRuleGroup = displayRuleGroup2;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup2 = keyAssetRuleGroup1;
      itemDisplayRules1.Add(keyAssetRuleGroup2);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules2 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup3 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup3.keyAsset = (Object) RoR2Content.Equipment.GoldGat;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local2 = ref keyAssetRuleGroup3;
      DisplayRuleGroup displayRuleGroup3 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup3).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldGat"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(2.56457f, 84.09991f, 223.4565f),
          localScale = new Vector3(0.2f, 0.2f, 0.2f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup4 = displayRuleGroup3;
      local2.displayRuleGroup = displayRuleGroup4;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup4 = keyAssetRuleGroup3;
      itemDisplayRules2.Add(keyAssetRuleGroup4);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules3 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup5 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup5.keyAsset = (Object) RoR2Content.Equipment.BFG;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local3 = ref keyAssetRuleGroup5;
      DisplayRuleGroup displayRuleGroup5 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup5).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
          childName = "Chest",
          localPos = new Vector3(0.0782f, 0.33674f, -0.00285f),
          localAngles = new Vector3(0.0f, 0.0f, 313.6211f),
          localScale = new Vector3(0.3f, 0.3f, 0.3f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup6 = displayRuleGroup5;
      local3.displayRuleGroup = displayRuleGroup6;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup6 = keyAssetRuleGroup5;
      itemDisplayRules3.Add(keyAssetRuleGroup6);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules4 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup7 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup7.keyAsset = (Object) RoR2Content.Items.CritGlasses;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local4 = ref keyAssetRuleGroup7;
      DisplayRuleGroup displayRuleGroup7 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup7).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.18391f, -0.01779f),
          localAngles = new Vector3(270.6798f, -0.00341f, 0.00345f),
          localScale = new Vector3(0.16666f, 0.15727f, 0.15727f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup8 = displayRuleGroup7;
      local4.displayRuleGroup = displayRuleGroup8;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup8 = keyAssetRuleGroup7;
      itemDisplayRules4.Add(keyAssetRuleGroup8);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules5 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup9 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup9.keyAsset = (Object) RoR2Content.Items.Syringe;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local5 = ref keyAssetRuleGroup9;
      DisplayRuleGroup displayRuleGroup9 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup9).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
          childName = "UpperArmR",
          localPos = new Vector3(0.01997f, -0.02756f, 0.00693f),
          localAngles = new Vector3(309.6017f, 26.74006f, 192.8567f),
          localScale = new Vector3(0.1f, 0.1f, 0.1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup10 = displayRuleGroup9;
      local5.displayRuleGroup = displayRuleGroup10;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup10 = keyAssetRuleGroup9;
      itemDisplayRules5.Add(keyAssetRuleGroup10);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules6 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup11 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup11.keyAsset = (Object) RoR2Content.Items.Behemoth;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local6 = ref keyAssetRuleGroup11;
      DisplayRuleGroup displayRuleGroup11 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup11).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
          childName = "Chest",
          localPos = new Vector3(0.15914f, 0.28954f, -0.2313f),
          localAngles = new Vector3(342.1061f, 180f, 0.0f),
          localScale = new Vector3(0.06017f, 0.06017f, 0.06017f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup12 = displayRuleGroup11;
      local6.displayRuleGroup = displayRuleGroup12;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup12 = keyAssetRuleGroup11;
      itemDisplayRules6.Add(keyAssetRuleGroup12);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules7 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup13 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup13.keyAsset = (Object) RoR2Content.Items.Missile;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local7 = ref keyAssetRuleGroup13;
      DisplayRuleGroup displayRuleGroup13 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup13).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
          childName = "Chest",
          localPos = new Vector3(-0.26159f, 0.37173f, -0.13857f),
          localAngles = new Vector3(0.0f, 0.0f, 51.9225f),
          localScale = new Vector3(0.0431f, 0.0431f, 0.0431f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup14 = displayRuleGroup13;
      local7.displayRuleGroup = displayRuleGroup14;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup14 = keyAssetRuleGroup13;
      itemDisplayRules7.Add(keyAssetRuleGroup14);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules8 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup15 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup15.keyAsset = (Object) RoR2Content.Items.Dagger;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local8 = ref keyAssetRuleGroup15;
      DisplayRuleGroup displayRuleGroup15 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup15).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
          childName = "Chest",
          localPos = new Vector3(-0.0553f, 0.2856f, 0.0945f),
          localAngles = new Vector3(334.8839f, 31.5284f, 34.6784f),
          localScale = new Vector3(1.2428f, 1.2428f, 1.2299f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup16 = displayRuleGroup15;
      local8.displayRuleGroup = displayRuleGroup16;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup16 = keyAssetRuleGroup15;
      itemDisplayRules8.Add(keyAssetRuleGroup16);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules9 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup17 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup17.keyAsset = (Object) RoR2Content.Items.Hoof;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local9 = ref keyAssetRuleGroup17;
      DisplayRuleGroup displayRuleGroup17 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup17).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
          childName = "CalfR",
          localPos = new Vector3(-0.00995f, 0.32262f, -0.08719f),
          localAngles = new Vector3(70.62775f, 0.70107f, 1.03991f),
          localScale = new Vector3(0.09833f, 0.08715f, 0.0758f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup18 = displayRuleGroup17;
      local9.displayRuleGroup = displayRuleGroup18;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup18 = keyAssetRuleGroup17;
      itemDisplayRules9.Add(keyAssetRuleGroup18);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules10 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup19 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup19.keyAsset = (Object) RoR2Content.Items.ChainLightning;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local10 = ref keyAssetRuleGroup19;
      DisplayRuleGroup displayRuleGroup19 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup19).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
          childName = "Chest",
          localPos = new Vector3(-0.0011f, -0.0095f, -0.19995f),
          localAngles = new Vector3(0.0f, 180f, 89.3997f),
          localScale = new Vector3(0.4749f, 0.4749f, 0.4749f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup20 = displayRuleGroup19;
      local10.displayRuleGroup = displayRuleGroup20;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup20 = keyAssetRuleGroup19;
      itemDisplayRules10.Add(keyAssetRuleGroup20);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules11 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup21 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup21.keyAsset = (Object) RoR2Content.Items.GhostOnKill;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local11 = ref keyAssetRuleGroup21;
      DisplayRuleGroup displayRuleGroup21 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup21).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.1343f, -0.00508f),
          localAngles = new Vector3(281.8823f, 180.0002f, 179.9998f),
          localScale = new Vector3(0.54317f, 0.54317f, 0.54317f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup22 = displayRuleGroup21;
      local11.displayRuleGroup = displayRuleGroup22;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup22 = keyAssetRuleGroup21;
      itemDisplayRules11.Add(keyAssetRuleGroup22);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules12 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup23 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup23.keyAsset = (Object) RoR2Content.Items.Mushroom;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local12 = ref keyAssetRuleGroup23;
      DisplayRuleGroup displayRuleGroup23 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup23).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
          childName = "Chest",
          localPos = new Vector3(-0.0139f, 0.21583f, -0.16674f),
          localAngles = new Vector3(0.88879f, 277.6868f, 90.00072f),
          localScale = new Vector3(0.0501f, 0.0501f, 0.0501f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup24 = displayRuleGroup23;
      local12.displayRuleGroup = displayRuleGroup24;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup24 = keyAssetRuleGroup23;
      itemDisplayRules12.Add(keyAssetRuleGroup24);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules13 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup25 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup25.keyAsset = (Object) RoR2Content.Items.AttackSpeedOnCrit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local13 = ref keyAssetRuleGroup25;
      DisplayRuleGroup displayRuleGroup25 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup25).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.11155f, -0.0958f),
          localAngles = new Vector3(285.2229f, 164.0233f, 195.6664f),
          localScale = new Vector3(0.29076f, 0.29076f, 0.29076f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup26 = displayRuleGroup25;
      local13.displayRuleGroup = displayRuleGroup26;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup26 = keyAssetRuleGroup25;
      itemDisplayRules13.Add(keyAssetRuleGroup26);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules14 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup27 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup27.keyAsset = (Object) RoR2Content.Items.BleedOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local14 = ref keyAssetRuleGroup27;
      DisplayRuleGroup displayRuleGroup27 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup27).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
          childName = "CalfL",
          localPos = new Vector3(-0.0533f, 0.24218f, -0.01261f),
          localAngles = new Vector3(277.2209f, 152.5847f, 150.3102f),
          localScale = new Vector3(0.2615f, 0.2615f, 0.2615f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup28 = displayRuleGroup27;
      local14.displayRuleGroup = displayRuleGroup28;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup28 = keyAssetRuleGroup27;
      itemDisplayRules14.Add(keyAssetRuleGroup28);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules15 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup29 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup29.keyAsset = (Object) RoR2Content.Items.WardOnLevel;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local15 = ref keyAssetRuleGroup29;
      DisplayRuleGroup displayRuleGroup29 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup29).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
          childName = "Pelvis",
          localPos = new Vector3(0.0f, 0.0817f, -0.0955f),
          localAngles = new Vector3(0.0f, 0.0f, 90f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup30 = displayRuleGroup29;
      local15.displayRuleGroup = displayRuleGroup30;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup30 = keyAssetRuleGroup29;
      itemDisplayRules15.Add(keyAssetRuleGroup30);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules16 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup31 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup31.keyAsset = (Object) RoR2Content.Items.HealOnCrit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local16 = ref keyAssetRuleGroup31;
      DisplayRuleGroup displayRuleGroup31 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup31).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
          childName = "Chest",
          localPos = new Vector3(0.08762f, 0.44194f, -0.14627f),
          localAngles = new Vector3(308.0577f, 353.5334f, 106.1726f),
          localScale = new Vector3(0.07529f, 0.07529f, 0.07529f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup32 = displayRuleGroup31;
      local16.displayRuleGroup = displayRuleGroup32;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup32 = keyAssetRuleGroup31;
      itemDisplayRules16.Add(keyAssetRuleGroup32);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules17 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup33 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup33.keyAsset = (Object) RoR2Content.Items.HealWhileSafe;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local17 = ref keyAssetRuleGroup33;
      DisplayRuleGroup displayRuleGroup33 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup33).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
          childName = "Chest",
          localPos = new Vector3(0.12272f, 0.2749f, -0.13173f),
          localAngles = new Vector3(289.7526f, 338.3101f, 358.7119f),
          localScale = new Vector3(0.0289f, 0.0289f, 0.0289f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup34 = displayRuleGroup33;
      local17.displayRuleGroup = displayRuleGroup34;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup34 = keyAssetRuleGroup33;
      itemDisplayRules17.Add(keyAssetRuleGroup34);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules18 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup35 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup35.keyAsset = (Object) RoR2Content.Items.Clover;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local18 = ref keyAssetRuleGroup35;
      DisplayRuleGroup displayRuleGroup35 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup35).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
          childName = "Gun",
          localPos = new Vector3(0.0004f, 0.1094f, -0.1329f),
          localAngles = new Vector3(85.6192f, 0.0001f, 179.4897f),
          localScale = new Vector3(0.2749f, 0.2749f, 0.2749f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup36 = displayRuleGroup35;
      local18.displayRuleGroup = displayRuleGroup36;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup36 = keyAssetRuleGroup35;
      itemDisplayRules18.Add(keyAssetRuleGroup36);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules19 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup37 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup37.keyAsset = (Object) RoR2Content.Items.BarrierOnOverHeal;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local19 = ref keyAssetRuleGroup37;
      DisplayRuleGroup displayRuleGroup37 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup37).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
          childName = "LowerArmL",
          localPos = new Vector3(-0.05684f, 0.05248f, -0.0138f),
          localAngles = new Vector3(87.2154f, 101.9521f, 357.0091f),
          localScale = new Vector3(0.2849f, 0.2849f, 0.2849f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup38 = displayRuleGroup37;
      local19.displayRuleGroup = displayRuleGroup38;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup38 = keyAssetRuleGroup37;
      itemDisplayRules19.Add(keyAssetRuleGroup38);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules20 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup39 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup39.keyAsset = (Object) RoR2Content.Items.GoldOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local20 = ref keyAssetRuleGroup39;
      DisplayRuleGroup displayRuleGroup39 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup39).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.04786f, -0.0534f),
          localAngles = new Vector3(276.2473f, 180.0004f, 179.9996f),
          localScale = new Vector3(0.76369f, 0.76369f, 0.76369f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup40 = displayRuleGroup39;
      local20.displayRuleGroup = displayRuleGroup40;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup40 = keyAssetRuleGroup39;
      itemDisplayRules20.Add(keyAssetRuleGroup40);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules21 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup41 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup41.keyAsset = (Object) RoR2Content.Items.WarCryOnMultiKill;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local21 = ref keyAssetRuleGroup41;
      DisplayRuleGroup displayRuleGroup41 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup41).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
          childName = "UpperArmR",
          localPos = new Vector3(0.05202f, 0.08054f, 0.01918f),
          localAngles = new Vector3(74.69036f, 80.85159f, 19.4037f),
          localScale = new Vector3(0.45128f, 0.45128f, 0.45128f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup42 = displayRuleGroup41;
      local21.displayRuleGroup = displayRuleGroup42;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup42 = keyAssetRuleGroup41;
      itemDisplayRules21.Add(keyAssetRuleGroup42);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules22 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup43 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup43.keyAsset = (Object) RoR2Content.Items.SprintArmor;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local22 = ref keyAssetRuleGroup43;
      DisplayRuleGroup displayRuleGroup43 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup43).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
          childName = "LowerArmR",
          localPos = new Vector3(0.00301f, 0.17958f, 0.01516f),
          localAngles = new Vector3(1.19919f, 54.70515f, 126.3237f),
          localScale = new Vector3(0.17511f, 0.17511f, 0.17511f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup44 = displayRuleGroup43;
      local22.displayRuleGroup = displayRuleGroup44;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup44 = keyAssetRuleGroup43;
      itemDisplayRules22.Add(keyAssetRuleGroup44);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules23 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup45 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup45.keyAsset = (Object) RoR2Content.Items.IceRing;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local23 = ref keyAssetRuleGroup45;
      DisplayRuleGroup displayRuleGroup45 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup45).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
          childName = "HandR",
          localPos = new Vector3(-0.0068f, 0.08889f, 0.02841f),
          localAngles = new Vector3(274.3965f, 90f, 270f),
          localScale = new Vector3(0.1f, 0.1f, 0.1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup46 = displayRuleGroup45;
      local23.displayRuleGroup = displayRuleGroup46;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup46 = keyAssetRuleGroup45;
      itemDisplayRules23.Add(keyAssetRuleGroup46);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules24 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup47 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup47.keyAsset = (Object) RoR2Content.Items.FireRing;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local24 = ref keyAssetRuleGroup47;
      DisplayRuleGroup displayRuleGroup47 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup47).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
          childName = "HandR",
          localPos = new Vector3(0.007f, 0.09038f, 0.01348f),
          localAngles = new Vector3(276.5588f, 355.0313f, 5.83008f),
          localScale = new Vector3(0.1f, 0.1f, 0.1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup48 = displayRuleGroup47;
      local24.displayRuleGroup = displayRuleGroup48;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup48 = keyAssetRuleGroup47;
      itemDisplayRules24.Add(keyAssetRuleGroup48);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules25 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup49 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup49.keyAsset = (Object) RoR2Content.Items.UtilitySkillMagazine;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local25 = ref keyAssetRuleGroup49;
      DisplayRuleGroup displayRuleGroup49 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup49).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.15024f, 0.00316f),
          localAngles = new Vector3(355.8665f, 180f, 180f),
          localScale = new Vector3(0.38391f, 0.38391f, 0.38391f),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup50 = displayRuleGroup49;
      local25.displayRuleGroup = displayRuleGroup50;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup50 = keyAssetRuleGroup49;
      itemDisplayRules25.Add(keyAssetRuleGroup50);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules26 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup51 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup51.keyAsset = (Object) RoR2Content.Items.JumpBoost;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local26 = ref keyAssetRuleGroup51;
      DisplayRuleGroup displayRuleGroup51 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup51).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
          childName = "UpperArmR",
          localPos = new Vector3(-0.08867f, 0.25236f, -0.05454f),
          localAngles = new Vector3(3.35221f, 131.5712f, 163.7028f),
          localScale = new Vector3(0.61867f, 0.5253f, 0.5253f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup52 = displayRuleGroup51;
      local26.displayRuleGroup = displayRuleGroup52;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup52 = keyAssetRuleGroup51;
      itemDisplayRules26.Add(keyAssetRuleGroup52);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules27 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup53 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup53.keyAsset = (Object) RoR2Content.Items.ArmorReductionOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local27 = ref keyAssetRuleGroup53;
      DisplayRuleGroup displayRuleGroup53 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup53).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
          childName = "Head",
          localPos = new Vector3(0.09652f, 0.08089f, 0.0587f),
          localAngles = new Vector3(341.414f, 10.86936f, 95.12854f),
          localScale = new Vector3(0.02584f, 0.02584f, 0.02584f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup54 = displayRuleGroup53;
      local27.displayRuleGroup = displayRuleGroup54;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup54 = keyAssetRuleGroup53;
      itemDisplayRules27.Add(keyAssetRuleGroup54);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules28 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup55 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup55.keyAsset = (Object) RoR2Content.Items.NearbyDamageBonus;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local28 = ref keyAssetRuleGroup55;
      DisplayRuleGroup displayRuleGroup55 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup55).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
          childName = "Sword",
          localPos = new Vector3(-1f / 500f, 0.1828f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.1236f, 0.1236f, 0.1236f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup56 = displayRuleGroup55;
      local28.displayRuleGroup = displayRuleGroup56;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup56 = keyAssetRuleGroup55;
      itemDisplayRules28.Add(keyAssetRuleGroup56);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules29 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup57 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup57.keyAsset = (Object) RoR2Content.Items.ArmorPlate;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local29 = ref keyAssetRuleGroup57;
      DisplayRuleGroup displayRuleGroup57 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup57).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
          childName = "UpperArmL",
          localPos = new Vector3(-0.08627f, 0.00747f, -0.02916f),
          localAngles = new Vector3(302.5928f, 295.7042f, 356.4166f),
          localScale = new Vector3(0.1971f, 0.1971f, 0.1971f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup58 = displayRuleGroup57;
      local29.displayRuleGroup = displayRuleGroup58;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup58 = keyAssetRuleGroup57;
      itemDisplayRules29.Add(keyAssetRuleGroup58);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules30 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup59 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup59.keyAsset = (Object) RoR2Content.Equipment.CommandMissile;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local30 = ref keyAssetRuleGroup59;
      DisplayRuleGroup displayRuleGroup59 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup59).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
          childName = "Head",
          localPos = new Vector3(0.0f, -0.04434f, -0.0492f),
          localAngles = new Vector3(43.909f, 4E-05f, 180f),
          localScale = new Vector3(0.26871f, 0.26871f, 0.26871f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup60 = displayRuleGroup59;
      local30.displayRuleGroup = displayRuleGroup60;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup60 = keyAssetRuleGroup59;
      itemDisplayRules30.Add(keyAssetRuleGroup60);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules31 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup61 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup61.keyAsset = (Object) RoR2Content.Items.Feather;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local31 = ref keyAssetRuleGroup61;
      DisplayRuleGroup displayRuleGroup61 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup61).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
          childName = "Chest",
          localPos = new Vector3(-0.05033f, 0.21104f, -0.09953f),
          localAngles = new Vector3(322.391f, 0.0f, 0.0f),
          localScale = new Vector3(0.0285f, 0.0285f, 0.0285f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup62 = displayRuleGroup61;
      local31.displayRuleGroup = displayRuleGroup62;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup62 = keyAssetRuleGroup61;
      itemDisplayRules31.Add(keyAssetRuleGroup62);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules32 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup63 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup63.keyAsset = (Object) RoR2Content.Items.Crowbar;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local32 = ref keyAssetRuleGroup63;
      DisplayRuleGroup displayRuleGroup63 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup63).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.03227f, -0.0389f),
          localAngles = new Vector3(0.0f, 0.0f, 180f),
          localScale = new Vector3(0.13052f, 0.13052f, 0.13052f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup64 = displayRuleGroup63;
      local32.displayRuleGroup = displayRuleGroup64;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup64 = keyAssetRuleGroup63;
      itemDisplayRules32.Add(keyAssetRuleGroup64);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules33 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup65 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup65.keyAsset = (Object) RoR2Content.Items.FallBoots;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local33 = ref keyAssetRuleGroup65;
      DisplayRuleGroup displayRuleGroup65 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup65).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
          childName = "CalfL",
          localPos = new Vector3(-0.0038f, 0.3729f, -0.0046f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.1485f, 0.1485f, 0.1485f),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
          childName = "CalfR",
          localPos = new Vector3(-0.0038f, 0.3729f, -0.0046f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.1485f, 0.1485f, 0.1485f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup66 = displayRuleGroup65;
      local33.displayRuleGroup = displayRuleGroup66;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup66 = keyAssetRuleGroup65;
      itemDisplayRules33.Add(keyAssetRuleGroup66);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules34 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup67 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup67.keyAsset = (Object) RoR2Content.Items.ExecuteLowHealthElite;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local34 = ref keyAssetRuleGroup67;
      DisplayRuleGroup displayRuleGroup67 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup67).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
          childName = "Pelvis",
          localPos = new Vector3(-0.14251f, -0.01864f, 0.00889f),
          localAngles = new Vector3(280.392f, 55.00724f, 37.32348f),
          localScale = new Vector3(0.16115f, 0.16115f, 0.16115f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup68 = displayRuleGroup67;
      local34.displayRuleGroup = displayRuleGroup68;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup68 = keyAssetRuleGroup67;
      itemDisplayRules34.Add(keyAssetRuleGroup68);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules35 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup69 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup69.keyAsset = (Object) RoR2Content.Items.EquipmentMagazine;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local35 = ref keyAssetRuleGroup69;
      DisplayRuleGroup displayRuleGroup69 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup69).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup70 = displayRuleGroup69;
      local35.displayRuleGroup = displayRuleGroup70;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup70 = keyAssetRuleGroup69;
      itemDisplayRules35.Add(keyAssetRuleGroup70);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules36 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup71 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup71.keyAsset = (Object) RoR2Content.Items.NovaOnHeal;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local36 = ref keyAssetRuleGroup71;
      DisplayRuleGroup displayRuleGroup71 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup71).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
          childName = "Head",
          localPos = new Vector3(0.02703f, 0.06073f, -0.02988f),
          localAngles = new Vector3(13.41394f, 66.95353f, 8.06389f),
          localScale = new Vector3(0.5349f, 0.5349f, 0.5349f),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
          childName = "Head",
          localPos = new Vector3(-0.02703f, 0.06073f, -0.02988f),
          localAngles = new Vector3(9.55873f, 105.6705f, 11.71106f),
          localScale = new Vector3(0.5349f, 0.5349f, -0.5349f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup72 = displayRuleGroup71;
      local36.displayRuleGroup = displayRuleGroup72;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup72 = keyAssetRuleGroup71;
      itemDisplayRules36.Add(keyAssetRuleGroup72);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules37 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup73 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup73.keyAsset = (Object) RoR2Content.Items.Infusion;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local37 = ref keyAssetRuleGroup73;
      DisplayRuleGroup displayRuleGroup73 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup73).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
          childName = "Base",
          localPos = new Vector3(0.08521f, 0.19861f, 0.06679f),
          localAngles = new Vector3(0.0f, 50.33201f, 0.0f),
          localScale = new Vector3(0.31127f, 0.31127f, 0.31127f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup74 = displayRuleGroup73;
      local37.displayRuleGroup = displayRuleGroup74;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup74 = keyAssetRuleGroup73;
      itemDisplayRules37.Add(keyAssetRuleGroup74);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules38 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup75 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup75.keyAsset = (Object) RoR2Content.Items.Medkit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local38 = ref keyAssetRuleGroup75;
      DisplayRuleGroup displayRuleGroup75 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup75).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
          childName = "Chest",
          localPos = new Vector3(0.0039f, 0.13044f, 0.04305f),
          localAngles = new Vector3(290f, 180f, 0.0f),
          localScale = new Vector3(0.01585f, 0.01585f, 0.01585f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup76 = displayRuleGroup75;
      local38.displayRuleGroup = displayRuleGroup76;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup76 = keyAssetRuleGroup75;
      itemDisplayRules38.Add(keyAssetRuleGroup76);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules39 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup77 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup77.keyAsset = (Object) RoR2Content.Items.Bandolier;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local39 = ref keyAssetRuleGroup77;
      DisplayRuleGroup displayRuleGroup77 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup77).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
          childName = "Chest",
          localPos = new Vector3(0.01665f, -0.0014f, 0.02336f),
          localAngles = new Vector3(283.7496f, 269.9772f, 89.84225f),
          localScale = new Vector3(0.24379f, 0.35034f, 0.35034f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup78 = displayRuleGroup77;
      local39.displayRuleGroup = displayRuleGroup78;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup78 = keyAssetRuleGroup77;
      itemDisplayRules39.Add(keyAssetRuleGroup78);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules40 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup79 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup79.keyAsset = (Object) RoR2Content.Items.BounceNearby;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local40 = ref keyAssetRuleGroup79;
      DisplayRuleGroup displayRuleGroup79 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup79).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
          childName = "Chest",
          localPos = new Vector3(-0.17947f, 0.18036f, -0.00389f),
          localAngles = new Vector3(290.3197f, 88.99999f, 0.0f),
          localScale = new Vector3(0.214f, 0.214f, 0.214f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup80 = displayRuleGroup79;
      local40.displayRuleGroup = displayRuleGroup80;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup80 = keyAssetRuleGroup79;
      itemDisplayRules40.Add(keyAssetRuleGroup80);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules41 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup81 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup81.keyAsset = (Object) RoR2Content.Items.IgniteOnKill;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local41 = ref keyAssetRuleGroup81;
      DisplayRuleGroup displayRuleGroup81 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup81).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
          childName = "Chest",
          localPos = new Vector3(-0.1039f, 0.32706f, -0.20256f),
          localAngles = new Vector3(78.9426f, 193.0253f, 177.007f),
          localScale = new Vector3(0.3165f, 0.3165f, 0.3165f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup82 = displayRuleGroup81;
      local41.displayRuleGroup = displayRuleGroup82;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup82 = keyAssetRuleGroup81;
      itemDisplayRules41.Add(keyAssetRuleGroup82);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules42 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup83 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup83.keyAsset = (Object) RoR2Content.Items.StunChanceOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local42 = ref keyAssetRuleGroup83;
      DisplayRuleGroup displayRuleGroup83 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup83).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
          childName = "Base",
          localPos = new Vector3(0.11151f, 0.23955f, 0.00624f),
          localAngles = new Vector3(1.79873f, 180f, 180f),
          localScale = new Vector3(0.41355f, 0.41355f, 0.41355f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup84 = displayRuleGroup83;
      local42.displayRuleGroup = displayRuleGroup84;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup84 = keyAssetRuleGroup83;
      itemDisplayRules42.Add(keyAssetRuleGroup84);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules43 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup85 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup85.keyAsset = (Object) RoR2Content.Items.Firework;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local43 = ref keyAssetRuleGroup85;
      DisplayRuleGroup displayRuleGroup85 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup85).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
          childName = "HandR",
          localPos = new Vector3(0.0086f, 0.0069f, 0.0565f),
          localAngles = new Vector3(283.2718f, 0.33557f, 74.63754f),
          localScale = new Vector3(0.1194f, 0.1194f, 0.1194f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup86 = displayRuleGroup85;
      local43.displayRuleGroup = displayRuleGroup86;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup86 = keyAssetRuleGroup85;
      itemDisplayRules43.Add(keyAssetRuleGroup86);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules44 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup87 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup87.keyAsset = (Object) RoR2Content.Items.LunarDagger;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local44 = ref keyAssetRuleGroup87;
      DisplayRuleGroup displayRuleGroup87 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup87).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
          childName = "Chest",
          localPos = new Vector3(-0.09689f, 0.4172f, -0.14314f),
          localAngles = new Vector3(350.8387f, 348.7021f, 268.0342f),
          localScale = new Vector3(0.11234f, 0.11234f, 0.11234f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup88 = displayRuleGroup87;
      local44.displayRuleGroup = displayRuleGroup88;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup88 = keyAssetRuleGroup87;
      itemDisplayRules44.Add(keyAssetRuleGroup88);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules45 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup89 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup89.keyAsset = (Object) RoR2Content.Items.Knurl;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local45 = ref keyAssetRuleGroup89;
      DisplayRuleGroup displayRuleGroup89 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup89).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
          childName = "Chest",
          localPos = new Vector3(-0.00777f, 0.11018f, 0.01039f),
          localAngles = new Vector3(78.87074f, 36.6722f, 105.8275f),
          localScale = new Vector3(0.03847f, 0.03847f, 0.03847f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup90 = displayRuleGroup89;
      local45.displayRuleGroup = displayRuleGroup90;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup90 = keyAssetRuleGroup89;
      itemDisplayRules45.Add(keyAssetRuleGroup90);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules46 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup91 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup91.keyAsset = (Object) RoR2Content.Items.BeetleGland;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local46 = ref keyAssetRuleGroup91;
      DisplayRuleGroup displayRuleGroup91 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup91).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetleGland"),
          childName = "Chest",
          localPos = new Vector3(0.10475f, -0.00489f, -0.07403f),
          localAngles = new Vector3(359.9584f, 0.1329f, 39.8304f),
          localScale = new Vector3(0.0553f, 0.0553f, 0.0553f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup92 = displayRuleGroup91;
      local46.displayRuleGroup = displayRuleGroup92;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup92 = keyAssetRuleGroup91;
      itemDisplayRules46.Add(keyAssetRuleGroup92);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules47 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup93 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup93.keyAsset = (Object) RoR2Content.Items.SprintBonus;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local47 = ref keyAssetRuleGroup93;
      DisplayRuleGroup displayRuleGroup93 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup93).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
          childName = "Chest",
          localPos = new Vector3(0.11542f, 0.34365f, -0.0454f),
          localAngles = new Vector3(71.27759f, 180.0012f, 71.01811f),
          localScale = new Vector3(0.1655f, 0.1655f, 0.1655f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup94 = displayRuleGroup93;
      local47.displayRuleGroup = displayRuleGroup94;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup94 = keyAssetRuleGroup93;
      itemDisplayRules47.Add(keyAssetRuleGroup94);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules48 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup95 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup95.keyAsset = (Object) RoR2Content.Items.SecondarySkillMagazine;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local48 = ref keyAssetRuleGroup95;
      DisplayRuleGroup displayRuleGroup95 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup95).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
          childName = "Chest",
          localPos = new Vector3(-0.13208f, 0.12727f, 0.10528f),
          localAngles = new Vector3(340.1868f, 55.4414f, 169.2369f),
          localScale = new Vector3(0.0441f, 0.0441f, 0.0441f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup96 = displayRuleGroup95;
      local48.displayRuleGroup = displayRuleGroup96;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup96 = keyAssetRuleGroup95;
      itemDisplayRules48.Add(keyAssetRuleGroup96);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules49 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup97 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup97.keyAsset = (Object) RoR2Content.Items.StickyBomb;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local49 = ref keyAssetRuleGroup97;
      DisplayRuleGroup displayRuleGroup97 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup97).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
          childName = "Pelvis",
          localPos = new Vector3(0.0594f, 0.05345f, 0.10823f),
          localAngles = new Vector3(8.4958f, 176.5473f, 162.7601f),
          localScale = new Vector3(0.0736f, 0.0736f, 0.0736f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup98 = displayRuleGroup97;
      local49.displayRuleGroup = displayRuleGroup98;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup98 = keyAssetRuleGroup97;
      itemDisplayRules49.Add(keyAssetRuleGroup98);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules50 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup99 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup99.keyAsset = (Object) RoR2Content.Items.TreasureCache;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local50 = ref keyAssetRuleGroup99;
      DisplayRuleGroup displayRuleGroup99 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup99).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.0f, 0.0f, 0.0f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup100 = displayRuleGroup99;
      local50.displayRuleGroup = displayRuleGroup100;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup100 = keyAssetRuleGroup99;
      itemDisplayRules50.Add(keyAssetRuleGroup100);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules51 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup101 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup101.keyAsset = (Object) RoR2Content.Items.BossDamageBonus;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local51 = ref keyAssetRuleGroup101;
      DisplayRuleGroup displayRuleGroup101 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup101).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
          childName = "Chest",
          localPos = new Vector3(0.1062f, 0.11162f, 0.09627f),
          localAngles = new Vector3(90f, 41.5689f, 0.0f),
          localScale = new Vector3(0.2279f, 0.2279f, 0.2279f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup102 = displayRuleGroup101;
      local51.displayRuleGroup = displayRuleGroup102;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup102 = keyAssetRuleGroup101;
      itemDisplayRules51.Add(keyAssetRuleGroup102);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules52 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup103 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup103.keyAsset = (Object) RoR2Content.Items.SlowOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local52 = ref keyAssetRuleGroup103;
      DisplayRuleGroup displayRuleGroup103 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup103).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
          childName = "Pelvis",
          localPos = new Vector3(-0.0074f, 0.076f, -0.0864f),
          localAngles = new Vector3(0.0f, 23.7651f, 0.0f),
          localScale = new Vector3(0.0687f, 0.0687f, 0.0687f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup104 = displayRuleGroup103;
      local52.displayRuleGroup = displayRuleGroup104;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup104 = keyAssetRuleGroup103;
      itemDisplayRules52.Add(keyAssetRuleGroup104);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules53 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup105 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup105.keyAsset = (Object) RoR2Content.Items.ExtraLife;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local53 = ref keyAssetRuleGroup105;
      DisplayRuleGroup displayRuleGroup105 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup105).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
          childName = "Chest",
          localPos = new Vector3(-0.12967f, 0.38034f, -0.07946f),
          localAngles = new Vector3(330.14f, 342.5552f, 1.57057f),
          localScale = new Vector3(0.20914f, 0.20914f, 0.20241f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup106 = displayRuleGroup105;
      local53.displayRuleGroup = displayRuleGroup106;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup106 = keyAssetRuleGroup105;
      itemDisplayRules53.Add(keyAssetRuleGroup106);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules54 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup107 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup107.keyAsset = (Object) RoR2Content.Items.KillEliteFrenzy;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local54 = ref keyAssetRuleGroup107;
      DisplayRuleGroup displayRuleGroup107 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup107).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.21957f, -0.11009f),
          localAngles = new Vector3(278.9036f, 0.0f, 0.0f),
          localScale = new Vector3(0.2638f, 0.2638f, 0.2638f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup108 = displayRuleGroup107;
      local54.displayRuleGroup = displayRuleGroup108;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup108 = keyAssetRuleGroup107;
      itemDisplayRules54.Add(keyAssetRuleGroup108);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules55 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup109 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup109.keyAsset = (Object) RoR2Content.Items.RepeatHeal;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local55 = ref keyAssetRuleGroup109;
      DisplayRuleGroup displayRuleGroup109 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup109).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
          childName = "Chest",
          localPos = new Vector3(0.07487f, 0.30183f, -0.24376f),
          localAngles = new Vector3(297.8014f, 5.34769f, 336.6403f),
          localScale = new Vector3(0.1511f, 0.1511f, 0.1511f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup110 = displayRuleGroup109;
      local55.displayRuleGroup = displayRuleGroup110;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup110 = keyAssetRuleGroup109;
      itemDisplayRules55.Add(keyAssetRuleGroup110);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules56 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup111 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup111.keyAsset = (Object) RoR2Content.Items.AutoCastEquipment;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local56 = ref keyAssetRuleGroup111;
      DisplayRuleGroup displayRuleGroup111 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup111).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
          childName = "HandR",
          localPos = new Vector3(0.02033f, 0.08756f, -0.02643f),
          localAngles = new Vector3(341.778f, 35.04337f, 99.35478f),
          localScale = new Vector3(0.0722f, 0.0722f, 0.0722f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup112 = displayRuleGroup111;
      local56.displayRuleGroup = displayRuleGroup112;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup112 = keyAssetRuleGroup111;
      itemDisplayRules56.Add(keyAssetRuleGroup112);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules57 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup113 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup113.keyAsset = (Object) RoR2Content.Items.IncreaseHealing;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local57 = ref keyAssetRuleGroup113;
      DisplayRuleGroup displayRuleGroup113 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup113).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
          childName = "Head",
          localPos = new Vector3(0.07899f, 0.04586f, -0.05757f),
          localAngles = new Vector3(0.0f, 90f, 0.0f),
          localScale = new Vector3(431f / (814f * Math.PI), 431f / (814f * Math.PI), 431f / (814f * Math.PI)),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
          childName = "Head",
          localPos = new Vector3(-0.07899f, 0.04586f, -0.05757f),
          localAngles = new Vector3(0.0f, 90f, 0.0f),
          localScale = new Vector3(431f / (814f * Math.PI), 431f / (814f * Math.PI), -431f / (814f * Math.PI)),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup114 = displayRuleGroup113;
      local57.displayRuleGroup = displayRuleGroup114;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup114 = keyAssetRuleGroup113;
      itemDisplayRules57.Add(keyAssetRuleGroup114);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules58 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup115 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup115.keyAsset = (Object) RoR2Content.Items.TitanGoldDuringTP;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local58 = ref keyAssetRuleGroup115;
      DisplayRuleGroup displayRuleGroup115 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup115).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
          childName = "Chest",
          localPos = new Vector3(-0.12607f, 0.1224f, -0.1718f),
          localAngles = new Vector3(308.4481f, 225.9465f, 333.6598f),
          localScale = new Vector3(0.1191f, 0.1191f, 0.1191f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup116 = displayRuleGroup115;
      local58.displayRuleGroup = displayRuleGroup116;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup116 = keyAssetRuleGroup115;
      itemDisplayRules58.Add(keyAssetRuleGroup116);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules59 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup117 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup117.keyAsset = (Object) RoR2Content.Items.SprintWisp;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local59 = ref keyAssetRuleGroup117;
      DisplayRuleGroup displayRuleGroup117 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup117).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.0f, 0.0f, 0.0f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup118 = displayRuleGroup117;
      local59.displayRuleGroup = displayRuleGroup118;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup118 = keyAssetRuleGroup117;
      itemDisplayRules59.Add(keyAssetRuleGroup118);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules60 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup119 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup119.keyAsset = (Object) RoR2Content.Items.BarrierOnKill;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local60 = ref keyAssetRuleGroup119;
      DisplayRuleGroup displayRuleGroup119 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup119).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
          childName = "HandR",
          localPos = new Vector3(0.02152f, 0.09368f, 0.00294f),
          localAngles = new Vector3(270.8865f, 15.31104f, 231.1227f),
          localScale = new Vector3(0.08837f, 0.08837f, 0.08837f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup120 = displayRuleGroup119;
      local60.displayRuleGroup = displayRuleGroup120;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup120 = keyAssetRuleGroup119;
      itemDisplayRules60.Add(keyAssetRuleGroup120);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules61 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup121 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup121.keyAsset = (Object) RoR2Content.Items.TPHealingNova;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local61 = ref keyAssetRuleGroup121;
      DisplayRuleGroup displayRuleGroup121 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup121).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
          childName = "Chest",
          localPos = new Vector3(0.03903f, 0.20738f, 0.08794f),
          localAngles = new Vector3(328.997f, 26.08873f, 0.92311f),
          localScale = new Vector3(0.13058f, 0.13058f, 0.01305f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup122 = displayRuleGroup121;
      local61.displayRuleGroup = displayRuleGroup122;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup122 = keyAssetRuleGroup121;
      itemDisplayRules61.Add(keyAssetRuleGroup122);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules62 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup123 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup123.keyAsset = (Object) RoR2Content.Items.LunarUtilityReplacement;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local62 = ref keyAssetRuleGroup123;
      DisplayRuleGroup displayRuleGroup123 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup123).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
          childName = "Chest",
          localPos = new Vector3(0.1135f, 0.23809f, -0.18159f),
          localAngles = new Vector3(53.33944f, 239.3017f, 36.17928f),
          localScale = new Vector3(0.2833f, 0.2833f, 0.2833f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup124 = displayRuleGroup123;
      local62.displayRuleGroup = displayRuleGroup124;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup124 = keyAssetRuleGroup123;
      itemDisplayRules62.Add(keyAssetRuleGroup124);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules63 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup125 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup125.keyAsset = (Object) RoR2Content.Items.Thorns;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local63 = ref keyAssetRuleGroup125;
      DisplayRuleGroup displayRuleGroup125 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup125).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
          childName = "UpperArmR",
          localPos = new Vector3(0.21954f, 0.25103f, 0.17563f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.5f, 0.5f, 0.5f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup126 = displayRuleGroup125;
      local63.displayRuleGroup = displayRuleGroup126;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup126 = keyAssetRuleGroup125;
      itemDisplayRules63.Add(keyAssetRuleGroup126);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules64 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup127 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup127.keyAsset = (Object) RoR2Content.Items.LunarPrimaryReplacement;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local64 = ref keyAssetRuleGroup127;
      DisplayRuleGroup displayRuleGroup127 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup127).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
          childName = "Head",
          localPos = new Vector3(0.04355f, 0.1722f, -0.01725f),
          localAngles = new Vector3(16.12343f, 180f, 180f),
          localScale = new Vector3(0.15152f, 0.15152f, 0.15152f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup128 = displayRuleGroup127;
      local64.displayRuleGroup = displayRuleGroup128;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup128 = keyAssetRuleGroup127;
      itemDisplayRules64.Add(keyAssetRuleGroup128);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules65 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup129 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup129.keyAsset = (Object) RoR2Content.Items.NovaOnLowHealth;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local65 = ref keyAssetRuleGroup129;
      DisplayRuleGroup displayRuleGroup129 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup129).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
          childName = "Chest",
          localPos = new Vector3(-0.0484f, 0.10984f, 0.02595f),
          localAngles = new Vector3(316.2306f, 45.1087f, 303.6165f),
          localScale = new Vector3(-0.01575f, -0.01575f, -0.01575f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup130 = displayRuleGroup129;
      local65.displayRuleGroup = displayRuleGroup130;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup130 = keyAssetRuleGroup129;
      itemDisplayRules65.Add(keyAssetRuleGroup130);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules66 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup131 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup131.keyAsset = (Object) RoR2Content.Items.LunarTrinket;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local66 = ref keyAssetRuleGroup131;
      DisplayRuleGroup displayRuleGroup131 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup131).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
          childName = "LowerArmL",
          localPos = new Vector3(0.0348f, 0.31195f, 0.00935f),
          localAngles = new Vector3(0.0f, 0.0f, 90f),
          localScale = new Vector3(1f, 1f, 1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup132 = displayRuleGroup131;
      local66.displayRuleGroup = displayRuleGroup132;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup132 = keyAssetRuleGroup131;
      itemDisplayRules66.Add(keyAssetRuleGroup132);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules67 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup133 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup133.keyAsset = (Object) RoR2Content.Items.Plant;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local67 = ref keyAssetRuleGroup133;
      DisplayRuleGroup displayRuleGroup133 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup133).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
          childName = "Chest",
          localPos = new Vector3(0.17787f, 0.31456f, -0.13987f),
          localAngles = new Vector3(323.3734f, 109.097f, 237.1637f),
          localScale = new Vector3(0.0429f, 0.0429f, 0.0429f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup134 = displayRuleGroup133;
      local67.displayRuleGroup = displayRuleGroup134;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup134 = keyAssetRuleGroup133;
      itemDisplayRules67.Add(keyAssetRuleGroup134);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules68 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup135 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup135.keyAsset = (Object) RoR2Content.Items.Bear;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local68 = ref keyAssetRuleGroup135;
      DisplayRuleGroup displayRuleGroup135 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup135).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.43227f, -0.17999f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.17815f, 0.17815f, 0.18152f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup136 = displayRuleGroup135;
      local68.displayRuleGroup = displayRuleGroup136;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup136 = keyAssetRuleGroup135;
      itemDisplayRules68.Add(keyAssetRuleGroup136);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules69 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup137 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup137.keyAsset = (Object) RoR2Content.Items.DeathMark;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local69 = ref keyAssetRuleGroup137;
      DisplayRuleGroup displayRuleGroup137 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup137).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.14709f, -0.03841f),
          localAngles = new Vector3(2.90161f, 4E-05f, -4E-05f),
          localScale = new Vector3(-0.0375f, -0.0341f, -0.0464f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup138 = displayRuleGroup137;
      local69.displayRuleGroup = displayRuleGroup138;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup138 = keyAssetRuleGroup137;
      itemDisplayRules69.Add(keyAssetRuleGroup138);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules70 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup139 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup139.keyAsset = (Object) RoR2Content.Items.ExplodeOnDeath;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local70 = ref keyAssetRuleGroup139;
      DisplayRuleGroup displayRuleGroup139 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup139).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
          childName = "Pelvis",
          localPos = new Vector3(0.13742f, 0.07936f, 0.01299f),
          localAngles = new Vector3(0.05995f, 358.84f, 8.76334f),
          localScale = new Vector3(0.0283f, 0.0283f, 0.0283f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup140 = displayRuleGroup139;
      local70.displayRuleGroup = displayRuleGroup140;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup140 = keyAssetRuleGroup139;
      itemDisplayRules70.Add(keyAssetRuleGroup140);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules71 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup141 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup141.keyAsset = (Object) RoR2Content.Items.Seed;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local71 = ref keyAssetRuleGroup141;
      DisplayRuleGroup displayRuleGroup141 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup141).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
          childName = "Head",
          localPos = new Vector3(-0.10265f, 0.07407f, -0.0262f),
          localAngles = new Vector3(31.54254f, 236.3528f, 91.58902f),
          localScale = new Vector3(0.01154f, 0.01009f, 0.01009f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup142 = displayRuleGroup141;
      local71.displayRuleGroup = displayRuleGroup142;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup142 = keyAssetRuleGroup141;
      itemDisplayRules71.Add(keyAssetRuleGroup142);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules72 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup143 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup143.keyAsset = (Object) RoR2Content.Items.SprintOutOfCombat;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local72 = ref keyAssetRuleGroup143;
      DisplayRuleGroup displayRuleGroup143 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup143).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
          childName = "Pelvis",
          localPos = new Vector3(0.13093f, -0.05603f, -0.00595f),
          localAngles = new Vector3(359.9796f, 0.38747f, 18.12464f),
          localScale = new Vector3(0.31015f, 0.31015f, 0.31015f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup144 = displayRuleGroup143;
      local72.displayRuleGroup = displayRuleGroup144;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup144 = keyAssetRuleGroup143;
      itemDisplayRules72.Add(keyAssetRuleGroup144);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules73 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup145 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup145.keyAsset = (Object) RoR2Content.Items.CooldownOnCrit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local73 = ref keyAssetRuleGroup145;
      DisplayRuleGroup displayRuleGroup145 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup145).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySkull"),
          childName = "HandR",
          localPos = new Vector3(0.03084f, 0.09123f, -0.01059f),
          localAngles = new Vector3(85.9473f, 80.3895f, 184.9944f),
          localScale = new Vector3(0.01652f, 0.01652f, 0.01652f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup146 = displayRuleGroup145;
      local73.displayRuleGroup = displayRuleGroup146;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup146 = keyAssetRuleGroup145;
      itemDisplayRules73.Add(keyAssetRuleGroup146);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules74 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup147 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup147.keyAsset = (Object) RoR2Content.Items.Phasing;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local74 = ref keyAssetRuleGroup147;
      DisplayRuleGroup displayRuleGroup147 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup147).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
          childName = "Chest",
          localPos = new Vector3(-0.0063f, 0.14073f, 0.00698f),
          localAngles = new Vector3(90f, 0.0f, 0.0f),
          localScale = new Vector3(0.02868f, 0.04732f, 0.03156f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup148 = displayRuleGroup147;
      local74.displayRuleGroup = displayRuleGroup148;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup148 = keyAssetRuleGroup147;
      itemDisplayRules74.Add(keyAssetRuleGroup148);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules75 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup149 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup149.keyAsset = (Object) RoR2Content.Items.PersonalShield;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local75 = ref keyAssetRuleGroup149;
      DisplayRuleGroup displayRuleGroup149 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup149).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.27486f, 0.05085f),
          localAngles = new Vector3(88.26055f, 232.4472f, 51.9133f),
          localScale = new Vector3(0.1057f, 0.1057f, 0.1057f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup150 = displayRuleGroup149;
      local75.displayRuleGroup = displayRuleGroup150;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup150 = keyAssetRuleGroup149;
      itemDisplayRules75.Add(keyAssetRuleGroup150);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules76 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup151 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup151.keyAsset = (Object) RoR2Content.Items.ShockNearby;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local76 = ref keyAssetRuleGroup151;
      DisplayRuleGroup displayRuleGroup151 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup151).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.30543f, -0.16814f),
          localAngles = new Vector3(285.2071f, 2.45629f, 357.4169f),
          localScale = new Vector3(0.24216f, 0.24216f, 0.24216f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup152 = displayRuleGroup151;
      local76.displayRuleGroup = displayRuleGroup152;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup152 = keyAssetRuleGroup151;
      itemDisplayRules76.Add(keyAssetRuleGroup152);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules77 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup153 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup153.keyAsset = (Object) RoR2Content.Items.ShieldOnly;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local77 = ref keyAssetRuleGroup153;
      DisplayRuleGroup displayRuleGroup153 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup153).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
          childName = "Head",
          localPos = new Vector3(0.05392f, 0.20212f, -0.01851f),
          localAngles = new Vector3(348.1819f, 268.0985f, 0.3896f),
          localScale = new Vector3(0.3521f, 0.3521f, 0.3521f),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
          childName = "Head",
          localPos = new Vector3(-0.05153f, 0.20892f, -0.01879f),
          localAngles = new Vector3(11.8181f, 268.0985f, 359.6104f),
          localScale = new Vector3(0.3521f, 0.3521f, -0.3521f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup154 = displayRuleGroup153;
      local77.displayRuleGroup = displayRuleGroup154;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup154 = keyAssetRuleGroup153;
      itemDisplayRules77.Add(keyAssetRuleGroup154);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules78 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup155 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup155.keyAsset = (Object) RoR2Content.Items.AlienHead;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local78 = ref keyAssetRuleGroup155;
      DisplayRuleGroup displayRuleGroup155 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup155).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
          childName = "Base",
          localPos = new Vector3(0.10939f, 0.13417f, -0.04608f),
          localAngles = new Vector3(284.1171f, 239.5653f, 260.8905f),
          localScale = new Vector3(0.6701f, 0.6701f, 0.6701f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup156 = displayRuleGroup155;
      local78.displayRuleGroup = displayRuleGroup156;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup156 = keyAssetRuleGroup155;
      itemDisplayRules78.Add(keyAssetRuleGroup156);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules79 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup157 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup157.keyAsset = (Object) RoR2Content.Items.HeadHunter;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local79 = ref keyAssetRuleGroup157;
      DisplayRuleGroup displayRuleGroup157 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup157).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
          childName = "Pelvis",
          localPos = new Vector3(-0.01582f, -0.0147f, -0.0275f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.4769f, 0.15897f, 0.15897f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup158 = displayRuleGroup157;
      local79.displayRuleGroup = displayRuleGroup158;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup158 = keyAssetRuleGroup157;
      itemDisplayRules79.Add(keyAssetRuleGroup158);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules80 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup159 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup159.keyAsset = (Object) RoR2Content.Items.EnergizedOnEquipmentUse;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local80 = ref keyAssetRuleGroup159;
      DisplayRuleGroup displayRuleGroup159 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup159).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
          childName = "Chest",
          localPos = new Vector3(-0.12219f, 0.22746f, 0.05431f),
          localAngles = new Vector3(319.4921f, 95.44549f, 180.451f),
          localScale = new Vector3(0.2732f, 0.2732f, 0.2732f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup160 = displayRuleGroup159;
      local80.displayRuleGroup = displayRuleGroup160;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup160 = keyAssetRuleGroup159;
      itemDisplayRules80.Add(keyAssetRuleGroup160);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules81 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup161 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup161.keyAsset = (Object) RoR2Content.Items.FlatHealth;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local81 = ref keyAssetRuleGroup161;
      DisplayRuleGroup displayRuleGroup161 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup161).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
          childName = "Pelvis",
          localPos = new Vector3(-0.1416f, 0.09794f, -0.09843f),
          localAngles = new Vector3(351.9934f, 237.8542f, 86.18441f),
          localScale = new Vector3(0.1245f, 0.1155f, 0.1155f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup162 = displayRuleGroup161;
      local81.displayRuleGroup = displayRuleGroup162;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup162 = keyAssetRuleGroup161;
      itemDisplayRules81.Add(keyAssetRuleGroup162);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules82 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup163 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup163.keyAsset = (Object) RoR2Content.Items.Tooth;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local82 = ref keyAssetRuleGroup163;
      DisplayRuleGroup displayRuleGroup163 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup163).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.0f, 0.0f, 0.0f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup164 = displayRuleGroup163;
      local82.displayRuleGroup = displayRuleGroup164;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup164 = keyAssetRuleGroup163;
      itemDisplayRules82.Add(keyAssetRuleGroup164);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules83 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup165 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup165.keyAsset = (Object) RoR2Content.Items.Pearl;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local83 = ref keyAssetRuleGroup165;
      DisplayRuleGroup displayRuleGroup165 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup165).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.086f, -0.02121f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.2f, 0.2f, 0.2f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup166 = displayRuleGroup165;
      local83.displayRuleGroup = displayRuleGroup166;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup166 = keyAssetRuleGroup165;
      itemDisplayRules83.Add(keyAssetRuleGroup166);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules84 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup167 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup167.keyAsset = (Object) RoR2Content.Items.ShinyPearl;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local84 = ref keyAssetRuleGroup167;
      DisplayRuleGroup displayRuleGroup167 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup167).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.2f, 0.2f, 0.2f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup168 = displayRuleGroup167;
      local84.displayRuleGroup = displayRuleGroup168;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup168 = keyAssetRuleGroup167;
      itemDisplayRules84.Add(keyAssetRuleGroup168);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules85 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup169 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup169.keyAsset = (Object) RoR2Content.Items.BonusGoldPackOnKill;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local85 = ref keyAssetRuleGroup169;
      DisplayRuleGroup displayRuleGroup169 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup169).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
          childName = "Base",
          localPos = new Vector3(0.14954f, 0.05886f, 0.01124f),
          localAngles = new Vector3(345.828f, 90.44725f, 85.56704f),
          localScale = new Vector3(0.05585f, 0.05585f, 0.05585f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup170 = displayRuleGroup169;
      local85.displayRuleGroup = displayRuleGroup170;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup170 = keyAssetRuleGroup169;
      itemDisplayRules85.Add(keyAssetRuleGroup170);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules86 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup171 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup171.keyAsset = (Object) RoR2Content.Items.Squid;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local86 = ref keyAssetRuleGroup171;
      DisplayRuleGroup displayRuleGroup171 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup171).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
          childName = "Chest",
          localPos = new Vector3(0.08975f, 0.16036f, -0.16564f),
          localAngles = new Vector3(5.65006f, 75.07253f, 276.3981f),
          localScale = new Vector3(0.02f, 0.02f, 0.02f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup172 = displayRuleGroup171;
      local86.displayRuleGroup = displayRuleGroup172;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup172 = keyAssetRuleGroup171;
      itemDisplayRules86.Add(keyAssetRuleGroup172);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules87 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup173 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup173.keyAsset = (Object) RoR2Content.Items.Icicle;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local87 = ref keyAssetRuleGroup173;
      DisplayRuleGroup displayRuleGroup173 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup173).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
          childName = "Base",
          localPos = new Vector3(0.45946f, 0.68513f, -0.43309f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(1f, 1f, 1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup174 = displayRuleGroup173;
      local87.displayRuleGroup = displayRuleGroup174;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup174 = keyAssetRuleGroup173;
      itemDisplayRules87.Add(keyAssetRuleGroup174);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules88 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup175 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup175.keyAsset = (Object) RoR2Content.Items.Talisman;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local88 = ref keyAssetRuleGroup175;
      DisplayRuleGroup displayRuleGroup175 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup175).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
          childName = "Base",
          localPos = new Vector3(0.2f, 0.95856f, -0.46796f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(1f, 1f, 1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup176 = displayRuleGroup175;
      local88.displayRuleGroup = displayRuleGroup176;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup176 = keyAssetRuleGroup175;
      itemDisplayRules88.Add(keyAssetRuleGroup176);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules89 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup177 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup177.keyAsset = (Object) RoR2Content.Items.LaserTurbine;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local89 = ref keyAssetRuleGroup177;
      DisplayRuleGroup displayRuleGroup177 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup177).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.00283f, -0.24214f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.2159f, 0.2159f, 0.2159f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup178 = displayRuleGroup177;
      local89.displayRuleGroup = displayRuleGroup178;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup178 = keyAssetRuleGroup177;
      itemDisplayRules89.Add(keyAssetRuleGroup178);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules90 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup179 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup179.keyAsset = (Object) RoR2Content.Items.FocusConvergence;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local90 = ref keyAssetRuleGroup179;
      DisplayRuleGroup displayRuleGroup179 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup179).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
          childName = "Base",
          localPos = new Vector3(-0.5429f, 0.96699f, -0.33138f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.1f, 0.1f, 0.1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup180 = displayRuleGroup179;
      local90.displayRuleGroup = displayRuleGroup180;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup180 = keyAssetRuleGroup179;
      itemDisplayRules90.Add(keyAssetRuleGroup180);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules91 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup181 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup181.keyAsset = (Object) RoR2Content.Items.Incubator;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local91 = ref keyAssetRuleGroup181;
      DisplayRuleGroup displayRuleGroup181 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup181).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
          childName = "Chest",
          localPos = new Vector3(-0.08381f, 0.17971f, -0.09568f),
          localAngles = new Vector3(353.0521f, 317.2421f, 69.6292f),
          localScale = new Vector3(0.02609f, 0.02609f, 0.02609f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup182 = displayRuleGroup181;
      local91.displayRuleGroup = displayRuleGroup182;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup182 = keyAssetRuleGroup181;
      itemDisplayRules91.Add(keyAssetRuleGroup182);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules92 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup183 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup183.keyAsset = (Object) RoR2Content.Items.FireballsOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local92 = ref keyAssetRuleGroup183;
      DisplayRuleGroup displayRuleGroup183 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup183).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
          childName = "Pelvis",
          localPos = new Vector3(0.11671f, 0.06175f, 0.03695f),
          localAngles = new Vector3(57.4526f, 80.80998f, 26.94625f),
          localScale = new Vector3(0.00805f, 0.00805f, 0.00805f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup184 = displayRuleGroup183;
      local92.displayRuleGroup = displayRuleGroup184;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup184 = keyAssetRuleGroup183;
      itemDisplayRules92.Add(keyAssetRuleGroup184);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules93 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup185 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup185.keyAsset = (Object) RoR2Content.Items.SiphonOnLowHealth;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local93 = ref keyAssetRuleGroup185;
      DisplayRuleGroup displayRuleGroup185 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup185).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.0f, 0.0f, 0.0f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup186 = displayRuleGroup185;
      local93.displayRuleGroup = displayRuleGroup186;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup186 = keyAssetRuleGroup185;
      itemDisplayRules93.Add(keyAssetRuleGroup186);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules94 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup187 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup187.keyAsset = (Object) RoR2Content.Items.BleedOnHitAndExplode;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local94 = ref keyAssetRuleGroup187;
      DisplayRuleGroup displayRuleGroup187 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup187).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
          childName = "Head",
          localPos = new Vector3(0.00576f, 0.04279f, 0.0169f),
          localAngles = new Vector3(326.6455f, -1E-05f, 3E-05f),
          localScale = new Vector3(0.0486f, 0.0486f, 0.0486f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup188 = displayRuleGroup187;
      local94.displayRuleGroup = displayRuleGroup188;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup188 = keyAssetRuleGroup187;
      itemDisplayRules94.Add(keyAssetRuleGroup188);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules95 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup189 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup189.keyAsset = (Object) RoR2Content.Items.MonstersOnShrineUse;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local95 = ref keyAssetRuleGroup189;
      DisplayRuleGroup displayRuleGroup189 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup189).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
          childName = "Chest",
          localPos = new Vector3(0.0022f, 0.084f, 0.066f),
          localAngles = new Vector3(352.4521f, 260.6884f, 341.5106f),
          localScale = new Vector3(-0.01067f, -0.01067f, -0.01067f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup190 = displayRuleGroup189;
      local95.displayRuleGroup = displayRuleGroup190;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup190 = keyAssetRuleGroup189;
      itemDisplayRules95.Add(keyAssetRuleGroup190);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules96 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup191 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup191.keyAsset = (Object) RoR2Content.Items.RandomDamageZone;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local96 = ref keyAssetRuleGroup191;
      DisplayRuleGroup displayRuleGroup191 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup191).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.20552f, 0.03736f),
          localAngles = new Vector3(349.218f, 235.9453f, 0.0f),
          localScale = new Vector3(0.00365f, 0.00365f, 0.00365f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup192 = displayRuleGroup191;
      local96.displayRuleGroup = displayRuleGroup192;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup192 = keyAssetRuleGroup191;
      itemDisplayRules96.Add(keyAssetRuleGroup192);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules97 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup193 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup193.keyAsset = (Object) RoR2Content.Equipment.Fruit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local97 = ref keyAssetRuleGroup193;
      DisplayRuleGroup displayRuleGroup193 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup193).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
          childName = "Chest",
          localPos = new Vector3(-0.08468f, 0.06027f, -0.05945f),
          localAngles = new Vector3(355.5085f, 54.78192f, 339.6581f),
          localScale = new Vector3(0.2118f, 0.2118f, 0.2118f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup194 = displayRuleGroup193;
      local97.displayRuleGroup = displayRuleGroup194;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup194 = keyAssetRuleGroup193;
      itemDisplayRules97.Add(keyAssetRuleGroup194);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules98 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup195 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup195.keyAsset = (Object) RoR2Content.Equipment.AffixRed;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local98 = ref keyAssetRuleGroup195;
      DisplayRuleGroup displayRuleGroup195 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup195).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup196 = displayRuleGroup195;
      local98.displayRuleGroup = displayRuleGroup196;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup196 = keyAssetRuleGroup195;
      itemDisplayRules98.Add(keyAssetRuleGroup196);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules99 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup197 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup197.keyAsset = (Object) RoR2Content.Equipment.AffixBlue;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local99 = ref keyAssetRuleGroup197;
      DisplayRuleGroup displayRuleGroup197 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup197).rules = new ItemDisplayRule[2]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        },
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup198 = displayRuleGroup197;
      local99.displayRuleGroup = displayRuleGroup198;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup198 = keyAssetRuleGroup197;
      itemDisplayRules99.Add(keyAssetRuleGroup198);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules100 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup199 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup199.keyAsset = (Object) RoR2Content.Equipment.AffixWhite;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local100 = ref keyAssetRuleGroup199;
      DisplayRuleGroup displayRuleGroup199 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup199).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup200 = displayRuleGroup199;
      local100.displayRuleGroup = displayRuleGroup200;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup200 = keyAssetRuleGroup199;
      itemDisplayRules100.Add(keyAssetRuleGroup200);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules101 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup201 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup201.keyAsset = (Object) RoR2Content.Equipment.AffixPoison;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local101 = ref keyAssetRuleGroup201;
      DisplayRuleGroup displayRuleGroup201 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup201).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup202 = displayRuleGroup201;
      local101.displayRuleGroup = displayRuleGroup202;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup202 = keyAssetRuleGroup201;
      itemDisplayRules101.Add(keyAssetRuleGroup202);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules102 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup203 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup203.keyAsset = (Object) RoR2Content.Equipment.AffixHaunted;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local102 = ref keyAssetRuleGroup203;
      DisplayRuleGroup displayRuleGroup203 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup203).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup204 = displayRuleGroup203;
      local102.displayRuleGroup = displayRuleGroup204;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup204 = keyAssetRuleGroup203;
      itemDisplayRules102.Add(keyAssetRuleGroup204);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules103 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup205 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup205.keyAsset = (Object) RoR2Content.Equipment.CritOnUse;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local103 = ref keyAssetRuleGroup205;
      DisplayRuleGroup displayRuleGroup205 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup205).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.24193f, -0.00185f),
          localAngles = new Vector3(272.112f, 180.0012f, 179.9989f),
          localScale = new Vector3(0.13259f, 0.13259f, 0.13259f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup206 = displayRuleGroup205;
      local103.displayRuleGroup = displayRuleGroup206;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup206 = keyAssetRuleGroup205;
      itemDisplayRules103.Add(keyAssetRuleGroup206);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules104 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup207 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup207.keyAsset = (Object) RoR2Content.Equipment.DroneBackup;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local104 = ref keyAssetRuleGroup207;
      DisplayRuleGroup displayRuleGroup207 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup207).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
          childName = "Pelvis",
          localPos = new Vector3(0.0f, 0.1269f, 0.0f),
          localAngles = new Vector3(0.0f, 90f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup208 = displayRuleGroup207;
      local104.displayRuleGroup = displayRuleGroup208;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup208 = keyAssetRuleGroup207;
      itemDisplayRules104.Add(keyAssetRuleGroup208);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules105 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup209 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup209.keyAsset = (Object) RoR2Content.Equipment.Lightning;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local105 = ref keyAssetRuleGroup209;
      DisplayRuleGroup displayRuleGroup209 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup209).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
          childName = "UpperArmR",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.3413f, 0.3413f, 0.3413f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup210 = displayRuleGroup209;
      local105.displayRuleGroup = displayRuleGroup210;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup210 = keyAssetRuleGroup209;
      itemDisplayRules105.Add(keyAssetRuleGroup210);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules106 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup211 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup211.keyAsset = (Object) RoR2Content.Equipment.BurnNearby;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local106 = ref keyAssetRuleGroup211;
      DisplayRuleGroup displayRuleGroup211 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup211).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup212 = displayRuleGroup211;
      local106.displayRuleGroup = displayRuleGroup212;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup212 = keyAssetRuleGroup211;
      itemDisplayRules106.Add(keyAssetRuleGroup212);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules107 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup213 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup213.keyAsset = (Object) RoR2Content.Equipment.CrippleWard;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local107 = ref keyAssetRuleGroup213;
      DisplayRuleGroup displayRuleGroup213 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup213).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup214 = displayRuleGroup213;
      local107.displayRuleGroup = displayRuleGroup214;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup214 = keyAssetRuleGroup213;
      itemDisplayRules107.Add(keyAssetRuleGroup214);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules108 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup215 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup215.keyAsset = (Object) RoR2Content.Equipment.QuestVolatileBattery;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local108 = ref keyAssetRuleGroup215;
      DisplayRuleGroup displayRuleGroup215 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup215).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.2584f, -0.0987f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.2188f, 0.2188f, 0.2188f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup216 = displayRuleGroup215;
      local108.displayRuleGroup = displayRuleGroup216;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup216 = keyAssetRuleGroup215;
      itemDisplayRules108.Add(keyAssetRuleGroup216);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules109 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup217 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup217.keyAsset = (Object) RoR2Content.Equipment.GainArmor;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local109 = ref keyAssetRuleGroup217;
      DisplayRuleGroup displayRuleGroup217 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup217).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup218 = displayRuleGroup217;
      local109.displayRuleGroup = displayRuleGroup218;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup218 = keyAssetRuleGroup217;
      itemDisplayRules109.Add(keyAssetRuleGroup218);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules110 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup219 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup219.keyAsset = (Object) RoR2Content.Equipment.Recycle;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local110 = ref keyAssetRuleGroup219;
      DisplayRuleGroup displayRuleGroup219 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup219).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup220 = displayRuleGroup219;
      local110.displayRuleGroup = displayRuleGroup220;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup220 = keyAssetRuleGroup219;
      itemDisplayRules110.Add(keyAssetRuleGroup220);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules111 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup221 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup221.keyAsset = (Object) RoR2Content.Equipment.FireBallDash;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local111 = ref keyAssetRuleGroup221;
      DisplayRuleGroup displayRuleGroup221 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup221).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
          childName = "Pelvis",
          localPos = new Vector3(0.0f, 0.1269f, 0.0f),
          localAngles = new Vector3(0.0f, 90f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup222 = displayRuleGroup221;
      local111.displayRuleGroup = displayRuleGroup222;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup222 = keyAssetRuleGroup221;
      itemDisplayRules111.Add(keyAssetRuleGroup222);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules112 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup223 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup223.keyAsset = (Object) RoR2Content.Equipment.Cleanse;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local112 = ref keyAssetRuleGroup223;
      DisplayRuleGroup displayRuleGroup223 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup223).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup224 = displayRuleGroup223;
      local112.displayRuleGroup = displayRuleGroup224;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup224 = keyAssetRuleGroup223;
      itemDisplayRules112.Add(keyAssetRuleGroup224);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules113 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup225 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup225.keyAsset = (Object) RoR2Content.Equipment.Tonic;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local113 = ref keyAssetRuleGroup225;
      DisplayRuleGroup displayRuleGroup225 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup225).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup226 = displayRuleGroup225;
      local113.displayRuleGroup = displayRuleGroup226;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup226 = keyAssetRuleGroup225;
      itemDisplayRules113.Add(keyAssetRuleGroup226);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules114 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup227 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup227.keyAsset = (Object) RoR2Content.Equipment.Gateway;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local114 = ref keyAssetRuleGroup227;
      DisplayRuleGroup displayRuleGroup227 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup227).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup228 = displayRuleGroup227;
      local114.displayRuleGroup = displayRuleGroup228;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup228 = keyAssetRuleGroup227;
      itemDisplayRules114.Add(keyAssetRuleGroup228);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules115 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup229 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup229.keyAsset = (Object) RoR2Content.Equipment.Meteor;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local115 = ref keyAssetRuleGroup229;
      DisplayRuleGroup displayRuleGroup229 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup229).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
          childName = "Base",
          localPos = new Vector3(0.0f, 1.18521f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(1f, 1f, 1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup230 = displayRuleGroup229;
      local115.displayRuleGroup = displayRuleGroup230;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup230 = keyAssetRuleGroup229;
      itemDisplayRules115.Add(keyAssetRuleGroup230);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules116 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup231 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup231.keyAsset = (Object) RoR2Content.Equipment.Saw;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local116 = ref keyAssetRuleGroup231;
      DisplayRuleGroup displayRuleGroup231 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup231).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerang"),
          childName = "Base",
          localPos = new Vector3(0.0f, 0.678f, -0.46362f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(1f, 1f, 1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup232 = displayRuleGroup231;
      local116.displayRuleGroup = displayRuleGroup232;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup232 = keyAssetRuleGroup231;
      itemDisplayRules116.Add(keyAssetRuleGroup232);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules117 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup233 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup233.keyAsset = (Object) RoR2Content.Equipment.Blackhole;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local117 = ref keyAssetRuleGroup233;
      DisplayRuleGroup displayRuleGroup233 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup233).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
          childName = "Base",
          localPos = new Vector3(-0.2f, 0.94981f, -0.46657f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.5f, 0.5f, 0.5f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup234 = displayRuleGroup233;
      local117.displayRuleGroup = displayRuleGroup234;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup234 = keyAssetRuleGroup233;
      itemDisplayRules117.Add(keyAssetRuleGroup234);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules118 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup235 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup235.keyAsset = (Object) RoR2Content.Equipment.Scanner;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local118 = ref keyAssetRuleGroup235;
      DisplayRuleGroup displayRuleGroup235 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup235).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup236 = displayRuleGroup235;
      local118.displayRuleGroup = displayRuleGroup236;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup236 = keyAssetRuleGroup235;
      itemDisplayRules118.Add(keyAssetRuleGroup236);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules119 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup237 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup237.keyAsset = (Object) RoR2Content.Equipment.DeathProjectile;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local119 = ref keyAssetRuleGroup237;
      DisplayRuleGroup displayRuleGroup237 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup237).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
          childName = "Pelvis",
          localPos = new Vector3(0.0f, 0.028f, -0.0977f),
          localAngles = new Vector3(0.0f, 180f, 0.0f),
          localScale = new Vector3(0.0596f, 0.0596f, 0.0596f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup238 = displayRuleGroup237;
      local119.displayRuleGroup = displayRuleGroup238;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup238 = keyAssetRuleGroup237;
      itemDisplayRules119.Add(keyAssetRuleGroup238);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules120 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup239 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup239.keyAsset = (Object) RoR2Content.Equipment.LifestealOnHit;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local120 = ref keyAssetRuleGroup239;
      DisplayRuleGroup displayRuleGroup239 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup239).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
          childName = "Head",
          localPos = new Vector3(0.0f, 0.07288f, 0.00119f),
          localAngles = new Vector3(274.4367f, 270f, 90f),
          localScale = new Vector3(0.1f, 0.1f, 0.1f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup240 = displayRuleGroup239;
      local120.displayRuleGroup = displayRuleGroup240;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup240 = keyAssetRuleGroup239;
      itemDisplayRules120.Add(keyAssetRuleGroup240);
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules121 = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup241 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup241.keyAsset = (Object) RoR2Content.Equipment.TeamWarCry;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local121 = ref keyAssetRuleGroup241;
      DisplayRuleGroup displayRuleGroup241 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup241).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
          childName = "Chest",
          localPos = new Vector3(0.0f, 0.0f, 0.0f),
          localAngles = new Vector3(0.0f, 0.0f, 0.0f),
          localScale = new Vector3(0.01f, 0.01f, 0.01f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup242 = displayRuleGroup241;
      local121.displayRuleGroup = displayRuleGroup242;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup242 = keyAssetRuleGroup241;
      itemDisplayRules121.Add(keyAssetRuleGroup242);
      if (RiskOfRuinaPlugin.ancientScepterInstalled)
        this.SetupScepterDisplay();
      this.itemDisplayRuleSet.keyAssetRuleGroups = this.itemDisplayRules.ToArray();
      this.itemDisplayRuleSet.GenerateRuntimeValues();
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void SetupScepterDisplay()
    {
      List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules = this.itemDisplayRules;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup1 = new ItemDisplayRuleSet.KeyAssetRuleGroup();
      keyAssetRuleGroup1.keyAsset = (Object) ((ItemBase) ItemBase<AncientScepterItem>.instance).ItemDef;
      ref ItemDisplayRuleSet.KeyAssetRuleGroup local = ref keyAssetRuleGroup1;
      DisplayRuleGroup displayRuleGroup1 = new DisplayRuleGroup();
      // ISSUE: explicit reference operation
      (^ref displayRuleGroup1).rules = new ItemDisplayRule[1]
      {
        new ItemDisplayRule()
        {
          ruleType = (ItemDisplayRuleType) 0,
          followerPrefab = ItemBase.displayPrefab,
          childName = "HandR",
          localPos = new Vector3(-0.00647f, 0.0854f, -0.02327f),
          localAngles = new Vector3(315.4133f, 227.5006f, 91.38048f),
          localScale = new Vector3(0.2235f, 0.2235f, 0.2235f),
          limbMask = (LimbFlags) 0
        }
      };
      DisplayRuleGroup displayRuleGroup2 = displayRuleGroup1;
      local.displayRuleGroup = displayRuleGroup2;
      ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup2 = keyAssetRuleGroup1;
      itemDisplayRules.Add(keyAssetRuleGroup2);
    }

    private static CharacterModel.RendererInfo[] SkinRendererInfos(
      CharacterModel.RendererInfo[] defaultRenderers,
      Material[] materials)
    {
      CharacterModel.RendererInfo[] rendererInfoArray = new CharacterModel.RendererInfo[defaultRenderers.Length];
      defaultRenderers.CopyTo((Array) rendererInfoArray, 0);
      rendererInfoArray[0].defaultMaterial = materials[0];
      rendererInfoArray[1].defaultMaterial = materials[1];
      rendererInfoArray[SurvivorBase.instance.mainRendererIndex].defaultMaterial = materials[2];
      return rendererInfoArray;
    }
  }
}
