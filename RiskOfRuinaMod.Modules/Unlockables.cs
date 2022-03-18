using System;
using System.Collections.Generic;
using System.Reflection;
using IL.RoR2;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{

	internal static class Unlockables
	{
		internal struct UnlockableInfo
		{
			internal string Name;

			internal Func<string> HowToUnlockString;

			internal Func<string> UnlockedString;

			internal int SortScore;
		}

		private static readonly HashSet<string> usedRewardIds = new HashSet<string>();

		internal static List<AchievementDef> achievementDefs = new List<AchievementDef>();

		internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();

		private static readonly List<(AchievementDef achDef, UnlockableDef unlockableDef, string unlockableName)> moddedUnlocks = new List<(AchievementDef, UnlockableDef, string)>();

		private static bool addingUnlockables;

		public static bool ableToAdd { get; private set; } = false;


		internal static UnlockableDef CreateNewUnlockable(UnlockableInfo unlockableInfo)
		{
			UnlockableDef val = ScriptableObject.CreateInstance<UnlockableDef>();
			val.nameToken = unlockableInfo.Name;
			val.set_cachedName(unlockableInfo.Name);
			val.set_getHowToUnlockString(unlockableInfo.HowToUnlockString);
			val.set_getUnlockedString(unlockableInfo.UnlockedString);
			val.set_sortScore(unlockableInfo.SortScore);
			return val;
		}

		public static UnlockableDef AddUnlockable<TUnlockable>(bool serverTracked) where TUnlockable : BaseAchievement, IModdedUnlockableDataProvider, new()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			TUnlockable val = new TUnlockable();
			string unlockableIdentifier = val.UnlockableIdentifier;
			if (!usedRewardIds.Add(unlockableIdentifier))
			{
				throw new InvalidOperationException("The unlockable identifier '" + unlockableIdentifier + "' is already used by another mod or the base game.");
			}
			AchievementDef val2 = new AchievementDef
			{
				identifier = val.AchievementIdentifier,
				unlockableRewardIdentifier = val.UnlockableIdentifier,
				prerequisiteAchievementIdentifier = val.PrerequisiteUnlockableIdentifier,
				nameToken = val.AchievementNameToken,
				descriptionToken = val.AchievementDescToken,
				achievedIcon = val.Sprite,
				type = val.GetType(),
				serverTrackerType = (serverTracked ? val.GetType() : null)
			};
			UnlockableInfo unlockableInfo = default(UnlockableInfo);
			unlockableInfo.Name = val.UnlockableIdentifier;
			unlockableInfo.HowToUnlockString = val.GetHowToUnlock;
			unlockableInfo.UnlockedString = val.GetUnlocked;
			unlockableInfo.SortScore = 200;
			UnlockableDef val3 = CreateNewUnlockable(unlockableInfo);
			unlockableDefs.Add(val3);
			achievementDefs.Add(val2);
			moddedUnlocks.Add((val2, val3, val.UnlockableIdentifier));
			if (!addingUnlockables)
			{
				addingUnlockables = true;
				AchievementManager.CollectAchievementDefs += CollectAchievementDefs;
				UnlockableCatalog.Init += Init_Il;
			}
			return val3;
		}

		public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target, out int index) where TDelegate : Delegate
		{
			index = cursor.EmitDelegate(target);
			return cursor;
		}

		public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target) where TDelegate : Delegate
		{
			int index;
			return cursor.CallDel_(target, out index);
		}

		private static void Init_Il(ILContext il)
		{
			new ILCursor(il).GotoNext(MoveType.AfterLabel, (Instruction x) => x.MatchCallOrCallvirt(typeof(UnlockableCatalog), "SetUnlockableDefs")).CallDel_(ArrayHelper.AppendDel(unlockableDefs));
		}

		private static void CollectAchievementDefs(ILContext il)
		{
			FieldInfo field = typeof(AchievementManager).GetField("achievementIdentifiers", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if ((object)field == null)
			{
				throw new NullReferenceException("Could not find field in AchievementManager");
			}
			ILCursor iLCursor = new ILCursor(il);
			iLCursor.GotoNext(MoveType.After, (Instruction x) => x.MatchEndfinally(), (Instruction x) => x.MatchLdloc(1));
			iLCursor.Emit(OpCodes.Ldarg_0);
			iLCursor.Emit(OpCodes.Ldsfld, field);
			iLCursor.EmitDelegate<Action<List<AchievementDef>, Dictionary<string, AchievementDef>, List<string>>>(EmittedDelegate);
			iLCursor.Emit(OpCodes.Ldloc_1);
			static void EmittedDelegate(List<AchievementDef> list, Dictionary<string, AchievementDef> map, List<string> identifiers)
			{
				ableToAdd = false;
				for (int i = 0; i < moddedUnlocks.Count; i++)
				{
					var (val, val2, text) = moddedUnlocks[i];
					if (val != null)
					{
						identifiers.Add(val.identifier);
						list.Add(val);
						map.Add(val.identifier, val);
					}
				}
			}
		}
	}
}