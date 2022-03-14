// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.TargettedSkillDef
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RiskOfRuinaMod.Modules.Components
{
  internal class TargettedSkillDef : SkillDef
  {
    public float cost;

    public virtual SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) => (SkillDef.BaseSkillInstanceData) new TargettedSkillDef.InstanceData()
    {
      TrackerComponent = ((Component) skillSlot).GetComponent<TargetTracker>()
    };

    private static bool HasTarget([NotNull] GenericSkill skillSlot)
    {
      TargetTracker trackerComponent = ((TargettedSkillDef.InstanceData) skillSlot.skillInstanceData).TrackerComponent;
      return Object.op_Inequality((Object) trackerComponent, (Object) null) && Object.op_Implicit((Object) trackerComponent.GetTrackingTarget());
    }

    public virtual bool CanExecute([NotNull] GenericSkill skillSlot) => TargettedSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);

    public virtual bool IsReady([NotNull] GenericSkill skillSlot) => base.IsReady(skillSlot) && TargettedSkillDef.HasTarget(skillSlot);

    protected class InstanceData : SkillDef.BaseSkillInstanceData
    {
      public TargetTracker TrackerComponent;
    }
  }
}
