using HarmonyLib;
using SafeZoneMod.Managers;

namespace SafeZoneMod.Patches
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlPatches
    {
        [HarmonyPatch(nameof(PlayerControl.CheckMurder))]
        [HarmonyPrefix]
        public static bool CheckMurder_Prefix(PlayerControl __instance, PlayerControl target)
        {
            if (target?.Data == null) return true;
            var pos = target.GetTruePosition();
            return !SafeZoneManager.IsProtected(target.PlayerId, pos.x, pos.y);
        }

        [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
        [HarmonyPrefix]
        public static bool MurderPlayer_Prefix(PlayerControl target)
        {
            if (target?.Data == null) return true;
            var pos = target.GetTruePosition();
            return !SafeZoneManager.IsProtected(target.PlayerId, pos.x, pos.y);
        }
    }
}
