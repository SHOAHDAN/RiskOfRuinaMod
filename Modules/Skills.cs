// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Skills
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal static class Skills
  {
    internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
    internal static List<SkillDef> skillDefs = new List<SkillDef>();
    public static SkillDef ScepterBasicAttack;
    public static SkillDef scepterShockwaveDef;
    public static SkillDef unlockSkillDef;

    public static void ScepterSkillSetup(string prefix)
    {
      RiskOfRuinaMod.Modules.Skills.ScepterBasicAttack = RiskOfRuinaMod.Modules.Skills.CreatePrimarySkillDef(new SerializableEntityStateType(typeof (BasicStringStart)), "Body", prefix + "_REDMIST_BODY_PRIMARY_UPSTANDINGSLASH_NAME", prefix + "_REDMIST_BODY_PRIMARY_UPSTANDINGSLASH_DESCRIPTION", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistPrimaryIcon"), false);
      RiskOfRuinaMod.Modules.Skills.skillDefs.Add(RiskOfRuinaMod.Modules.Skills.ScepterBasicAttack);
      States.AddSkill(typeof (ScepterChannelShockwave));
      States.AddSkill(typeof (ScepterCastShockwave));
      RiskOfRuinaMod.Modules.Skills.scepterShockwaveDef = RiskOfRuinaMod.Modules.Skills.CreateSkillDef(new SkillDefInfo()
      {
        skillName = prefix + "_ARBITER_BODY_SPECIAL_SCEPTERSHOCKWAVE_NAME",
        skillNameToken = prefix + "_ARBITER_BODY_SPECIAL_SCEPTERSHOCKWAVE_NAME",
        skillDescriptionToken = prefix + "_ARBITER_BODY_SPECIAL_SCEPTERSHOCKWAVE_DESCRIPTION",
        skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterSpecialIconScepter"),
        activationState = new SerializableEntityStateType(typeof (ScepterChannelShockwave)),
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
        stockToConsume = 5
      });
      RiskOfRuinaMod.Modules.Skills.skillDefs.Add(RiskOfRuinaMod.Modules.Skills.scepterShockwaveDef);
    }

    internal static void CreateSkillFamilies(GameObject targetPrefab)
    {
      foreach (Object componentsInChild in targetPrefab.GetComponentsInChildren<GenericSkill>())
        Object.DestroyImmediate(componentsInChild);
      SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
      component.primary = targetPrefab.AddComponent<GenericSkill>();
      SkillFamily instance1 = ScriptableObject.CreateInstance<SkillFamily>();
      ((Object) instance1).name = ((Object) targetPrefab).name + "PrimaryFamily";
      instance1.variants = new SkillFamily.Variant[0];
      component.primary._skillFamily = instance1;
      component.secondary = targetPrefab.AddComponent<GenericSkill>();
      SkillFamily instance2 = ScriptableObject.CreateInstance<SkillFamily>();
      ((Object) instance2).name = ((Object) targetPrefab).name + "SecondaryFamily";
      instance2.variants = new SkillFamily.Variant[0];
      component.secondary._skillFamily = instance2;
      component.utility = targetPrefab.AddComponent<GenericSkill>();
      SkillFamily instance3 = ScriptableObject.CreateInstance<SkillFamily>();
      ((Object) instance3).name = ((Object) targetPrefab).name + "UtilityFamily";
      instance3.variants = new SkillFamily.Variant[0];
      component.utility._skillFamily = instance3;
      component.special = targetPrefab.AddComponent<GenericSkill>();
      SkillFamily instance4 = ScriptableObject.CreateInstance<SkillFamily>();
      ((Object) instance4).name = ((Object) targetPrefab).name + "SpecialFamily";
      instance4.variants = new SkillFamily.Variant[0];
      component.special._skillFamily = instance4;
      RiskOfRuinaMod.Modules.Skills.skillFamilies.Add(instance1);
      RiskOfRuinaMod.Modules.Skills.skillFamilies.Add(instance2);
      RiskOfRuinaMod.Modules.Skills.skillFamilies.Add(instance3);
      RiskOfRuinaMod.Modules.Skills.skillFamilies.Add(instance4);
    }

    internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
    {
      SkillFamily skillFamily = targetPrefab.GetComponent<SkillLocator>().primary.skillFamily;
      Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
      SkillFamily.Variant[] variants = skillFamily.variants;
      int index = skillFamily.variants.Length - 1;
      SkillFamily.Variant variant1 = new SkillFamily.Variant();
      variant1.skillDef = skillDef;
      ((SkillFamily.Variant) ref variant1).viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, (ViewablesCatalog.Node) null);
      SkillFamily.Variant variant2 = variant1;
      variants[index] = variant2;
    }

    internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
    {
      SkillFamily skillFamily = targetPrefab.GetComponent<SkillLocator>().secondary.skillFamily;
      Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
      SkillFamily.Variant[] variants = skillFamily.variants;
      int index = skillFamily.variants.Length - 1;
      SkillFamily.Variant variant1 = new SkillFamily.Variant();
      variant1.skillDef = skillDef;
      ((SkillFamily.Variant) ref variant1).viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, (ViewablesCatalog.Node) null);
      SkillFamily.Variant variant2 = variant1;
      variants[index] = variant2;
    }

    internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
    {
      foreach (SkillDef skillDef in skillDefs)
        RiskOfRuinaMod.Modules.Skills.AddSecondarySkill(targetPrefab, skillDef);
    }

    internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
    {
      SkillFamily skillFamily = targetPrefab.GetComponent<SkillLocator>().utility.skillFamily;
      Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
      SkillFamily.Variant[] variants = skillFamily.variants;
      int index = skillFamily.variants.Length - 1;
      SkillFamily.Variant variant1 = new SkillFamily.Variant();
      variant1.skillDef = skillDef;
      ((SkillFamily.Variant) ref variant1).viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, (ViewablesCatalog.Node) null);
      SkillFamily.Variant variant2 = variant1;
      variants[index] = variant2;
    }

    internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
    {
      foreach (SkillDef skillDef in skillDefs)
        RiskOfRuinaMod.Modules.Skills.AddUtilitySkill(targetPrefab, skillDef);
    }

    internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
    {
      SkillFamily skillFamily = targetPrefab.GetComponent<SkillLocator>().special.skillFamily;
      Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
      SkillFamily.Variant[] variants = skillFamily.variants;
      int index = skillFamily.variants.Length - 1;
      SkillFamily.Variant variant1 = new SkillFamily.Variant();
      variant1.skillDef = skillDef;
      ((SkillFamily.Variant) ref variant1).viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, (ViewablesCatalog.Node) null);
      SkillFamily.Variant variant2 = variant1;
      variants[index] = variant2;
    }

    internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
    {
      foreach (SkillDef skillDef in skillDefs)
        RiskOfRuinaMod.Modules.Skills.AddSpecialSkill(targetPrefab, skillDef);
    }

    internal static SkillDef CreatePrimarySkillDef(
      SerializableEntityStateType state,
      string stateMachine,
      string skillNameToken,
      string skillDescriptionToken,
      Sprite skillIcon,
      bool agile)
    {
      SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
      instance.skillName = skillNameToken;
      instance.skillNameToken = skillNameToken;
      instance.skillDescriptionToken = skillDescriptionToken;
      instance.icon = skillIcon;
      instance.activationState = state;
      instance.activationStateMachineName = stateMachine;
      instance.baseMaxStock = 1;
      instance.baseRechargeInterval = 0.0f;
      instance.beginSkillCooldownOnSkillEnd = false;
      instance.canceledFromSprinting = false;
      instance.forceSprintDuringState = false;
      instance.fullRestockOnAssign = true;
      instance.interruptPriority = (InterruptPriority) 0;
      instance.resetCooldownTimerOnUse = false;
      instance.isCombatSkill = true;
      instance.mustKeyPress = false;
      instance.cancelSprintingOnActivation = !agile;
      instance.rechargeStock = 1;
      instance.requiredStock = 0;
      instance.stockToConsume = 0;
      if (agile)
        instance.keywordTokens = new string[1]
        {
          "KEYWORD_AGILE"
        };
      RiskOfRuinaMod.Modules.Skills.skillDefs.Add(instance);
      return instance;
    }

    internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
    {
      SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
      instance.skillName = skillDefInfo.skillName;
      instance.skillNameToken = skillDefInfo.skillNameToken;
      instance.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
      instance.icon = skillDefInfo.skillIcon;
      instance.activationState = skillDefInfo.activationState;
      instance.activationStateMachineName = skillDefInfo.activationStateMachineName;
      instance.baseMaxStock = skillDefInfo.baseMaxStock;
      instance.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
      instance.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
      instance.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
      instance.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
      instance.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
      instance.interruptPriority = skillDefInfo.interruptPriority;
      instance.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
      instance.isCombatSkill = skillDefInfo.isCombatSkill;
      instance.mustKeyPress = skillDefInfo.mustKeyPress;
      instance.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
      instance.rechargeStock = skillDefInfo.rechargeStock;
      instance.requiredStock = skillDefInfo.requiredStock;
      instance.stockToConsume = skillDefInfo.stockToConsume;
      instance.keywordTokens = skillDefInfo.keywordTokens;
      RiskOfRuinaMod.Modules.Skills.skillDefs.Add(instance);
      return instance;
    }

    internal static SkillDef CreateEGOSkillDef(SkillDefInfo skillDefInfo)
    {
      RedMistEGOSkillDef instance = ScriptableObject.CreateInstance<RedMistEGOSkillDef>();
      instance.skillName = skillDefInfo.skillName;
      instance.skillNameToken = skillDefInfo.skillNameToken;
      instance.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
      instance.icon = skillDefInfo.skillIcon;
      instance.activationState = skillDefInfo.activationState;
      instance.activationStateMachineName = skillDefInfo.activationStateMachineName;
      instance.baseMaxStock = skillDefInfo.baseMaxStock;
      instance.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
      instance.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
      instance.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
      instance.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
      instance.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
      instance.interruptPriority = skillDefInfo.interruptPriority;
      instance.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
      instance.isCombatSkill = skillDefInfo.isCombatSkill;
      instance.mustKeyPress = skillDefInfo.mustKeyPress;
      instance.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
      instance.rechargeStock = skillDefInfo.rechargeStock;
      instance.requiredStock = skillDefInfo.requiredStock;
      instance.stockToConsume = skillDefInfo.stockToConsume;
      instance.keywordTokens = skillDefInfo.keywordTokens;
      RiskOfRuinaMod.Modules.Skills.skillDefs.Add((SkillDef) instance);
      return (SkillDef) instance;
    }

    internal static SkillDef CreateTrackerSkillDef(SkillDefInfo skillDefInfo)
    {
      TargettedSkillDef instance = ScriptableObject.CreateInstance<TargettedSkillDef>();
      instance.skillName = skillDefInfo.skillName;
      instance.skillNameToken = skillDefInfo.skillNameToken;
      instance.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
      instance.icon = skillDefInfo.skillIcon;
      instance.activationState = skillDefInfo.activationState;
      instance.activationStateMachineName = skillDefInfo.activationStateMachineName;
      instance.baseMaxStock = skillDefInfo.baseMaxStock;
      instance.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
      instance.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
      instance.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
      instance.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
      instance.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
      instance.interruptPriority = skillDefInfo.interruptPriority;
      instance.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
      instance.isCombatSkill = skillDefInfo.isCombatSkill;
      instance.mustKeyPress = skillDefInfo.mustKeyPress;
      instance.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
      instance.rechargeStock = skillDefInfo.rechargeStock;
      instance.requiredStock = skillDefInfo.requiredStock;
      instance.stockToConsume = skillDefInfo.stockToConsume;
      instance.keywordTokens = skillDefInfo.keywordTokens;
      RiskOfRuinaMod.Modules.Skills.skillDefs.Add((SkillDef) instance);
      return (SkillDef) instance;
    }
  }
}
