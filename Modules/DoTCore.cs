// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.DoTCore
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using R2API;
using R2API.Utils;
using RoR2;

namespace RiskOfRuinaMod.Modules
{
  [R2APISubmoduleDependency(new string[] {"DotAPI"})]
  internal class DoTCore
  {
    internal static DotController.DotIndex FairyIndex;

    public DoTCore() => this.RegisterDoTs();

    protected internal void RegisterDoTs() => DoTCore.FairyIndex = DotAPI.RegisterDotDef(new DotController.DotDef()
    {
      interval = 1f,
      damageCoefficient = 0.0f,
      damageColorIndex = (DamageColorIndex) 2,
      associatedBuff = Buffs.fairyDebuff
    }, (DotAPI.CustomDotBehaviour) null, (DotAPI.CustomDotVisual) null);
  }
}
