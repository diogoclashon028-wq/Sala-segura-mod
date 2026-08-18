using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI.PluginLoading;
using MiraAPI;
using Reactor;

namespace SafeZoneMod
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    [BepInDependency(MiraApiPlugin.Id)]
    public class SafeZoneModPlugin : BasePlugin, IMiraPlugin
    {
        public const string Id = "com.seunome.safezonemod";
        public const string Name = "Safe Zone Mod";
        public const string Version = "1.0.0";
        public static SafeZoneModPlugin Instance { get; private set; } = null!;
        public static ManualLogSource Log => ((BasePlugin)Instance).Log;
        public string OptionsTitleText => "Safe Zone";
        public ConfigFile GetConfigFile() => Config;
        public static ConfigEntry<bool> SafeZoneEnabled = null!;
        public static ConfigEntry<float> SafeZoneRadius = null!;
        public static ConfigEntry<float> MaxStayDuration = null!;
        private readonly Harmony _harmony = new(Id);

        public override void Load()
        {
            Instance = this;
            SafeZoneEnabled = Config.Bind("Geral", "Ativar Zona Segura", true, "");
            SafeZoneRadius = Config.Bind("Geral", "Raio da Zona", 2.5f, "");
            MaxStayDuration = Config.Bind("Geral", "Tempo max de permanencia (s)", 15f, "");
            _harmony.PatchAll();
            Log.LogInfo($"{Name} v{Version} carregado");
        }
    }
}
