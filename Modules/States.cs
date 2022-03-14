// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.States
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RiskOfRuinaMod.Modules.Misc;
using RiskOfRuinaMod.SkillStates;
using RiskOfRuinaMod.SkillStates.BaseStates;
using System;
using System.Collections.Generic;

namespace RiskOfRuinaMod.Modules
{
  public static class States
  {
    internal static List<Type> entityStates = new List<Type>();

    internal static void RegisterStates()
    {
      States.AddSkill(typeof (BaseDirectionalSkill));
      States.AddSkill(typeof (BaseMeleeAttack));
      States.AddSkill(typeof (BaseChargeSpellState));
      States.AddSkill(typeof (BaseChannelSpellState));
      States.AddSkill(typeof (BaseCastChanneledSpellState));
      States.AddSkill(typeof (EGOActivate));
      States.AddSkill(typeof (EGOActivateOut));
      States.AddSkill(typeof (EGODeactivate));
      States.AddSkill(typeof (EGOHorizontal));
      States.AddSkill(typeof (AirBackAttack));
      States.AddSkill(typeof (AirBackFallingAttack));
      States.AddSkill(typeof (AirBasicAttack));
      States.AddSkill(typeof (BackAttack));
      States.AddSkill(typeof (BasicAttack));
      States.AddSkill(typeof (BasicStringStart));
      States.AddSkill(typeof (Dodge));
      States.AddSkill(typeof (ForwardAttack));
      States.AddSkill(typeof (JumpAttack));
      States.AddSkill(typeof (JumpRisingAttack));
      States.AddSkill(typeof (Onrush));
      States.AddSkill(typeof (SideAttack));
      States.AddSkill(typeof (Block));
      States.AddSkill(typeof (BlockCounter));
      States.AddSkill(typeof (EGOAirBackAttack));
      States.AddSkill(typeof (EGOAirBackFallingAttack));
      States.AddSkill(typeof (EGOAirBasicAttack));
      States.AddSkill(typeof (EGOBackAttack));
      States.AddSkill(typeof (EGOBasicAttack));
      States.AddSkill(typeof (EGODodge));
      States.AddSkill(typeof (EGOForwardAttack));
      States.AddSkill(typeof (EGOJumpAttack));
      States.AddSkill(typeof (EGOJumpRisingAttack));
      States.AddSkill(typeof (EGOSideAttack));
      States.AddSkill(typeof (EGOBlock));
      States.AddSkill(typeof (EGOBlockCounter));
      States.AddSkill(typeof (FairyCombo));
      States.AddSkill(typeof (Lock));
      States.AddSkill(typeof (Unlock));
      States.AddSkill(typeof (CastPillar));
      States.AddSkill(typeof (ChannelPillar));
      States.AddSkill(typeof (ChargePillarSpear));
      States.AddSkill(typeof (ThrowPillarSpear));
      States.AddSkill(typeof (CastShockwave));
      States.AddSkill(typeof (ChannelShockwave));
      States.AddSkill(typeof (LockState));
    }

    internal static void AddSkill(Type t) => States.entityStates.Add(t);
  }
}
