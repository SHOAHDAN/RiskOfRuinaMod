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
			defaultCameraParamsRedMist = NewCameraParams("ccpRedMist", 70f, 1.37f, new Vector3(0f, 0.75f, -10.5f));
			EGOActivateCameraParamsRedMist = NewCameraParams("ccpRedMistEGOActivate", 70f, 1.37f, new Vector3(0f, -1.2f, -6.5f));
			EGOActivateOutCameraParamsRedMist = NewCameraParams("ccpRedMistEGOActivateOut", 70f, 1.37f, new Vector3(0f, 0.75f, -12f));
			HorizontalSlashCameraParamsRedMist = NewCameraParams("ccpRedMistHorizontalSlash", 70f, 1.37f, new Vector3(0f, 0.75f, -15f));
			defaultCameraParamsArbiter = NewCameraParams("ccpArbiter", 70f, 1.37f, new Vector3(0f, 0.75f, -10.5f));
			channelCameraParamsArbiter = NewCameraParams("ccpArbiterSpellChannel", 70f, 1.37f, new Vector3(2f, 0.5f, -8f));
			channelFullCameraParamsArbiter = NewCameraParams("ccpArbiterSpellChannel", 70f, 1.37f, new Vector3(2f, 0.75f, -12f));
		}

		private static CharacterCameraParams NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition)
		{
			return NewCameraParams(name, pitch, pivotVerticalOffset, standardPosition, 0.1f);
		}

		private static CharacterCameraParams NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition, float wallCushion)
		{
			CharacterCameraParams val = ScriptableObject.CreateInstance<CharacterCameraParams>();
			val.maxPitch = pitch;
			val.minPitch = 0f - pitch;
			val.pivotVerticalOffset = pivotVerticalOffset;
			val.standardLocalCameraPos = standardPosition;
			val.wallCushion = wallCushion;
			return val;
		}
	}
}