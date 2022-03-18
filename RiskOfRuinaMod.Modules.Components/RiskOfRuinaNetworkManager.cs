using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components
{

	internal class RiskOfRuinaNetworkManager : NetworkBehaviour
	{
		public delegate void hook_ServerOnHit(float damage, GameObject attacker, GameObject victim);

		private static RiskOfRuinaNetworkManager _instance;

		private static int kRpcRpcSetInvisible;

		private static int kRpcRpcOnHitInvoke;

		public static event hook_ServerOnHit ServerOnHit;

		private void Awake()
		{
			_instance = this;
		}

		public static void SetInvisible(GameObject target)
		{
			if (!RiskOfRuinaPlugin._centralNetworkObjectSpawned)
			{
				RiskOfRuinaPlugin._centralNetworkObjectSpawned = Object.Instantiate(RiskOfRuinaPlugin.CentralNetworkObject);
				NetworkServer.Spawn(RiskOfRuinaPlugin._centralNetworkObjectSpawned);
			}
			_instance.CallRpcSetInvisible(target);
		}

		public static void OnHit(GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
		{
			if (!RiskOfRuinaPlugin._centralNetworkObjectSpawned)
			{
				RiskOfRuinaPlugin._centralNetworkObjectSpawned = Object.Instantiate(RiskOfRuinaPlugin.CentralNetworkObject);
				NetworkServer.Spawn(RiskOfRuinaPlugin._centralNetworkObjectSpawned);
			}
			_instance.CallRpcOnHitInvoke(damageInfo.damage, damageInfo.attacker, victim);
		}

		[ClientRpc]
		private void RpcSetInvisible(GameObject target)
		{
			if ((bool)target && (bool)(Object)(object)target.GetComponent<CharacterBody>() && (bool)(Object)(object)target.GetComponent<CharacterBody>().get_modelLocator() && (bool)target.GetComponent<CharacterBody>().get_modelLocator().get_modelTransform() && (bool)(Object)(object)target.GetComponent<CharacterBody>().get_modelLocator().get_modelTransform()
				.GetComponent<CharacterModel>())
			{
				CharacterModel component = target.GetComponent<CharacterBody>().get_modelLocator().get_modelTransform()
					.GetComponent<CharacterModel>();
				component.invisibilityCount++;
			}
		}

		[ClientRpc]
		private void RpcOnHitInvoke(float damage, GameObject attacker, GameObject victim)
		{
			RiskOfRuinaNetworkManager.ServerOnHit?.Invoke(damage, attacker, victim);
		}

		private void UNetVersion()
		{
		}

		protected static void InvokeRpcRpcSetInvisible(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkClient.get_active())
			{
				Debug.LogError("RPC RpcSetInvisible called on server.");
			}
			else
			{
				((RiskOfRuinaNetworkManager)(object)obj).RpcSetInvisible(reader.ReadGameObject());
			}
		}

		protected static void InvokeRpcRpcOnHitInvoke(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkClient.get_active())
			{
				Debug.LogError("RPC RpcOnHitInvoke called on server.");
			}
			else
			{
				((RiskOfRuinaNetworkManager)(object)obj).RpcOnHitInvoke(reader.ReadSingle(), reader.ReadGameObject(), reader.ReadGameObject());
			}
		}

		public void CallRpcSetInvisible(GameObject target)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkServer.get_active())
			{
				Debug.LogError("RPC Function RpcSetInvisible called on client.");
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)2);
			val.WritePackedUInt32((uint)kRpcRpcSetInvisible);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			val.Write(target);
			((NetworkBehaviour)this).SendRPCInternal(val, 0, "RpcSetInvisible");
		}

		public void CallRpcOnHitInvoke(float damage, GameObject attacker, GameObject victim)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkServer.get_active())
			{
				Debug.LogError("RPC Function RpcOnHitInvoke called on client.");
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)2);
			val.WritePackedUInt32((uint)kRpcRpcOnHitInvoke);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			val.Write(damage);
			val.Write(attacker);
			val.Write(victim);
			((NetworkBehaviour)this).SendRPCInternal(val, 0, "RpcOnHitInvoke");
		}

		static RiskOfRuinaNetworkManager()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			kRpcRpcSetInvisible = -1035752877;
			NetworkBehaviour.RegisterRpcDelegate(typeof(RiskOfRuinaNetworkManager), kRpcRpcSetInvisible, new CmdDelegate(InvokeRpcRpcSetInvisible));
			kRpcRpcOnHitInvoke = -888123164;
			NetworkBehaviour.RegisterRpcDelegate(typeof(RiskOfRuinaNetworkManager), kRpcRpcOnHitInvoke, new CmdDelegate(InvokeRpcRpcOnHitInvoke));
			NetworkCRC.RegisterBehaviour("RiskOfRuinaNetworkManager", 0);
		}

		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			bool result = default(bool);
			return result;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
		}
	}
}