using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;


using CommsRadioAPI;
using CustomCamera.CommsRadio;

namespace DvMod.CustomCamera
{
    public static class Main
    {
        public static ModEntry modEntry;
        public static Settings settings { get; private set; }
        public static string DisplayName { get { return modEntry.Info.DisplayName; } }
        public static string Id { get { return modEntry.Info.Id; } }
        public static CommsRadioMode CommsRadioMode { get; private set; }

        public static Vector3 customVector;
        public static Quaternion customQuaternion;

        public static bool OnLoad(ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            if (!modEntry.Enabled) { return false; }
            ControllerAPI.Ready += CameraMode.Create;
            PlayerManager.PlayerChanged += Initializer.Initialize;
            try
            {
                Settings? loaded = Settings.Load<Settings>(modEntry);
                settings = loaded.version == modEntry.Info.Version ? loaded : new Settings();
            }
            catch
            {
                settings = new Settings();
            }
            modEntry.OnGUI = settings.Draw;
            modEntry.OnSaveGUI = settings.Save;
            Harmony harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
            return true;
        }

        private static bool OnToggle(ModEntry modEntry, bool value)
        {
            Harmony harmony = new Harmony(modEntry.Info.Id);

            if (value)
            {
                harmony.PatchAll();
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }

        public static void Unsubscribe()
        {
            PlayerManager.PlayerChanged -= Initializer.Initialize;
        }

        public static void DebugLog(string message)
        {
            //if (settings.isLoggingEnabled)
            modEntry?.Logger.Log(message);
        }
    }
}