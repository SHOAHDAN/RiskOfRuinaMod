// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Components.RedMistEmotionComponent
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using EntityStates;
using RiskOfRuinaMod.SkillStates;
using RoR2;
using RoR2.CharacterAI;
using System.Runtime.InteropServices;
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
    private float EGOage = 0.0f;
    private bool bossMode = false;
    private uint playID;
    private bool isPlaying;
    private bool exitRequested = false;
    private CharacterBody characterBody;
    private Animator modelAnimator;
    private HealthComponent healthComponent;
    private RedMistStatTracker statTracker;
    private static int kCmdCmdAddEmotion = -1322102294;
    private static int kCmdCmdExitEgo;
    private static int kCmdCmdDeathExitEgo;
    private static int kRpcRpcExitEgo;
    private static int kRpcRpcDeathExitEgo;
    private static int kRpcRpcAddEmotion;

    private void Awake()
    {
      this.characterBody = ((Component) this).gameObject.GetComponent<CharacterBody>();
      this.modelAnimator = ((Component) this).gameObject.GetComponentInChildren<Animator>();
      this.healthComponent = this.characterBody.healthComponent;
      this.statTracker = ((Component) this).gameObject.GetComponent<RedMistStatTracker>();
      Scene activeScene1 = SceneManager.GetActiveScene();
      int num;
      if (!((Scene) ref activeScene1).name.Contains("moon"))
      {
        Scene activeScene2 = SceneManager.GetActiveScene();
        if (!((Scene) ref activeScene2).name.Contains("limbo"))
        {
          Scene activeScene3 = SceneManager.GetActiveScene();
          num = ((Scene) ref activeScene3).name.Contains("goldshores") ? 1 : 0;
          goto label_4;
        }
      }
      num = 1;
label_4:
      if (num != 0)
      {
        this.bossMode = true;
        ((MonoBehaviour) this).Invoke("SendEGOMessage", 2f);
      }
      ((MonoBehaviour) this).Invoke("CheckSkill", 0.2f);
      ((MonoBehaviour) this).InvokeRepeating("CheckAlive", 0.25f, 0.25f);
    }

    private void FixedUpdate()
    {
      if (!this.bossMode && Object.op_Implicit((Object) this.characterBody.master) && Object.op_Implicit((Object) ((Component) this.characterBody.master).GetComponent<BaseAI>()))
        this.bossMode = true;
      if ((double) this.currentEmotion < (double) this.maxEmotion || this.inEGO)
      {
        float amount = Config.emotionDecay.Value;
        if (this.inEGO)
          amount += this.EGOage * Config.EGOAgeRatio.Value;
        if (this.bossMode)
          amount = 0.0f;
        if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive())
          amount = 0.0f;
        this.SpendEmotion(amount);
      }
      if (this.inEGO)
      {
        this.NetworkEGOage = this.EGOage + Time.fixedDeltaTime;
        if (this.hasAuthority && (double) this.currentEmotion <= 0.0 && !this.exitRequested)
        {
          this.ExitEGO();
          this.exitRequested = true;
        }
        if (RiskOfRuinaPlugin.kombatArenaInstalled && RiskOfRuinaPlugin.KombatGamemodeActive() && this.isServer && RiskOfRuinaPlugin.KombatIsWaiting(((Component) this.characterBody).gameObject))
        {
          this.ExitEGO();
          this.exitRequested = true;
        }
      }
      if (this.isServer && this.bossMode && (double) this.currentEmotion < (double) this.maxEmotion)
        this.CallCmdAddEmotion(100f);
      if (!this.hasAuthority || !RiskOfRuinaPlugin.DEBUG_MODE || !Input.GetKeyDown((KeyCode) 122))
        return;
      this.CallCmdAddEmotion(100f);
    }

    private void CheckAlive()
    {
      if (!this.inEGO || !Object.op_Implicit((Object) this.healthComponent) || this.healthComponent.alive)
        return;
      this.DeathExitEGO();
    }

    private void OnDestroy() => this.StopMusic();

    private void OnDisable() => this.StopMusic();

    private void SendEGOMessage()
    {
      if (this.statTracker.argalia)
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
      if (!Object.op_Implicit((Object) this.characterBody) || !Object.op_Implicit((Object) this.characterBody.skillLocator) || !(this.characterBody.skillLocator.special.skillNameToken != "COF_REDMIST_BODY_SPECIAL_EGO_NAME"))
        return;
      Object.Destroy((Object) this);
    }

    public bool AddEmotion() => this.AddEmotion(2f);

    public bool SpendEmotion(float amount)
    {
      this.NetworkcurrentEmotion = Mathf.Clamp(this.currentEmotion - amount, 0.0f, this.maxEmotion);
      return true;
    }

    public bool AddEmotion(float amount)
    {
      if (!this.isServer)
        return false;
      this.CallRpcAddEmotion(amount);
      return true;
    }

    public void EnterEGO()
    {
      this.NetworkinEGO = true;
      this.NetworkcurrentEmotion = 100f;
      this.NetworkEGOage = 0.0f;
      if (this.isServer && Object.op_Implicit((Object) this.healthComponent))
      {
        double num = (double) this.healthComponent.Heal(this.healthComponent.fullHealth, new ProcChainMask(), true);
      }
      this.StartMusic();
      if (!Object.op_Implicit((Object) this.modelAnimator))
        return;
      this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("EGO"), 1f);
    }

    public void ExitEGO() => this.CallCmdExitEgo();

    public void DeathExitEGO() => this.CallCmdDeathExitEgo();

    private void StartMusic()
    {
      if (!Config.themeMusic.Value)
        return;
      this.playID = Util.PlaySound(this.statTracker.musicName, ((Component) this).gameObject);
      ++Music.musicSources;
      this.isPlaying = true;
    }

    private void StopMusic()
    {
      if (!Config.themeMusic.Value || !this.isPlaying)
        return;
      this.isPlaying = false;
      int num = (int) AkSoundEngine.PostEvent("StopThemes", ((Component) this).gameObject);
      --Music.musicSources;
    }

    [Command]
    public void CmdAddEmotion(float amount) => this.CallRpcAddEmotion(amount);

    [Command]
    public void CmdExitEgo() => this.CallRpcExitEgo();

    [Command]
    public void CmdDeathExitEgo() => this.CallRpcDeathExitEgo();

    [ClientRpc]
    public void RpcExitEgo()
    {
      this.NetworkinEGO = false;
      this.NetworkcurrentEmotion = 0.0f;
      this.exitRequested = false;
      this.StopMusic();
      if (this.isServer && !Config.redMistBuffMaintain.Value)
      {
        this.characterBody.RemoveBuff(Buffs.EGOBuff);
        int buffCount = this.characterBody.GetBuffCount(Buffs.RedMistBuff);
        for (int index = 0; index < buffCount; ++index)
          this.characterBody.RemoveBuff(Buffs.RedMistBuff);
      }
      if (Object.op_Implicit((Object) this.modelAnimator))
        this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("EGO"), 0.0f);
      EntityStateMachine entityStateMachine = (EntityStateMachine) null;
      foreach (EntityStateMachine component in ((Component) this).gameObject.GetComponents<EntityStateMachine>())
      {
        if (Object.op_Implicit((Object) component) && component.customName == "Body")
          entityStateMachine = component;
      }
      if (!Object.op_Implicit((Object) entityStateMachine))
        return;
      entityStateMachine.SetNextState((EntityState) new EGODeactivate());
    }

    [ClientRpc]
    public void RpcDeathExitEgo()
    {
      this.NetworkinEGO = false;
      this.NetworkcurrentEmotion = 0.0f;
      this.exitRequested = false;
      this.StopMusic();
      if (!Object.op_Implicit((Object) this.modelAnimator))
        return;
      this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("EGO"), 0.0f);
    }

    [ClientRpc]
    public void RpcAddEmotion(float amount)
    {
      if ((double) this.currentEmotion >= (double) this.maxEmotion)
        return;
      this.NetworkcurrentEmotion = Mathf.Clamp(this.currentEmotion + amount, 0.0f, this.maxEmotion);
    }

    private void UNetVersion()
    {
    }

    public bool NetworkinEGO
    {
      get => this.inEGO;
      [param: In] set => this.SetSyncVar<bool>(value, ref this.inEGO, 1U);
    }

    public float NetworkcurrentEmotion
    {
      get => this.currentEmotion;
      [param: In] set => this.SetSyncVar<float>(value, ref this.currentEmotion, 2U);
    }

    public float NetworkEGOage
    {
      get => this.EGOage;
      [param: In] set => this.SetSyncVar<float>(value, ref this.EGOage, 4U);
    }

    protected static void InvokeCmdCmdAddEmotion(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkServer.active)
        Debug.LogError((object) "Command CmdAddEmotion called on client.");
      else
        ((RedMistEmotionComponent) obj).CmdAddEmotion(reader.ReadSingle());
    }

    protected static void InvokeCmdCmdExitEgo(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkServer.active)
        Debug.LogError((object) "Command CmdExitEgo called on client.");
      else
        ((RedMistEmotionComponent) obj).CmdExitEgo();
    }

    protected static void InvokeCmdCmdDeathExitEgo(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkServer.active)
        Debug.LogError((object) "Command CmdDeathExitEgo called on client.");
      else
        ((RedMistEmotionComponent) obj).CmdDeathExitEgo();
    }

    public void CallCmdAddEmotion(float amount)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "Command function CmdAddEmotion called on server.");
      else if (this.isServer)
      {
        this.CmdAddEmotion(amount);
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 5);
        networkWriter.WritePackedUInt32((uint) RedMistEmotionComponent.kCmdCmdAddEmotion);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        networkWriter.Write(amount);
        this.SendCommandInternal(networkWriter, 0, "CmdAddEmotion");
      }
    }

    public void CallCmdExitEgo()
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "Command function CmdExitEgo called on server.");
      else if (this.isServer)
      {
        this.CmdExitEgo();
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 5);
        networkWriter.WritePackedUInt32((uint) RedMistEmotionComponent.kCmdCmdExitEgo);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        this.SendCommandInternal(networkWriter, 0, "CmdExitEgo");
      }
    }

    public void CallCmdDeathExitEgo()
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "Command function CmdDeathExitEgo called on server.");
      else if (this.isServer)
      {
        this.CmdDeathExitEgo();
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 5);
        networkWriter.WritePackedUInt32((uint) RedMistEmotionComponent.kCmdCmdDeathExitEgo);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        this.SendCommandInternal(networkWriter, 0, "CmdDeathExitEgo");
      }
    }

    protected static void InvokeRpcRpcExitEgo(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "RPC RpcExitEgo called on server.");
      else
        ((RedMistEmotionComponent) obj).RpcExitEgo();
    }

    protected static void InvokeRpcRpcDeathExitEgo(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "RPC RpcDeathExitEgo called on server.");
      else
        ((RedMistEmotionComponent) obj).RpcDeathExitEgo();
    }

    protected static void InvokeRpcRpcAddEmotion(NetworkBehaviour obj, NetworkReader reader)
    {
      if (!NetworkClient.active)
        Debug.LogError((object) "RPC RpcAddEmotion called on server.");
      else
        ((RedMistEmotionComponent) obj).RpcAddEmotion(reader.ReadSingle());
    }

    public void CallRpcExitEgo()
    {
      if (!NetworkServer.active)
      {
        Debug.LogError((object) "RPC Function RpcExitEgo called on client.");
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 2);
        networkWriter.WritePackedUInt32((uint) RedMistEmotionComponent.kRpcRpcExitEgo);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        this.SendRPCInternal(networkWriter, 0, "RpcExitEgo");
      }
    }

    public void CallRpcDeathExitEgo()
    {
      if (!NetworkServer.active)
      {
        Debug.LogError((object) "RPC Function RpcDeathExitEgo called on client.");
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 2);
        networkWriter.WritePackedUInt32((uint) RedMistEmotionComponent.kRpcRpcDeathExitEgo);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        this.SendRPCInternal(networkWriter, 0, "RpcDeathExitEgo");
      }
    }

    public void CallRpcAddEmotion(float amount)
    {
      if (!NetworkServer.active)
      {
        Debug.LogError((object) "RPC Function RpcAddEmotion called on client.");
      }
      else
      {
        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short) 0);
        networkWriter.Write((short) 2);
        networkWriter.WritePackedUInt32((uint) RedMistEmotionComponent.kRpcRpcAddEmotion);
        networkWriter.Write(((Component) this).GetComponent<NetworkIdentity>().netId);
        networkWriter.Write(amount);
        this.SendRPCInternal(networkWriter, 0, "RpcAddEmotion");
      }
    }

    static RedMistEmotionComponent()
    {
      // ISSUE: method pointer
      NetworkBehaviour.RegisterCommandDelegate(typeof (RedMistEmotionComponent), RedMistEmotionComponent.kCmdCmdAddEmotion, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeCmdCmdAddEmotion)));
      RedMistEmotionComponent.kCmdCmdExitEgo = 1206351519;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterCommandDelegate(typeof (RedMistEmotionComponent), RedMistEmotionComponent.kCmdCmdExitEgo, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeCmdCmdExitEgo)));
      RedMistEmotionComponent.kCmdCmdDeathExitEgo = -2026065301;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterCommandDelegate(typeof (RedMistEmotionComponent), RedMistEmotionComponent.kCmdCmdDeathExitEgo, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeCmdCmdDeathExitEgo)));
      RedMistEmotionComponent.kRpcRpcExitEgo = -319326711;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterRpcDelegate(typeof (RedMistEmotionComponent), RedMistEmotionComponent.kRpcRpcExitEgo, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeRpcRpcExitEgo)));
      RedMistEmotionComponent.kRpcRpcDeathExitEgo = -1942633151;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterRpcDelegate(typeof (RedMistEmotionComponent), RedMistEmotionComponent.kRpcRpcDeathExitEgo, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeRpcRpcDeathExitEgo)));
      RedMistEmotionComponent.kRpcRpcAddEmotion = 836641344;
      // ISSUE: method pointer
      NetworkBehaviour.RegisterRpcDelegate(typeof (RedMistEmotionComponent), RedMistEmotionComponent.kRpcRpcAddEmotion, new NetworkBehaviour.CmdDelegate((object) null, __methodptr(InvokeRpcRpcAddEmotion)));
      NetworkCRC.RegisterBehaviour(nameof (RedMistEmotionComponent), 0);
    }

    public virtual bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
      if (forceAll)
      {
        writer.Write(this.inEGO);
        writer.Write(this.currentEmotion);
        writer.Write(this.EGOage);
        return true;
      }
      bool flag = false;
      if (((int) this.syncVarDirtyBits & 1) != 0)
      {
        if (!flag)
        {
          writer.WritePackedUInt32(this.syncVarDirtyBits);
          flag = true;
        }
        writer.Write(this.inEGO);
      }
      if (((int) this.syncVarDirtyBits & 2) != 0)
      {
        if (!flag)
        {
          writer.WritePackedUInt32(this.syncVarDirtyBits);
          flag = true;
        }
        writer.Write(this.currentEmotion);
      }
      if (((int) this.syncVarDirtyBits & 4) != 0)
      {
        if (!flag)
        {
          writer.WritePackedUInt32(this.syncVarDirtyBits);
          flag = true;
        }
        writer.Write(this.EGOage);
      }
      if (!flag)
        writer.WritePackedUInt32(this.syncVarDirtyBits);
      return flag;
    }

    public virtual void OnDeserialize(NetworkReader reader, bool initialState)
    {
      if (initialState)
      {
        this.inEGO = reader.ReadBoolean();
        this.currentEmotion = reader.ReadSingle();
        this.EGOage = reader.ReadSingle();
      }
      else
      {
        int num = (int) reader.ReadPackedUInt32();
        if ((num & 1) != 0)
          this.inEGO = reader.ReadBoolean();
        if ((num & 2) != 0)
          this.currentEmotion = reader.ReadSingle();
        if ((num & 4) == 0)
          return;
        this.EGOage = reader.ReadSingle();
      }
    }
  }
}
