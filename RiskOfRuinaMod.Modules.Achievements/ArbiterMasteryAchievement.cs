using System;
using RoR2;
using RoR2.Achievements;
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


		public override Func<string> GetHowToUnlock { get; } = () => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[2]
		{
		Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME"),
		Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC")
		});


		public override Func<string> GetUnlocked { get; } = () => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[2]
		{
		Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME"),
		Language.GetString("COF_ARBITER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC")
		});


		public override BodyIndex LookUpRequiredBodyIndex()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return BodyCatalog.FindBodyIndex("ArbiterBody");
		}

		public void ClearCheck(Run run, RunReport runReport)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (run != null && runReport != null && (bool)(UnityEngine.Object)(object)runReport.gameEnding && runReport.gameEnding.isWin)
			{
				DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
				if (difficultyDef != null && difficultyDef.get_countsAsHardMode() && ((BaseAchievement)this).get_meetsBodyRequirement())
				{
					((BaseAchievement)this).Grant();
				}
			}
		}

		public override void OnInstall()
		{
			base.OnInstall();
			Run.add_onClientGameOverGlobal((Action<Run, RunReport>)ClearCheck);
		}

		public override void OnUninstall()
		{
			base.OnUninstall();
			Run.remove_onClientGameOverGlobal((Action<Run, RunReport>)ClearCheck);
		}
	}

}