// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Config
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using System;

namespace RiskOfRuinaMod.Modules
{
  public static class Config
  {
    public static ConfigEntry<bool> themeMusic;
    public static ConfigEntry<bool> snapLevel;
    public static ConfigEntry<bool> arbiterSound;
    public static ConfigEntry<float> statRatio;
    public static ConfigEntry<float> moveSpeedMult;
    public static ConfigEntry<float> attackSpeedMult;
    public static ConfigEntry<float> sprintSpeedMult;
    public static ConfigEntry<float> emotionRatio;
    public static ConfigEntry<float> emotionDecay;
    public static ConfigEntry<float> EGOAgeRatio;
    public static ConfigEntry<float> redMistBuffDamage;
    public static ConfigEntry<bool> redMistBuffMaintain;
    public static ConfigEntry<bool> iframeOverlay;
    public static ConfigEntry<bool> redMistCoatShader;

    public static void ReadConfig()
    {
      Config.themeMusic = RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Audio", "Theme Music"), true, new ConfigDescription("Set to false to disable theme music on Red Mist transformation.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.snapLevel = RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Audio", "Snap On Level Up"), true, new ConfigDescription("Set to false to disable snapping on level up.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.arbiterSound = RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Audio", "Arbiter Primary Attack"), true, new ConfigDescription("Set to false to make Arbiter's Primary less grating.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.statRatio = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: Stats", "Stat Ratio"), 0.0f, new ConfigDescription("Alter this to change how much speed gets converted into damage. 1.0 for none, 0.0 for all.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.moveSpeedMult = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: Stats", "Move Speed Ratio"), 1f, new ConfigDescription("Alter this to change how much 1% move speed is worth in damage.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.attackSpeedMult = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: Stats", "Attack Speed Ratio"), 1f, new ConfigDescription("Alter this to change how much 1% attack speed is worth in damage.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.sprintSpeedMult = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: Stats", "Sprint Speed Multiplier"), 0.2f, new ConfigDescription("Alter this to change how much 1 energy drink is worth for sprint speed.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.emotionRatio = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: EGO", "EGO Per Hit"), 0.4f, new ConfigDescription("Alter this to change how much EGO is gained per hit.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.emotionDecay = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: EGO", "EGO Decay"), 0.005f, new ConfigDescription("Alter this to change how fast EGO is lost.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.EGOAgeRatio = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: EGO", "EGO Decay Increase"), 0.0005f, new ConfigDescription("Alter this to change how much EGO Decay increases per tick while in EGO.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.redMistBuffDamage = RiskOfRuinaPlugin.instance.Config.Bind<float>(new ConfigDefinition("Red Mist :: EGO", "Buff Damage Increase"), 0.01f, new ConfigDescription("Alter this to change how much a stack of the EGO buff increases your damage, value is a percentage of your total damage.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.redMistBuffMaintain = RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Red Mist :: EGO", "Buff Maintain"), true, new ConfigDescription("Set this to false to have Red Mist's EGO stacks get cleared when EGO is lost.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.iframeOverlay = RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Red Mist :: Misc", "Iframe Overlay"), true, new ConfigDescription("Set to false to disable character overlay on IFrames.", (AcceptableValueBase) null, Array.Empty<object>()));
      Config.redMistCoatShader = RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Red Mist :: Misc", "EGO Shader"), true, new ConfigDescription("Set to false to disable the usage of non-standard shaders for EGO.", (AcceptableValueBase) null, Array.Empty<object>()));
    }

    internal static ConfigEntry<bool> CharacterEnableConfig(string characterName) => RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character", (AcceptableValueBase) null, Array.Empty<object>()));

    internal static ConfigEntry<bool> ItemEnableConfig(string itemName) => RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition(itemName, "Enabled"), true, new ConfigDescription("Set to false to disable this item", (AcceptableValueBase) null, Array.Empty<object>()));

    internal static ConfigEntry<bool> EnemyEnableConfig(string characterName) => RiskOfRuinaPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy", (AcceptableValueBase) null, Array.Empty<object>()));
  }
}
