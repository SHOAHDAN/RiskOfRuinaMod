using System;
using System.Collections.Generic;
using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.SkillStates;
using RoR2;
using RoR2.Skills;
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
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			ScepterBasicAttack = CreatePrimarySkillDef(new SerializableEntityStateType(typeof(BasicStringStart)), "Body", prefix + "_REDMIST_BODY_PRIMARY_UPSTANDINGSLASH_NAME", prefix + "_REDMIST_BODY_PRIMARY_UPSTANDINGSLASH_DESCRIPTION", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistPrimaryIcon"), agile: false);
			skillDefs.Add(ScepterBasicAttack);
			States.AddSkill(typeof(ScepterChannelShockwave));
			States.AddSkill(typeof(ScepterCastShockwave));
			SkillDefInfo skillDefInfo = new SkillDefInfo();
			skillDefInfo.skillName = prefix + "_ARBITER_BODY_SPECIAL_SCEPTERSHOCKWAVE_NAME";
			skillDefInfo.skillNameToken = prefix + "_ARBITER_BODY_SPECIAL_SCEPTERSHOCKWAVE_NAME";
			skillDefInfo.skillDescriptionToken = prefix + "_ARBITER_BODY_SPECIAL_SCEPTERSHOCKWAVE_DESCRIPTION";
			skillDefInfo.skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterSpecialIconScepter");
			skillDefInfo.activationState = new SerializableEntityStateType(typeof(ScepterChannelShockwave));
			skillDefInfo.activationStateMachineName = "Weapon";
			skillDefInfo.baseMaxStock = 5;
			skillDefInfo.baseRechargeInterval = 10f;
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
			skillDefInfo.requiredStock = 5;
			skillDefInfo.stockToConsume = 5;
			scepterShockwaveDef = CreateSkillDef(skillDefInfo);
			skillDefs.Add(scepterShockwaveDef);
		}

		internal static void CreateSkillFamilies(GameObject targetPrefab)
		{
			GenericSkill[] componentsInChildren = targetPrefab.GetComponentsInChildren<GenericSkill>();
			foreach (GenericSkill obj in componentsInChildren)
			{
				UnityEngine.Object.DestroyImmediate((UnityEngine.Object)(object)obj);
			}
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			component.primary = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily val = ScriptableObject.CreateInstance<SkillFamily>();
			((UnityEngine.Object)(object)val).name = targetPrefab.name + "PrimaryFamily";
			val.variants = (Variant[])(object)new Variant[0];
			component.primary._skillFamily = val;
			component.secondary = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily val2 = ScriptableObject.CreateInstance<SkillFamily>();
			((UnityEngine.Object)(object)val2).name = targetPrefab.name + "SecondaryFamily";
			val2.variants = (Variant[])(object)new Variant[0];
			component.secondary._skillFamily = val2;
			component.utility = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily val3 = ScriptableObject.CreateInstance<SkillFamily>();
			((UnityEngine.Object)(object)val3).name = targetPrefab.name + "UtilityFamily";
			val3.variants = (Variant[])(object)new Variant[0];
			component.utility._skillFamily = val3;
			component.special = targetPrefab.AddComponent<GenericSkill>();
			SkillFamily val4 = ScriptableObject.CreateInstance<SkillFamily>();
			((UnityEngine.Object)(object)val4).name = targetPrefab.name + "SpecialFamily";
			val4.variants = (Variant[])(object)new Variant[0];
			component.special._skillFamily = val4;
			skillFamilies.Add(val);
			skillFamilies.Add(val2);
			skillFamilies.Add(val3);
			skillFamilies.Add(val4);
		}

		internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.primary.get_skillFamily();
			Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
			Variant[] variants = skillFamily.variants;
			int num = skillFamily.variants.Length - 1;
			Variant val = new Variant
			{
				skillDef = skillDef
			};
			((Variant)(ref val)).set_viewableNode(new Node(skillDef.skillNameToken, false, (Node)null));
			variants[num] = val;
		}

		internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.secondary.get_skillFamily();
			Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
			Variant[] variants = skillFamily.variants;
			int num = skillFamily.variants.Length - 1;
			Variant val = new Variant
			{
				skillDef = skillDef
			};
			((Variant)(ref val)).set_viewableNode(new Node(skillDef.skillNameToken, false, (Node)null));
			variants[num] = val;
		}

		internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				AddSecondarySkill(targetPrefab, skillDef);
			}
		}

		internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.utility.get_skillFamily();
			Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
			Variant[] variants = skillFamily.variants;
			int num = skillFamily.variants.Length - 1;
			Variant val = new Variant
			{
				skillDef = skillDef
			};
			((Variant)(ref val)).set_viewableNode(new Node(skillDef.skillNameToken, false, (Node)null));
			variants[num] = val;
		}

		internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				AddUtilitySkill(targetPrefab, skillDef);
			}
		}

		internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
			SkillFamily skillFamily = component.special.get_skillFamily();
			Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
			Variant[] variants = skillFamily.variants;
			int num = skillFamily.variants.Length - 1;
			Variant val = new Variant
			{
				skillDef = skillDef
			};
			((Variant)(ref val)).set_viewableNode(new Node(skillDef.skillNameToken, false, (Node)null));
			variants[num] = val;
		}

		internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
		{
			foreach (SkillDef skillDef in skillDefs)
			{
				AddSpecialSkill(targetPrefab, skillDef);
			}
		}

		internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			SkillDef val = ScriptableObject.CreateInstance<SkillDef>();
			val.skillName = skillNameToken;
			val.skillNameToken = skillNameToken;
			val.skillDescriptionToken = skillDescriptionToken;
			val.icon = skillIcon;
			val.activationState = state;
			val.activationStateMachineName = stateMachine;
			val.baseMaxStock = 1;
			val.baseRechargeInterval = 0f;
			val.beginSkillCooldownOnSkillEnd = false;
			val.canceledFromSprinting = false;
			val.forceSprintDuringState = false;
			val.fullRestockOnAssign = true;
			val.interruptPriority = (InterruptPriority)0;
			val.resetCooldownTimerOnUse = false;
			val.isCombatSkill = true;
			val.mustKeyPress = false;
			val.cancelSprintingOnActivation = !agile;
			val.rechargeStock = 1;
			val.requiredStock = 0;
			val.stockToConsume = 0;
			if (agile)
			{
				val.keywordTokens = new string[1] { "KEYWORD_AGILE" };
			}
			skillDefs.Add(val);
			return val;
		}

		internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			SkillDef val = ScriptableObject.CreateInstance<SkillDef>();
			val.skillName = skillDefInfo.skillName;
			val.skillNameToken = skillDefInfo.skillNameToken;
			val.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
			val.icon = skillDefInfo.skillIcon;
			val.activationState = skillDefInfo.activationState;
			val.activationStateMachineName = skillDefInfo.activationStateMachineName;
			val.baseMaxStock = skillDefInfo.baseMaxStock;
			val.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
			val.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
			val.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
			val.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
			val.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
			val.interruptPriority = skillDefInfo.interruptPriority;
			val.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
			val.isCombatSkill = skillDefInfo.isCombatSkill;
			val.mustKeyPress = skillDefInfo.mustKeyPress;
			val.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
			val.rechargeStock = skillDefInfo.rechargeStock;
			val.requiredStock = skillDefInfo.requiredStock;
			val.stockToConsume = skillDefInfo.stockToConsume;
			val.keywordTokens = skillDefInfo.keywordTokens;
			skillDefs.Add(val);
			return val;
		}

		internal static SkillDef CreateEGOSkillDef(SkillDefInfo skillDefInfo)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			RedMistEGOSkillDef redMistEGOSkillDef = ScriptableObject.CreateInstance<RedMistEGOSkillDef>();
			((SkillDef)redMistEGOSkillDef).skillName = skillDefInfo.skillName;
			((SkillDef)redMistEGOSkillDef).skillNameToken = skillDefInfo.skillNameToken;
			((SkillDef)redMistEGOSkillDef).skillDescriptionToken = skillDefInfo.skillDescriptionToken;
			((SkillDef)redMistEGOSkillDef).icon = skillDefInfo.skillIcon;
			((SkillDef)redMistEGOSkillDef).activationState = skillDefInfo.activationState;
			((SkillDef)redMistEGOSkillDef).activationStateMachineName = skillDefInfo.activationStateMachineName;
			((SkillDef)redMistEGOSkillDef).baseMaxStock = skillDefInfo.baseMaxStock;
			((SkillDef)redMistEGOSkillDef).baseRechargeInterval = skillDefInfo.baseRechargeInterval;
			((SkillDef)redMistEGOSkillDef).beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
			((SkillDef)redMistEGOSkillDef).canceledFromSprinting = skillDefInfo.canceledFromSprinting;
			((SkillDef)redMistEGOSkillDef).forceSprintDuringState = skillDefInfo.forceSprintDuringState;
			((SkillDef)redMistEGOSkillDef).fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
			((SkillDef)redMistEGOSkillDef).interruptPriority = skillDefInfo.interruptPriority;
			((SkillDef)redMistEGOSkillDef).resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
			((SkillDef)redMistEGOSkillDef).isCombatSkill = skillDefInfo.isCombatSkill;
			((SkillDef)redMistEGOSkillDef).mustKeyPress = skillDefInfo.mustKeyPress;
			((SkillDef)redMistEGOSkillDef).cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
			((SkillDef)redMistEGOSkillDef).rechargeStock = skillDefInfo.rechargeStock;
			((SkillDef)redMistEGOSkillDef).requiredStock = skillDefInfo.requiredStock;
			((SkillDef)redMistEGOSkillDef).stockToConsume = skillDefInfo.stockToConsume;
			((SkillDef)redMistEGOSkillDef).keywordTokens = skillDefInfo.keywordTokens;
			skillDefs.Add((SkillDef)(object)redMistEGOSkillDef);
			return (SkillDef)(object)redMistEGOSkillDef;
		}

		internal static SkillDef CreateTrackerSkillDef(SkillDefInfo skillDefInfo)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			TargettedSkillDef targettedSkillDef = ScriptableObject.CreateInstance<TargettedSkillDef>();
			((SkillDef)targettedSkillDef).skillName = skillDefInfo.skillName;
			((SkillDef)targettedSkillDef).skillNameToken = skillDefInfo.skillNameToken;
			((SkillDef)targettedSkillDef).skillDescriptionToken = skillDefInfo.skillDescriptionToken;
			((SkillDef)targettedSkillDef).icon = skillDefInfo.skillIcon;
			((SkillDef)targettedSkillDef).activationState = skillDefInfo.activationState;
			((SkillDef)targettedSkillDef).activationStateMachineName = skillDefInfo.activationStateMachineName;
			((SkillDef)targettedSkillDef).baseMaxStock = skillDefInfo.baseMaxStock;
			((SkillDef)targettedSkillDef).baseRechargeInterval = skillDefInfo.baseRechargeInterval;
			((SkillDef)targettedSkillDef).beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
			((SkillDef)targettedSkillDef).canceledFromSprinting = skillDefInfo.canceledFromSprinting;
			((SkillDef)targettedSkillDef).forceSprintDuringState = skillDefInfo.forceSprintDuringState;
			((SkillDef)targettedSkillDef).fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
			((SkillDef)targettedSkillDef).interruptPriority = skillDefInfo.interruptPriority;
			((SkillDef)targettedSkillDef).resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
			((SkillDef)targettedSkillDef).isCombatSkill = skillDefInfo.isCombatSkill;
			((SkillDef)targettedSkillDef).mustKeyPress = skillDefInfo.mustKeyPress;
			((SkillDef)targettedSkillDef).cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
			((SkillDef)targettedSkillDef).rechargeStock = skillDefInfo.rechargeStock;
			((SkillDef)targettedSkillDef).requiredStock = skillDefInfo.requiredStock;
			((SkillDef)targettedSkillDef).stockToConsume = skillDefInfo.stockToConsume;
			((SkillDef)targettedSkillDef).keywordTokens = skillDefInfo.keywordTokens;
			skillDefs.Add((SkillDef)(object)targettedSkillDef);
			return (SkillDef)(object)targettedSkillDef;
		}
	}
}