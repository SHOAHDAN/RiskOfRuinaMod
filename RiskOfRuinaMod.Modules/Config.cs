using BepInEx.Configuration;

namespace RiskOfRuinaMod.Modules;

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
		themeMusic = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Audio", "Theme Music"), defaultValue: true, new ConfigDescription("Set to false to disable theme music on Red Mist transformation.", null));
		snapLevel = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Audio", "Snap On Level Up"), defaultValue: true, new ConfigDescription("Set to false to disable snapping on level up.", null));
		arbiterSound = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Audio", "Arbiter Primary Attack"), defaultValue: true, new ConfigDescription("Set to false to make Arbiter's Primary less grating.", null));
		statRatio = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: Stats", "Stat Ratio"), 0f, new ConfigDescription("Alter this to change how much speed gets converted into damage. 1.0 for none, 0.0 for all.", null));
		moveSpeedMult = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: Stats", "Move Speed Ratio"), 1f, new ConfigDescription("Alter this to change how much 1% move speed is worth in damage.", null));
		attackSpeedMult = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: Stats", "Attack Speed Ratio"), 1f, new ConfigDescription("Alter this to change how much 1% attack speed is worth in damage.", null));
		sprintSpeedMult = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: Stats", "Sprint Speed Multiplier"), 0.2f, new ConfigDescription("Alter this to change how much 1 energy drink is worth for sprint speed.", null));
		emotionRatio = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: EGO", "EGO Per Hit"), 0.4f, new ConfigDescription("Alter this to change how much EGO is gained per hit.", null));
		emotionDecay = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: EGO", "EGO Decay"), 0.005f, new ConfigDescription("Alter this to change how fast EGO is lost.", null));
		EGOAgeRatio = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: EGO", "EGO Decay Increase"), 0.0005f, new ConfigDescription("Alter this to change how much EGO Decay increases per tick while in EGO.", null));
		redMistBuffDamage = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: EGO", "Buff Damage Increase"), 0.01f, new ConfigDescription("Alter this to change how much a stack of the EGO buff increases your damage, value is a percentage of your total damage.", null));
		redMistBuffMaintain = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: EGO", "Buff Maintain"), defaultValue: true, new ConfigDescription("Set this to false to have Red Mist's EGO stacks get cleared when EGO is lost.", null));
		iframeOverlay = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: Misc", "Iframe Overlay"), defaultValue: true, new ConfigDescription("Set to false to disable character overlay on IFrames.", null));
		redMistCoatShader = RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition("Red Mist :: Misc", "EGO Shader"), defaultValue: true, new ConfigDescription("Set to false to disable the usage of non-standard shaders for EGO.", null));
	}

	internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
	{
		return RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition(characterName, "Enabled"), defaultValue: true, new ConfigDescription("Set to false to disable this character", null));
	}

	internal static ConfigEntry<bool> ItemEnableConfig(string itemName)
	{
		return RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition(itemName, "Enabled"), defaultValue: true, new ConfigDescription("Set to false to disable this item", null));
	}

	internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
	{
		return RiskOfRuinaPlugin.instance.Config.Bind(new ConfigDefinition(characterName, "Enabled"), defaultValue: true, new ConfigDescription("Set to false to disable this enemy", null));
	}
}
