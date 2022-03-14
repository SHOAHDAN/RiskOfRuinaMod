// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.RiskOfRuinaNetworkManager
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components
{
  internal class RiskOfRuinaNetworkManager : NetworkBehaviour
  {
    private static RiskOfRuinaNetworkManager _instance;
    private static int kRpcRpcSetInvisible = -1035752877;
    private static int kRpcRpcOnHitInvoke;

    public static event RiskOfRuinaNetworkManager.hook_ServerOnHit ServerOnHit;

    private void Awake() => RiskOfRuinaNetworkManager._instance = this;

    public static void SetInvisible(GameObject target)
    {
      if (!Object.op_Implicit((Object) RiskOfRuinaPlugin._centralNetworkObjectSpawned))
      {
        RiskOfRuinaPlugin._centralNetworkObjectSpawned = Object.Instantiate<GameObject>(RiskOfRuinaPlugin.CentralNetworkObject);
        NetworkServer.Spawn(RiskOfRuinaPlugin._centralNetworkObjectSpawned);
      }
      RiskOfRuinaNetworkManager._instance.CallRpcSetInvisible(target);
    }

    public static void OnHit(GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
    {
      if (!Object.op_Implicit((Object) RiskOfRuinaPlugin._centralNetworkObjectSpawned))
      {
        RiskOfRuinaPlugin._centralNetworkObjectSpawned = Object.Instantiate<GameObject>(RiskOfRuinaPlugin.CentralNetworkObject);
        NetworkServer.Spawn(RiskOfRuinaPlugin._centralNetworkObjectSpawned);
      }
      RiskOfRuinaNetworkManager._instance.CallRpcOnHitInvoke(damageInfo.damage, damageInfo.attacker, victim);
    }

    [ClientRpc]
    private void RpcSetInvisible(GameObject target)
    {
      if (!Object.op_Implicit((Object) target) || !Object.op_Implicit((Object) target.GetComponent<CharacterBody>()) || !Object.op_Implicit((Object) target.GetComponent<CharacterBody>().modelLocator) || !Object.op_Implicit((Object) target.GetComponent<CharacterBody>().modelLocator.modelTransform) || !Object.op_Implicit((Object) ((Component) target.GetComponent<CharacterBody>().modelLocator.modelTransform).GetComponent<CharacterModel>()))
        return;
      ++((Component) target.GetComponent<CharacterBody>().modelLocator.modelTransform).GetComponent<CharacterModel>().invisibilityCount;
    }

    [ClientRpc]
    private void RpcOnHitInvoke(float damage, GameObject attacker, GameObject victim)
    {
      RiskOfRuinaNetworkManager.hook_ServerOnHit serverOnHit = RiskOfRuinaNetworkManager.ServerOnHit;
      if (serverOnHit == null)
        return;
      serverOnHit(damage, attacker, victim);
    }

    private void UNetVersion()
    {
    }

    protected static void InvokeRpcRpcSetInvisible(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "RPC RpcSetInvisible called on server.");
      else
        ((RiskOfRuinaNetworkManager) obj).RpcSetInvisible((GameObject) reader.ReadGameObject());
    }

    protected static void InvokeRpcRpcOnHitInvoke(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "RPC RpcOnHitInvoke called on server.");
      else
        ((RiskOfRuinaNetworkManager) obj).RpcOnHitInvoke(reader.ReadSingle(), (GameObject) reader.ReadGameObject(), (GameObject) reader.ReadGameObject());
    }

    public void CallRpcSetInvisible(GameObject target)
    {
      if (!NetworkServer.active)
      {
        Debug.LogError((object) "RPC Function RpcSetInvisible called on client.");
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 2);
        networkWriter.WritePackedUInt32((uint) RiskOfRuinaNetworkManager.kRpcRpcSetInvisible);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        networkWriter.Write((GameObject) target);
        this.SendRPCInternal(networkWriter, 0, "RpcSetInvisible");
      }
    }

    public void CallRpcOnHitInvoke(float damage, GameObject attacker, GameObject victim)
    {
      if (!NetworkServer.active)
      {
        Debug.LogError((object) "RPC Function RpcOnHitInvoke called on client.");
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 2);
        networkWriter.WritePackedUInt32((uint) RiskOfRuinaNetworkManager.kRpcRpcOnHitInvoke);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        networkWriter.Write(damage);
        networkWriter.Write((GameObject) attacker);
        networkWriter.Write((GameObject) victim);
        this.SendRPCInternal(networkWriter, 0, "RpcOnHitInvoke");
      }
    }

    static RiskOfRuinaNetworkManager()
    {
      // ISSUE: method pointer
      NetworkBehaviour.RegisterRpcDelegate(typeof (RiskOfRuinaNetworkManager), RiskOfRuinaNetworkManager.kRpcRpcSetInvisible, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeRpcRpcSetInvisible)));
      RiskOfRuinaNetworkManager.kRpcRpcOnHitInvoke = -888123164;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterRpcDelegate(typeof (RiskOfRuinaNetworkManager), RiskOfRuinaNetworkManager.kRpcRpcOnHitInvoke, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeRpcRpcOnHitInvoke)));
      NetworkCRC.RegisterBehaviour(nameof (RiskOfRuinaNetworkManager), 0);
    }

    public virtual bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
      bool flag;
      return flag;
    }

    public virtual void OnDeserialize(NetworkReader reader, bool initialState)
    {
    }

    public delegate void hook_ServerOnHit(float damage, GameObject attacker, GameObject victim);
  }
}
