// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.CameraParams
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal static class CameraParams
  {
    internal static CharacterCameraParams defaultCameraParamsRedMist;
    internal static CharacterCameraParams EGOActivateCameraParamsRedMist;
    internal static CharacterCameraParams EGOActivateOutCameraParamsRedMist;
    internal static CharacterCameraParams HorizontalSlashCameraParamsRedMist;
    internal static CharacterCameraParams defaultCameraParamsArbiter;
    internal static CharacterCameraParams channelCameraParamsArbiter;
    internal static CharacterCameraParams channelFullCameraParamsArbiter;

    internal static void InitializeParams()
    {
      CameraParams.defaultCameraParamsRedMist = CameraParams.NewCameraParams("ccpRedMist", 70f, 1.37f, new Vector3(0.0f, 0.75f, -10.5f));
      CameraParams.EGOActivateCameraParamsRedMist = CameraParams.NewCameraParams("ccpRedMistEGOActivate", 70f, 1.37f, new Vector3(0.0f, -1.2f, -6.5f));
      CameraParams.EGOActivateOutCameraParamsRedMist = CameraParams.NewCameraParams("ccpRedMistEGOActivateOut", 70f, 1.37f, new Vector3(0.0f, 0.75f, -12f));
      CameraParams.HorizontalSlashCameraParamsRedMist = CameraParams.NewCameraParams("ccpRedMistHorizontalSlash", 70f, 1.37f, new Vector3(0.0f, 0.75f, -15f));
      CameraParams.defaultCameraParamsArbiter = CameraParams.NewCameraParams("ccpArbiter", 70f, 1.37f, new Vector3(0.0f, 0.75f, -10.5f));
      CameraParams.channelCameraParamsArbiter = CameraParams.NewCameraParams("ccpArbiterSpellChannel", 70f, 1.37f, new Vector3(2f, 0.5f, -8f));
      CameraParams.channelFullCameraParamsArbiter = CameraParams.NewCameraParams("ccpArbiterSpellChannel", 70f, 1.37f, new Vector3(2f, 0.75f, -12f));
    }

    private static CharacterCameraParams NewCameraParams(
      string name,
      float pitch,
      float pivotVerticalOffset,
      Vector3 standardPosition)
    {
      return CameraParams.NewCameraParams(name, pitch, pivotVerticalOffset, standardPosition, 0.1f);
    }

    private static CharacterCameraParams NewCameraParams(
      string name,
      float pitch,
      float pivotVerticalOffset,
      Vector3 standardPosition,
      float wallCushion)
    {
      CharacterCameraParams instance = ScriptableObject.CreateInstance<CharacterCameraParams>();
      instance.maxPitch = pitch;
      instance.minPitch = -pitch;
      instance.pivotVerticalOffset = pivotVerticalOffset;
      instance.standardLocalCameraPos = standardPosition;
      instance.wallCushion = wallCushion;
      return instance;
    }
  }
}
