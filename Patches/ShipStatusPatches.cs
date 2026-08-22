using System.Collections;
using HarmonyLib;
using MiraAPI.Hud;
using Reactor.Utilities;
using SafeZoneMod.Managers;
using SafeZoneMod.UI;
using UnityEngine;

namespace SafeZoneMod.Patches
{
    [HarmonyPatch(typeof(ShipStatus))]
    public static class ShipStatusPatches
    {
        [HarmonyPatch(nameof(ShipStatus.Begin))]
        [HarmonyPostfix]
        public static void Begin_Postfix()
        {
            SafeZoneManager.Reset();
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return;
            Coroutines.Start(CloseSelectionAfterDelay());
        }

        [HarmonyPatch(nameof(ShipStatus.OnDestroy))]
        [HarmonyPostfix]
        public static void OnDestroy_Postfix() => SafeZoneManager.Reset();

        private static IEnumerator CloseSelectionAfterDelay()
        {
            yield return new WaitForSeconds(SafeZoneModPlugin.SelectionWindowSeconds.Value);
            SafeZoneManager.SelectionPhaseOpen = false;

            var localPlayer = PlayerControl.LocalPlayer;
            if (localPlayer == null || localPlayer.Data == null) yield break;

            foreach (var customButton in CustomButtonManager.Buttons)
            {
                if (customButton is ClaimRoomButton)
                {
                    customButton.SetActive(false, localPlayer.Data.Role);
                }
            }
        }
    }
}
