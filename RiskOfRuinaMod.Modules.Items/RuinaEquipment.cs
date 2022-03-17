using BepInEx.Configuration;

namespace RiskOfRuinaMod.Modules.Items;

public abstract class RuinaEquipment
{
	internal abstract ConfigEntry<bool> equipEnabled { get; set; }

	internal abstract string equipName { get; set; }

	public virtual void Init()
	{
		equipEnabled = Config.ItemEnableConfig(equipName);
		if (equipEnabled.Value)
		{
			EquipSetup();
			HookSetup();
		}
	}

	public abstract void EquipSetup();

	public abstract void HookSetup();
}
