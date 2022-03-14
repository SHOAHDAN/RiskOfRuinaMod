// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.BlockCounter
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class BlockCounter : BaseSkillState
  {
    public float damageCounter = 0.0f;
    public int hits = 0;
    public float duration = 0.5f;
    public float bonusMult = 1f;
    protected RedMistEmotionComponent emotionComponent;
    protected RedMistStatTracker statTracker;
    protected BlastAttack attack;

    public virtual void OnEnter()
    {
      this.emotionComponent = ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      this.statTracker = ((EntityState) this).gameObject.GetComponent<RedMistStatTracker>();
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
      if (((EntityState) this).isAuthority)
      {
        this.attack = new BlastAttack();
        this.attack.damageType = (DamageType) 0;
        this.attack.procCoefficient = 1f;
        this.attack.baseForce = 300f;
        this.attack.bonusForce = Vector3.zero;
        this.attack.baseDamage = this.damageCounter * 1.5f * this.bonusMult;
        this.attack.crit = ((BaseState) this).RollCrit();
        this.attack.attacker = ((Component) ((EntityState) this).characterBody).gameObject;
        this.attack.damageColorIndex = (DamageColorIndex) 0;
        this.attack.falloffModel = (BlastAttack.FalloffModel) 0;
        this.attack.radius = (float) (15 + Mathf.Clamp(this.hits, 0, 15));
        this.attack.inflictor = ((Component) ((EntityState) this).characterBody).gameObject;
        this.attack.position = ((EntityState) this).characterBody.footPosition;
        this.attack.procCoefficient = 1f;
        this.attack.teamIndex = TeamComponent.GetObjectTeam(((Component) ((EntityState) this).characterBody).gameObject);
        this.attack.Fire();
      }
      ((BaseState) this).OnEnter();
      int num1 = (int) Util.PlaySound("Play_Kali_Normal_Hori", ((EntityState) this).gameObject);
      ((EntityState) this).PlayAnimation("FullBody, Override", nameof (BlockCounter));
      EffectManager.SpawnEffect(this.statTracker.spinPrefab, new EffectData()
      {
        rotation = Quaternion.identity,
        origin = ((EntityState) this).characterBody.footPosition
      }, true);
      if (this.hits <= 5)
        return;
      int num2 = (int) Util.PlaySound("Play_Kali_Special_Vert_Fin", ((EntityState) this).gameObject);
      EffectManager.SpawnEffect(this.statTracker.spinPrefabTwo, new EffectData()
      {
        rotation = Quaternion.identity,
        origin = ((EntityState) this).characterBody.footPosition
      }, true);
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      if ((double) ((EntityState) this).fixedAge < (double) this.duration || !((EntityState) this).isAuthority)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      if (NetworkServer.active && ((EntityState) this).characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
        ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
      ((EntityState) this).OnExit();
    }
  }
}
