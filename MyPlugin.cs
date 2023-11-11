using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using AnimateOnTwos.Patches;
using Reptile;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;

namespace AnimateOnTwos
{

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class AnimateOnTwosPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.cspotcode.AnimateOnTwos";
        private const string PluginName = "AnimateOnTwos";
        private const string VersionString = "1.0.0";

        // Config
        internal static int PlayerTargetFps = 24;
        internal static int PedestrianTargetFps = 8;

        public static string PlayerTargetFpsConfigKey = "Player animation fps";
        public static string PedestrianTargetFpsConfigKey = "Pedestrian animation fps";
        public static ConfigEntry<int> PlayerTargetFpsConfig;
        public static ConfigEntry<int> PedestrianTargetFpsConfig;

        private Harmony harmony;
        public static Player player;

        internal static ConditionalWeakTable<Player, AnimatorPatchState> PlayerPatchStates = new ConditionalWeakTable<Player, AnimatorPatchState>();
        internal static ConditionalWeakTable<Pedestrian, AnimatorPatchState> PedestrianPatchStates = new ConditionalWeakTable<Pedestrian, AnimatorPatchState>();
        internal static HashSet<Pedestrian> Pedestrians = new HashSet<Pedestrian>();

        private FileSystemWatcher ConfigFileWatcher;
        private object ConfigFileChangedLock = new object();
        private bool ConfigDirty = false;

        private void Awake()
        {
            harmony = new Harmony(MyGUID);

            PlayerTargetFpsConfig = Config.Bind("General",
                PlayerTargetFpsConfigKey,
                PlayerTargetFps,
                new ConfigDescription("Set to zero to disable effect",
                    new AcceptableValueRange<int>(0, 60)));
            PedestrianTargetFpsConfig= Config.Bind("General",
                PedestrianTargetFpsConfigKey,
                PedestrianTargetFps,
                new ConfigDescription("Set to zero to disable effect",
                    new AcceptableValueRange<int>(0, 60)));

            PlayerTargetFpsConfig.SettingChanged += ConfigSettingChanged;
            PedestrianTargetFpsConfig.SettingChanged += ConfigSettingChanged;

            ConfigFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(Config.ConfigFilePath),
                Filter = Path.GetFileName(Config.ConfigFilePath),
                // NotifyFilter = NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.CreationTime
            };
            ConfigFileWatcher.Changed += ConfigSettingChanged;
            ConfigFileWatcher.EnableRaisingEvents = true;

            ReloadConfig();

            harmony.PatchAll(typeof(PlayerPatch));
            harmony.PatchAll(typeof(PedestrianPatch));
        }

        private void ConfigSettingChanged(object sender, System.EventArgs e) {
            lock(ConfigFileChangedLock) {
                ConfigDirty = true;
            }
        }

        private void ReloadConfig() {
            lock(ConfigFileChangedLock) {
                Config.Reload();
                PlayerTargetFps = (int)PlayerTargetFpsConfig.BoxedValue;
                PedestrianTargetFps = (int)PedestrianTargetFpsConfig.BoxedValue;
                ConfigDirty = false;
            }
        }

        private void Update() {
            if(ConfigDirty) {
                ReloadConfig();
            }
            PedestrianPatch.Update();
        }
    }
}
