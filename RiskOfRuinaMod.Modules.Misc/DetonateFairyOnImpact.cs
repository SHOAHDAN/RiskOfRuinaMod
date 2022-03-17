using System.Collections.Generic;
using System.Runtime.InteropServices;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Misc;

[RequireComponent(typeof(ProjectileController))]
public class DetonateFairyOnImpact : NetworkBehaviour, IProjectileImpactBehavior
{
	[SyncVar]
	public float radius;

	private ProjectileController controller;

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
		controller = ((Component)this).GetComponent<ProjectileController>();
	}

	public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		if (!NetworkServer.get_active())
		{
			return;
		}
		float radiusSqr = radius * radius;
		Vector3 position = ((Component)this).transform.position;
		TeamIndex val = (TeamIndex)0;
		while ((int)val < 4)
		{
			if (val != controller.get_teamFilter().get_teamIndex())
			{
				FairyBurst(TeamComponent.GetTeamMembers(val), radiusSqr, position);
			}
			val = (TeamIndex)(sbyte)(val + 1);
		}
	}

	private void FairyBurst(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		if (!NetworkServer.get_active())
		{
			return;
		}
		foreach (TeamComponent recipient in recipients)
		{
			if (!((((Component)(object)recipient).transform.position - currentPosition).sqrMagnitude <= radiusSqr))
			{
				continue;
			}
			CharacterBody body = recipient.get_body();
			if (!(Object)(object)body)
			{
				continue;
			}
			DotController val = DotController.FindDotController(((Component)(object)body).gameObject);
			if (!(Object)(object)val || !val.HasDotActive(DoTCore.FairyIndex))
			{
				continue;
			}
			for (int i = 0; i < val.dotStackList.Count; i++)
			{
				DotStack val2 = val.dotStackList[i];
				if (val2.dotIndex == DoTCore.FairyIndex)
				{
					DamageInfo val3 = new DamageInfo
					{
						attacker = val2.attackerObject,
						inflictor = val2.attackerObject,
						crit = val2.attackerObject.GetComponent<CharacterBody>().RollCrit(),
						damage = val2.attackerObject.GetComponent<CharacterBody>().get_damage() * 1f,
						position = body.get_corePosition(),
						force = Vector3.zero,
						damageType = (DamageType)0,
						damageColorIndex = (DamageColorIndex)2,
						dotIndex = DoTCore.FairyIndex,
						procCoefficient = 0.75f
					};
					body.get_healthComponent().TakeDamage(val3);
					GlobalEventManager.instance.OnHitEnemy(val3, ((Component)(object)body).gameObject);
					GlobalEventManager.instance.OnHitAll(val3, ((Component)(object)body).gameObject);
				}
			}
			EffectData val4 = new EffectData();
			val4.rotation = Util.QuaternionSafeLookRotation(Vector3.zero);
			val4.set_origin(body.get_corePosition());
			EffectManager.SpawnEffect(Assets.fairyProcEffect, val4, false);
		}
	}

	private void UNetVersion()
	{
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
}
