// Decompiled with JetBrains decompiler
// Type: SkillDefInfo
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using UnityEngine;

internal class SkillDefInfo
{
  public string skillName;
  public string skillNameToken;
  public string skillDescriptionToken;
  public Sprite skillIcon;
  public SerializableEntityStateType activationState;
  public string activationStateMachineName;
  public int baseMaxStock;
  public float baseRechargeInterval;
  public bool beginSkillCooldownOnSkillEnd;
  public bool canceledFromSprinting;
  public bool forceSprintDuringState;
  public bool fullRestockOnAssign;
  public InterruptPriority interruptPriority;
  public bool resetCooldownTimerOnUse;
  public bool isCombatSkill;
  public bool mustKeyPress;
  public bool cancelSprintingOnActivation;
  public int rechargeStock;
  public int requiredStock;
  public int stockToConsume;
  public string[] keywordTokens;
}
