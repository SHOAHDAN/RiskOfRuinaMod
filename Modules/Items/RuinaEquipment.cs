// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.RuinaEquipment
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;

namespace RiskOfRuinaMod.Modules.Items
{
  public abstract class RuinaEquipment
  {
    internal abstract ConfigEntry<bool> equipEnabled { get; set; }

    internal abstract string equipName { get; set; }

    public virtual void Init()
    {
      this.equipEnabled = Config.ItemEnableConfig(this.equipName);
      if (!this.equipEnabled.Value)
        return;
      this.EquipSetup();
      this.HookSetup();
    }

    public abstract void EquipSetup();

    public abstract void HookSetup();
  }
}
