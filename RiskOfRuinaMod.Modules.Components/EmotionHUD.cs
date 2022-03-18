using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfRuinaMod.Modules.Components
{

	public class EmotionHUD : MonoBehaviour
	{
		public GameObject emotionGauge;

		public Image emotionFill;

		private Color startColor;

		private Color endColor;

		private float currentFill;

		private HUD hud;

		private void Awake()
		{
			hud = GetComponent<HUD>();
			startColor = new Color(171f, 115f, 10f);
			endColor = new Color(236f, 82f, 0f);
		}

		private void FillGauge(float desiredFill)
		{
			if (desiredFill > currentFill)
			{
				currentFill += 15f * Time.deltaTime;
				if (currentFill > desiredFill)
				{
					currentFill = desiredFill;
				}
			}
			else
			{
				currentFill -= 15f * Time.deltaTime;
				if (currentFill < desiredFill)
				{
					currentFill = desiredFill;
				}
			}
		}

		public void Update()
		{
			if (!hud.get_targetBodyObject())
			{
				return;
			}
			RedMistEmotionComponent component = hud.get_targetBodyObject().GetComponent<RedMistEmotionComponent>();
			if ((bool)(Object)(object)component)
			{
				PlayerCharacterMasterController val = (((Object)(object)hud.get_targetMaster()) ? hud.get_targetMaster().get_playerCharacterMasterController() : null);
				if ((bool)emotionGauge)
				{
					emotionGauge.gameObject.SetActive(value: true);
					float desiredFill = component.currentEmotion / component.maxEmotion;
					float fillAmount = emotionFill.fillAmount;
					FillGauge(desiredFill);
					emotionFill.fillAmount = currentFill;
					float r = Mathf.Lerp(startColor.r, endColor.r, currentFill);
					float g = Mathf.Lerp(startColor.g, endColor.g, currentFill);
					float b = Mathf.Lerp(startColor.b, endColor.b, currentFill);
					Color color = new Color(r, g, b);
					if (currentFill >= 1f)
					{
						color = Color.cyan;
					}
					emotionFill.color = color;
				}
			}
			else if ((bool)emotionGauge)
			{
				emotionGauge.gameObject.SetActive(value: false);
			}
		}
	}

}