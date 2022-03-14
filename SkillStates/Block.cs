// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.Block
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
  internal class Block : BaseSkillState
  {
    public float duration = 0.7f;
    public float invulEnd = 0.35f;
    public float hitBonus = 0.6f;
    public bool invul = false;
    public bool blockOut = false;
    public float damageCounter = 0.0f;
    public float bonusMult = 1f;
    public float stockBonus = 0.4f;
    public int hits = 0;
    protected TemporaryOverlay iframeOverlay;
    protected RedMistEmotionComponent emotionComponent;
    protected RedMistStatTracker statTracker;
    private float originalHeight;
    private float originalRadius;

    public virtual void OnEnter()
    {
      this.emotionComponent = ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      this.statTracker = ((EntityState) this).gameObject.GetComponent<RedMistStatTracker>();
      if (((EntityState) this).skillLocator.utility.stock > 1)
        this.bonusMult += (float) (((EntityState) this).skillLocator.utility.stock - 1) * this.stockBonus;
      this.AddOverlay(this.invulEnd);
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.invul = true;
      ((BaseState) this).OnEnter();
      int num = (int) Util.PlaySound("Ruina_Swipe", ((EntityState) this).gameObject);
      ((EntityState) this).PlayAnimation("FullBody, Override", "BlockIn");
      RiskOfRuinaNetworkManager.ServerOnHit += new RiskOfRuinaNetworkManager.hook_ServerOnHit(this.OnHit);
      CapsuleCollider collider = (CapsuleCollider) ((EntityState) this).characterBody.mainHurtBox.collider;
      this.originalHeight = collider.height;
      this.originalRadius = collider.radius;
      collider.height = this.originalHeight * 1.5f;
      collider.radius = this.originalRadius * 10f;
    }

    public void OnHit(float damage, GameObject attacker, GameObject victim)
    {
      if (!Object.op_Equality((Object) victim, (Object) ((EntityState) this).gameObject) || !this.invul)
        return;
      int num1 = (int) Util.PlaySound("Play_Defense_Guard", ((EntityState) this).gameObject);
      if (Object.op_Implicit((Object) attacker) && Object.op_Implicit((Object) attacker.GetComponent<CharacterBody>()) && Object.op_Inequality((Object) attacker.GetComponent<CharacterBody>(), (Object) ((EntityState) this).characterBody))
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(attacker.GetComponent<CharacterBody>().footPosition, ((EntityState) this).characterBody.footPosition);
        vector3_1.y = 0.0f;
        ((Vector3) ref vector3_1).Normalize();
        Vector3 forward = ((EntityState) this).characterDirection.forward;
        Vector3 vector3_2 = Vector3.Cross(((EntityState) this).transform.up, forward);
        Vector3 normalized1 = ((Vector3) ref vector3_2).normalized;
        Vector2 vector2 = new Vector2(Vector3.Dot(vector3_1, forward), Vector3.Dot(vector3_1, normalized1));
        Vector2 normalized2 = ((Vector2) ref vector2).normalized;
        if ((double) normalized2.x >= 0.5)
          ((EntityState) this).PlayAnimation("FullBody, Override", "BlockHit");
        else if ((double) normalized2.x <= -0.5)
          ((EntityState) this).PlayAnimation("FullBody, Override", "BlockHitBack");
        else if ((double) normalized2.y >= 0.5)
          ((EntityState) this).PlayAnimation("FullBody, Override", "BlockHitRight");
        else if ((double) normalized2.y <= -0.5)
          ((EntityState) this).PlayAnimation("FullBody, Override", "BlockHitLeft");
        else
          ((EntityState) this).PlayAnimation("FullBody, Override", "BlockHit");
        this.invulEnd = ((EntityState) this).fixedAge + this.hitBonus;
        this.duration = this.invulEnd + this.hitBonus;
        if (Object.op_Implicit((Object) attacker) && Object.op_Implicit((Object) attacker.GetComponent<CharacterBody>()))
        {
          float num2 = damage;
          if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && Object.op_Implicit((Object) ((EntityState) this).characterBody.master) && RiskOfRuinaPlugin.KombatIsDueling(((EntityState) this).characterBody.master))
            num2 = damage * 5f;
          this.damageCounter += num2;
          ++this.hits;
        }
        if (((EntityState) this).isAuthority)
          EffectManager.SpawnEffect(Assets.blockEffect, new EffectData()
          {
            rotation = Util.QuaternionSafeLookRotation(Vector3.zero),
            origin = ((EntityState) this).characterBody.corePosition
          }, true);
      }
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      ((EntityState) this).characterMotor.velocity = Vector3.zero;
      if ((double) ((EntityState) this).fixedAge >= (double) this.invulEnd && this.invul)
      {
        if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
          ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        this.RemoveOverlay();
        this.invul = false;
        CapsuleCollider collider = (CapsuleCollider) ((EntityState) this).characterBody.mainHurtBox.collider;
        collider.height = 1.5f;
        collider.radius = 0.2f;
        if ((double) this.damageCounter > 0.0 && ((EntityState) this).isAuthority)
          ((EntityState) this).outer.SetNextState((EntityState) new BlockCounter()
          {
            damageCounter = this.damageCounter,
            hits = this.hits,
            bonusMult = this.bonusMult
          });
      }
      if ((double) this.damageCounter > 0.0 && !((EntityState) this).inputBank.skill3.down && ((EntityState) this).isAuthority)
        ((EntityState) this).outer.SetNextState((EntityState) new BlockCounter()
        {
          damageCounter = this.damageCounter,
          hits = this.hits,
          bonusMult = this.bonusMult
        });
      if ((double) ((EntityState) this).fixedAge >= (double) this.invulEnd && !this.blockOut)
      {
        this.blockOut = true;
        ((EntityState) this).PlayAnimation("FullBody, Override", "BlockOut");
      }
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      this.RemoveOverlay();
      CapsuleCollider collider = (CapsuleCollider) ((EntityState) this).characterBody.mainHurtBox.collider;
      collider.height = 1.5f;
      collider.radius = 0.2f;
      RiskOfRuinaNetworkManager.ServerOnHit -= new RiskOfRuinaNetworkManager.hook_ServerOnHit(this.OnHit);
      if (!this.blockOut)
        ((EntityState) this).PlayAnimation("FullBody, Override", "BlockOut");
      ((EntityState) this).OnExit();
    }

    protected void AddOverlay(float duration)
    {
      if (!Config.iframeOverlay.Value)
        return;
      this.iframeOverlay = ((Component) ((EntityState) this).characterBody).gameObject.AddComponent<TemporaryOverlay>();
      this.iframeOverlay.duration = duration;
      this.iframeOverlay.alphaCurve = AnimationCurve.Constant(0.0f, duration, 0.1f);
      this.iframeOverlay.animateShaderAlpha = true;
      this.iframeOverlay.destroyComponentOnEnd = true;
      this.iframeOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
      this.iframeOverlay.AddToCharacerModel(((Component) ((EntityState) this).modelLocator.modelTransform).GetComponent<CharacterModel>());
    }

    protected void RemoveOverlay()
    {
      if (!Object.op_Implicit((Object) this.iframeOverlay))
        return;
      Object.Destroy((Object) this.iframeOverlay);
    }

    public virtual void OnSerialize(NetworkWriter writer)
    {
      base.OnSerialize(writer);
      writer.Write(this.damageCounter);
      writer.Write(this.hits);
      writer.Write(this.invulEnd);
      writer.Write(this.duration);
    }

    public virtual void OnDeserialize(NetworkReader reader)
    {
      base.OnDeserialize(reader);
      this.damageCounter = reader.ReadSingle();
      this.hits = reader.ReadInt32();
      this.invulEnd = reader.ReadSingle();
      this.duration = reader.ReadSingle();
    }
  }
}
