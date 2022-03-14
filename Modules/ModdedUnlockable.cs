// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.ModdedUnlockable
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using RoR2.Achievements;
using System;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal abstract class ModdedUnlockable : BaseAchievement, IModdedUnlockableDataProvider
  {
    public void Revoke()
    {
      if (this.userProfile.HasAchievement(this.AchievementIdentifier))
        this.userProfile.RevokeAchievement(this.AchievementIdentifier);
      this.userProfile.RevokeUnlockable(UnlockableCatalog.GetUnlockableDef(this.UnlockableIdentifier));
    }

    public abstract string AchievementIdentifier { get; }

    public abstract string UnlockableIdentifier { get; }

    public abstract string AchievementNameToken { get; }

    public abstract string PrerequisiteUnlockableIdentifier { get; }

    public abstract string UnlockableNameToken { get; }

    public abstract string AchievementDescToken { get; }

    public abstract Sprite Sprite { get; }

    public abstract Func<string> GetHowToUnlock { get; }

    public abstract Func<string> GetUnlocked { get; }

    public virtual void OnGranted() => base.OnGranted();

    public virtual void OnInstall() => base.OnInstall();

    public virtual void OnUninstall() => base.OnUninstall();

    public virtual float ProgressForAchievement() => base.ProgressForAchievement();

    public virtual BodyIndex LookUpRequiredBodyIndex() => base.LookUpRequiredBodyIndex();

    public virtual void OnBodyRequirementBroken() => base.OnBodyRequirementBroken();

    public virtual void OnBodyRequirementMet() => base.OnBodyRequirementMet();

    public virtual bool wantsBodyCallbacks => base.wantsBodyCallbacks;
  }
}
