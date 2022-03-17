using System;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace RiskOfRuinaMod.Modules;

internal abstract class ModdedUnlockable : BaseAchievement, IModdedUnlockableDataProvider
{
	public abstract string AchievementIdentifier { get; }

	public abstract string UnlockableIdentifier { get; }

	public abstract string AchievementNameToken { get; }

	public abstract string PrerequisiteUnlockableIdentifier { get; }

	public abstract string UnlockableNameToken { get; }

	public abstract string AchievementDescToken { get; }

	public abstract Sprite Sprite { get; }

	public abstract Func<string> GetHowToUnlock { get; }

	public abstract Func<string> GetUnlocked { get; }

	public override bool wantsBodyCallbacks => ((BaseAchievement)this).get_wantsBodyCallbacks();

	public void Revoke()
	{
		if (((BaseAchievement)this).get_userProfile().HasAchievement(AchievementIdentifier))
		{
			((BaseAchievement)this).get_userProfile().RevokeAchievement(AchievementIdentifier);
		}
		((BaseAchievement)this).get_userProfile().RevokeUnlockable(UnlockableCatalog.GetUnlockableDef(UnlockableIdentifier));
	}

	public override void OnGranted()
	{
		((BaseAchievement)this).OnGranted();
	}

	public override void OnInstall()
	{
		((BaseAchievement)this).OnInstall();
	}

	public override void OnUninstall()
	{
		((BaseAchievement)this).OnUninstall();
	}

	public override float ProgressForAchievement()
	{
		return ((BaseAchievement)this).ProgressForAchievement();
	}

	public override BodyIndex LookUpRequiredBodyIndex()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return ((BaseAchievement)this).LookUpRequiredBodyIndex();
	}

	public override void OnBodyRequirementBroken()
	{
		((BaseAchievement)this).OnBodyRequirementBroken();
	}

	public override void OnBodyRequirementMet()
	{
		((BaseAchievement)this).OnBodyRequirementMet();
	}
}
