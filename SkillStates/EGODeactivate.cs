// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGODeactivate
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGODeactivate : BaseSkillState
  {
    public static float baseDuration = 1f;
    private float duration;
    private RedMistEmotionComponent EGOController;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.EGOController = ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      int num = (int) Util.PlaySound("Play_Effect_Break", ((EntityState) this).gameObject);
      EffectManager.SpawnEffect(Assets.EGODeactivate, new EffectData()
      {
        origin = ((EntityState) this).characterBody.footPosition,
        scale = 1f
      }, false);
      ((EntityState) this).PlayAnimation("FullBody, Override", "BufferEmpty");
      if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalBlock))
        ((EntityState) this).skillLocator.utility.UnsetSkillOverride((object) ((EntityState) this).skillLocator.utility, RedMist.EGOBlock, (GenericSkill.SkillOverridePriority) 4);
      else if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalDodge))
        ((EntityState) this).skillLocator.utility.UnsetSkillOverride((object) ((EntityState) this).skillLocator.utility, RedMist.EGODodge, (GenericSkill.SkillOverridePriority) 4);
      ((EntityState) this).skillLocator.special.UnsetSkillOverride((object) ((EntityState) this).skillLocator.special, RedMist.HorizontalSlash, (GenericSkill.SkillOverridePriority) 4);
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit() => ((EntityState) this).OnExit();

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;
  }
}
