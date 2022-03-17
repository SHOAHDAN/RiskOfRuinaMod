using System.Collections.Generic;
using System.Runtime.InteropServices;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Misc;

[RequireComponent(typeof(TeamFilter))]
public class ArbiterShockwaveController : NetworkBehaviour
{
	[SyncVar]
	[Tooltip("The area of effect.")]
	public float radius;

	[Tooltip("The buff type to grant")]
	public BuffDef buffDef;

	[Tooltip("The buff duration")]
	public float buffDuration;

	[Tooltip("Barrier amount (based on percentage)")]
	public float barrierAmount;

	[Tooltip("If set, destroys all projectiles in the vicinity.")]
	public bool destroyProjectiles;

	private TeamFilter teamFilter;

	public float Networkradius
	{
		get
		{
			return radius;
		}
		[param: In]
		set
		{
			((NetworkBehaviour)this).SetSyncVar<float>(value, ref radius, 1u);
		}
	}

	private void Awake()
	{
		teamFilter = ((Component)this).GetComponent<TeamFilter>();
	}

	private void Start()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (NetworkServer.get_active())
		{
			float radiusSqr = radius * radius;
			Vector3 position = ((Component)this).transform.position;
			TeamIndex val = (TeamIndex)0;
			while ((int)val < 4)
			{
				if (val != teamFilter.get_teamIndex())
				{
					HarmTeam(TeamComponent.GetTeamMembers(val), radiusSqr, position);
				}
				val = (TeamIndex)(sbyte)(val + 1);
			}
			HelpTeam(TeamComponent.GetTeamMembers(teamFilter.get_teamIndex()), radiusSqr, position);
		}
		if (destroyProjectiles)
		{
			DestroyProjectiles(radius * radius, ((Component)this).transform.position);
		}
	}

	private void HarmTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
	{
		if (!NetworkServer.get_active())
		{
			return;
		}
		foreach (TeamComponent recipient in recipients)
		{
			if ((((Component)(object)recipient).transform.position - currentPosition).sqrMagnitude <= radiusSqr)
			{
				CharacterBody body = recipient.get_body();
				if ((bool)(Object)(object)body && (bool)(Object)(object)buffDef)
				{
					body.AddTimedBuff(buffDef, buffDuration);
				}
			}
		}
	}

	private void HelpTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
	{
		if (!NetworkServer.get_active())
		{
			return;
		}
		foreach (TeamComponent recipient in recipients)
		{
			if ((((Component)(object)recipient).transform.position - currentPosition).sqrMagnitude <= radiusSqr)
			{
				CharacterBody body = recipient.get_body();
				if ((bool)(Object)(object)body && barrierAmount != 0f && (bool)(Object)(object)body.get_healthComponent())
				{
					body.get_healthComponent().AddBarrier(barrierAmount * body.get_healthComponent().get_fullBarrier());
				}
			}
		}
	}

	private void DestroyProjectiles(float radiusSqr, Vector3 currentPosition)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		Collider[] array = Physics.OverlapSphere(currentPosition, radiusSqr, ((LayerIndex)(ref LayerIndex.projectile)).get_mask());
		for (int i = 0; i < array.Length; i++)
		{
			ProjectileController component = array[i].GetComponent<ProjectileController>();
			if ((bool)(Object)(object)component)
			{
				TeamComponent component2 = component.owner.GetComponent<TeamComponent>();
				if ((bool)(Object)(object)component2 && component2.get_teamIndex() != teamFilter.get_teamIndex())
				{
					EffectData val = new EffectData();
					val.set_origin(((Component)(object)component).transform.position);
					val.scale = 4f;
					EffectManager.SpawnEffect(Assets.fairyDeleteEffect, val, false);
					Object.Destroy(((Component)(object)component).gameObject);
				}
			}
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(radius);
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
			writer.Write(radius);
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
			radius = reader.ReadSingle();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if (((uint)num & (true ? 1u : 0u)) != 0)
		{
			radius = reader.ReadSingle();
		}
	}

	private void UNetVersion()
	{
	}
}
