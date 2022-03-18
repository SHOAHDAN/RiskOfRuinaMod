using System.Collections.Generic;
using RoR2;
using UnityEngine;

namespace RiskOfRuinaMod.Modules
{

	internal static class ItemDisplays
	{
		private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

		internal static void PopulateDisplays()
		{
			PopulateFromBody("Commando");
			PopulateFromBody("Croco");
			PopulateFromBody("Mage");
		}

		private static void PopulateFromBody(string bodyName)
		{
			ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName + "Body").GetComponent<ModelLocator>().get_modelTransform()
				.GetComponent<CharacterModel>()
				.itemDisplayRuleSet;
			KeyAssetRuleGroup[] keyAssetRuleGroups = itemDisplayRuleSet.keyAssetRuleGroups;
			for (int i = 0; i < keyAssetRuleGroups.Length; i++)
			{
				ItemDisplayRule[] rules = keyAssetRuleGroups[i].displayRuleGroup.rules;
				for (int j = 0; j < rules.Length; j++)
				{
					GameObject followerPrefab = rules[j].followerPrefab;
					if ((bool)followerPrefab)
					{
						string key = followerPrefab.name?.ToLower();
						if (!itemDisplayPrefabs.ContainsKey(key))
						{
							itemDisplayPrefabs[key] = followerPrefab;
						}
					}
				}
			}
		}

		internal static GameObject LoadDisplay(string name)
		{
			if (itemDisplayPrefabs.ContainsKey(name.ToLower()) && (bool)itemDisplayPrefabs[name.ToLower()])
			{
				return itemDisplayPrefabs[name.ToLower()];
			}
			return null;
		}
	}
}