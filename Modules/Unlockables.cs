// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Unlockables
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using IL.RoR2;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Achievements;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal static class Unlockables
  {
    private static readonly HashSet<string> usedRewardIds = new HashSet<string>();
    internal static List<AchievementDef> achievementDefs = new List<AchievementDef>();
    internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();
    private static readonly List<(AchievementDef achDef, UnlockableDef unlockableDef, string unlockableName)> moddedUnlocks = new List<(AchievementDef, UnlockableDef, string)>();
    private static bool addingUnlockables;

    public static bool ableToAdd { get; private set; } = false;

    internal static UnlockableDef CreateNewUnlockable(
      Unlockables.UnlockableInfo unlockableInfo)
    {
      UnlockableDef instance = ScriptableObject.CreateInstance<UnlockableDef>();
      instance.nameToken = unlockableInfo.Name;
      instance.cachedName = unlockableInfo.Name;
      instance.getHowToUnlockString = unlockableInfo.HowToUnlockString;
      instance.getUnlockedString = unlockableInfo.UnlockedString;
      instance.sortScore = unlockableInfo.SortScore;
      return instance;
    }

    public static UnlockableDef AddUnlockable<TUnlockable>(bool serverTracked) where TUnlockable : BaseAchievement, IModdedUnlockableDataProvider, new()
    {
      TUnlockable unlockable = new TUnlockable();
      string unlockableIdentifier = unlockable.UnlockableIdentifier;
      if (!Unlockables.usedRewardIds.Add(unlockableIdentifier))
        throw new InvalidOperationException("The unlockable identifier '" + unlockableIdentifier + "' is already used by another mod or the base game.");
      AchievementDef achievementDef = new AchievementDef()
      {
        identifier = unlockable.AchievementIdentifier,
        unlockableRewardIdentifier = unlockable.UnlockableIdentifier,
        prerequisiteAchievementIdentifier = unlockable.PrerequisiteUnlockableIdentifier,
        nameToken = unlockable.AchievementNameToken,
        descriptionToken = unlockable.AchievementDescToken,
        achievedIcon = unlockable.Sprite,
        type = unlockable.GetType(),
        serverTrackerType = serverTracked ? unlockable.GetType() : (Type) null
      };
      UnlockableDef newUnlockable = Unlockables.CreateNewUnlockable(new Unlockables.UnlockableInfo()
      {
        Name = unlockable.UnlockableIdentifier,
        HowToUnlockString = unlockable.GetHowToUnlock,
        UnlockedString = unlockable.GetUnlocked,
        SortScore = 200
      });
      Unlockables.unlockableDefs.Add(newUnlockable);
      Unlockables.achievementDefs.Add(achievementDef);
      Unlockables.moddedUnlocks.Add((achievementDef, newUnlockable, unlockable.UnlockableIdentifier));
      if (!Unlockables.addingUnlockables)
      {
        Unlockables.addingUnlockables = true;
        // ISSUE: method pointer
        AchievementManager.CollectAchievementDefs += new ILContext.Manipulator((object) null, __methodptr(CollectAchievementDefs));
        // ISSUE: method pointer
        UnlockableCatalog.Init += new ILContext.Manipulator((object) null, __methodptr(Init_Il));
      }
      return newUnlockable;
    }

    public static ILCursor CallDel_<TDelegate>(
      this ILCursor cursor,
      TDelegate target,
      out int index)
      where TDelegate : Delegate
    {
      index = cursor.EmitDelegate<TDelegate>(target);
      return cursor;
    }

    public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target) where TDelegate : Delegate => cursor.CallDel_<TDelegate>(target, out int _);

    private static void Init_Il(ILContext il) => new ILCursor(il).GotoNext((MoveType) 1, new Func<Instruction, bool>[1]
    {
      (Func<Instruction, bool>) (x => ILPatternMatchingExt.MatchCallOrCallvirt(x, typeof (UnlockableCatalog), "SetUnlockableDefs"))
    }).CallDel_<Func<UnlockableDef[], UnlockableDef[]>>(ArrayHelper.AppendDel<UnlockableDef>(Unlockables.unlockableDefs));

    private static void CollectAchievementDefs(ILContext il)
    {
      FieldInfo field = typeof (AchievementManager).GetField("achievementIdentifiers", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if ((object) field == null)
        throw new NullReferenceException("Could not find field in AchievementManager");
      ILCursor ilCursor = new ILCursor(il);
      ilCursor.GotoNext((MoveType) 2, new Func<Instruction, bool>[2]
      {
        (Func<Instruction, bool>) (x => ILPatternMatchingExt.MatchEndfinally(x)),
        (Func<Instruction, bool>) (x => ILPatternMatchingExt.MatchLdloc(x, 1))
      });
      ilCursor.Emit(OpCodes.Ldarg_0);
      ilCursor.Emit(OpCodes.Ldsfld, field);
      ilCursor.EmitDelegate<Action<List<AchievementDef>, Dictionary<string, AchievementDef>, List<string>>>(new Action<List<AchievementDef>, Dictionary<string, AchievementDef>, List<string>>(EmittedDelegate));
      ilCursor.Emit(OpCodes.Ldloc_1);

      static void EmittedDelegate(
        List<AchievementDef> list,
        Dictionary<string, AchievementDef> map,
        List<string> identifiers)
      {
        Unlockables.ableToAdd = false;
        for (int index = 0; index < Unlockables.moddedUnlocks.Count; ++index)
        {
          (AchievementDef achDef, UnlockableDef _, string _) = Unlockables.moddedUnlocks[index];
          if (achDef != null)
          {
            identifiers.Add(achDef.identifier);
            list.Add(achDef);
            map.Add(achDef.identifier, achDef);
          }
        }
      }
    }

    internal struct UnlockableInfo
    {
      internal string Name;
      internal Func<string> HowToUnlockString;
      internal Func<string> UnlockedString;
      internal int SortScore;
    }
  }
}
