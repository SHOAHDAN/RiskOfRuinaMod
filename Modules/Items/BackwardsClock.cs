// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Items.BackwardsClock
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using BepInEx.Configuration;
using EntityStates;
using On.RoR2;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Items
{
  internal class BackwardsClock : RuinaEquipment
  {
    public EquipmentDef equipDef;

    internal override ConfigEntry<bool> equipEnabled { get; set; }

    internal override string equipName { get; set; } = "RuinaBackwardsClock";

    public override void EquipSetup()
    {
      this.equipDef = ScriptableObject.CreateInstance<EquipmentDef>();
      ((Object) this.equipDef).name = this.equipName;
      this.equipDef.appearsInMultiPlayer = true;
      this.equipDef.appearsInSinglePlayer = false;
      this.equipDef.isLunar = true;
      this.equipDef.pickupModelPrefab = Assets.backwardsClock;
      this.equipDef.pickupIconSprite = Assets.mainAssetBundle.LoadAsset<Sprite>("texIconPickupRuinaBackwardsClock");
      this.equipDef.nameToken = this.equipName.ToUpper() + "_NAME";
      this.equipDef.pickupToken = this.equipName.ToUpper() + "_PICKUP";
      this.equipDef.descriptionToken = this.equipName.ToUpper() + "_DESC";
      this.equipDef.loreToken = this.equipName.ToUpper() + "_LORE";
      this.equipDef.enigmaCompatible = false;
      this.equipDef.canDrop = true;
      this.equipDef.cooldown = 0.0f;
      ItemAPI.Add(new CustomEquipment(this.equipDef, new ItemDisplayRule[0]));
    }

    public override void HookSetup() => EquipmentSlot.PerformEquipmentAction += new EquipmentSlot.hook_PerformEquipmentAction((object) this, __methodptr(EquipmentSlot_PerformEquipmentAction));

    private bool EquipmentSlot_PerformEquipmentAction(
      EquipmentSlot.orig_PerformEquipmentAction orig,
      EquipmentSlot self,
      EquipmentDef equipmentIndex)
    {
      if (!Object.op_Equality((Object) equipmentIndex, (Object) this.equipDef))
        return orig.Invoke(self, equipmentIndex);
      if (NetworkServer.active)
      {
        foreach (PlayerCharacterMasterController allDeadCharacter in this.GetAllDeadCharacters())
        {
          GameObject bodyPrefab = BodyCatalog.GetBodyPrefab(allDeadCharacter.networkUser.NetworkbodyIndexPreference);
          if (Object.op_Inequality((Object) bodyPrefab, (Object) null))
            allDeadCharacter.master.bodyPrefab = bodyPrefab;
          allDeadCharacter.master.Respawn(Reflection.GetFieldValue<Vector3>((object) allDeadCharacter.master, "deathFootPosition"), ((Component) allDeadCharacter.master).transform.rotation);
          EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/HippoRezEffect"), new EffectData()
          {
            origin = allDeadCharacter.master.GetBody().footPosition,
            rotation = ((Component) allDeadCharacter.master).gameObject.transform.rotation
          }, true);
        }
        DamageInfo damageInfo = new DamageInfo()
        {
          attacker = (GameObject) null,
          inflictor = (GameObject) null,
          crit = true,
          damage = self.characterBody.healthComponent.combinedHealth + self.characterBody.healthComponent.fullBarrier,
          position = ((Component) self).transform.position,
          force = Vector3.zero,
          damageType = (DamageType) 262144,
          damageColorIndex = (DamageColorIndex) 0,
          procCoefficient = 0.0f
        };
        self.characterBody.inventory.SetEquipmentIndex((EquipmentIndex) -1);
        if (self.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
          self.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        self.characterBody.healthComponent.TakeDamage(damageInfo);
        GlobalEventManager.instance.OnHitAll(damageInfo, ((Component) self).gameObject);
        for (TeamIndex teamIndex = (TeamIndex) 0; teamIndex < 4; teamIndex = (TeamIndex) (int) (sbyte) (teamIndex + 1))
        {
          if (teamIndex != self.characterBody.teamComponent.teamIndex)
          {
            foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(teamIndex))
            {
              CharacterBody body = teamMember.body;
              if (Object.op_Implicit((Object) body))
              {
                EntityStateMachine component = ((Component) body).GetComponent<EntityStateMachine>();
                if (Object.op_Inequality((Object) component, (Object) null))
                {
                  StunState stunState = new StunState()
                  {
                    duration = 5f,
                    stunDuration = 5f
                  };
                  component.SetState((EntityState) stunState);
                }
              }
            }
          }
        }
      }
      return true;
    }

    private List<PlayerCharacterMasterController> GetAllDeadCharacters()
    {
      List<PlayerCharacterMasterController> allDeadCharacters = new List<PlayerCharacterMasterController>();
      foreach (PlayerCharacterMasterController instance in PlayerCharacterMasterController.instances)
      {
        NetworkUser networkUser = instance.networkUser;
        if (instance.isConnected && (networkUser.master.IsDeadAndOutOfLivesServer() || Object.op_Inequality((Object) networkUser.master.bodyPrefab, (Object) BodyCatalog.GetBodyPrefab(networkUser.NetworkbodyIndexPreference))))
          allDeadCharacters.Add(networkUser.master.playerCharacterMasterController);
      }
      return allDeadCharacters;
    }
  }
}
