// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.EmotionHUD
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

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
      this.hud = ((Component) this).GetComponent<HUD>();
      this.startColor = new Color(171f, 115f, 10f);
      this.endColor = new Color(236f, 82f, 0.0f);
    }

    private void FillGauge(float desiredFill)
    {
      if ((double) desiredFill > (double) this.currentFill)
      {
        this.currentFill += 15f * Time.deltaTime;
        if ((double) this.currentFill <= (double) desiredFill)
          return;
        this.currentFill = desiredFill;
      }
      else
      {
        this.currentFill -= 15f * Time.deltaTime;
        if ((double) this.currentFill < (double) desiredFill)
          this.currentFill = desiredFill;
      }
    }

    public void Update()
    {
      if (!Object.op_Implicit((Object) this.hud.targetBodyObject))
        return;
      RedMistEmotionComponent component = this.hud.targetBodyObject.GetComponent<RedMistEmotionComponent>();
      if (Object.op_Implicit((Object) component))
      {
        PlayerCharacterMasterController masterController = Object.op_Implicit((Object) this.hud.targetMaster) ? this.hud.targetMaster.playerCharacterMasterController : (PlayerCharacterMasterController) null;
        if (Object.op_Implicit((Object) this.emotionGauge))
        {
          this.emotionGauge.gameObject.SetActive(true);
          float desiredFill = component.currentEmotion / component.maxEmotion;
          float fillAmount = this.emotionFill.fillAmount;
          this.FillGauge(desiredFill);
          this.emotionFill.fillAmount = this.currentFill;
          float num1 = Mathf.Lerp(this.startColor.r, this.endColor.r, this.currentFill);
          float num2 = Mathf.Lerp(this.startColor.g, this.endColor.g, this.currentFill);
          float num3 = Mathf.Lerp(this.startColor.b, this.endColor.b, this.currentFill);
          Color cyan;
          // ISSUE: explicit constructor call
          ((Color) ref cyan).\u002Ector(num1, num2, num3);
          if ((double) this.currentFill >= 1.0)
            cyan = Color.cyan;
          ((Graphic) this.emotionFill).color = cyan;
        }
      }
      else if (Object.op_Implicit((Object) this.emotionGauge))
        this.emotionGauge.gameObject.SetActive(false);
    }
  }
}
