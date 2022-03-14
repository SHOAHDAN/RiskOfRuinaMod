// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.Tokens
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using R2API;
using System;

namespace RiskOfRuinaMod.Modules
{
  internal static class Tokens
  {
    internal static void AddTokens()
    {
      string str1 = "COF_REDMIST_BODY_";
      string str2 = "Red Mist is an aggresive melee survivor with a versatile primary attack and a powerful but temporary transformation.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine + "< ! > Her primary, Level Slash, performs different combos depending on what directional buttons you are pressing. She is stronger while stationary, but her mobile attacks give her a short burst of invulnerability." + Environment.NewLine + Environment.NewLine + "< ! > Onrush is a mobility tool that excels at dealing with swarms of weak enemies. Try to target the weakest enemies with it first, so that you can continue chaining it while you get kills." + Environment.NewLine + Environment.NewLine + "< ! > Evade is a simple but powerful move. Any of her basic attacks (and some specials) can be canceled into Evade, allowing you to react to your enemies whenever needed." + Environment.NewLine + Environment.NewLine + "< ! > EGO is Red Mist's transformation ability. Fill her EGO bar by doing damage, large groups of enemies will make this quicker. EGO also becomes easier to gain as a run progresses. Upon transformation, every one of her skills will be modified to be more versatile while doing the same damage. EGO drains faster the longer you are transformed, so you will need to be aggressive to maintain your new state." + Environment.NewLine + Environment.NewLine;
      string str3 = "..and so she left, walking out of a sea of pain.";
      string str4 = "..and so she remained, lost to hatred.";
      LanguageAPI.Add(str1 + "NAME", "Red Mist");
      LanguageAPI.Add(str1 + "DESCRIPTION", str2);
      LanguageAPI.Add(str1 + "SUBTITLE", "The Strongest ");
      LanguageAPI.Add(str1 + "LORE", "The Red Mist.");
      LanguageAPI.Add(str1 + "OUTRO_FLAVOR", str3);
      LanguageAPI.Add(str1 + "OUTRO_FAILURE", str4);
      LanguageAPI.Add(str1 + "DEFAULT_SKIN_NAME", "Default");
      LanguageAPI.Add(str1 + "MASTERY_SKIN_NAME", "Mastery: Conductor");
      LanguageAPI.Add(str1 + "PASSIVE_NAME", "Gebura's Prowess");
      LanguageAPI.Add(str1 + "PASSIVE_DESCRIPTION", "All <style=cIsDamage>Attack Speed</style> and <style=cIsUtility>Movement Speed</style> bonuses are converted into <style=cIsDamage>Damage</style>.");
      LanguageAPI.Add(str1 + "PRIMARY_LEVELSLASH_NAME", "Level Slash");
      LanguageAPI.Add(str1 + "PRIMARY_LEVELSLASH_DESCRIPTION", "Perform attack combos for varying damage. <color=#7a21a3>This move is affected by directional input/jumping.</color>");
      LanguageAPI.Add(str1 + "PRIMARY_UPSTANDINGSLASH_NAME", "Upstanding Slash");
      LanguageAPI.Add(str1 + "PRIMARY_UPSTANDINGSLASH_DESCRIPTION", "<style=cIsUtility>Slayer</style>. Perform attack combos for varying damage. <color=#7a21a3>This move is affected by directional input/jumping.</color>");
      LanguageAPI.Add(str1 + "SECONDARY_ONRUSH_NAME", "Onrush");
      LanguageAPI.Add(str1 + "SECONDARY_ONRUSH_DESCRIPTION", string.Format("Dash to a targeted enemy and deal <style=cIsDamage>{0}% damage</style>. If you kill the enemy, <style=cIsUtility>you can dash again</style>.", (object) 400f));
      LanguageAPI.Add(str1 + "UTILITY_DODGE_NAME", "Evade");
      LanguageAPI.Add(str1 + "UTILITY_DODGE_DESCRIPTION", "<style=cIsUtility>Evade</style> all attacks for a split second while repositioning.");
      LanguageAPI.Add(str1 + "UTILITY_BLOCK_NAME", "Counter");
      LanguageAPI.Add(str1 + "UTILITY_BLOCK_DESCRIPTION", string.Format("<style=cIsUtility>Block</style> all attacks for a split second, then counter for <style=cIsDamage>{0}% of damage recieved</style>. Hold to block additional attacks before countering.", (object) 150f));
      LanguageAPI.Add(str1 + "SPECIAL_EGO_NAME", "Red Mist: EGO");
      LanguageAPI.Add(str1 + "SPECIAL_EGO_DESCRIPTION", "<color=red>100% EGO</color>. Activate EGO Mode, replacing special with <style=cIsDamage>Greater Split</style> and <style=cIsUtility>upgrading your other abilities</style>.");
      LanguageAPI.Add(str1 + "SPECIAL_HORIZONTAL_NAME", "Greater Split: Horizontal");
      LanguageAPI.Add(str1 + "SPECIAL_HORIZONTAL_DESCRIPTION", string.Format("Swing your sword in a massive arc, dealing <style=cIsDamage>{0}% damage</style>.", (object) 2000f));
      LanguageAPI.Add("KEYWORD_EGO", string.Format("<color=red><style=cKeywordName>EGO Mode</style></color><style=cSub>Gain <color=red>EGO</color> by <style=cIsDamage>damaging enemies</style>. Drains over time. While in EGO Mode, gain a <style=cIsDamage>{0}% base damage increase</style> for every enemy killed.", (object) (float) (100.0 * (double) Config.redMistBuffDamage.Value)));
      LanguageAPI.Add(str1 + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Red Mist: Mastery");
      LanguageAPI.Add(str1 + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Red Mist, beat the game or obliterate on Monsoon.");
      LanguageAPI.Add(str1 + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Red Mist: Mastery");
      LanguageAPI.Add("BROTHER_SEE_REDMIST_1", "Boss music? Foolish.");
      LanguageAPI.Add("BROTHER_KILL_REDMIST_1", "Silence that noise.");
      string str5 = "COF_ARBITER_BODY_";
      string str6 = "An Arbiter is a mid-range survivor who can manage large groups of enemies with numerous debuffs and area attacks - and can even disable high priority targets if needed.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine + "< ! > Use your basic attack to apply the Fairy debuff to enemies. They will take damage for every stack of Fairy on them every time they attack." + Environment.NewLine + Environment.NewLine + "< ! > Lock can be used to disable a dangerous enemy while you take care of smaller ones. It will become weaker with repeated uses on the same target." + Environment.NewLine + Environment.NewLine + "< ! > Pillar will make not only you and your allies faster, but also enemies. Use this with Fairy to make them damage themselves faster." + Environment.NewLine + Environment.NewLine + "< ! > Shockwave covers a massive area, dealing consistently increasing damage and weakening enemies. Use it while near allies to provide cover. Keep in mind that Shockwave requires all five stocks to cast." + Environment.NewLine + Environment.NewLine;
      string str7 = "..and so she left, the cycle broken.";
      string str8 = "'Oh Sorrow, you see, finally I have come to respect you, for I know you will never depart.'";
      LanguageAPI.Add(str5 + "NAME", "An Arbiter");
      LanguageAPI.Add(str5 + "DESCRIPTION", str6);
      LanguageAPI.Add(str5 + "SUBTITLE", "Agent of The Head ");
      LanguageAPI.Add(str5 + "LORE", "A singularity-powered assassin for A-Corp. Meeting one is rare, and living to tell the tale is unheard of.");
      LanguageAPI.Add(str5 + "OUTRO_FLAVOR", str7);
      LanguageAPI.Add(str5 + "OUTRO_FAILURE", str8);
      LanguageAPI.Add(str5 + "DEFAULT_SKIN_NAME", "Default");
      LanguageAPI.Add(str5 + "MASTERY_SKIN_NAME", "Mastery: Core Suppression");
      LanguageAPI.Add(str5 + "SECOND_SKIN_NAME", "Fire");
      LanguageAPI.Add(str5 + "THIRD_SKIN_NAME", "Turquoise");
      LanguageAPI.Add(str5 + "PASSIVE_NAME", "F Corp Singularity");
      LanguageAPI.Add(str5 + "PASSIVE_DESCRIPTION", "All attacks apply the <style=cIsUtility>Fairy</style> debuff.");
      LanguageAPI.Add("KEYWORD_FAIRY", "<color=yellow><style=cKeywordName>Fairy</style></color><style=cSub>Enemies afflicted with <style=cIsUtility>Fairy</style> " + string.Format("will take <style=cIsDamage>{0}% damage</style> whenever they use an ability.", (object) 100f));
      LanguageAPI.Add(str5 + "PRIMARY_FAIRY_NAME", "Fairy");
      LanguageAPI.Add(str5 + "PRIMARY_FAIRY_DESCRIPTION", string.Format("Fire explosive projectiles for <style=cIsDamage>{0}% damage</style>.", (object) 75f));
      LanguageAPI.Add(str5 + "SECONDARY_LOCK_NAME", "Lock");
      LanguageAPI.Add(str5 + "SECONDARY_LOCK_DESCRIPTION", string.Format("<style=cIsUtility>Lock</style> a target enemy and deal <style=cIsDamage>{0}% damage</style>.", (object) 300f));
      LanguageAPI.Add("KEYWORD_LOCK", "<color=yellow><style=cKeywordName>Lock</style></color><style=cSub>Enemies afflicted with <style=cIsUtility>Lock</style> are <style=cIsUtility>frozen in time</style> for the duration. Enemies gain resistance to repeated locking.");
      LanguageAPI.Add(str5 + "SECONDARY_UNLOCK_NAME", "Unlock");
      LanguageAPI.Add(str5 + "SECONDARY_UNLOCK_DESCRIPTION", "Consume all charges. <style=cIsUtility>Unlock</style> a target ally. Duration is increased for each charge used.");
      LanguageAPI.Add("KEYWORD_UNLOCK", "<color=yellow><style=cKeywordName>Unlock</style></color><style=cSub>Allies with <style=cIsUtility>Unlock</style> gain <style=cIsUtility>increased damage, armor, regen, attack speed, movement speed, and ability cooldown speed</style> for the duration.");
      LanguageAPI.Add(str5 + "UTILITY_PILLARS_NAME", "Ominous Pillar");
      LanguageAPI.Add(str5 + "UTILITY_PILLARS_DESCRIPTION", string.Format("A pillar emerges for <style=cIsDamage>{0}% damage</style>. Everything in the area gains double <style=cIsUtility>movement</style> and <style=cIsDamage>attack</style> speed, and enemy projectiles are <style=cIsUtility>destroyed</color>.", (object) 500f));
      LanguageAPI.Add(str5 + "UTILITY_PILLARSSPEAR_NAME", "Pillar Spear");
      LanguageAPI.Add(str5 + "UTILITY_PILLARSSPEAR_DESCRIPTION", string.Format("Charge and launch a pillar for <style=cIsDamage>{0}% - {1}% damage</style>. All enemies hit will activate their ", (object) 300f, (object) 600f) + "<style=cIsUtility>Fairy</style> stacks.");
      LanguageAPI.Add(str5 + "SPECIAL_SHOCKWAVE_NAME", "Shockwave");
      LanguageAPI.Add(str5 + "SPECIAL_SHOCKWAVE_DESCRIPTION", string.Format("Consumes all charges. Fire 3 AOE bursts for <style=cIsDamage>{0}% - {1}% damage</style>. Enemies hit are made <style=cIsUtility>Feeble</style> and allies gain <style=cIsUtility>Barrier</style>.", (object) 500f, (object) 1500f));
      LanguageAPI.Add(str5 + "SPECIAL_SCEPTERSHOCKWAVE_NAME", "Undegraded Shockwave");
      LanguageAPI.Add(str5 + "SPECIAL_SCEPTERSHOCKWAVE_DESCRIPTION", string.Format("Consumes all charges. Fire an AOE bursts for <style=cIsDamage>{0}% damage</style>. Enemies hit are made <style=cIsUtility>Feeble</style> and allies gain <style=cIsUtility>Barrier</style>.", (object) 5000f));
      LanguageAPI.Add("KEYWORD_FEEBLE", "<color=yellow><style=cKeywordName>Feeble</style></color><style=cSub>Enemies afflicted with <style=cIsUtility>Feeble</style> lose half of their <style=cIsDamage>damage</style> and <style=cIsUtility>armor</style> for the duration.");
      LanguageAPI.Add(str5 + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Arbiter: Mastery");
      LanguageAPI.Add(str5 + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As An Arbiter, beat the game or obliterate on Monsoon.");
      LanguageAPI.Add(str5 + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Arbiter: Mastery");
      string str9 = "COF_BLACKSILENCE_BODY_";
      string str10 = "Black Silence" + Environment.NewLine + Environment.NewLine;
      string str11 = "..and so he left, with the resolve to make a different choice.";
      string str12 = "..and so he remained, left with the greatest suffering at the end of it all.";
      LanguageAPI.Add(str9 + "NAME", "Black Silence");
      LanguageAPI.Add(str9 + "DESCRIPTION", str10);
      LanguageAPI.Add(str9 + "SUBTITLE", "Spectre of Vengeance ");
      LanguageAPI.Add(str9 + "LORE", "The Black Silence.");
      LanguageAPI.Add(str9 + "OUTRO_FLAVOR", str11);
      LanguageAPI.Add(str9 + "OUTRO_FAILURE", str12);
      LanguageAPI.Add(str9 + "DEFAULT_SKIN_NAME", "Default");
      LanguageAPI.Add(str9 + "MASTERY_SKIN_NAME", "Mastery: Waltz in White");
      LanguageAPI.Add(str9 + "PASSIVE_NAME", "Gebura's Prowess");
      LanguageAPI.Add(str9 + "PASSIVE_DESCRIPTION", "Red Mist can <style=cIsUtility>jump twice</style>. All <style=cIsDamage>Attack Speed</style> and <style=cIsUtility>Movement Speed</style> bonuses are converted into <style=cIsDamage>Damage</style>.");
      LanguageAPI.Add(str9 + "PRIMARY_LEVELSLASH_NAME", "Level Slash");
      LanguageAPI.Add(str9 + "PRIMARY_LEVELSLASH_DESCRIPTION", "Perform attack combos for varying damage. <color=#7a21a3>This move is affected by directional input/jumping.</color>");
      LanguageAPI.Add(str9 + "PRIMARY_UPSTANDINGSLASH_NAME", "Upstanding Slash");
      LanguageAPI.Add(str9 + "PRIMARY_UPSTANDINGSLASH_DESCRIPTION", "<style=cIsUtility>Slayer</style>. Perform attack combos for varying damage. <color=#7a21a3>This move is affected by directional input/jumping.</color>");
      LanguageAPI.Add(str9 + "SECONDARY_ONRUSH_NAME", "Onrush");
      LanguageAPI.Add(str9 + "SECONDARY_ONRUSH_DESCRIPTION", string.Format("Dash to a targeted enemy and deal <style=cIsDamage>{0}% damage</style>. If you kill the enemy, <style=cIsUtility>you can dash again</style>.", (object) 400f));
      LanguageAPI.Add(str9 + "UTILITY_DODGE_NAME", "Evade");
      LanguageAPI.Add(str9 + "UTILITY_DODGE_DESCRIPTION", "<style=cIsUtility>Evade</style> all attacks for a split second while repositioning.");
      LanguageAPI.Add(str9 + "UTILITY_BLOCK_NAME", "Counter");
      LanguageAPI.Add(str9 + "UTILITY_BLOCK_DESCRIPTION", string.Format("<style=cIsUtility>Block</style> all attacks for a split second, then counter for <style=cIsDamage>{0}% of damage recieved</style>. Hold to block additional attacks before countering.", (object) 150f));
      LanguageAPI.Add(str9 + "SPECIAL_EGO_NAME", "Red Mist: EGO");
      LanguageAPI.Add(str9 + "SPECIAL_EGO_DESCRIPTION", "<color=red>100% EGO</color>. Activate EGO Mode, replacing special with <style=cIsDamage>Greater Split</style> and <style=cIsUtility>upgrading your other abilities</style>.");
      LanguageAPI.Add(str9 + "SPECIAL_HORIZONTAL_NAME", "Greater Split: Horizontal");
      LanguageAPI.Add(str9 + "SPECIAL_HORIZONTAL_DESCRIPTION", string.Format("Swing your sword in a massive arc, dealing <style=cIsDamage>{0}% damage</style>.", (object) 2000f));
      LanguageAPI.Add("KEYWORD_EGO", string.Format("<color=red><style=cKeywordName>EGO Mode</style></color><style=cSub>Gain <color=red>EGO</color> by <style=cIsDamage>damaging enemies</style>. Drains over time. While in EGO Mode, gain a <style=cIsDamage>{0}% damage increase</style> for every enemy killed.", (object) (float) (100.0 * (double) Config.redMistBuffDamage.Value)));
      LanguageAPI.Add(str9 + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Black Silence: Mastery");
      LanguageAPI.Add(str9 + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Black Silence, beat the game or obliterate on Monsoon.");
      LanguageAPI.Add(str9 + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Black Silence: Mastery");
      LanguageAPI.Add("ARBITERTROPHY_NAME", "An Arbiter's Trophy");
      LanguageAPI.Add("ARBITERTROPHY_PICKUP", "Chance to Lock on hit");
      LanguageAPI.Add("ARBITERTROPHY_DESC", "Grants <style=cIsUtility>1%</style> <style=cStack>(+1% per item stack)</style> chance to Lock enemies on hit.");
      LanguageAPI.Add("ARBITERTROPHY_LORE", "A trophy from a past battle.");
      LanguageAPI.Add("RUINABLACKTEA_NAME", "Black Tea");
      LanguageAPI.Add("RUINABLACKTEA_PICKUP", "Chance to apply Fairy on hit");
      LanguageAPI.Add("RUINABLACKTEA_DESC", "Grants <style=cIsUtility>10%</style> <style=cStack>(+5% per item stack)</style> chance to apply Fairy to enemies on hit.");
      LanguageAPI.Add("RUINABLACKTEA_LORE", "The agony in the tea leaves is palpable.");
      LanguageAPI.Add("RUINAPRESCRIPT_NAME", "Prescript");
      LanguageAPI.Add("RUINAPRESCRIPT_PICKUP", "Increase Base Damage by number of unique items");
      LanguageAPI.Add("RUINAPRESCRIPT_DESC", "Grants <style=cIsDamage>1%</style> <style=cStack>(+1% per item stack)</style> base damage for each unique item that you possess.");
      LanguageAPI.Add("RUINAPRESCRIPT_LORE", "To ●●●. Pet quadrupedal animals five times.");
      LanguageAPI.Add("RUINALIUBADGE_NAME", "Liu Badge");
      LanguageAPI.Add("RUINALIUBADGE_PICKUP", "Increase Base Damage by number of stages cleared");
      LanguageAPI.Add("RUINALIUBADGE_DESC", "Grants <style=cIsDamage>10%</style> <style=cStack>(+5% per item stack)</style> base damage for each stage you have cleared.");
      LanguageAPI.Add("RUINALIUBADGE_LORE", "");
      LanguageAPI.Add("RUINAWORKSHOPAMMO_NAME", "Workshop Ammunition");
      LanguageAPI.Add("RUINAWORKSHOPAMMO_PICKUP", "Deal more damage the further away you are");
      LanguageAPI.Add("RUINAWORKSHOPAMMO_DESC", "Deal up to <style=cIsDamage>25%</style> <style=cStack>(+10% per item stack)</style> damage based on how far you are from your target. (Minimum 10 meters distance)");
      LanguageAPI.Add("RUINAWORKSHOPAMMO_LORE", "");
      LanguageAPI.Add("RUINAMOONLIGHTSTONE_NAME", "Moonlight Stone");
      LanguageAPI.Add("RUINAMOONLIGHTSTONE_PICKUP", "Remove debuffs over time");
      LanguageAPI.Add("RUINAMOONLIGHTSTONE_DESC", "Remove <style=cIsUtility>1 stack</style> <style=cStack>(+1 per item stack)</style> of a debuff from yourself every two seconds.");
      LanguageAPI.Add("RUINAMOONLIGHTSTONE_LORE", "");
      LanguageAPI.Add("RUINAWEDDINGRING_NAME", "Wedding Ring");
      LanguageAPI.Add("RUINAWEDDINGRING_PICKUP", "Deal more damage near allies with the same item");
      LanguageAPI.Add("RUINAWEDDINGRING_DESC", "Deal <style=cIsDamage>10%</style> <style=cStack>(+5% per item stack)</style> more damage for each stack of this item that nearby allies have.");
      LanguageAPI.Add("RUINAWEDDINGRING_LORE", "");
      LanguageAPI.Add("RUINAUDJATMASK_NAME", "Udjat Mask");
      LanguageAPI.Add("RUINAUDJATMASK_PICKUP", "Gain armor when hit");
      LanguageAPI.Add("RUINAUDJATMASK_DESC", "Gain <style=cIsUtility>5</style> <style=cStack>(+5 per item stack)</style> armor for 5 seconds each time you are hit.");
      LanguageAPI.Add("RUINAUDJATMASK_LORE", "");
      LanguageAPI.Add("RUINAREVERBERATION_NAME", "Reverberation");
      LanguageAPI.Add("RUINAREVERBERATION_PICKUP", "Attacks reflect projectiles");
      LanguageAPI.Add("RUINAREVERBERATION_DESC", "All of your attacks <style=cIsUtility>reflect</style> projectiles. Reflection range increases with stacks.");
      LanguageAPI.Add("RUINAREVERBERATION_LORE", "");
      LanguageAPI.Add("RUINABACKWARDSCLOCK_NAME", "Backwards Clock");
      LanguageAPI.Add("RUINABACKWARDSCLOCK_PICKUP", "Sacrifice yourself to resurrect all allies");
      LanguageAPI.Add("RUINABACKWARDSCLOCK_DESC", "Sacrifice yourself to resurrect all allies. Destroyed on use.");
      LanguageAPI.Add("RUINABACKWARDSCLOCK_LORE", "");
    }
  }
}
