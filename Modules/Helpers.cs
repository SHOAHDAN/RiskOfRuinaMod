// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Helpers
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using System;
using System.Collections.Generic;

namespace RiskOfRuinaMod.Modules
{
  internal static class Helpers
  {
    internal const string agilePrefix = "<style=cIsUtility>Agile</style> ";
    internal const string fairyPrefix = "<style=cIsUtility>Fairy</style> ";

    internal static string ScepterDescription(string desc) => "\n<color=#d299ff>SCEPTER: " + desc + "</color>";

    public static T[] Append<T>(ref T[] array, List<T> list)
    {
      int length = array.Length;
      int count = list.Count;
      Array.Resize<T>(ref array, length + count);
      list.CopyTo(array, length);
      return array;
    }

    public static Func<T[], T[]> AppendDel<T>(List<T> list) => (Func<T[], T[]>) (r => Helpers.Append<T>(ref r, list));
  }
}
