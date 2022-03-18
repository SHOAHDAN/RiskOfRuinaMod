using System.Collections.Generic;
using System.Runtime.InteropServices;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.Modules.Misc
{

	[RequireComponent(typeof(TeamFilter))]
	public class ArbiterPillarController : NetworkBehaviour
	{
		[Tooltip("The area of effect.")]
		[SyncVar]
		public float radius;

		[Tooltip("How long between buff pulses in the area of effect.")]
		public float interval = 1f;

		[Tooltip("The child range indicator object. Will be scaled to the radius.")]
		public Transform rangeIndicator;

		[Tooltip("The buff type to grant")]
		public BuffDef buffDef;

		[Tooltip("The buff duration")]
		public float buffDuration;

		[Tooltip("Does the ward disappear over time?")]
		public bool expires;

		[Tooltip("If set, stops all projectiles in the vicinity.")]
		public bool freezeProjectiles;

		public float expireDuration;

		public bool animateRadius;

		public AnimationCurve radiusCoefficientCurve;

		[Tooltip("If set, the ward will give you this amount of time to play removal effects.")]
		public float removalTime;

		private bool needsRemovalTime;

		public string removalSoundString = "";

		public UnityEvent onRemoval;

		private float buffTimer;

		private float rangeIndicatorScaleVelocity;

		private float stopwatch;

		private float calculatedRadius;

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

		private void OnEnable()
		{
			if ((bool)rangeIndicator)
			{
				rangeIndicator.gameObject.SetActive(value: true);
			}
		}

		private void OnDisable()
		{
			if ((bool)rangeIndicator)
			{
				rangeIndicator.gameObject.SetActive(value: false);
			}
		}

		private void Start()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Invalid comparison between Unknown and I4
			Util.PlaySound("Play_Binah_Stone_Fire", ((Component)this).gameObject);
			if (removalTime > 0f)
			{
				needsRemovalTime = true;
			}
			if ((bool)rangeIndicator && expires)
			{
				ScaleParticleSystemDuration component = rangeIndicator.GetComponent<ScaleParticleSystemDuration>();
				if ((bool)(Object)(object)component)
				{
					component.set_newDuration(expireDuration);
				}
			}
			if (NetworkServer.get_active())
			{
				float radiusSqr = calculatedRadius * calculatedRadius;
				Vector3 position = ((Component)this).transform.position;
				TeamIndex val = (TeamIndex)0;
				while ((int)val < 4)
				{
					BuffTeam(TeamComponent.GetTeamMembers(val), radiusSqr, position);
					val = (TeamIndex)(sbyte)(val + 1);
				}
			}
		}

		private void Update()
		{
			calculatedRadius = (animateRadius ? (radius * radiusCoefficientCurve.Evaluate(stopwatch / expireDuration)) : radius);
			stopwatch += Time.deltaTime;
			if (expires && NetworkServer.get_active())
			{
				if (needsRemovalTime)
				{
					if (stopwatch >= expireDuration - removalTime)
					{
						needsRemovalTime = false;
						Util.PlaySound(removalSoundString, ((Component)this).gameObject);
						onRemoval.Invoke();
					}
				}
				else if (expireDuration <= stopwatch)
				{
					Object.Destroy(((Component)this).gameObject);
				}
			}
			if ((bool)rangeIndicator)
			{
				float num = Mathf.SmoothDamp(rangeIndicator.localScale.x, calculatedRadius, ref rangeIndicatorScaleVelocity, 0.2f);
				rangeIndicator.localScale = new Vector3(num, num, num);
			}
		}

		private void FixedUpdate()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Invalid comparison between Unknown and I4
			if (NetworkServer.get_active())
			{
				buffTimer -= Time.fixedDeltaTime;
				if (buffTimer <= 0f)
				{
					buffTimer = interval;
					float radiusSqr = calculatedRadius * calculatedRadius;
					Vector3 position = ((Component)this).transform.position;
					TeamIndex val = (TeamIndex)0;
					while ((int)val < 4)
					{
						BuffTeam(TeamComponent.GetTeamMembers(val), radiusSqr, position);
						val = (TeamIndex)(sbyte)(val + 1);
					}
				}
			}
			if (freezeProjectiles)
			{
				FreezeProjectiles(calculatedRadius, ((Component)this).transform.position);
			}
		}

		private void BuffTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
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

		private void FreezeProjectiles(float radius, Vector3 currentPosition)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			Collider[] array = Physics.OverlapSphere(currentPosition, radius, ((LayerIndex)(ref LayerIndex.projectile)).get_mask());
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
}