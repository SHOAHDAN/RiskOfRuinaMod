// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.SkillStates.EGOActivateOut
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.Modules;
using RiskOfRuinaMod.Modules.Components;
using RiskOfRuinaMod.Modules.Survivors;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{
  internal class EGOActivateOut : BaseSkillState
  {
    public static float baseDuration = 1f;
    public static float shockwaveRadius = 15f;
    public static float shockwaveForce = 8000f;
    public static float shockwaveBonusForce = 1500f;
    private float duration;
    private RedMistEmotionComponent EGOController;
    private RedMistStatTracker statTracker;

    public virtual void OnEnter()
    {
      ((BaseState) this).OnEnter();
      this.duration = EGOActivateOut.baseDuration;
      this.EGOController = ((EntityState) this).gameObject.GetComponent<RedMistEmotionComponent>();
      this.statTracker = ((EntityState) this).gameObject.GetComponent<RedMistStatTracker>();
      ((EntityState) this).PlayAnimation("FullBody, Override", "BufferEmpty");
      ((EntityState) this).PlayAnimation("FullBody, Override", nameof (EGOActivateOut), "EGOActivate.playbackRate", this.duration);
      if (NetworkServer.active)
        ((EntityState) this).characterBody.AddBuff(Buffs.EGOBuff);
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.EGOActivateOutCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 2;
      this.FireShockwave();
      if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalBlock))
        ((EntityState) this).skillLocator.utility.SetSkillOverride((object) ((EntityState) this).skillLocator.utility, RedMist.EGOBlock, (GenericSkill.SkillOverridePriority) 4);
      else if (Object.op_Equality((Object) ((EntityState) this).skillLocator.utility.baseSkill, (Object) RedMist.NormalDodge))
        ((EntityState) this).skillLocator.utility.SetSkillOverride((object) ((EntityState) this).skillLocator.utility, RedMist.EGODodge, (GenericSkill.SkillOverridePriority) 4);
      ((EntityState) this).skillLocator.special.SetSkillOverride((object) ((EntityState) this).skillLocator.special, RedMist.HorizontalSlash, (GenericSkill.SkillOverridePriority) 4);
    }

    private void FireShockwave()
    {
      int num = (int) Util.PlaySound("Play_Effect_Index_Unlock", ((EntityState) this).gameObject);
      EffectManager.SpawnEffect(this.statTracker.EGOActivatePrefab, new EffectData()
      {
        origin = ((EntityState) this).characterBody.corePosition,
        scale = 1f
      }, false);
      if (((EntityState) this).isAuthority)
      {
        BlastAttack blastAttack = new BlastAttack()
        {
          attacker = ((EntityState) this).gameObject,
          inflictor = ((EntityState) this).gameObject
        };
        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
        blastAttack.position = ((EntityState) this).characterBody.corePosition;
        blastAttack.procCoefficient = 0.0f;
        blastAttack.radius = EGOActivateOut.shockwaveRadius;
        blastAttack.baseForce = EGOActivateOut.shockwaveForce;
        blastAttack.bonusForce = Vector3.op_Multiply(Vector3.up, EGOActivateOut.shockwaveBonusForce);
        blastAttack.baseDamage = 0.0f;
        blastAttack.falloffModel = (BlastAttack.FalloffModel) 0;
        blastAttack.damageColorIndex = (DamageColorIndex) 3;
        blastAttack.attackerFiltering = (AttackerFiltering) 2;
        blastAttack.Fire();
      }
      if (!Object.op_Implicit((Object) this.EGOController))
        return;
      this.EGOController.EnterEGO();
    }

    public virtual void FixedUpdate()
    {
      ((EntityState) this).FixedUpdate();
      ((EntityState) this).characterMotor.velocity = Vector3.zero;
      if (!((EntityState) this).isAuthority || (double) ((EntityState) this).fixedAge < (double) this.duration)
        return;
      ((EntityState) this).outer.SetNextStateToMain();
    }

    public virtual void OnExit()
    {
      ((EntityState) this).OnExit();
      ((EntityState) this).cameraTargetParams.cameraParams = CameraParams.defaultCameraParamsRedMist;
      ((EntityState) this).cameraTargetParams.aimMode = (CameraTargetParams.AimType) 0;
      if (!NetworkServer.active)
        return;
      ((EntityState) this).characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
    }

    public virtual InterruptPriority GetMinimumInterruptPriority() => (InterruptPriority) 4;
  }
}
