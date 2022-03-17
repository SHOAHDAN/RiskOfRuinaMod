using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules;

public static class Buffs
{
	internal static BuffDef EGOBuff;

	internal static BuffDef RedMistBuff;

	internal static BuffDef fairyDebuff;

	internal static BuffDef lockDebuff;

	internal static BuffDef lockResistBuff;

	internal static BuffDef feebleDebuff;

	internal static BuffDef strengthBuff;

	internal static BuffDef warpBuff;

	internal static BuffDef udjatBuff;

	internal static BuffDef reverbBuff;

	internal static List<BuffDef> buffDefs = new List<BuffDef>();

	internal static void RegisterBuffs()
	{
		EGOBuff = AddNewBuff("EGOBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texEGOBuffIcon"), Color.white, canStack: false, isDebuff: false);
		RedMistBuff = AddNewBuff("RedMistBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistBuffIcon"), Color.white, canStack: true, isDebuff: false);
		fairyDebuff = AddNewBuff("FairyDebuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texFairyDebuff"), Color.white, canStack: true, isDebuff: true);
		lockDebuff = AddNewBuff("LockDebuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texLockDebuff"), Color.white, canStack: false, isDebuff: true);
		lockResistBuff = AddNewBuff("LockResistance", Assets.mainAssetBundle.LoadAsset<Sprite>("texEnduringBuff"), Color.white, canStack: true, isDebuff: false);
		feebleDebuff = AddNewBuff("FeebleDebuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texFeebleDebuff"), Color.white, canStack: false, isDebuff: true);
		strengthBuff = AddNewBuff("StrengthBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texStrengenedBuff"), Color.white, canStack: false, isDebuff: false);
		warpBuff = AddNewBuff("WarpBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texChargeBuff"), Color.white, canStack: false, isDebuff: false);
		udjatBuff = AddNewBuff("UdjatBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texUdjatBuff"), Color.white, canStack: true, isDebuff: false);
		reverbBuff = AddNewBuff("ReverbBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texReverberationBuff"), Color.white, canStack: false, isDebuff: false);
	}

	internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
	{
		BuffDef val = ScriptableObject.CreateInstance<BuffDef>();
		((Object)(object)val).name = buffName;
		val.buffColor = buffColor;
		val.canStack = canStack;
		val.isDebuff = isDebuff;
		val.eliteDef = null;
		val.iconSprite = buffIcon;
		buffDefs.Add(val);
		return val;
	}
}
