using UnityEngine;

internal class BodyInfo
{
	internal string bodyName = "";

	internal string bodyNameToken = "";

	internal string subtitleNameToken = "";

	internal string bodyNameToClone = "Commando";

	internal Texture characterPortrait = null;

	internal GameObject crosshair = null;

	internal GameObject podPrefab = null;

	internal float maxHealth = 100f;

	internal float healthGrowth = 2f;

	internal float healthRegen = 0f;

	internal float shield = 0f;

	internal float shieldGrowth = 0f;

	internal float moveSpeed = 7f;

	internal float moveSpeedGrowth = 0f;

	internal float acceleration = 80f;

	internal float jumpPower = 15f;

	internal float jumpPowerGrowth = 0f;

	internal float damage = 12f;

	internal float attackSpeed = 1f;

	internal float attackSpeedGrowth = 0f;

	internal float armor = 0f;

	internal float armorGrowth = 0f;

	internal float crit = 1f;

	internal float critGrowth = 0f;

	internal float sprintSpeedMult = 1.45f;

	internal int jumpCount = 1;

	internal Color bodyColor = Color.grey;

	internal Vector3 aimOriginPosition = new Vector3(0f, 1.8f, 0f);

	internal Vector3 modelBasePosition = new Vector3(0f, -0.92f, 0f);

	internal Vector3 cameraPivotPosition = new Vector3(0f, 1.6f, 0f);
}
