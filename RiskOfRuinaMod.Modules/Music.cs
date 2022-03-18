using System;
using IL.RoR2;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace RiskOfRuinaMod.Modules
{

	internal static class Music
	{
		internal static int musicSources = 0;

		internal static void Initialize()
		{
			MusicController.LateUpdate += delegate (ILContext il)
			{
				ILCursor iLCursor = new ILCursor(il);
				ILCursor iLCursor2 = iLCursor;
				iLCursor2.GotoNext((Instruction i) => i.MatchStloc(out var _));
				iLCursor.EmitDelegate<Func<bool, bool>>((bool b) => b || musicSources != 0);
			};
		}
	}
}