using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using EntityStates;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Survivors
{
	internal abstract class SurvivorBase
	{
		internal static SurvivorBase instance;

		internal abstract string bodyName { get; set; }

		internal abstract GameObject bodyPrefab { get; set; }

		internal abstract GameObject displayPrefab { get; set; }

		internal abstract float sortPosition { get; set; }

		internal string fullBodyName => bodyName + "Body";

		internal abstract ConfigEntry<bool> characterEnabled { get; set; }

		internal abstract UnlockableDef characterUnlockableDef { get; set; }

		internal abstract BodyInfo bodyInfo { get; set; }

		internal abstract int mainRendererIndex { get; set; }

		internal abstract CustomRendererInfo[] customRendererInfos { get; set; }

		internal abstract Type characterMainState { get; set; }

		internal abstract ItemDisplayRuleSet itemDisplayRuleSet { get; set; }

		internal abstract List<KeyAssetRuleGroup> itemDisplayRules { get; set; }

		internal virtual void Initialize()
		{
			instance = this;
			InitializeCharacter();
		}

		internal virtual void InitializeCharacter()
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			characterEnabled = Config.CharacterEnableConfig(bodyName);
			if (characterEnabled.Value)
			{
				InitializeUnlockables();
				bodyPrefab = Prefabs.CreatePrefab(bodyName + "Body", "mdl" + bodyName, bodyInfo);
				bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new SerializableEntityStateType(characterMainState);
				Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos, mainRendererIndex);
				displayPrefab = Prefabs.CreateDisplayPrefab(bodyName + "Display", bodyPrefab, bodyInfo);
				Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, Color.grey, bodyName.ToUpper(), characterUnlockableDef, sortPosition);
				InitializeHitboxes();
				InitializeSkills();
				InitializeSkins();
				InitializeItemDisplays();
				InitializeDoppelganger();
				RiskOfRuinaPlugin.characterPrefab = bodyPrefab;
			}
		}

		internal virtual void InitializeUnlockables()
		{
		}

		internal virtual void InitializeSkills()
		{
		}

		internal virtual void InitializeHitboxes()
		{
		}

		internal virtual void InitializeSkins()
		{
		}

		internal virtual void InitializeDoppelganger()
		{
			Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, bodyName + "MonsterMaster", "Merc");
		}

		internal virtual void InitializeItemDisplays()
		{
			CharacterModel componentInChildren = bodyPrefab.GetComponentInChildren<CharacterModel>();
			itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
			((UnityEngine.Object)(object)itemDisplayRuleSet).name = "idrs" + bodyName;
			componentInChildren.itemDisplayRuleSet = itemDisplayRuleSet;
		}

		internal virtual void SetItemDisplays()
		{
		}
	}
}