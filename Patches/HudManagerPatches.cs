using HarmonyLib;
using SafeZoneMod.Components;
using SafeZoneMod.Managers;
using UnityEngine;

namespace SafeZoneMod.Patches
{
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerPatches
    {
        [HarmonyPatch(nameof(HudManager.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix(HudManager __instance)
        {
            if (__instance.GetComponentInChildren<SafeZoneIndicator>() != null) return;

            var go = new GameObject("SafeZoneMod_Indicator");
            go.AddComponent<SafeZoneIndicator>();
        }
    }

    [HarmonyPatch(typeof(PlayerControl))]
    public static class HudManagerFeedbackPatches
    {
        [HarmonyPatch(nameof(PlayerControl.CheckMurder))]
        [HarmonyPostfix]
        public static void CheckMurder_Postfix(PlayerControl __instance, PlayerControl target)
        {
            if (__instance != PlayerControl.LocalPlayer) return;
            if (target?.Data == null) return;
            if (!SafeZoneManager.IsProtected(target.PlayerId)) return;

            var hud = HudManager.Instance;
            if (hud == null) return;

            SafeZoneModPlugin.Log.LogInfo($"{target.Data.PlayerName} está protegido (kill bloqueado)");
        }
    }
}
