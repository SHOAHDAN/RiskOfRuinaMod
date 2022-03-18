using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Components
{

	internal class RedMistEGOSkillDef : SkillDef
	{
		protected class InstanceData : BaseSkillInstanceData
		{
			public RedMistEmotionComponent EGOComponent;
		}

		public float cost;

		public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
		{
			return (BaseSkillInstanceData)(object)new InstanceData
			{
				EGOComponent = ((Component)(object)skillSlot).GetComponent<RedMistEmotionComponent>()
			};
		}

		private static bool HasSufficientEGO([NotNull] GenericSkill skillSlot)
		{
			RedMistEmotionComponent eGOComponent = ((InstanceData)(object)skillSlot.get_skillInstanceData()).EGOComponent;
			return (Object)(object)eGOComponent != null && eGOComponent.currentEmotion >= (float)skillSlot.get_rechargeStock();
		}

		public override bool CanExecute([NotNull] GenericSkill skillSlot)
		{
			return HasSufficientEGO(skillSlot) && ((SkillDef)this).CanExecute(skillSlot);
		}

		public override bool IsReady([NotNull] GenericSkill skillSlot)
		{
			return ((SkillDef)this).IsReady(skillSlot) && HasSufficientEGO(skillSlot);
		}
	}
}