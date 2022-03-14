// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.RedMistEGOSkillDef
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Components
{
  internal class RedMistEGOSkillDef : SkillDef
  {
    public float cost;

    public virtual SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) => (SkillDef.BaseSkillInstanceData) new RedMistEGOSkillDef.InstanceData()
    {
      EGOComponent = ((Component) skillSlot).GetComponent<RedMistEmotionComponent>()
    };

    private static bool HasSufficientEGO([NotNull] GenericSkill skillSlot)
    {
      RedMistEmotionComponent egoComponent = ((RedMistEGOSkillDef.InstanceData) skillSlot.skillInstanceData).EGOComponent;
      return Object.op_Inequality((Object) egoComponent, (Object) null) && (double) egoComponent.currentEmotion >= (double) skillSlot.rechargeStock;
    }

    public virtual bool CanExecute([NotNull] GenericSkill skillSlot) => RedMistEGOSkillDef.HasSufficientEGO(skillSlot) && base.CanExecute(skillSlot);

    public virtual bool IsReady([NotNull] GenericSkill skillSlot) => base.IsReady(skillSlot) && RedMistEGOSkillDef.HasSufficientEGO(skillSlot);

    protected class InstanceData : SkillDef.BaseSkillInstanceData
    {
      public RedMistEmotionComponent EGOComponent;
    }
  }
}
