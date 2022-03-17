using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components;

public class RedMistStatTracker : NetworkBehaviour
{
	public CharacterBody characterBody;

	public bool argalia = false;

	public GameObject slashPrefab = Assets.swordSwingEffect;

	public GameObject piercePrefab = Assets.spearPierceEffect;

	public GameObject spinPrefab = Assets.swordSpinEffect;

	public GameObject spinPrefabTwo = Assets.swordSpinEffectTwo;

	public GameObject EGOSlashPrefab = Assets.EGOSwordSwingEffect;

	public GameObject EGOPiercePrefab = Assets.EGOSpearPierceEffect;

	public GameObject EGOHorizontalPrefab = Assets.HorizontalSwordSwingEffect;

	public GameObject EGOActivatePrefab = Assets.EGOActivate;

	public GameObject hitEffect = Assets.swordHitEffect;

	public GameObject phaseEffect = Assets.phaseEffect;

	public GameObject groundPoundEffect = Assets.groundPoundEffect;

	public GameObject afterimageSlash = Assets.afterimageSlash;

	public GameObject afterimageBlock = Assets.afterimageBlock;

	public GameObject counterBurst = Assets.counterBurst;

	public float totalAttackSpeed = 1.2f;

	public float totalMoveSpeed = 10f;

	public float lastAttackSpeed = 1.2f;

	public float lastMoveSpeed = 10f;

	public float modifiedAttackSpeed = 1.2f;

	public float modifiedMoveSpeed = 10f;

	public ParticleSystem mistEffect;

	public string musicName = "Play_Ruina_Boss_Music";

	private void Start()
	{
		characterBody = ((Component)this).GetComponent<CharacterBody>();
		ChildLocator componentInChildren = ((Component)this).gameObject.GetComponentInChildren<ChildLocator>();
		if ((bool)(Object)(object)componentInChildren)
		{
			mistEffect = componentInChildren.FindChild("BloodCloud").GetComponent<ParticleSystem>();
		}
		argalia = characterBody.skinIndex == RiskOfRuinaPlugin.argaliaSkinIndex;
		if (argalia)
		{
			musicName = "Play_ArgaliaMusic";
			slashPrefab = Assets.argaliaSwordSwingEffect;
			piercePrefab = Assets.argaliaSpearPierceEffect;
			EGOSlashPrefab = Assets.argaliaEGOSwordSwingEffect;
			EGOPiercePrefab = Assets.argaliaEGOSpearPierceEffect;
			EGOHorizontalPrefab = Assets.argaliaHorizontalSwordSwingEffect;
			EGOActivatePrefab = Assets.argaliaEGOActivate;
			hitEffect = Assets.argaliaSwordHitEffect;
			phaseEffect = Assets.argaliaPhaseEffect;
			groundPoundEffect = Assets.argaliaGroundPoundEffect;
			spinPrefab = Assets.argaliaSwordSpinEffect;
			spinPrefabTwo = Assets.argaliaSwordSpinEffectTwo;
			counterBurst = Assets.argaliaCounterBurst;
			afterimageBlock = Assets.argaliaAfterimageBlock;
			afterimageSlash = Assets.argaliaAfterimageSlash;
			if ((bool)(Object)(object)componentInChildren)
			{
				mistEffect = componentInChildren.FindChild("ArgaliaCloud").GetComponent<ParticleSystem>();
				componentInChildren.FindChild("ParticleHair").GetChild(0).gameObject.SetActive(value: false);
				componentInChildren.FindChild("ParticleHair").GetChild(1).gameObject.SetActive(value: true);
			}
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
