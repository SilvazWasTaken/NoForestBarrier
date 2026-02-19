using GTModTemplate.Patches;
using GTModTemplate.Utilities;
using BepInEx;
using UnityEngine;
using Utilla.Attributes;
using UnityEngine.SceneManagement;

namespace GTModTemplate;

// See Constants.cs to edit these arguments
[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
[BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.0")]
[ModdedGamemode]
public class Main : BaseUnityPlugin
{
    public static Main? Instance;
    private GameObject? ForestBarrier;
    private GameObject? MountainBarrier;
    private GameObject? CanyonBarrier;

    private bool InModded;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

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

        InModded = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Mountain")
        {
            MountainBarrier = GameObject.Find("Mountain/Mountain_ForceVolumes/MountainDomeCollision");

            if (MountainBarrier == null)
                print("Mountain barrier not found!");
            else
                MountainBarrier.SetActive(!InModded);
        }

        if (scene.name == "Canyon2")
        {
            CanyonBarrier = GameObject.Find("Canyon/Canyon/Canyon_ForceVolumes/CanyonDome_CollisionOnly");

            if (CanyonBarrier == null)
                print("Canyon barrier not found!");
            else
                CanyonBarrier.SetActive(!InModded);
        }
    }

    [ModdedGamemodeJoin]
    private void OnJoin()
    {
        print("Disabling Barriers");
        if (ForestBarrier != null) ForestBarrier.SetActive(false);
        if (MountainBarrier != null) MountainBarrier.SetActive(false);
        if (CanyonBarrier != null) CanyonBarrier.SetActive(false);

        InModded = true;
    }

    [ModdedGamemodeLeave]
    private void OnLeave()
    {
        print("Enabling Barriers");
        if (ForestBarrier != null) ForestBarrier.SetActive(true);
        if (MountainBarrier != null) MountainBarrier.SetActive(true);
        if (CanyonBarrier != null) CanyonBarrier.SetActive(true);

        InModded = false;
    }

}
