using GTModTemplate.Patches;
using GTModTemplate.Utilities;
using BepInEx;
using UnityEngine;
using Utilla.Attributes;

namespace GTModTemplate;

// See Constants.cs to edit these arguments
[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
[BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.0")]
[ModdedGamemode]
public class Main : BaseUnityPlugin
{
    public static Main? Instance;
    private GameObject? ForestBarrier;

    // The starting point of the mod. This methodd is called when the mod is loaded.
    private void Start()
    {
        Instance ??= this;
        HarmonyPatches.Patch();
        
        // Here is the spot to load any Asset Bundles in your program.

        // MethodUtilities.Attempt is a wrapper for the try block, we use it
        // here to prevent your mod from breaking every other mod in the case
        // that your OnPlayerSpawned() method causes errors.
        GorillaTagger.OnPlayerSpawned(() => MethodUtilities.Attempt(OnPlayerSpawned));
        
        // This would be the spot to unload all of your AssetBundles when your
        // assets have been loaded in. Save some memory!
        AssetBundleUtilities.FreeCache();
    }

    // This method is called when the player spawns into the world.
    private void OnPlayerSpawned()
    {
        ForestBarrier = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToHoverboard/TurnOnInForestAndHoverboard/ForestDome_CollisionOnly");
        if (ForestBarrier == null)
        {
            print("Unable to find forest barrier!!");
        }
    }

    [ModdedGamemodeJoin]
    private void OnJoin()
    {
        print("Disabling Forest Barrier");
        ForestBarrier.SetActive(false);
    }

    [ModdedGamemodeLeave]
    private void OnLeave()
    {
        print("Enabling Forest Barrier");
        ForestBarrier.SetActive(true);
    }

}
