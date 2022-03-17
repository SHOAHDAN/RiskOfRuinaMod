using System.Collections.Generic;
using System.IO;
using System.Reflection;
using R2API;
using RoR2;
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
			bool flag = false;
			LoadAssetBundle();
			LoadSoundbank();
			PopulateAssets();
		}

		internal static void LoadAssetBundle()
		{
			if (mainAssetBundle == null)
			{
				using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RiskOfRuinaMod.ruinabundle");
				mainAssetBundle = AssetBundle.LoadFromStream(stream);
			}
			assetNames = mainAssetBundle.GetAllAssetNames();
		}

		internal static void LoadSoundbank()
		{
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RiskOfRuinaMod.ruina.bnk");
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			SoundAPI.SoundBanks.Add(array);
		}

		internal static void PopulateAssets()
		{
			if (!mainAssetBundle)
			{
				Debug.LogError("There is no AssetBundle to load assets from.");
				return;
			}
			pagePoof = LoadEffect("PagesExplosion", "Play_Battle_Dead");
			trackerPrefab = Resources.Load<GameObject>("Prefabs/HuntressTrackingIndicator").InstantiateClone("RedMistTrackerPrefab", registerNetwork: false);
			trackerPrefab.transform.Find("Core Pip").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
			trackerPrefab.transform.Find("Core Pip").localScale = new Vector3(0.15f, 0.15f, 0.15f);
			trackerPrefab.transform.Find("Core, Dark").gameObject.GetComponent<SpriteRenderer>().color = Color.black;
			trackerPrefab.transform.Find("Core, Dark").localScale = new Vector3(0.1f, 0.1f, 0.1f);
			SpriteRenderer[] componentsInChildren = trackerPrefab.transform.Find("Holder").gameObject.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in componentsInChildren)
			{
				if ((bool)spriteRenderer)
				{
					spriteRenderer.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
					spriteRenderer.color = Color.white;
				}
			}
			arbiterTrophy = mainAssetBundle.LoadAsset<GameObject>("mdlRedMistArm");
			liuBadge = mainAssetBundle.LoadAsset<GameObject>("mdlLiuBadge");
			moonlightStone = mainAssetBundle.LoadAsset<GameObject>("mdlMoonlightStone");
			prescript = mainAssetBundle.LoadAsset<GameObject>("mdlPrescript");
			blackTea = mainAssetBundle.LoadAsset<GameObject>("mdlTeaCup");
			udjatMask = mainAssetBundle.LoadAsset<GameObject>("mdlUdjatMask");
			workshopAmmo = mainAssetBundle.LoadAsset<GameObject>("mdlWorkshopAmmo");
			weddingRing = mainAssetBundle.LoadAsset<GameObject>("mdlWeddingRing");
			backwardsClock = mainAssetBundle.LoadAsset<GameObject>("mdlBackwardsClock");
			reverberation = mainAssetBundle.LoadAsset<GameObject>("mdlReverberation");
			swordSwingEffect = LoadEffect("RedMistSwordSwing", parentToTransform: true);
			EGOSwordSwingEffect = LoadEffect("RedMistEGOSwordSwing", parentToTransform: true);
			spearPierceEffect = LoadEffect("RedMistSpearPierce", parentToTransform: true);
			EGOSpearPierceEffect = LoadEffect("RedMistEGOSpearPierce", parentToTransform: true);
			HorizontalSwordSwingEffect = LoadEffect("RedMistHorizontalSwordSwing", parentToTransform: true);
			groundPoundEffect = LoadEffect("RedMistGroundPound");
			swordSpinEffect = LoadEffect("RedMistSpin", parentToTransform: true);
			swordSpinEffectTwo = LoadEffect("RedMistSpinTwo", parentToTransform: true);
			mistEffect = LoadEffect("MistEffect");
			swordHitEffect = LoadEffect("SwordHitEffect");
			blockEffect = LoadEffect("BlockEffect");
			counterBurst = LoadEffect("CounterBurst");
			argaliaSwordSwingEffect = LoadEffect("ArgaliaSwordSwing", parentToTransform: true);
			argaliaEGOSwordSwingEffect = LoadEffect("ArgaliaEGOSwordSwing", parentToTransform: true);
			argaliaSpearPierceEffect = LoadEffect("ArgaliaSpearPierce", parentToTransform: true);
			argaliaEGOSpearPierceEffect = LoadEffect("ArgaliaEGOSpearPierce", parentToTransform: true);
			argaliaHorizontalSwordSwingEffect = LoadEffect("ArgaliaHorizontalSwordSwing", parentToTransform: true);
			argaliaSwordSpinEffect = LoadEffect("ArgaliaSpin", parentToTransform: true);
			argaliaSwordSpinEffectTwo = LoadEffect("ArgaliaSpinTwo", parentToTransform: true);
			argaliaSwordHitEffect = LoadEffect("ArgaliaSwordHitEffect");
			argaliaGroundPoundEffect = LoadEffect("ArgaliaGroundPound");
			argaliaCounterBurst = LoadEffect("ArgaliaCounterBurst");
			if (Config.redMistCoatShader.Value)
			{
				afterimageSlash = LoadEffect("RedMistAfterimageSwing", "Play_Kali_EGO_Hori");
				afterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
				afterimageBlock = LoadEffect("RedMistAfterimageBlock", parentToTransform: true);
				afterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
				argaliaAfterimageSlash = LoadEffect("ArgaliaAfterimageSwing", "Play_Kali_EGO_Hori");
				argaliaAfterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
				argaliaAfterimageBlock = LoadEffect("ArgaliaAfterimageBlock", parentToTransform: true);
				argaliaAfterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
			}
			else
			{
				afterimageSlash = LoadEffect("RedMistAfterimageSwingFallback", "Play_Kali_EGO_Hori");
				afterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
				afterimageBlock = LoadEffect("RedMistAfterimageBlockFallback", parentToTransform: true);
				afterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
				argaliaAfterimageSlash = LoadEffect("ArgaliaAfterimageSwingFallback", "Play_Kali_EGO_Hori");
				argaliaAfterimageSlash.AddComponent<DestroyOnTimer>().duration = 0.75f;
				argaliaAfterimageBlock = LoadEffect("ArgaliaAfterimageBlockFallback", parentToTransform: true);
				argaliaAfterimageBlock.AddComponent<DestroyOnTimer>().duration = 0.5f;
			}
			phaseEffect = LoadEffect("PhaseEffect");
			argaliaPhaseEffect = LoadEffect("ArgaliaPhaseEffect");
			phaseMistEffect = LoadEffect("PhaseMistEffect", parentToTransform: true);
			EGOActivate = LoadEffect("TransformationBurst");
			argaliaEGOActivate = LoadEffect("ArgaliaTransformationBurst");
			EGODeactivate = LoadEffect("TransformationLost");
			swordHitSoundVert = CreateNetworkSoundEventDef("Play_Kali_Normal_Vert");
			swordHitSoundHori = CreateNetworkSoundEventDef("Play_Kali_Normal_Hori");
			swordHitSoundStab = CreateNetworkSoundEventDef("Play_Kali_Normal_Stab");
			swordHitEGOSoundVert = CreateNetworkSoundEventDef("Play_Kali_EGO_Vert");
			swordHitEGOSoundHori = CreateNetworkSoundEventDef("Play_Kali_EGO_Hori");
			swordHitEGOSoundStab = CreateNetworkSoundEventDef("Play_Kali_EGO_Stab");
			swordHitEGOSoundGRHorizontal = CreateNetworkSoundEventDef("Play_Kali_Special_Hori_Fin");
			shockwaveEffect = mainAssetBundle.LoadAsset<GameObject>("ShockwaveEffect");
			armSwingEffect = LoadEffect("ArbiterArmSwingEffect", parentToTransform: true);
			fairyProcEffect = LoadEffect("FairyProcEffect", "Play_Effect_Stun");
			if (Config.arbiterSound.Value)
			{
				fairyExplodeEffect = LoadEffect("FairyExplodeEffect", "Play_Binah_Fairy");
			}
			else
			{
				fairyExplodeEffect = LoadEffect("FairyExplodeEffect", "Play_Effect_Stun");
			}
			fairyHitEffect = LoadEffect("FairyHitEffect");
			fairyDeleteEffect = LoadEffect("FairyDeleteEffect", "Play_Effect_Stun");
			lockEffect5s = LoadEffect("LockEffect5s", "Play_Binah_Lock");
			lockEffect4s = LoadEffect("LockEffect4s", "Play_Binah_Lock");
			lockEffect3s = LoadEffect("LockEffect3s", "Play_Binah_Lock");
			lockEffect2s = LoadEffect("LockEffect2s", "Play_Binah_Lock");
			lockEffect1s = LoadEffect("LockEffect1s", "Play_Binah_Lock");
			lockEffectBreak = LoadEffect("LockEffectBreak", "Play_Binah_Lock");
			unlockEffect = LoadEffect("UnlockBurst", "Play_Binah_Chain");
			pillarImpactEffect = LoadEffect("PillarImpact");
			fairyTrail = mainAssetBundle.LoadAsset<GameObject>("FairyTrail");
			pillarSpear = mainAssetBundle.LoadAsset<GameObject>("PillarSpear");
			pillarObject = mainAssetBundle.LoadAsset<GameObject>("Pillar");
			fairyHitSound = CreateNetworkSoundEventDef("Play_Binah_Fairy");
		}

		private static GameObject CreateTracer(string originalTracerName, string newTracerName)
		{
			if (Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null)
			{
				return null;
			}
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName).InstantiateClone(newTracerName, registerNetwork: true);
			if (!(Object)(object)gameObject.GetComponent<EffectComponent>())
			{
				gameObject.AddComponent<EffectComponent>();
			}
			if (!(Object)(object)gameObject.GetComponent<VFXAttributes>())
			{
				gameObject.AddComponent<VFXAttributes>();
			}
			if (!(Object)(object)gameObject.GetComponent<NetworkIdentity>())
			{
				gameObject.AddComponent<NetworkIdentity>();
			}
			gameObject.GetComponent<Tracer>().speed = 250f;
			gameObject.GetComponent<Tracer>().length = 50f;
			AddNewEffectDef(gameObject);
			return gameObject;
		}

		internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
		{
			NetworkSoundEventDef val = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
			val.set_akId(AkSoundEngine.GetIDFromString(eventName));
			val.eventName = eventName;
			networkSoundEventDefs.Add(val);
			return val;
		}

		internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
		{
			if (!objectToConvert)
			{
				return;
			}
			MeshRenderer[] componentsInChildren = objectToConvert.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in componentsInChildren)
			{
				if ((bool)meshRenderer && (bool)meshRenderer.material)
				{
					meshRenderer.material.shader = hotpoo;
				}
			}
			SkinnedMeshRenderer[] componentsInChildren2 = objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren2)
			{
				if ((bool)skinnedMeshRenderer && (bool)skinnedMeshRenderer.material)
				{
					skinnedMeshRenderer.material.shader = hotpoo;
				}
			}
		}

		internal static RendererInfo[] SetupRendererInfos(GameObject obj)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
			RendererInfo[] array = (RendererInfo[])(object)new RendererInfo[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				array[i] = new RendererInfo
				{
					defaultMaterial = componentsInChildren[i].material,
					renderer = componentsInChildren[i],
					defaultShadowCastingMode = ShadowCastingMode.On,
					ignoreOverlays = false
				};
			}
			return array;
		}

		internal static Texture LoadCharacterIcon(string characterName)
		{
			return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
		}

		internal static GameObject LoadCrosshair(string crosshairName)
		{
			if (Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null)
			{
				return Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
			}
			return Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
		}

		private static GameObject LoadEffect(string resourceName)
		{
			return LoadEffect(resourceName, "", parentToTransform: false);
		}

		private static GameObject LoadEffect(string resourceName, string soundName)
		{
			return LoadEffect(resourceName, soundName, parentToTransform: false);
		}

		private static GameObject LoadEffect(string resourceName, bool parentToTransform)
		{
			return LoadEffect(resourceName, "", parentToTransform);
		}

		private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			for (int i = 0; i < assetNames.Length; i++)
			{
				if (assetNames[i].Contains(resourceName.ToLower()))
				{
					flag = true;
					i = assetNames.Length;
				}
			}
			if (!flag)
			{
				Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
				return null;
			}
			GameObject gameObject = mainAssetBundle.LoadAsset<GameObject>(resourceName);
			gameObject.AddComponent<DestroyOnTimer>().duration = 12f;
			gameObject.AddComponent<NetworkIdentity>();
			gameObject.AddComponent<VFXAttributes>().vfxPriority = (VFXPriority)2;
			EffectComponent val = gameObject.AddComponent<EffectComponent>();
			val.applyScale = false;
			val.effectIndex = (EffectIndex)(-1);
			val.parentToReferencedTransform = parentToTransform;
			val.positionAtReferencedTransform = true;
			val.soundName = soundName;
			AddNewEffectDef(gameObject, soundName);
			return gameObject;
		}

		private static void AddNewEffectDef(GameObject effectPrefab)
		{
			AddNewEffectDef(effectPrefab, "");
		}

		private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			EffectDef val = new EffectDef();
			val.set_prefab(effectPrefab);
			val.set_prefabEffectComponent(effectPrefab.GetComponent<EffectComponent>());
			val.set_prefabName(effectPrefab.name);
			val.set_prefabVfxAttributes(effectPrefab.GetComponent<VFXAttributes>());
			val.set_spawnSoundEventName(soundName);
			effectDefs.Add(val);
		}

		public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
		{
			if (!commandoMat)
			{
				commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;
			}
			Material material = Object.Instantiate(commandoMat);
			Material material2 = mainAssetBundle.LoadAsset<Material>(materialName);
			if (!material2)
			{
				Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
				return commandoMat;
			}
			material.name = materialName;
			material.SetColor("_Color", material2.GetColor("_Color"));
			material.SetTexture("_MainTex", material2.GetTexture("_MainTex"));
			material.SetColor("_EmColor", emissionColor);
			material.SetFloat("_EmPower", emission);
			material.SetTexture("_EmTex", material2.GetTexture("_EmissionMap"));
			material.SetFloat("_NormalStrength", normalStrength);
			return material;
		}

		public static Material CreateMaterial(string materialName)
		{
			return CreateMaterial(materialName, 0f);
		}

		public static Material CreateMaterial(string materialName, float emission)
		{
			return CreateMaterial(materialName, emission, Color.white);
		}

		public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
		{
			return CreateMaterial(materialName, emission, emissionColor, 0f);
		}
	}
}