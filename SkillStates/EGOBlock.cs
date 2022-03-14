// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOBlock
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOBlock : BaseSkillState
  {
    public float duration = 1f;
    public float invulStart = 0.0f;
    public float invulEnd = 0.35f;
    public float bonusMult = 1f;
    public float stockBonus = 0.4f;
    public float hitBonus = 0.6f;
    public bool invul = false;
    public bool blockOut = false;
    public float damageCounter = 0.0f;
    protected RedMistEmotionComponent emotionComponent;
    protected RedMistStatTracker statTracker;
    private Transform modelTransform;
    private HurtBoxGroup hurtboxGroup;
    private CharacterModel characterModel;
    private ParticleSystem mistEffect;
    private float originalHeight;
    private float originalRadius;

    public virtual void OnEnter()
    {
      if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) ((EntityState) this).characterBody.master) && RiskOfRuinaPlugin.KombatIsDueling(((EntityState) this).characterBody.master))
        this.duration += 0.2f;
      this.emotionComponent = ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      this.statTracker = ((EntityState) this).gameObject.GetComponent<RedMistStatTracker>();
      this.modelTransform = ((EntityState) this).GetModelTransform();
      if (Object.op_Implicit((Object) this.modelTransform))
      {
        this.characterModel = ((Component) this.modelTransform).GetComponent<CharacterModel>();
        this.hurtboxGroup = ((Component) this.modelTransform).GetComponent<HurtBoxGroup>();
      }
      if (Object.op_Implicit((Object) this.characterModel))
        ++this.characterModel.invisibilityCount;
      int num = (int) Util.PlaySound("Play_DaeChi", ((EntityState) this).gameObject);
      ((EntityState) this).PlayAnimation("EGODodge", "EGODodge", "Dodge.playbackRate", this.invulEnd);
      EffectManager.SpawnEffect(this.statTracker.phaseEffect, new EffectData()
      {
        rotation = Quaternion.identity,
        origin = ((EntityState) this).characterBody.corePosition
      }, true);
      if (Object.op_Implicit((Object) ((EntityState) this).GetModelChildLocator()))
      {
        this.mistEffect = ((EntityState) this).GetComponent<RedMistStatTracker>().mistEffect;
        this.mistEffect.Play();
      }
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.invul = true;
      if (((EntityState) this).skillLocator.utility.stock > 1)
        this.bonusMult += (float) (((EntityState) this).skillLocator.utility.stock - 1) * this.stockBonus;
      ((BaseState) this).OnEnter();
      RiskOfRuinaNetworkManager.ServerOnHit += new RiskOfRuinaNetworkManager.hook_ServerOnHit(this.OnHit);
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.HorizontalSlashCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 2;
      CapsuleCollider collider = (CapsuleCollider) ((EntityState) this).characterBody.mainHurtBox.collider;
      this.originalHeight = collider.height;
      this.originalRadius = collider.radius;
      collider.height = this.originalHeight * 2f;
      collider.radius = this.originalRadius * 25f;
    }

    public void OnHit(float damage, GameObject attacker, GameObject victim)
    {
      if (!Object.op_Equality((Object) victim, (Object) ((EntityState) this).gameObject) || !this.invul)
        return;
      ((EntityState) this).PlayAnimation("EGODodge", "EGODodge", "Dodge.playbackRate", this.hitBonus);
      int num1 = (int) Util.PlaySound("Play_Defense_Guard", ((EntityState) this).gameObject);
      this.invulEnd = ((EntityState) this).fixedAge + this.hitBonus;
      this.duration = this.invulEnd + this.hitBonus;
      if (Object.op_Implicit((Object) attacker) && Object.op_Implicit((Object) attacker.GetComponent<CharacterBody>()) && Object.op_Inequality((Object) attacker.GetComponent<CharacterBody>(), (Object) ((EntityState) this).characterBody))
      {
        float num2 = damage;
        if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) ((EntityState) this).characterBody.master) && RiskOfRuinaPlugin.KombatIsDueling(((EntityState) this).characterBody.master))
          num2 = damage * 5f;
        this.damageCounter += num2;
        CharacterBody component = attacker.GetComponent<CharacterBody>();
        if (NetworkServer.active)
        {
          DamageInfo damageInfo = new DamageInfo()
          {
            attacker = ((Component) ((EntityState) this).characterBody).gameObject,
            inflictor = ((Component) ((EntityState) this).characterBody).gameObject,
            crit = ((BaseState) this).RollCrit(),
            damage = (float) ((1.0 + (double) Config.redMistBuffDamage.Value * (double) ((EntityState) this).characterBody.GetBuffCount(Buffs.RedMistBuff)) * ((double) num2 * 1.5 * (double) this.bonusMult)),
            position = attacker.transform.position,
            force = Vector3.zero,
            damageType = (DamageType) 0,
            damageColorIndex = (DamageColorIndex) 0,
            procCoefficient = 1f
          };
          component.healthComponent.TakeDamage(damageInfo);
          GlobalEventManager.instance.OnHitEnemy(damageInfo, attacker);
          GlobalEventManager.instance.OnHitAll(damageInfo, attacker);
        }
        if (((EntityState) this).isAuthority)
        {
          EffectManager.SpawnEffect(this.statTracker.afterimageSlash, new EffectData()
          {
            rotation = Random.rotation,
            origin = component.healthComponent.body.corePosition
          }, true);
          Vector3 vector3 = Vector3.op_Subtraction(component.footPosition, ((EntityState) this).characterBody.footPosition);
          vector3.y = 0.0f;
          EffectManager.SpawnEffect(this.statTracker.afterimageBlock, new EffectData()
          {
            rotation = Quaternion.LookRotation(((Vector3) ref vector3).normalized, Vector3.up),
            origin = Vector3.op_Addition(((EntityState) this).characterBody.footPosition, Vector3.op_Multiply(Random.Range(0.0f, 2.5f), ((Vector3) ref vector3).normalized))
          }, true);
        }
      }
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if (NetworkServer.active && (double) ((EntityState) this).fixedAge < (double) this.invulEnd && !((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      if ((double) ((EntityState) this).fixedAge >= (double) this.invulEnd && this.invul)
      {
        if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
          ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        if (Object.op_Implicit((Object) this.characterModel))
          --this.characterModel.invisibilityCount;
        if (((EntityState) this).isAuthority)
          EffectManager.SpawnEffect(this.statTracker.phaseEffect, new EffectData()
          {
            rotation = Quaternion.identity,
            origin = ((EntityState) this).characterBody.corePosition
          }, true);
        this.mistEffect.Stop();
        this.invul = false;
        CapsuleCollider collider = (CapsuleCollider) ((EntityState) this).characterBody.mainHurtBox.collider;
        collider.height = 1.5f;
        collider.radius = 0.2f;
        if ((double) this.damageCounter > 0.0)
        {
          if (((EntityState) this).isAuthority)
            ((EntityState) this).outer.SetNextState((EntityState) new EGOBlockCounter()
            {
              damageCounter = this.damageCounter,
              bonusMult = this.bonusMult
            });
        }
        else
        {
          ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsRedMist;
          ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 0;
        }
      }
      if ((double) this.damageCounter > 0.0 && !((EntityState) this).inputBank.skill3.down && ((EntityState) this).isAuthority)
        ((EntityState) this).outer.SetNextState((EntityState) new EGOBlockCounter()
        {
          damageCounter = this.damageCounter,
          bonusMult = this.bonusMult
        });
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      CapsuleCollider collider = (CapsuleCollider) ((EntityState) this).characterBody.mainHurtBox.collider;
      collider.height = 1.5f;
      collider.radius = 0.2f;
      RiskOfRuinaNetworkManager.ServerOnHit -= new RiskOfRuinaNetworkManager.hook_ServerOnHit(this.OnHit);
      ((EntityState) this).OnExit();
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 6;

    public virtual void OnSerialize(NetworkWriter writer)
    {
      base.OnSerialize(writer);
      writer.Write(this.damageCounter);
      writer.Write(this.invulEnd);
      writer.Write(this.duration);
    }

    public virtual void OnDeserialize(NetworkReader reader)
    {
      base.OnDeserialize(reader);
      this.damageCounter = reader.ReadSingle();
      this.invulEnd = reader.ReadSingle();
      this.duration = reader.ReadSingle();
    }
  }
}
