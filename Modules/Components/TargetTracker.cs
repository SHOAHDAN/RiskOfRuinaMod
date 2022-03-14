// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.TargetTracker
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components
{
  public class TargetTracker : NetworkBehaviour
  {
    public float maxTrackingDistance = 60f;
    public float maxTrackingAngle = 45f;
    public float trackerUpdateFrequency = 10f;
    public bool trackEnemy = true;
    private CharacterBody trackingTarget;
    private CharacterBody characterBody;
    private TeamComponent teamComponent;
    private InputBankTest inputBank;
    private float trackerUpdateStopwatch;
    private Indicator indicator;
    private readonly BullseyeSearch search = new BullseyeSearch();
    private TeamMask friendlies;
    private static int kCmdCmdSetTarget = 615813580;
    private static int kRpcRpcSetTarget;

    private void Awake() => this.indicator = new Indicator(((Component) this).gameObject, Assets.trackerPrefab);

    private void Start()
    {
      this.characterBody = ((Component) this).GetComponent<CharacterBody>();
      this.inputBank = ((Component) this).GetComponent<InputBankTest>();
      this.teamComponent = ((Component) this).GetComponent<TeamComponent>();
      this.friendlies = new TeamMask();
      ((TeamMask) ref this.friendlies).AddTeam(this.teamComponent.teamIndex);
      if (!Object.op_Equality((Object) this.characterBody.skillLocator.secondary.skillDef, (Object) Skills.unlockSkillDef))
        return;
      this.trackEnemy = false;
    }

    public CharacterBody GetTrackingTarget() => this.trackingTarget;

    private void OnEnable() => this.indicator.active = true;

    private void OnDisable() => this.indicator.active = false;

    private void OnDestroy() => this.indicator.active = false;

    private void FixedUpdate()
    {
      if (!this.hasAuthority)
        return;
      this.trackerUpdateStopwatch += Time.fixedDeltaTime;
      if ((double) this.trackerUpdateStopwatch < 1.0 / (double) this.trackerUpdateFrequency)
        return;
      this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
      Ray aimRay;
      // ISSUE: explicit constructor call
      ((Ray) ref aimRay).\u002Ector(this.inputBank.aimOrigin, this.inputBank.aimDirection);
      this.SearchForTarget(aimRay);
      this.indicator.targetTransform = Object.op_Implicit((Object) this.trackingTarget) ? this.trackingTarget.transform : (Transform) null;
    }

    private void SearchForTarget(Ray aimRay)
    {
      this.search.teamMaskFilter = !this.trackEnemy ? this.friendlies : TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
      this.search.filterByLoS = true;
      this.search.searchOrigin = ((Ray) ref aimRay).origin;
      this.search.searchDirection = ((Ray) ref aimRay).direction;
      this.search.sortMode = (BullseyeSearch.SortMode) 2;
      this.search.maxDistanceFilter = this.maxTrackingDistance;
      this.search.maxAngleFilter = this.maxTrackingAngle;
      this.search.RefreshCandidates();
      this.search.FilterOutGameObject(((Component) this).gameObject);
      HurtBox hurtBox = this.search.GetResults().FirstOrDefault<HurtBox>();
      if (Object.op_Implicit((Object) hurtBox) && Object.op_Implicit((Object) hurtBox.healthComponent) && Object.op_Implicit((Object) hurtBox.healthComponent.body))
      {
        if (!Object.op_Inequality((Object) hurtBox.healthComponent.body, (Object) this.trackingTarget))
          return;
        this.CallCmdSetTarget(((Component) hurtBox.healthComponent.body).gameObject);
      }
      else if (Object.op_Inequality((Object) this.trackingTarget, (Object) null))
        this.CallCmdSetTarget((GameObject) null);
    }

    [Command]
    public void CmdSetTarget(GameObject targetNet) => this.CallRpcSetTarget(targetNet);

    [ClientRpc]
    public void RpcSetTarget(GameObject targetNet)
    {
      if (Object.op_Implicit((Object) targetNet) && Object.op_Implicit((Object) targetNet.GetComponent<CharacterBody>()))
        this.trackingTarget = targetNet.GetComponent<CharacterBody>();
      else
        this.trackingTarget = (CharacterBody) null;
    }

    private void UNetVersion()
    {
    }

    protected static void InvokeCmdCmdSetTarget(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkServer.active)
        Debug.LogError((object) "Command CmdSetTarget called on client.");
      else
        ((TargetTracker) obj).CmdSetTarget((GameObject) reader.ReadGameObject());
    }

    public void CallCmdSetTarget(GameObject targetNet)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "Command function CmdSetTarget called on server.");
      else if (this.isServer)
      {
        this.CmdSetTarget(targetNet);
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 5);
        networkWriter.WritePackedUInt32((uint) TargetTracker.kCmdCmdSetTarget);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        networkWriter.Write((GameObject) targetNet);
        this.SendCommandInternal(networkWriter, 0, "CmdSetTarget");
      }
    }

    protected static void InvokeRpcRpcSetTarget(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "RPC RpcSetTarget called on server.");
      else
        ((TargetTracker) obj).RpcSetTarget((GameObject) reader.ReadGameObject());
    }

    public void CallRpcSetTarget(GameObject targetNet)
    {
      if (!NetworkServer.active)
      {
        Debug.LogError((object) "RPC Function RpcSetTarget called on client.");
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 2);
        networkWriter.WritePackedUInt32((uint) TargetTracker.kRpcRpcSetTarget);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        networkWriter.Write((GameObject) targetNet);
        this.SendRPCInternal(networkWriter, 0, "RpcSetTarget");
      }
    }

    static TargetTracker()
    {
      // ISSUE: method pointer
      NetworkBehaviour.RegisterCommandDelegate(typeof (TargetTracker), TargetTracker.kCmdCmdSetTarget, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeCmdCmdSetTarget)));
      TargetTracker.kRpcRpcSetTarget = -977117514;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterRpcDelegate(typeof (TargetTracker), TargetTracker.kRpcRpcSetTarget, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeRpcRpcSetTarget)));
      NetworkCRC.RegisterBehaviour(nameof (TargetTracker), 0);
    }

    public virtual bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
      bool flag;
      return flag;
    }

    public virtual void OnDeserialize(NetworkReader reader, bool initialState)
    {
    }
  }
}
