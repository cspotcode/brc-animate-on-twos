using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using SafeProjectName.Patches;
using Reptile;

namespace SafeProjectName
{

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SafeProjectNamePlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.$username$.SafeProjectName";
        private const string PluginName = "SafeProjectName";
        private const string VersionString = "1.0.0";

        private Harmony harmony;
        public static Player player;

        private void Awake()
        {
            harmony = new Harmony(MyGUID);
            harmony.PatchAll(typeof(PlayerPatch));
        }
    }
}
