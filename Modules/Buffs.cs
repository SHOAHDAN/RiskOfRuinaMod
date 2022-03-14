// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Buffs
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
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
      Buffs.EGOBuff = Buffs.AddNewBuff("EGOBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texEGOBuffIcon"), Color.white, false, false);
      Buffs.RedMistBuff = Buffs.AddNewBuff("RedMistBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texRedMistBuffIcon"), Color.white, true, false);
      Buffs.fairyDebuff = Buffs.AddNewBuff("FairyDebuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texFairyDebuff"), Color.white, true, true);
      Buffs.lockDebuff = Buffs.AddNewBuff("LockDebuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texLockDebuff"), Color.white, false, true);
      Buffs.lockResistBuff = Buffs.AddNewBuff("LockResistance", Assets.mainAssetBundle.LoadAsset<Sprite>("texEnduringBuff"), Color.white, true, false);
      Buffs.feebleDebuff = Buffs.AddNewBuff("FeebleDebuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texFeebleDebuff"), Color.white, false, true);
      Buffs.strengthBuff = Buffs.AddNewBuff("StrengthBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texStrengenedBuff"), Color.white, false, false);
      Buffs.warpBuff = Buffs.AddNewBuff("WarpBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texChargeBuff"), Color.white, false, false);
      Buffs.udjatBuff = Buffs.AddNewBuff("UdjatBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texUdjatBuff"), Color.white, true, false);
      Buffs.reverbBuff = Buffs.AddNewBuff("ReverbBuff", Assets.mainAssetBundle.LoadAsset<Sprite>("texReverberationBuff"), Color.white, false, false);
    }

    internal static BuffDef AddNewBuff(
      string buffName,
      Sprite buffIcon,
      Color buffColor,
      bool canStack,
      bool isDebuff)
    {
      BuffDef instance = ScriptableObject.CreateInstance<BuffDef>();
      ((Object) instance).name = buffName;
      instance.buffColor = buffColor;
      instance.canStack = canStack;
      instance.isDebuff = isDebuff;
      instance.eliteDef = (EliteDef) null;
      instance.iconSprite = buffIcon;
      Buffs.buffDefs.Add(instance);
      return instance;
    }
  }
}
