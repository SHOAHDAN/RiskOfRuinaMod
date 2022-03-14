// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Survivors.SurvivorBase
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
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

    internal string fullBodyName => this.bodyName + "Body";

    internal abstract ConfigEntry<bool> characterEnabled { get; set; }

    internal abstract UnlockableDef characterUnlockableDef { get; set; }

    internal abstract BodyInfo bodyInfo { get; set; }

    internal abstract int mainRendererIndex { get; set; }

    internal abstract CustomRendererInfo[] customRendererInfos { get; set; }

    internal abstract Type characterMainState { get; set; }

    internal abstract ItemDisplayRuleSet itemDisplayRuleSet { get; set; }

    internal abstract List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

    internal virtual void Initialize()
    {
      SurvivorBase.instance = this;
      this.InitializeCharacter();
    }

    internal virtual void InitializeCharacter()
    {
      this.characterEnabled = Config.CharacterEnableConfig(this.bodyName);
      if (!this.characterEnabled.Value)
        return;
      this.InitializeUnlockables();
      this.bodyPrefab = Prefabs.CreatePrefab(this.bodyName + "Body", "mdl" + this.bodyName, this.bodyInfo);
      this.bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new SerializableEntityStateType(this.characterMainState);
      Prefabs.SetupCharacterModel(this.bodyPrefab, this.customRendererInfos, this.mainRendererIndex);
      this.displayPrefab = Prefabs.CreateDisplayPrefab(this.bodyName + "Display", this.bodyPrefab, this.bodyInfo);
      Prefabs.RegisterNewSurvivor(this.bodyPrefab, this.displayPrefab, Color.grey, this.bodyName.ToUpper(), this.characterUnlockableDef, this.sortPosition);
      this.InitializeHitboxes();
      this.InitializeSkills();
      this.InitializeSkins();
      this.InitializeItemDisplays();
      this.InitializeDoppelganger();
      RiskOfRuinaPlugin.characterPrefab = this.bodyPrefab;
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

    internal virtual void InitializeDoppelganger() => Prefabs.CreateGenericDoppelganger(SurvivorBase.instance.bodyPrefab, this.bodyName + "MonsterMaster", "Merc");

    internal virtual void InitializeItemDisplays()
    {
      CharacterModel componentInChildren = this.bodyPrefab.GetComponentInChildren<CharacterModel>();
      this.itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
      ((Object) this.itemDisplayRuleSet).name = "idrs" + this.bodyName;
      componentInChildren.itemDisplayRuleSet = this.itemDisplayRuleSet;
    }

    internal virtual void SetItemDisplays()
    {
    }
  }
}
