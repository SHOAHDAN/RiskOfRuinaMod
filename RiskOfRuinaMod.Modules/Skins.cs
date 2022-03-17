using System;
using On.RoR2;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules;

internal static class Skins
{
	internal struct SkinDefInfo
	{
		internal SkinDef[] BaseSkins;

		internal Sprite Icon;

		internal string NameToken;

		internal UnlockableDef UnlockableDef;

		internal GameObject RootObject;

		internal RendererInfo[] RendererInfos;

		internal MeshReplacement[] MeshReplacements;

		internal GameObjectActivation[] GameObjectActivations;

		internal ProjectileGhostReplacement[] ProjectileGhostReplacements;

		internal MinionSkinReplacement[] MinionSkinReplacements;

		internal string Name;
	}

	internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root)
	{
		return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, null);
	}

	internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef)
	{
		SkinDefInfo skinDefInfo = default(SkinDefInfo);
		skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
		skinDefInfo.GameObjectActivations = (GameObjectActivation[])(object)new GameObjectActivation[0];
		skinDefInfo.Icon = skinIcon;
		skinDefInfo.MeshReplacements = (MeshReplacement[])(object)new MeshReplacement[0];
		skinDefInfo.MinionSkinReplacements = (MinionSkinReplacement[])(object)new MinionSkinReplacement[0];
		skinDefInfo.Name = skinName;
		skinDefInfo.NameToken = skinName;
		skinDefInfo.ProjectileGhostReplacements = (ProjectileGhostReplacement[])(object)new ProjectileGhostReplacement[0];
		skinDefInfo.RendererInfos = rendererInfos;
		skinDefInfo.RootObject = root;
		skinDefInfo.UnlockableDef = unlockableDef;
		SkinDefInfo skinDefInfo2 = skinDefInfo;
		SkinDef.Awake += DoNothing;
		SkinDef val = ScriptableObject.CreateInstance<SkinDef>();
		val.baseSkins = skinDefInfo2.BaseSkins;
		val.icon = skinDefInfo2.Icon;
		val.unlockableDef = skinDefInfo2.UnlockableDef;
		val.rootObject = skinDefInfo2.RootObject;
		val.rendererInfos = skinDefInfo2.RendererInfos;
		val.gameObjectActivations = skinDefInfo2.GameObjectActivations;
		val.meshReplacements = skinDefInfo2.MeshReplacements;
		val.projectileGhostReplacements = skinDefInfo2.ProjectileGhostReplacements;
		val.minionSkinReplacements = skinDefInfo2.MinionSkinReplacements;
		val.nameToken = skinDefInfo2.NameToken;
		((UnityEngine.Object)(object)val).name = skinDefInfo2.Name;
		SkinDef.Awake -= DoNothing;
		return val;
	}

	private static void DoNothing(SkinDef.orig_Awake orig, SkinDef self)
	{
	}
}
