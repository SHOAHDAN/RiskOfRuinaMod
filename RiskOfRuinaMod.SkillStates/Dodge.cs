using EntityStates;
using EntityStates.Mage;
using RiskOfRuinaMod.Modules;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RiskOfRuinaMod.SkillStates
{

	internal class Dodge : BaseSkillState
	{
		public Vector3 dodgeVector;

		public float duration = 0.65f;

		public float moveEnd = 0.65f;

		public float invulStart = 0f;

		public float invulEnd = 0.4f;

		public float stockBonus = 0.05f;

		public bool invul = false;

		public bool aerial = false;

		protected TemporaryOverlay iframeOverlay;

		public override void OnEnter()
		{
			dodgeVector = ((EntityState)this).get_inputBank().moveVector;
			if (!((EntityState)this).get_characterMotor().get_isGrounded())
			{
				aerial = true;
				((BaseState)this).SmallHop(((EntityState)this).get_characterMotor(), 10f);
			}
			if (((EntityState)this).get_skillLocator().utility.get_stock() > 1)
			{
				invulEnd += (float)(((EntityState)this).get_skillLocator().utility.get_stock() - 1) * stockBonus;
			}
			AddOverlay(invulEnd);
			((BaseState)this).OnEnter();
			Util.PlaySound("Ruina_Swipe", ((EntityState)this).get_gameObject());
			((EntityState)this).PlayCrossfade("FullBody, Override", "Dodge", "Dodge.playbackRate", duration, 0.1f);
		}

		public override void FixedUpdate()
		{
			((EntityState)this).FixedUpdate();
			if (!aerial)
			{
				if (((EntityState)this).get_fixedAge() <= moveEnd)
				{
					CharacterMotor characterMotor = ((EntityState)this).get_characterMotor();
					characterMotor.rootMotion += dodgeVector * (3.5f * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / (moveEnd * 1.3f)) * Time.fixedDeltaTime);
					((EntityState)this).get_characterMotor().velocity.y = 0f;
					CharacterMotor characterMotor2 = ((EntityState)this).get_characterMotor();
					characterMotor2.set_moveDirection(characterMotor2.get_moveDirection() * 2f);
				}
			}
			else if (((EntityState)this).get_fixedAge() <= moveEnd)
			{
				CharacterMotor characterMotor3 = ((EntityState)this).get_characterMotor();
				characterMotor3.rootMotion += dodgeVector * (2f * FlyUpState.speedCoefficientCurve.Evaluate(((EntityState)this).get_fixedAge() / (moveEnd * 1.3f)) * Time.fixedDeltaTime);
				CharacterMotor characterMotor4 = ((EntityState)this).get_characterMotor();
				characterMotor4.set_moveDirection(characterMotor4.get_moveDirection() * 2f);
			}
			if (NetworkServer.get_active() && ((EntityState)this).get_fixedAge() >= invulStart && !invul)
			{
				((EntityState)this).get_characterBody().AddBuff(Buffs.HiddenInvincibility);
				invul = true;
			}
			if (NetworkServer.get_active() && ((EntityState)this).get_fixedAge() >= invulEnd && invul)
			{
				if (((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
				{
					((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
				}
				RemoveOverlay();
				invul = false;
			}
			if (((EntityState)this).get_fixedAge() >= duration && ((EntityState)this).get_isAuthority())
			{
				((EntityState)this).outer.SetNextStateToMain();
			}
		}

		public override void OnExit()
		{
			if (NetworkServer.get_active() && ((EntityState)this).get_characterBody().HasBuff(Buffs.HiddenInvincibility))
			{
				((EntityState)this).get_characterBody().RemoveBuff(Buffs.HiddenInvincibility);
			}
			RemoveOverlay();
			((EntityState)this).OnExit();
		}

		protected void AddOverlay(float duration)
		{
			if (Config.iframeOverlay.Value)
			{
				iframeOverlay = ((Component)(object)((EntityState)this).get_characterBody()).gameObject.AddComponent<TemporaryOverlay>();
				iframeOverlay.duration = duration;
				iframeOverlay.alphaCurve = AnimationCurve.Constant(0f, duration, 0.1f);
				iframeOverlay.animateShaderAlpha = true;
				iframeOverlay.destroyComponentOnEnd = true;
				iframeOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
				iframeOverlay.AddToCharacerModel(((EntityState)this).get_modelLocator().get_modelTransform().GetComponent<CharacterModel>());
			}
		}

		protected void RemoveOverlay()
		{
			if ((bool)(Object)(object)iframeOverlay)
			{
				Object.Destroy((Object)(object)iframeOverlay);
			}
		}
	}
}