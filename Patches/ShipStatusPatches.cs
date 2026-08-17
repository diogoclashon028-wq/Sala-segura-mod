using HarmonyLib;
using SafeZoneMod.Components;
using SafeZoneMod.Managers;
using UnityEngine;

namespace SafeZoneMod.Patches
{
    [HarmonyPatch(typeof(ShipStatus))]
    public static class ShipStatusPatches
    {
        private const string ZoneObjectName = "SafeZoneMod_Trigger";

        [HarmonyPatch(nameof(ShipStatus.Begin))]
        [HarmonyPostfix]
        public static void Begin_Postfix()
        {
            SafeZoneManager.Reset();

            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return;
            if (GameObject.Find(ZoneObjectName) != null) return;

            var zone = new GameObject(ZoneObjectName);
            zone.transform.position = new Vector3(0f, 0f, 0f);
            zone.AddComponent<SafeZoneTrigger>();
        }

        [HarmonyPatch(nameof(ShipStatus.OnDestroy))]
        [HarmonyPostfix]
        public static void OnDestroy_Postfix()
        {
            SafeZoneManager.Reset();
        }
    }
}
