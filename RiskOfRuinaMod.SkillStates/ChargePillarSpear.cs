using EntityStates;
using RiskOfRuinaMod.SkillStates.BaseStates;
using UnityEngine;

namespace RiskOfRuinaMod.SkillStates;

public class ChargePillarSpear : BaseChargeSpellState
{
	private GameObject chargeEffect;

	private Vector3 originalScale;

	public override void OnEnter()
	{
		baseDuration = 2f;
		chargeEffectPrefab = null;
		chargeSoundString = "Play_Binah_Stone_Ready";
		crosshairOverridePrefab = Resources.Load<GameObject>("Prefabs/Crosshair/ToolbotGrenadeLauncherCrosshair");
		maxBloomRadius = 0.1f;
		minBloomRadius = 1f;
		base.OnEnter();
		ChildLocator modelChildLocator = ((EntityState)this).GetModelChildLocator();
		if ((bool)(Object)(object)modelChildLocator)
		{
			chargeEffect = modelChildLocator.FindChild("SpearSummon").gameObject;
			chargeEffect.SetActive(value: true);
			originalScale = chargeEffect.transform.localScale;
		}
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		chargeEffect.transform.localScale = originalScale * (1f + CalcCharge());
	}

	public override void OnExit()
	{
		base.OnExit();
		if ((bool)chargeEffect)
		{
			chargeEffect.transform.localScale = originalScale;
			chargeEffect.SetActive(value: false);
		}
	}

	protected override BaseThrowSpellState GetNextState()
	{
		return new ThrowPillarSpear();
	}
}
