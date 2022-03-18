using System.Runtime.InteropServices;
using EntityStates;
using RiskOfRuinaMod.SkillStates;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace RiskOfRuinaMod.Modules.Components
{

	public class RedMistEmotionComponent : NetworkBehaviour
	{
		[SyncVar]
		public bool inEGO = false;

		public float maxEmotion = 100f;

		[SyncVar]
		public float currentEmotion;

		[SyncVar]
		private float EGOage = 0f;

		private bool bossMode = false;

		private uint playID;

		private bool isPlaying;

		private bool exitRequested = false;

		private CharacterBody characterBody;

		private Animator modelAnimator;

		private HealthComponent healthComponent;

		private RedMistStatTracker statTracker;

		private static int kCmdCmdAddEmotion;

		private static int kCmdCmdExitEgo;

		private static int kCmdCmdDeathExitEgo;

		private static int kRpcRpcExitEgo;

		private static int kRpcRpcDeathExitEgo;

		private static int kRpcRpcAddEmotion;

		public bool NetworkinEGO
		{
			get
			{
				return inEGO;
			}
			[param: In]
			set
			{
				((NetworkBehaviour)this).SetSyncVar<bool>(value, ref inEGO, 1u);
			}
		}

		public float NetworkcurrentEmotion
		{
			get
			{
				return currentEmotion;
			}
			[param: In]
			set
			{
				((NetworkBehaviour)this).SetSyncVar<float>(value, ref currentEmotion, 2u);
			}
		}

		public float NetworkEGOage
		{
			get
			{
				return EGOage;
			}
			[param: In]
			set
			{
				((NetworkBehaviour)this).SetSyncVar<float>(value, ref EGOage, 4u);
			}
		}

		private void Awake()
		{
			characterBody = ((Component)this).gameObject.GetComponent<CharacterBody>();
			modelAnimator = ((Component)this).gameObject.GetComponentInChildren<Animator>();
			healthComponent = characterBody.get_healthComponent();
			statTracker = ((Component)this).gameObject.GetComponent<RedMistStatTracker>();
			if (SceneManager.GetActiveScene().name.Contains("moon") || SceneManager.GetActiveScene().name.Contains("limbo") || SceneManager.GetActiveScene().name.Contains("goldshores"))
			{
				bossMode = true;
				((MonoBehaviour)this).Invoke("SendEGOMessage", 2f);
			}
			((MonoBehaviour)this).Invoke("CheckSkill", 0.2f);
			((MonoBehaviour)this).InvokeRepeating("CheckAlive", 0.25f, 0.25f);
		}

		private void FixedUpdate()
		{
			if (!bossMode && (bool)(Object)(object)characterBody.get_master() && (bool)(Object)(object)((Component)(object)characterBody.get_master()).GetComponent<BaseAI>())
			{
				bossMode = true;
			}
			if (currentEmotion < maxEmotion || inEGO)
			{
				float num = Config.emotionDecay.Value;
				if (inEGO)
				{
					num += EGOage * Config.EGOAgeRatio.Value;
				}
				if (bossMode)
				{
					num = 0f;
				}
				if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive())
				{
					num = 0f;
				}
				SpendEmotion(num);
			}
			if (inEGO)
			{
				NetworkEGOage = EGOage + Time.fixedDeltaTime;
				if (((NetworkBehaviour)this).get_hasAuthority() && currentEmotion <= 0f && !exitRequested)
				{
					ExitEGO();
					exitRequested = true;
				}
				if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && ((NetworkBehaviour)this).get_isServer() && RiskOfRuinaPlugin.KombatIsWaiting(((Component)(object)characterBody).gameObject))
				{
					ExitEGO();
					exitRequested = true;
				}
			}
			if (((NetworkBehaviour)this).get_isServer() && bossMode && currentEmotion < maxEmotion)
			{
				CallCmdAddEmotion(100f);
			}
			if (((NetworkBehaviour)this).get_hasAuthority() && RiskOfRuinaPlugin.DEBUG_MODE && Input.GetKeyDown(KeyCode.Z))
			{
				CallCmdAddEmotion(100f);
			}
		}

		private void CheckAlive()
		{
			if (inEGO && (bool)(Object)(object)healthComponent && !healthComponent.get_alive())
			{
				DeathExitEGO();
			}
		}

		private void OnDestroy()
		{
			StopMusic();
		}

		private void OnDisable()
		{
			StopMusic();
		}

		private void SendEGOMessage()
		{
			if (statTracker.argalia)
			{
				Chat.AddMessage("<color=#075ed9>This performance will be my final enlightenment.</color>");
				Chat.AddMessage("<color=#075ed9>Let's have a dance together.</color>");
			}
			else
			{
				Chat.AddMessage("<color=#ad0e0e>Guess it's time to stop holding back...</color>");
				Chat.AddMessage("<color=#ad0e0e>Let me show you how to actually wield EGO.</color>");
			}
		}

		private void CheckSkill()
		{
			if ((bool)(Object)(object)characterBody && (bool)(Object)(object)characterBody.get_skillLocator() && characterBody.get_skillLocator().special.get_skillNameToken() != "COF_REDMIST_BODY_SPECIAL_EGO_NAME")
			{
				Object.Destroy((Object)(object)this);
			}
		}

		public bool AddEmotion()
		{
			return AddEmotion(2f);
		}

		public bool SpendEmotion(float amount)
		{
			NetworkcurrentEmotion = Mathf.Clamp(currentEmotion - amount, 0f, maxEmotion);
			return true;
		}

		public bool AddEmotion(float amount)
		{
			if (!((NetworkBehaviour)this).get_isServer())
			{
				return false;
			}
			CallRpcAddEmotion(amount);
			return true;
		}

		public void EnterEGO()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			NetworkinEGO = true;
			NetworkcurrentEmotion = 100f;
			NetworkEGOage = 0f;
			if (((NetworkBehaviour)this).get_isServer() && (bool)(Object)(object)healthComponent)
			{
				healthComponent.Heal(healthComponent.get_fullHealth(), default(ProcChainMask), true);
			}
			StartMusic();
			if ((bool)modelAnimator)
			{
				modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("EGO"), 1f);
			}
		}

		public void ExitEGO()
		{
			CallCmdExitEgo();
		}

		public void DeathExitEGO()
		{
			CallCmdDeathExitEgo();
		}

		private void StartMusic()
		{
			if (Config.themeMusic.Value)
			{
				playID = Util.PlaySound(statTracker.musicName, ((Component)this).gameObject);
				Music.musicSources++;
				isPlaying = true;
			}
		}

		private void StopMusic()
		{
			if (Config.themeMusic.Value && isPlaying)
			{
				isPlaying = false;
				AkSoundEngine.PostEvent("StopThemes", ((Component)this).gameObject);
				Music.musicSources--;
			}
		}

		[Command]
		public void CmdAddEmotion(float amount)
		{
			CallRpcAddEmotion(amount);
		}

		[Command]
		public void CmdExitEgo()
		{
			CallRpcExitEgo();
		}

		[Command]
		public void CmdDeathExitEgo()
		{
			CallRpcDeathExitEgo();
		}

		[ClientRpc]
		public void RpcExitEgo()
		{
			NetworkinEGO = false;
			NetworkcurrentEmotion = 0f;
			exitRequested = false;
			StopMusic();
			if (((NetworkBehaviour)this).get_isServer() && !Config.redMistBuffMaintain.Value)
			{
				characterBody.RemoveBuff(Buffs.EGOBuff);
				int buffCount = characterBody.GetBuffCount(Buffs.RedMistBuff);
				for (int i = 0; i < buffCount; i++)
				{
					characterBody.RemoveBuff(Buffs.RedMistBuff);
				}
			}
			if ((bool)modelAnimator)
			{
				modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("EGO"), 0f);
			}
			EntityStateMachine val = null;
			EntityStateMachine[] components = ((Component)this).gameObject.GetComponents<EntityStateMachine>();
			foreach (EntityStateMachine val2 in components)
			{
				if ((bool)(Object)(object)val2 && val2.customName == "Body")
				{
					val = val2;
				}
			}
			if ((bool)(Object)(object)val)
			{
				val.SetNextState((EntityState)(object)new EGODeactivate());
			}
		}

		[ClientRpc]
		public void RpcDeathExitEgo()
		{
			NetworkinEGO = false;
			NetworkcurrentEmotion = 0f;
			exitRequested = false;
			StopMusic();
			if ((bool)modelAnimator)
			{
				modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("EGO"), 0f);
			}
		}

		[ClientRpc]
		public void RpcAddEmotion(float amount)
		{
			if (currentEmotion < maxEmotion)
			{
				NetworkcurrentEmotion = Mathf.Clamp(currentEmotion + amount, 0f, maxEmotion);
			}
		}

		private void UNetVersion()
		{
		}

		protected static void InvokeCmdCmdAddEmotion(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkServer.get_active())
			{
				Debug.LogError("Command CmdAddEmotion called on client.");
			}
			else
			{
				((RedMistEmotionComponent)(object)obj).CmdAddEmotion(reader.ReadSingle());
			}
		}

		protected static void InvokeCmdCmdExitEgo(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkServer.get_active())
			{
				Debug.LogError("Command CmdExitEgo called on client.");
			}
			else
			{
				((RedMistEmotionComponent)(object)obj).CmdExitEgo();
			}
		}

		protected static void InvokeCmdCmdDeathExitEgo(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkServer.get_active())
			{
				Debug.LogError("Command CmdDeathExitEgo called on client.");
			}
			else
			{
				((RedMistEmotionComponent)(object)obj).CmdDeathExitEgo();
			}
		}

		public void CallCmdAddEmotion(float amount)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkClient.get_active())
			{
				Debug.LogError("Command function CmdAddEmotion called on server.");
				return;
			}
			if (((NetworkBehaviour)this).get_isServer())
			{
				CmdAddEmotion(amount);
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)5);
			val.WritePackedUInt32((uint)kCmdCmdAddEmotion);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			val.Write(amount);
			((NetworkBehaviour)this).SendCommandInternal(val, 0, "CmdAddEmotion");
		}

		public void CallCmdExitEgo()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkClient.get_active())
			{
				Debug.LogError("Command function CmdExitEgo called on server.");
				return;
			}
			if (((NetworkBehaviour)this).get_isServer())
			{
				CmdExitEgo();
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)5);
			val.WritePackedUInt32((uint)kCmdCmdExitEgo);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			((NetworkBehaviour)this).SendCommandInternal(val, 0, "CmdExitEgo");
		}

		public void CallCmdDeathExitEgo()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkClient.get_active())
			{
				Debug.LogError("Command function CmdDeathExitEgo called on server.");
				return;
			}
			if (((NetworkBehaviour)this).get_isServer())
			{
				CmdDeathExitEgo();
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)5);
			val.WritePackedUInt32((uint)kCmdCmdDeathExitEgo);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			((NetworkBehaviour)this).SendCommandInternal(val, 0, "CmdDeathExitEgo");
		}

		protected static void InvokeRpcRpcExitEgo(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkClient.get_active())
			{
				Debug.LogError("RPC RpcExitEgo called on server.");
			}
			else
			{
				((RedMistEmotionComponent)(object)obj).RpcExitEgo();
			}
		}

		protected static void InvokeRpcRpcDeathExitEgo(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkClient.get_active())
			{
				Debug.LogError("RPC RpcDeathExitEgo called on server.");
			}
			else
			{
				((RedMistEmotionComponent)(object)obj).RpcDeathExitEgo();
			}
		}

		protected static void InvokeRpcRpcAddEmotion(NetworkBehaviour obj, NetworkReader reader)
		{
			if (!NetworkClient.get_active())
			{
				Debug.LogError("RPC RpcAddEmotion called on server.");
			}
			else
			{
				((RedMistEmotionComponent)(object)obj).RpcAddEmotion(reader.ReadSingle());
			}
		}

		public void CallRpcExitEgo()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkServer.get_active())
			{
				Debug.LogError("RPC Function RpcExitEgo called on client.");
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)2);
			val.WritePackedUInt32((uint)kRpcRpcExitEgo);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			((NetworkBehaviour)this).SendRPCInternal(val, 0, "RpcExitEgo");
		}

		public void CallRpcDeathExitEgo()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkServer.get_active())
			{
				Debug.LogError("RPC Function RpcDeathExitEgo called on client.");
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)2);
			val.WritePackedUInt32((uint)kRpcRpcDeathExitEgo);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			((NetworkBehaviour)this).SendRPCInternal(val, 0, "RpcDeathExitEgo");
		}

		public void CallRpcAddEmotion(float amount)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (!NetworkServer.get_active())
			{
				Debug.LogError("RPC Function RpcAddEmotion called on client.");
				return;
			}
			NetworkWriter val = new NetworkWriter();
			val.Write((short)0);
			val.Write((short)2);
			val.WritePackedUInt32((uint)kRpcRpcAddEmotion);
			val.Write(((Component)this).GetComponent<NetworkIdentity>().get_netId());
			val.Write(amount);
			((NetworkBehaviour)this).SendRPCInternal(val, 0, "RpcAddEmotion");
		}

		static RedMistEmotionComponent()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			kCmdCmdAddEmotion = -1322102294;
			NetworkBehaviour.RegisterCommandDelegate(typeof(RedMistEmotionComponent), kCmdCmdAddEmotion, new CmdDelegate(InvokeCmdCmdAddEmotion));
			kCmdCmdExitEgo = 1206351519;
			NetworkBehaviour.RegisterCommandDelegate(typeof(RedMistEmotionComponent), kCmdCmdExitEgo, new CmdDelegate(InvokeCmdCmdExitEgo));
			kCmdCmdDeathExitEgo = -2026065301;
			NetworkBehaviour.RegisterCommandDelegate(typeof(RedMistEmotionComponent), kCmdCmdDeathExitEgo, new CmdDelegate(InvokeCmdCmdDeathExitEgo));
			kRpcRpcExitEgo = -319326711;
			NetworkBehaviour.RegisterRpcDelegate(typeof(RedMistEmotionComponent), kRpcRpcExitEgo, new CmdDelegate(InvokeRpcRpcExitEgo));
			kRpcRpcDeathExitEgo = -1942633151;
			NetworkBehaviour.RegisterRpcDelegate(typeof(RedMistEmotionComponent), kRpcRpcDeathExitEgo, new CmdDelegate(InvokeRpcRpcDeathExitEgo));
			kRpcRpcAddEmotion = 836641344;
			NetworkBehaviour.RegisterRpcDelegate(typeof(RedMistEmotionComponent), kRpcRpcAddEmotion, new CmdDelegate(InvokeRpcRpcAddEmotion));
			NetworkCRC.RegisterBehaviour("RedMistEmotionComponent", 0);
		}

		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			if (forceAll)
			{
				writer.Write(inEGO);
				writer.Write(currentEmotion);
				writer.Write(EGOage);
				return true;
			}
			bool flag = false;
			if ((((NetworkBehaviour)this).get_syncVarDirtyBits() & (true ? 1u : 0u)) != 0)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(((NetworkBehaviour)this).get_syncVarDirtyBits());
					flag = true;
				}
				writer.Write(inEGO);
			}
			if ((((NetworkBehaviour)this).get_syncVarDirtyBits() & 2u) != 0)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(((NetworkBehaviour)this).get_syncVarDirtyBits());
					flag = true;
				}
				writer.Write(currentEmotion);
			}
			if ((((NetworkBehaviour)this).get_syncVarDirtyBits() & 4u) != 0)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(((NetworkBehaviour)this).get_syncVarDirtyBits());
					flag = true;
				}
				writer.Write(EGOage);
			}
			if (!flag)
			{
				writer.WritePackedUInt32(((NetworkBehaviour)this).get_syncVarDirtyBits());
			}
			return flag;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (initialState)
			{
				inEGO = reader.ReadBoolean();
				currentEmotion = reader.ReadSingle();
				EGOage = reader.ReadSingle();
				return;
			}
			int num = (int)reader.ReadPackedUInt32();
			if (((uint)num & (true ? 1u : 0u)) != 0)
			{
				inEGO = reader.ReadBoolean();
			}
			if (((uint)num & 2u) != 0)
			{
				currentEmotion = reader.ReadSingle();
			}
			if (((uint)num & 4u) != 0)
			{
				EGOage = reader.ReadSingle();
			}
		}
	}

}