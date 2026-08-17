using HarmonyLib;
using SafeZoneMod.Managers;

namespace SafeZoneMod.Patches
{
    [HarmonyPatch(typeof(PlayerControl))]
    public static class PlayerControlPatches
    {
        [HarmonyPatch(nameof(PlayerControl.CheckMurder))]
        [HarmonyPrefix]
        public static bool CheckMurder_Prefix(PlayerControl target)
        {
            if (target?.Data == null) return true;
            if (!SafeZoneManager.IsProtected(target.PlayerId)) return true;

            return false;
        }

        [HarmonyPatch(nameof(PlayerControl.MurderPlayer))]
        [HarmonyPrefix]
        public static bool MurderPlayer_Prefix(PlayerControl target)
        {
            if (target?.Data == null) return true;
            return !SafeZoneManager.IsProtected(target.PlayerId);
        }
    }
}
