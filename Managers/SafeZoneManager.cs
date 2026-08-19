using System.Collections.Generic;
using SafeZoneMod.Data;

namespace SafeZoneMod.Managers
{
    public static class SafeZoneManager
    {
        private static readonly Dictionary<byte, RoomZone> ClaimedRooms = new();

        public static bool SelectionPhaseOpen { get; set; }

        public static void Reset()
        {
            ClaimedRooms.Clear();
            SelectionPhaseOpen = true;
        }

        public static void SetClaimedRoom(byte playerId, RoomZone room) =>
            ClaimedRooms[playerId] = room;

        public static RoomZone? GetClaimedRoom(byte playerId) =>
            ClaimedRooms.TryGetValue(playerId, out var room) ? room : null;

        public static bool IsProtected(byte playerId, float x, float y)
        {
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return false;
            if (!ClaimedRooms.TryGetValue(playerId, out var room)) return false;
            return room.Contains(x, y);
        }

        public static bool IsProtected(byte playerId)
        {
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p.PlayerId == playerId)
                {
                    var pos = p.GetTruePosition();
                    return IsProtected(playerId, pos.x, pos.y);
                }
            }
            return false;
        }
    }
}
