// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Assets
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using R2API;
using RoR2;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace RiskOfRuinaMod.Modules
{
  internal static class Assets
  {
    internal static AssetBundle mainAssetBundle;
    internal static GameObject trackerPrefab;
    internal static GameObject pillarObject;
    internal static GameObject arbiterTrophy;
    internal static GameObject blackTea;
    internal static GameObject backwardsClock;
    internal static GameObject moonlightStone;
    internal static GameObject prescript;
    internal static GameObject liuBadge;
    internal static GameObject udjatMask;
    internal static GameObject workshopAmmo;
    internal static GameObject weddingRing;
    internal static GameObject reverberation;
    internal static GameObject fairyTrail;
    internal static GameObject pillarSpear;
    internal static GameObject swordHitEffect;
    internal static GameObject argaliaSwordHitEffect;
    internal static GameObject blockEffect;
    internal static GameObject mistEffect;
    internal static GameObject swordSwingEffect;
    internal static GameObject EGOSwordSwingEffect;
    internal static GameObject spearPierceEffect;
    internal static GameObject EGOSpearPierceEffect;
    internal static GameObject HorizontalSwordSwingEffect;
    internal static GameObject groundPoundEffect;
    internal static GameObject swordSpinEffect;
    internal static GameObject swordSpinEffectTwo;
    internal static GameObject counterBurst;
    internal static GameObject argaliaSwordSwingEffect;
    internal static GameObject argaliaEGOSwordSwingEffect;
    internal static GameObject argaliaSpearPierceEffect;
    internal static GameObject argaliaEGOSpearPierceEffect;
    internal static GameObject argaliaHorizontalSwordSwingEffect;
    internal static GameObject argaliaGroundPoundEffect;
    internal static GameObject argaliaSwordSpinEffect;
    internal static GameObject argaliaSwordSpinEffectTwo;
    internal static GameObject argaliaCounterBurst;
    internal static GameObject phaseEffect;
    internal static GameObject argaliaPhaseEffect;
    internal static GameObject phaseMistEffect;
    internal static GameObject afterimageSlash;
    internal static GameObject afterimageBlock;
    internal static GameObject argaliaAfterimageSlash;
    internal static GameObject argaliaAfterimageBlock;
    internal static GameObject EGOActivate;
    internal static GameObject argaliaEGOActivate;
    internal static GameObject EGODeactivate;
    internal static GameObject fairyProcEffect;
    internal static GameObject fairyExplodeEffect;
    internal static GameObject fairyDeleteEffect;
    internal static GameObject fairyHitEffect;
    internal static GameObject lockEffect5s;
    internal static GameObject lockEffect4s;
    internal static GameObject lockEffect3s;
    internal static GameObject lockEffect2s;
    internal static GameObject lockEffect1s;
    internal static GameObject lockEffectBreak;
    internal static GameObject unlockEffect;
    internal static GameObject pillarImpactEffect;
    internal static GameObject shockwaveEffect;
    internal static GameObject armSwingEffect;
    internal static GameObject pagePoof;
    internal static NetworkSoundEventDef swordHitSoundVert;
    internal static NetworkSoundEventDef swordHitSoundHori;
    internal static NetworkSoundEventDef swordHitSoundStab;
    internal static NetworkSoundEventDef swordHitEGOSoundVert;
    internal static NetworkSoundEventDef swordHitEGOSoundHori;
    internal static NetworkSoundEventDef swordHitEGOSoundStab;
    internal static NetworkSoundEventDef swordHitEGOSoundGRHorizontal;
    internal static NetworkSoundEventDef fairyHitSound;
    internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
    internal static List<EffectDef> effectDefs = new List<EffectDef>();
    internal static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");
    internal static Material commandoMat;
    private static string[] assetNames = new string[0];
    private const string assetbundleName = "ruinabundle";

    internal static void Initialize()
    {
      Assets.LoadAssetBundle();
      Assets.LoadSoundbank();
      Assets.PopulateAssets();
    }

    internal static void LoadAssetBundle()
    {
      if (Object.op_Equality((Object) Assets.mainAssetBundle, (Object) null))
      {
        using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RiskOfRuinaMod.ruinabundle"))
          Assets.mainAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream);
      }
      Assets.assetNames = Assets.mainAssetBundle.GetAllAssetNames();
    }

    internal static void LoadSoundbank()
    {
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RiskOfRuinaMod.ruina.bnk"))
      {
        byte[] buffer = new byte[manifestResourceStream.Length];
        manifestResourceStream.Read(buffer, 0, buffer.Length);
        int num = (int) SoundAPI.SoundBanks.Add(buffer);
      }
    }

    internal static void PopulateAssets()
    {
      if (!Object.op_Implicit((Object) Assets.mainAssetBundle))
      {
        Debug.LogError((object) "There is no AssetBundle to load assets from.");
      }
      else
      {
        Assets.pagePoof = Assets.LoadEffect("PagesExplosion", "Play_Battle_Dead");
        Assets.trackerPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/HuntressTrackingIndicator"), "RedMistTrackerPrefab", false);
        ((Component) Assets.trackerPrefab.transform.Find("Core Pip")).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Assets.trackerPrefab.transform.Find("Core Pip").localScale = new Vector3(0.15f, 0.15f, 0.15f);
        ((Component) Assets.trackerPrefab.transform.Find("Core, Dark")).gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        Assets.trackerPrefab.transform.Find("Core, Dark").localScale = new Vector3(0.1f, 0.1f, 0.1f);
        foreach (SpriteRenderer componentsInChild in ((Component) Assets.trackerPrefab.transform.Find("Holder")).gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
          if (Object.op_Implicit((Object) componentsInChild))
          {
            ((Component) componentsInChild).gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
            componentsInChild.color = Color.white;
          }
        }
        Assets.arbiterTrophy = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlRedMistArm");
        Assets.liuBadge = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlLiuBadge");
        Assets.moonlightStone = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlMoonlightStone");
        Assets.prescript = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlPrescript");
        Assets.blackTea = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlTeaCup");
        Assets.udjatMask = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlUdjatMask");
        Assets.workshopAmmo = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlWorkshopAmmo");
        Assets.weddingRing = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlWeddingRing");
        Assets.backwardsClock = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlBackwardsClock");
        Assets.reverberation = Assets.mainAssetBundle.LoadAsset<GameObject>("mdlReverberation");
        Assets.swordSwingEffect = Assets.LoadEffect("RedMistSwordSwing", true);
        Assets.EGOSwordSwingEffect = Assets.LoadEffect("RedMistEGOSwordSwing", true);
        Assets.spearPierceEffect = Assets.LoadEffect("RedMistSpearPierce", true);
        Assets.EGOSpearPierceEffect = Assets.LoadEffect("RedMistEGOSpearPierce", true);
        Assets.HorizontalSwordSwingEffect = Assets.LoadEffect("RedMistHorizontalSwordSwing", true);
        Assets.groundPoundEffect = Assets.LoadEffect("RedMistGroundPound");
        Assets.swordSpinEffect = Assets.LoadEffect("RedMistSpin", true);
        Assets.swordSpinEffectTwo = Assets.LoadEffect("RedMistSpinTwo", true);
        Assets.mistEffect = Assets.LoadEffect("MistEffect");
        Assets.swordHitEffect = Assets.LoadEffect("SwordHitEffect");
        Assets.blockEffect = Assets.LoadEffect("BlockEffect");
        Assets.counterBurst = Assets.LoadEffect("CounterBurst");
        Assets.argaliaSwordSwingEffect = Assets.LoadEffect("ArgaliaSwordSwing", true);
        Assets.argaliaEGOSwordSwingEffect = Assets.LoadEffect("ArgaliaEGOSwordSwing", true);
        Assets.argaliaSpearPierceEffect = Assets.LoadEffect("ArgaliaSpearPierce", true);
        Assets.argaliaEGOSpearPierceEffect = Assets.LoadEffect("ArgaliaEGOSpearPierce", true);
        Assets.argaliaHorizontalSwordSwingEffect = Assets.LoadEffect("ArgaliaHorizontalSwordSwing", true);
        Assets.argaliaSwordSpinEffect = Assets.LoadEffect("ArgaliaSpin", true);
        Assets.argaliaSwordSpinEffectTwo = Assets.LoadEffect("ArgaliaSpinTwo", true);
        Assets.argaliaSwordHitEffect = Assets.LoadEffect("ArgaliaSwordHitEffect");
        Assets.argaliaGroundPoundEffect = Assets.LoadEffect("ArgaliaGroundPound");
        Assets.argaliaCounterBurst = Assets.LoadEffect("ArgaliaCounterBurst");
        if (Config.redMistCoatShader.Value)
        {
          Assets.afterimageSlash = Assets.LoadEffect("RedMistAfterimageSwing", "Play_Kali_EGO_Hori");
          Assets.afterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
          Assets.afterimageBlock = Assets.LoadEffect("RedMistAfterimageBlock", true);
          Assets.afterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
          Assets.argaliaAfterimageSlash = Assets.LoadEffect("ArgaliaAfterimageSwing", "Play_Kali_EGO_Hori");
          Assets.argaliaAfterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
          Assets.argaliaAfterimageBlock = Assets.LoadEffect("ArgaliaAfterimageBlock", true);
          Assets.argaliaAfterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
        }
        else
        {
          Assets.afterimageSlash = Assets.LoadEffect("RedMistAfterimageSwingFallback", "Play_Kali_EGO_Hori");
          Assets.afterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
          Assets.afterimageBlock = Assets.LoadEffect("RedMistAfterimageBlockFallback", true);
          Assets.afterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
          Assets.argaliaAfterimageSlash = Assets.LoadEffect("ArgaliaAfterimageSwingFallback", "Play_Kali_EGO_Hori");
          Assets.argaliaAfterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
          Assets.argaliaAfterimageBlock = Assets.LoadEffect("ArgaliaAfterimageBlockFallback", true);
          Assets.argaliaAfterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
        }
        Assets.phaseEffect = Assets.LoadEffect("PhaseEffect");
        Assets.argaliaPhaseEffect = Assets.LoadEffect("ArgaliaPhaseEffect");
        Assets.phaseMistEffect = Assets.LoadEffect("PhaseMistEffect", true);
        Assets.EGOActivate = Assets.LoadEffect("TransformationBurst");
        Assets.argaliaEGOActivate = Assets.LoadEffect("ArgaliaTransformationBurst");
        Assets.EGODeactivate = Assets.LoadEffect("TransformationLost");
        Assets.swordHitSoundVert = Assets.CreateNetworkSoundEventDef("Play_Kali_Normal_Vert");
        Assets.swordHitSoundHori = Assets.CreateNetworkSoundEventDef("Play_Kali_Normal_Hori");
        Assets.swordHitSoundStab = Assets.CreateNetworkSoundEventDef("Play_Kali_Normal_Stab");
        Assets.swordHitEGOSoundVert = Assets.CreateNetworkSoundEventDef("Play_Kali_EGO_Vert");
        Assets.swordHitEGOSoundHori = Assets.CreateNetworkSoundEventDef("Play_Kali_EGO_Hori");
        Assets.swordHitEGOSoundStab = Assets.CreateNetworkSoundEventDef("Play_Kali_EGO_Stab");
        Assets.swordHitEGOSoundGRHorizontal = Assets.CreateNetworkSoundEventDef("Play_Kali_Special_Hori_Fin");
        Assets.shockwaveEffect = Assets.mainAssetBundle.LoadAsset<GameObject>("ShockwaveEffect");
        Assets.armSwingEffect = Assets.LoadEffect("ArbiterArmSwingEffect", true);
        Assets.fairyProcEffect = Assets.LoadEffect("FairyProcEffect", "Play_Effect_Stun");
        Assets.fairyExplodeEffect = !Config.arbiterSound.Value ? Assets.LoadEffect("FairyExplodeEffect", "Play_Effect_Stun") : Assets.LoadEffect("FairyExplodeEffect", "Play_Binah_Fairy");
        Assets.fairyHitEffect = Assets.LoadEffect("FairyHitEffect");
        Assets.fairyDeleteEffect = Assets.LoadEffect("FairyDeleteEffect", "Play_Effect_Stun");
        Assets.lockEffect5s = Assets.LoadEffect("LockEffect5s", "Play_Binah_Lock");
        Assets.lockEffect4s = Assets.LoadEffect("LockEffect4s", "Play_Binah_Lock");
        Assets.lockEffect3s = Assets.LoadEffect("LockEffect3s", "Play_Binah_Lock");
        Assets.lockEffect2s = Assets.LoadEffect("LockEffect2s", "Play_Binah_Lock");
        Assets.lockEffect1s = Assets.LoadEffect("LockEffect1s", "Play_Binah_Lock");
        Assets.lockEffectBreak = Assets.LoadEffect("LockEffectBreak", "Play_Binah_Lock");
        Assets.unlockEffect = Assets.LoadEffect("UnlockBurst", "Play_Binah_Chain");
        Assets.pillarImpactEffect = Assets.LoadEffect("PillarImpact");
        Assets.fairyTrail = Assets.mainAssetBundle.LoadAsset<GameObject>("FairyTrail");
        Assets.pillarSpear = Assets.mainAssetBundle.LoadAsset<GameObject>("PillarSpear");
        Assets.pillarObject = Assets.mainAssetBundle.LoadAsset<GameObject>("Pillar");
        Assets.fairyHitSound = Assets.CreateNetworkSoundEventDef("Play_Binah_Fairy");
      }
    }

    private static GameObject CreateTracer(
      string originalTracerName,
      string newTracerName)
    {
      if (Object.op_Equality((Object) Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), (Object) null))
        return (GameObject) null;
      GameObject effectPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);
      if (!Object.op_Implicit((Object) effectPrefab.GetComponent<EffectComponent>()))
        effectPrefab.AddComponent<EffectComponent>();
      if (!Object.op_Implicit((Object) effectPrefab.GetComponent<VFXAttributes>()))
        effectPrefab.AddComponent<VFXAttributes>();
      if (!Object.op_Implicit((Object) effectPrefab.GetComponent<NetworkIdentity>()))
        effectPrefab.AddComponent<NetworkIdentity>();
      effectPrefab.GetComponent<Tracer>().speed = 250f;
      effectPrefab.GetComponent<Tracer>().length = 50f;
      Assets.AddNewEffectDef(effectPrefab);
      return effectPrefab;
    }

    internal static NetworkSoundEventDef CreateNetworkSoundEventDef(
      string eventName)
    {
      NetworkSoundEventDef instance = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
      instance.akId = AkSoundEngine.GetIDFromString(eventName);
      instance.eventName = eventName;
      Assets.networkSoundEventDefs.Add(instance);
      return instance;
    }

    internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
    {
      if (!Object.op_Implicit((Object) objectToConvert))
        return;
      foreach (MeshRenderer componentsInChild in objectToConvert.GetComponentsInChildren<MeshRenderer>())
      {
        if (Object.op_Implicit((Object) componentsInChild) && Object.op_Implicit((Object) ((Renderer) componentsInChild).material))
          ((Renderer) componentsInChild).material.shader = Assets.hotpoo;
      }
      foreach (SkinnedMeshRenderer componentsInChild in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
      {
        if (Object.op_Implicit((Object) componentsInChild) && Object.op_Implicit((Object) ((Renderer) componentsInChild).material))
          ((Renderer) componentsInChild).material.shader = Assets.hotpoo;
      }
    }

    internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
    {
      MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
      CharacterModel.RendererInfo[] rendererInfoArray = new CharacterModel.RendererInfo[componentsInChildren.Length];
      for (int index = 0; index < componentsInChildren.Length; ++index)
        rendererInfoArray[index] = new CharacterModel.RendererInfo()
        {
          defaultMaterial = ((Renderer) componentsInChildren[index]).material,
          renderer = (Renderer) componentsInChildren[index],
          defaultShadowCastingMode = (ShadowCastingMode) 1,
          ignoreOverlays = false
        };
      return rendererInfoArray;
    }

    internal static Texture LoadCharacterIcon(string characterName) => Assets.mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");

    internal static GameObject LoadCrosshair(string crosshairName) => Object.op_Equality((Object) Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair"), (Object) null) ? Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair") : Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");

    private static GameObject LoadEffect(string resourceName) => Assets.LoadEffect(resourceName, "", false);

    private static GameObject LoadEffect(string resourceName, string soundName) => Assets.LoadEffect(resourceName, soundName, false);

    private static GameObject LoadEffect(string resourceName, bool parentToTransform) => Assets.LoadEffect(resourceName, "", parentToTransform);

    private static GameObject LoadEffect(
      string resourceName,
      string soundName,
      bool parentToTransform)
    {
      bool flag = false;
      for (int index = 0; index < Assets.assetNames.Length; ++index)
      {
        if (Assets.assetNames[index].Contains(resourceName.ToLower()))
        {
          flag = true;
          index = Assets.assetNames.Length;
        }
      }
      if (!flag)
      {
        Debug.LogError((object) ("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle"));
        return (GameObject) null;
      }
      GameObject effectPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>(resourceName);
      effectPrefab.AddComponent<DestroyOnTimer>().duration = 12f;
      effectPrefab.AddComponent<NetworkIdentity>();
      effectPrefab.AddComponent<VFXAttributes>().vfxPriority = (VFXAttributes.VFXPriority) 2;
      EffectComponent effectComponent = effectPrefab.AddComponent<EffectComponent>();
      effectComponent.applyScale = false;
      effectComponent.effectIndex = (EffectIndex) -1;
      effectComponent.parentToReferencedTransform = parentToTransform;
      effectComponent.positionAtReferencedTransform = true;
      effectComponent.soundName = soundName;
      Assets.AddNewEffectDef(effectPrefab, soundName);
      return effectPrefab;
    }

    private static void AddNewEffectDef(GameObject effectPrefab) => Assets.AddNewEffectDef(effectPrefab, "");

    private static void AddNewEffectDef(GameObject effectPrefab, string soundName) => Assets.effectDefs.Add(new EffectDef()
    {
      prefab = effectPrefab,
      prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>(),
      prefabName = ((Object) effectPrefab).name,
      prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>(),
      spawnSoundEventName = soundName
    });

    public static Material CreateMaterial(
      string materialName,
      float emission,
      Color emissionColor,
      float normalStrength)
    {
      if (!Object.op_Implicit((Object) Assets.commandoMat))
        Assets.commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;
      Material material1 = Object.Instantiate<Material>(Assets.commandoMat);
      Material material2 = Assets.mainAssetBundle.LoadAsset<Material>(materialName);
      if (!Object.op_Implicit((Object) material2))
      {
        Debug.LogError((object) ("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code"));
        return Assets.commandoMat;
      }
      ((Object) material1).name = materialName;
      material1.SetColor("_Color", material2.GetColor("_Color"));
      material1.SetTexture("_MainTex", material2.GetTexture("_MainTex"));
      material1.SetColor("_EmColor", emissionColor);
      material1.SetFloat("_EmPower", emission);
      material1.SetTexture("_EmTex", material2.GetTexture("_EmissionMap"));
      material1.SetFloat("_NormalStrength", normalStrength);
      return material1;
    }

    public static Material CreateMaterial(string materialName) => Assets.CreateMaterial(materialName, 0.0f);

    public static Material CreateMaterial(string materialName, float emission) => Assets.CreateMaterial(materialName, emission, Color.white);

    public static Material CreateMaterial(
      string materialName,
      float emission,
      Color emissionColor)
    {
      return Assets.CreateMaterial(materialName, emission, emissionColor, 0.0f);
    }
  }
}
