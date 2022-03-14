// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Music
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using IL.RoR2;
using MonoMod.Cil;

namespace RiskOfRuinaMod.Modules
{
  internal static class Music
  {
    internal static int musicSources = 0;

    internal static void Initialize() => MusicController.LateUpdate += Music.\u003C\u003Ec.\u003C\u003E9__1_0 ?? (Music.\u003C\u003Ec.\u003C\u003E9__1_0 = new ILContext.Manipulator((object) Music.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CInitialize\u003Eb__1_0)));
  }
}
