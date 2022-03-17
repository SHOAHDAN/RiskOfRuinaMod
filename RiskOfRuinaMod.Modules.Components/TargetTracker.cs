using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components;

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

	private static int kCmdCmdSetTarget;

	private static int kRpcRpcSetTarget;

	private void Awake()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		indicator = new Indicator(((Component)this).gameObject, Assets.trackerPrefab);
	}

	private void Start()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		characterBody = ((Component)this).GetComponent<CharacterBody>();
		inputBank = ((Component)this).GetComponent<InputBankTest>();
		teamComponent = ((Component)this).GetComponent<TeamComponent>();
		friendlies = default(TeamMask);
		((TeamMask)(ref friendlies)).AddTeam(teamComponent.get_teamIndex());
		if ((Object)(object)characterBody.get_skillLocator().secondary.get_skillDef() == (Object)(object)Skills.unlockSkillDef)
		{
			trackEnemy = false;
		}
	}

	public CharacterBody GetTrackingTarget()
	{
		return trackingTarget;
	}

	private void OnEnable()
	{
		indicator.set_active(true);
	}

	private void OnDisable()
	{
		indicator.set_active(false);
	}

	private void OnDestroy()
	{
		indicator.set_active(false);
	}

	private void FixedUpdate()
	{
		if (((NetworkBehaviour)this).get_hasAuthority())
		{
			trackerUpdateStopwatch += Time.fixedDeltaTime;
			if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
			{
				trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
				Ray aimRay = new Ray(inputBank.get_aimOrigin(), inputBank.get_aimDirection());
				SearchForTarget(aimRay);
				indicator.targetTransform = (((Object)(object)trackingTarget) ? trackingTarget.transform : null);
			}
		}
	}

	private void SearchForTarget(Ray aimRay)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		if (trackEnemy)
		{
			search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.get_teamIndex());
		}
		else
		{
			search.teamMaskFilter = friendlies;
		}
		search.filterByLoS = true;
		search.searchOrigin = aimRay.origin;
		search.searchDirection = aimRay.direction;
		search.sortMode = (SortMode)2;
		search.maxDistanceFilter = maxTrackingDistance;
		search.set_maxAngleFilter(maxTrackingAngle);
		search.RefreshCandidates();
		search.FilterOutGameObject(((Component)this).gameObject);
		HurtBox val = search.GetResults().FirstOrDefault();
		if ((bool)(Object)(object)val && (bool)(Object)(object)val.healthComponent && (bool)(Object)(object)val.healthComponent.body)
		{
			if ((Object)(object)val.healthComponent.body != (Object)(object)trackingTarget)
			{
				CallCmdSetTarget(((Component)(object)val.healthComponent.body).gameObject);
			}
		}
		else if ((Object)(object)trackingTarget != null)
		{
			CallCmdSetTarget(null);
		}
	}

	[Command]
	public void CmdSetTarget(GameObject targetNet)
	{
		CallRpcSetTarget(targetNet);
	}

	[ClientRpc]
	public void RpcSetTarget(GameObject targetNet)
	{
		if ((bool)targetNet && (bool)(Object)(object)targetNet.GetComponent<CharacterBody>())
		{
			trackingTarget = targetNet.GetComponent<CharacterBody>();
		}
		else
		{
			trackingTarget = null;
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdSetTarget(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.get_active())
		{
			Debug.LogError("Command CmdSetTarget called on client.");
		}
		else
		{
			((TargetTracker)(object)obj).CmdSetTarget(reader.ReadGameObject());
		}
	}

	public void CallCmdSetTarget(GameObject targetNet)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (!NetworkClient.get_active())
		{
			Debug.LogError("Command function CmdSetTarget called on server.");
			return;
		}
		if (((NetworkBehaviour)this).get_isServer())
		{
			CmdSetTarget(targetNet);
			return;
		}
		NetworkWriter val = new NetworkWriter();
		val.Write((short)0);
		val.Write((short)5);
		val.WritePackedUInt32((uint)kCmdCmdSetTarget);
		val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
		val.Write(targetNet);
		((NetworkBehaviour)this).SendCommandInternal(val, 0, "CmdSetTarget");
	}

	protected static void InvokeRpcRpcSetTarget(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.get_active())
		{
			Debug.LogError("RPC RpcSetTarget called on server.");
		}
		else
		{
			((TargetTracker)(object)obj).RpcSetTarget(reader.ReadGameObject());
		}
	}

	public void CallRpcSetTarget(GameObject targetNet)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (!NetworkServer.get_active())
		{
			Debug.LogError("RPC Function RpcSetTarget called on client.");
			return;
		}
		NetworkWriter val = new NetworkWriter();
		val.Write((short)0);
		val.Write((short)2);
		val.WritePackedUInt32((uint)kRpcRpcSetTarget);
		val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
		val.Write(targetNet);
		((NetworkBehaviour)this).SendRPCInternal(val, 0, "RpcSetTarget");
	}

	static TargetTracker()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		kCmdCmdSetTarget = 615813580;
		NetworkBehaviour.RegisterCommandDelegate(typeof(TargetTracker), kCmdCmdSetTarget, new CmdDelegate(InvokeCmdCmdSetTarget));
		kRpcRpcSetTarget = -977117514;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TargetTracker), kRpcRpcSetTarget, new CmdDelegate(InvokeRpcRpcSetTarget));
		NetworkCRC.RegisterBehaviour("TargetTracker", 0);
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
