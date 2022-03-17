using BepInEx.Configuration;
using EntityStates;
using On.RoR2;
using R2API;
using RiskOfRuinaMod.Modules.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items;

internal class ArbitersTrophy : RuinaItem
{
	public ItemDef itemDef;

	public float procChance = 1f;

	public float stackChance = 1f;

	internal override ConfigEntry<bool> itemEnabled { get; set; }

	internal override string itemName { get; set; } = "RuinaArbitersTrophy";


	public override void ItemSetup()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		itemDef = ScriptableObject.CreateInstance<ItemDef>();
		((Object)(object)itemDef).name = itemName;
		itemDef.tier = (ItemTier)2;
		itemDef.pickupModelPrefab = Assets.arbiterTrophy;
		itemDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaArbitersTrophy");
		itemDef.nameToken = "ARBITERTROPHY_NAME";
		itemDef.pickupToken = "ARBITERTROPHY_PICKUP";
		itemDef.descriptionToken = "ARBITERTROPHY_DESC";
		itemDef.loreToken = "ARBITERTROPHY_LORE";
		itemDef.tags = (ItemTag[])(object)new ItemTag[1] { (ItemTag)3 };
		ItemDisplayRule[] itemDisplayRules = (ItemDisplayRule[])(object)new ItemDisplayRule[0];
		CustomItem item = new CustomItem(itemDef, itemDisplayRules);
		ItemAPI.Add(item);
	}

	public override void HookSetup()
	{
		GlobalEventManager.OnHitEnemy += GlobalEvent_OnHitEnemy;
	}

	private void GlobalEvent_OnHitEnemy(GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		GameObject attacker = damageInfo.attacker;
		if ((bool)(Object)(object)self && (bool)attacker)
		{
			CharacterBody component = attacker.GetComponent<CharacterBody>();
			CharacterBody component2 = victim.GetComponent<CharacterBody>();
			if (component.get_teamComponent().get_teamIndex() != component2.get_teamComponent().get_teamIndex())
			{
				CharacterMaster master = component.get_master();
				if ((bool)(Object)(object)master)
				{
					int itemCount = component.get_inventory().GetItemCount(ItemCatalog.FindItemIndex(itemName));
					if (itemCount > 0)
					{
						float num = procChance + stackChance * (float)(itemCount - 1);
						if (Util.CheckRoll(num * damageInfo.procCoefficient, master))
						{
							int buffCount = component2.GetBuffCount(Buffs.lockResistBuff);
							if (buffCount <= 4 && component2.GetBuffCount(Buffs.lockDebuff) == 0)
							{
								if (NetworkServer.get_active())
								{
									component2.AddTimedBuff(Buffs.lockDebuff, 5f - (float)buffCount, 1);
									component2.AddBuff(Buffs.lockResistBuff);
								}
								Transform modelTransform = component2.get_modelLocator().get_modelTransform();
								if ((bool)(Object)(object)component2 && (bool)modelTransform)
								{
									TemporaryOverlay val = ((Component)(object)component2).gameObject.AddComponent<TemporaryOverlay>();
									val.duration = 5f - (float)buffCount;
									val.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
									val.animateShaderAlpha = true;
									val.destroyComponentOnEnd = true;
									val.originalMaterial = Assets.mainAssetBundle.LoadAsset<Material>("matChains");
									val.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
								}
								EntityStateMachine component3 = ((Component)(object)component2).GetComponent<EntityStateMachine>();
								if ((Object)(object)component3 != null)
								{
									LockState state = new LockState
									{
										duration = 5f - (float)buffCount
									};
									component3.SetState((EntityState)(object)state);
								}
							}
							int num2 = 5 - buffCount;
							GameObject gameObject = null;
							gameObject = num2 switch
							{
								5 => Assets.lockEffect5s, 
								4 => Assets.lockEffect4s, 
								3 => Assets.lockEffect3s, 
								2 => Assets.lockEffect2s, 
								1 => Assets.lockEffect1s, 
								_ => Assets.lockEffectBreak, 
							};
							if ((bool)(Object)(object)component2.get_healthComponent() && component2.get_healthComponent().get_combinedHealthFraction() <= 0f)
							{
								gameObject = Assets.lockEffectBreak;
							}
							EffectData val2 = new EffectData();
							val2.rotation = Util.QuaternionSafeLookRotation(Vector3.zero);
							val2.set_origin(component2.get_corePosition());
							EffectManager.SpawnEffect(gameObject, val2, true);
						}
					}
				}
			}
		}
		orig(self, damageInfo, victim);
	}
}
