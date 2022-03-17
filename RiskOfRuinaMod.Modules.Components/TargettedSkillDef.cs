using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Components;

internal class TargettedSkillDef : SkillDef
{
	protected class InstanceData : BaseSkillInstanceData
	{
		public TargetTracker TrackerComponent;
	}

	public float cost;

	public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
	{
		return (BaseSkillInstanceData)(object)new InstanceData
		{
			TrackerComponent = ((Component)(object)skillSlot).GetComponent<TargetTracker>()
		};
	}

	private static bool HasTarget([NotNull] GenericSkill skillSlot)
	{
		TargetTracker trackerComponent = ((InstanceData)(object)skillSlot.get_skillInstanceData()).TrackerComponent;
		return (Object)(object)trackerComponent != null && (bool)(Object)(object)trackerComponent.GetTrackingTarget();
	}

	public override bool CanExecute([NotNull] GenericSkill skillSlot)
	{
		return HasTarget(skillSlot) && ((SkillDef)this).CanExecute(skillSlot);
	}

	public override bool IsReady([NotNull] GenericSkill skillSlot)
	{
		return ((SkillDef)this).IsReady(skillSlot) && HasTarget(skillSlot);
	}
}
