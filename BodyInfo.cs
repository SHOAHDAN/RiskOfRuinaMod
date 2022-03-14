// Decompiled with JetBrains decompiler
// Type: BodyInfo
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using UnityEngine;

internal class BodyInfo
{
  internal string bodyName = "";
  internal string bodyNameToken = "";
  internal string subtitleNameToken = "";
  internal string bodyNameToClone = "Commando";
  internal Texture characterPortrait = (Texture) null;
  internal GameObject crosshair = (GameObject) null;
  internal GameObject podPrefab = (GameObject) null;
  internal float maxHealth = 100f;
  internal float healthGrowth = 2f;
  internal float healthRegen = 0.0f;
  internal float shield = 0.0f;
  internal float shieldGrowth = 0.0f;
  internal float moveSpeed = 7f;
  internal float moveSpeedGrowth = 0.0f;
  internal float acceleration = 80f;
  internal float jumpPower = 15f;
  internal float jumpPowerGrowth = 0.0f;
  internal float damage = 12f;
  internal float attackSpeed = 1f;
  internal float attackSpeedGrowth = 0.0f;
  internal float armor = 0.0f;
  internal float armorGrowth = 0.0f;
  internal float crit = 1f;
  internal float critGrowth = 0.0f;
  internal float sprintSpeedMult = 1.45f;
  internal int jumpCount = 1;
  internal Color bodyColor = Color.grey;
  internal Vector3 aimOriginPosition = new Vector3(0.0f, 1.8f, 0.0f);
  internal Vector3 modelBasePosition = new Vector3(0.0f, -0.92f, 0.0f);
  internal Vector3 cameraPivotPosition = new Vector3(0.0f, 1.6f, 0.0f);
}
