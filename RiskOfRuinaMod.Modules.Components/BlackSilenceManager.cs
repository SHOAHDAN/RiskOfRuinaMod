using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Components;

public class BlackSilenceManager : NetworkBehaviour
{
	public CharacterBody characterBody;

	public bool angelica = false;

	private void Start()
	{
		characterBody = ((Component)this).GetComponent<CharacterBody>();
	}

	private void UNetVersion()
	{
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
