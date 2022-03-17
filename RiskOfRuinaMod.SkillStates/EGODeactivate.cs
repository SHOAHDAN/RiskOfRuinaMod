using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates;

internal class EGODeactivate : BaseSkillState
{
	public static float baseDuration = 1f;

	private float duration;

	private RedMistEmotionComponent EGOController;

	public override void OnEnter()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		((BaseState)this).OnEnter();
		EGOController = ((EntityState)this).get_gameObject().GetComponent<RedMistEmotionComponent>();
		Util.PlaySound("Play_Effect_Break", ((EntityState)this).get_gameObject());
		EffectData val = new EffectData();
		val.set_origin(((EntityState)this).get_characterBody().get_footPosition());
		val.scale = 1f;
		EffectManager.SpawnEffect(Assets.EGODeactivate, val, false);
		((EntityState)this).PlayAnimation("FullBody, Override", "BufferEmpty");
		if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalBlock)
		{
			((EntityState)this).get_skillLocator().utility.UnsetSkillOverride((object)((EntityState)this).get_skillLocator().utility, RedMist.EGOBlock, (SkillOverridePriority)4);
		}
		else if ((Object)(object)((EntityState)this).get_skillLocator().utility.get_baseSkill() == (Object)(object)RedMist.NormalDodge)
		{
			((EntityState)this).get_skillLocator().utility.UnsetSkillOverride((object)((EntityState)this).get_skillLocator().utility, RedMist.EGODodge, (SkillOverridePriority)4);
		}
		((EntityState)this).get_skillLocator().special.UnsetSkillOverride((object)((EntityState)this).get_skillLocator().special, RedMist.HorizontalSlash, (SkillOverridePriority)4);
		((EntityState)this).outer.SetNextStateToMain();
	}

	public override void OnExit()
	{
		((EntityState)this).OnExit();
	}

	public override InterruptPriority GetMinimumInterruptPriority()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (InterruptPriority)6;
	}
}
