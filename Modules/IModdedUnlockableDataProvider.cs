// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.IModdedUnlockableDataProvider
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using System;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal interface IModdedUnlockableDataProvider
  {
    string AchievementIdentifier { get; }

    string UnlockableIdentifier { get; }

    string AchievementNameToken { get; }

    string PrerequisiteUnlockableIdentifier { get; }

    string UnlockableNameToken { get; }

    string AchievementDescToken { get; }

    Sprite Sprite { get; }

    Func<string> GetHowToUnlock { get; }

    Func<string> GetUnlocked { get; }
  }
}
