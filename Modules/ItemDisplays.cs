// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.ItemDisplays
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{
  internal static class ItemDisplays
  {
    private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

    internal static void PopulateDisplays()
    {
      ItemDisplays.PopulateFromBody("Commando");
      ItemDisplays.PopulateFromBody("Croco");
      ItemDisplays.PopulateFromBody("Mage");
    }

    private static void PopulateFromBody(string bodyName)
    {
      foreach (ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetRuleGroup in ((Component) Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName + "Body").GetComponent<ModelLocator>().modelTransform).GetComponent<CharacterModel>().itemDisplayRuleSet.keyAssetRuleGroups)
      {
        foreach (ItemDisplayRule rule in keyAssetRuleGroup.displayRuleGroup.rules)
        {
          GameObject followerPrefab = rule.followerPrefab;
          if (Object.op_Implicit((Object) followerPrefab))
          {
            string lower = ((Object) followerPrefab).name?.ToLower();
            if (!ItemDisplays.itemDisplayPrefabs.ContainsKey(lower))
              ItemDisplays.itemDisplayPrefabs[lower] = followerPrefab;
          }
        }
      }
    }

    internal static GameObject LoadDisplay(string name) => ItemDisplays.itemDisplayPrefabs.ContainsKey(name.ToLower()) && Object.op_Implicit((Object) ItemDisplays.itemDisplayPrefabs[name.ToLower()]) ? ItemDisplays.itemDisplayPrefabs[name.ToLower()] : (GameObject) null;
  }
}
