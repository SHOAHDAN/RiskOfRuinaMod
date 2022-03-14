// Decompiled with JetBrains decompiler
// Type: RiskOfRuinaMod.Modules.ContentPacks
// Assembly: RiskOfRuinaMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC89EB2D-2E0B-40F4-9AF1-10089A417494
// Assembly location: C:\Users\Meme\AppData\Roaming\r2modmanPlus-local\RiskOfRain2\profiles\modtest\BepInEx\plugins\Scoops-Risk_Of_Ruina\RiskOfRuinaMod.dll

using RoR2.ContentManagement;
using System.Collections;

namespace RiskOfRuinaMod.Modules
{
  internal class ContentPacks : IContentPackProvider
  {
    internal ContentPack contentPack = new ContentPack();

    public string identifier => "com.Scoops.RiskOfRuina";

    public void Initialize() => ContentManager.collectContentPackProviders += new ContentManager.CollectContentPackProvidersDelegate((object) this, __methodptr(ContentManager_collectContentPackProviders));

    private void ContentManager_collectContentPackProviders(
      ContentManager.AddContentPackProviderDelegate addContentPackProvider)
    {
      addContentPackProvider.Invoke((IContentPackProvider) this);
    }

    public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
    {
      this.contentPack.identifier = this.identifier;
      this.contentPack.bodyPrefabs.Add(Prefabs.bodyPrefabs.ToArray());
      this.contentPack.buffDefs.Add(Buffs.buffDefs.ToArray());
      this.contentPack.effectDefs.Add(Assets.effectDefs.ToArray());
      this.contentPack.entityStateTypes.Add(States.entityStates.ToArray());
      this.contentPack.masterPrefabs.Add(Prefabs.masterPrefabs.ToArray());
      this.contentPack.networkSoundEventDefs.Add(Assets.networkSoundEventDefs.ToArray());
      this.contentPack.projectilePrefabs.Add(Prefabs.projectilePrefabs.ToArray());
      this.contentPack.skillDefs.Add(Skills.skillDefs.ToArray());
      this.contentPack.skillFamilies.Add(Skills.skillFamilies.ToArray());
      this.contentPack.survivorDefs.Add(Prefabs.survivorDefinitions.ToArray());
      this.contentPack.unlockableDefs.Add(Unlockables.unlockableDefs.ToArray());
      args.ReportProgress(1f);
      yield break;
    }

    public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
    {
      ContentPack.Copy(this.contentPack, args.output);
      args.ReportProgress(1f);
      yield break;
    }

    public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
    {
      args.ReportProgress(1f);
      yield break;
    }
  }
}
