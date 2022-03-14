// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Achievements.ArbiterMasteryAchievement
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using System;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Achievements
{
  internal class ArbiterMasteryAchievement : ModdedUnlockable
  {
    public override string AchievementIdentifier { get; } = "COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_ID";

    public override string UnlockableIdentifier { get; } = "COF_ARBITER_BODY_MASTERYUNLOCKABLE_REWARD_ID";

    public override string AchievementNameToken { get; } = "COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME";

    public override string PrerequisiteUnlockableIdentifier { get; } = "COF_ARBITER_BODY_UNLOCKABLE_REWARD_ID";

    public override string UnlockableNameToken { get; } = "COF_ARBITER_BODY_MASTERYUNLOCKABLE_UNLOCKABLE_NAME";

    public override string AchievementDescToken { get; } = "COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC";

    public override Sprite Sprite { get; } = Assets.mainAssetBundle.LoadAsset<Sprite>("texArbiterMasterySkin");

    public override Func<string> GetHowToUnlock { get; } = (Func<string>) (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
    {
      (object) Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME"),
      (object) Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC")
    }));

    public override Func<string> GetUnlocked { get; } = (Func<string>) (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
    {
      (object) Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME"),
      (object) Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC")
    }));

    public override BodyIndex LookUpRequiredBodyIndex() => BodyCatalog.FindBodyIndex("ArbiterBody");

    public void ClearCheck(Run run, RunReport runReport)
    {
      if (run == null || runReport == null || !Object.op_Implicit((Object) runReport.gameEnding) || !runReport.gameEnding.isWin)
        return;
      DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
      if (difficultyDef != null && difficultyDef.countsAsHardMode && this.meetsBodyRequirement)
        this.Grant();
    }

    public override void OnInstall()
    {
      base.OnInstall();
      Run.onClientGameOverGlobal += new Action<Run, RunReport>(this.ClearCheck);
    }

    public override void OnUninstall()
    {
      base.OnUninstall();
      Run.onClientGameOverGlobal -= new Action<Run, RunReport>(this.ClearCheck);
    }
  }
}
