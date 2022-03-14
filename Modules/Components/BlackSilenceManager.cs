// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.BlackSilenceManager
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components
{
  public class BlackSilenceManager : NetworkBehaviour
  {
    public CharacterBody characterBody;
    public bool angelica = false;

    private void Start() => this.characterBody = ((Component) this).GetComponent<CharacterBody>();

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
