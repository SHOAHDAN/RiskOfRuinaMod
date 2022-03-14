// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Skins
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using On.RoR2;
using RoR2;
using System;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal static class Skins
  {
    internal static SkinDef CreateSkinDef(
      string skinName,
      Sprite skinIcon,
      CharacterModel.RendererInfo[] rendererInfos,
      SkinnedMeshRenderer mainRenderer,
      GameObject root)
    {
      return Skins.CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, (UnlockableDef) null);
    }

    internal static SkinDef CreateSkinDef(
      string skinName,
      Sprite skinIcon,
      CharacterModel.RendererInfo[] rendererInfos,
      SkinnedMeshRenderer mainRenderer,
      GameObject root,
      UnlockableDef unlockableDef)
    {
      Skins.SkinDefInfo skinDefInfo = new Skins.SkinDefInfo()
      {
        BaseSkins = Array.Empty<SkinDef>(),
        GameObjectActivations = new SkinDef.GameObjectActivation[0],
        Icon = skinIcon,
        MeshReplacements = new SkinDef.MeshReplacement[0],
        MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
        Name = skinName,
        NameToken = skinName,
        ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
        RendererInfos = rendererInfos,
        RootObject = root,
        UnlockableDef = unlockableDef
      };
      // ISSUE: method pointer
      SkinDef.Awake += new SkinDef.hook_Awake((object) null, __methodptr(DoNothing));
      SkinDef instance = ScriptableObject.CreateInstance<SkinDef>();
      instance.baseSkins = skinDefInfo.BaseSkins;
      instance.icon = skinDefInfo.Icon;
      instance.unlockableDef = skinDefInfo.UnlockableDef;
      instance.rootObject = skinDefInfo.RootObject;
      instance.rendererInfos = skinDefInfo.RendererInfos;
      instance.gameObjectActivations = skinDefInfo.GameObjectActivations;
      instance.meshReplacements = skinDefInfo.MeshReplacements;
      instance.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
      instance.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
      instance.nameToken = skinDefInfo.NameToken;
      ((Object) instance).name = skinDefInfo.Name;
      // ISSUE: method pointer
      SkinDef.Awake -= new SkinDef.hook_Awake((object) null, __methodptr(DoNothing));
      return instance;
    }

    private static void DoNothing(SkinDef.orig_Awake orig, SkinDef self)
    {
    }

    internal struct SkinDefInfo
    {
      internal SkinDef[] BaseSkins;
      internal Sprite Icon;
      internal string NameToken;
      internal UnlockableDef UnlockableDef;
      internal GameObject RootObject;
      internal CharacterModel.RendererInfo[] RendererInfos;
      internal SkinDef.MeshReplacement[] MeshReplacements;
      internal SkinDef.GameObjectActivation[] GameObjectActivations;
      internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
      internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
      internal string Name;
    }
  }
}
