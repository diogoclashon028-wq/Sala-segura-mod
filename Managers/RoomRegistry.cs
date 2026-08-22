using System.Collections.Generic;
using UnityEngine;

namespace SafeZoneMod.Managers
{
    public static class RoomRegistry
    {
        private static readonly List<PlainShipRoom> Rooms = new();

        public static void Refresh()
        {
            Rooms.Clear();
            var found = Object.FindObjectsOfType<PlainShipRoom>();
            foreach (var r in found)
            {
                if (r != null && r.roomArea != null) Rooms.Add(r);
            }
        }

        public static PlainShipRoom? FindRoomAt(float x, float y)
        {
            var point = new Vector2(x, y);
            foreach (var r in Rooms)
            {
                if (r.roomArea.OverlapPoint(point)) return r;
            }
            return null;
        }

        public static PlainShipRoom? FindRoomById(SystemTypes id)
        {
            foreach (var r in Rooms)
            {
                if (r.RoomId == id) return r;
            }
            return null;
        }
    }
}
