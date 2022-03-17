using R2API;
using R2API.Utils;
using RoR2;

namespace RiskOfRuinaMod.Modules;

[R2APISubmoduleDependency(new string[] { "DotAPI" })]
internal class DoTCore
{
	internal static DotIndex FairyIndex;

	public DoTCore()
	{
		RegisterDoTs();
	}

	protected internal void RegisterDoTs()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		DotDef dotDef = new DotDef
		{
			interval = 1f,
			damageCoefficient = 0f,
			damageColorIndex = (DamageColorIndex)2,
			associatedBuff = Buffs.fairyDebuff
		};
		FairyIndex = DotAPI.RegisterDotDef(dotDef);
	}
}
