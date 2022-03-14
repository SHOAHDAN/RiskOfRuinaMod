// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.RedMistStatTracker
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components
{
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
      this.characterBody = ((Component) this).GetComponent<CharacterBody>();
      ChildLocator componentInChildren = ((Component) this).gameObject.GetComponentInChildren<ChildLocator>();
      if (Object.op_Implicit((Object) componentInChildren))
        this.mistEffect = ((Component) componentInChildren.FindChild("BloodCloud")).GetComponent<ParticleSystem>();
      this.argalia = (int) this.characterBody.skinIndex == (int) RiskOfRuinaPlugin.argaliaSkinIndex;
      if (!this.argalia)
        return;
      this.musicName = "Play_ArgaliaMusic";
      this.slashPrefab = Assets.argaliaSwordSwingEffect;
      this.piercePrefab = Assets.argaliaSpearPierceEffect;
      this.EGOSlashPrefab = Assets.argaliaEGOSwordSwingEffect;
      this.EGOPiercePrefab = Assets.argaliaEGOSpearPierceEffect;
      this.EGOHorizontalPrefab = Assets.argaliaHorizontalSwordSwingEffect;
      this.EGOActivatePrefab = Assets.argaliaEGOActivate;
      this.hitEffect = Assets.argaliaSwordHitEffect;
      this.phaseEffect = Assets.argaliaPhaseEffect;
      this.groundPoundEffect = Assets.argaliaGroundPoundEffect;
      this.spinPrefab = Assets.argaliaSwordSpinEffect;
      this.spinPrefabTwo = Assets.argaliaSwordSpinEffectTwo;
      this.counterBurst = Assets.argaliaCounterBurst;
      this.afterimageBlock = Assets.argaliaAfterimageBlock;
      this.afterimageSlash = Assets.argaliaAfterimageSlash;
      if (Object.op_Implicit((Object) componentInChildren))
      {
        this.mistEffect = ((Component) componentInChildren.FindChild("ArgaliaCloud")).GetComponent<ParticleSystem>();
        ((Component) componentInChildren.FindChild("ParticleHair").GetChild(0)).gameObject.SetActive(false);
        ((Component) componentInChildren.FindChild("ParticleHair").GetChild(1)).gameObject.SetActive(true);
      }
    }

    private void UNetVersion()
    {
    }

    public virtual bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
      bool flag;
      return flag;
    }

    public virtual void OnDeserialize(NetworkReader reader, bool initialState)
    {
    }
  }
}
