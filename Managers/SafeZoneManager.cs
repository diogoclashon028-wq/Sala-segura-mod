using System.Collections.Generic;
using UnityEngine;

namespace SafeZoneMod.Managers
{
    public static class SafeZoneManager
    {
        private static readonly Dictionary<byte, SystemTypes> ClaimedRooms = new();
        private static readonly Dictionary<byte, float> StayStartTime = new();
        private static readonly Dictionary<byte, float> LastExitTime = new();

        public static bool SelectionPhaseOpen { get; set; }

        public static void Reset()
        {
            ClaimedRooms.Clear();
            StayStartTime.Clear();
            LastExitTime.Clear();
            SelectionPhaseOpen = true;
            RoomRegistry.Refresh();
        }

        public static void SetClaimedRoom(byte playerId, SystemTypes roomId)
        {
            ClaimedRooms[playerId] = roomId;
            StayStartTime.Remove(playerId);
            LastExitTime.Remove(playerId);
        }

        public static SystemTypes? GetClaimedRoom(byte playerId) =>
            ClaimedRooms.TryGetValue(playerId, out var room) ? room : null;

        public static bool IsRoomFull(SystemTypes roomId, byte excludingPlayerId = 255)
        {
            int max = SafeZoneModPlugin.MaxPlayersPerRoom.Value;
            if (max <= 0) return false;

            int count = 0;
            foreach (var kvp in ClaimedRooms)
            {
                if (kvp.Key == excludingPlayerId) continue;
                if (kvp.Value == roomId) count++;
            }
            return count >= max;
        }

        public static void Tick()
        {
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return;
            float now = Time.time;
            float grace = SafeZoneModPlugin.LeaveGraceSeconds.Value;

            foreach (var kvp in ClaimedRooms)
            {
                byte playerId = kvp.Key;
                var player = FindPlayer(playerId);
                if (player == null) continue;

                var pos = player.GetTruePosition();
                var room = RoomRegistry.FindRoomById(kvp.Value);
                bool inside = room != null && room.roomArea.OverlapPoint(new Vector2(pos.x, pos.y));

                if (inside)
                {
                    LastExitTime.Remove(playerId);
                    if (!StayStartTime.ContainsKey(playerId))
                    {
                        StayStartTime[playerId] = now;
                    }
                }
                else
                {
                    if (!LastExitTime.ContainsKey(playerId))
                    {
                        LastExitTime[playerId] = now;
                    }
                    else if (now - LastExitTime[playerId] > grace)
                    {
                        StayStartTime.Remove(playerId);
                        LastExitTime.Remove(playerId);
                    }
                }
            }
        }

        public static bool IsProtected(byte playerId, float x, float y)
        {
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return false;
            if (!ClaimedRooms.TryGetValue(playerId, out var roomId)) return false;

            var room = RoomRegistry.FindRoomById(roomId);
            if (room == null || !room.roomArea.OverlapPoint(new Vector2(x, y))) return false;

            float maxStay = SafeZoneModPlugin.MaxStayDuration.Value;
            if (maxStay <= 0f) return true;

            float start = StayStartTime.TryGetValue(playerId, out var s) ? s : Time.time;
            return Time.time - start <= maxStay;
        }

        public static bool IsProtected(byte playerId)
        {
            var player = FindPlayer(playerId);
            if (player == null) return false;
            var pos = player.GetTruePosition();
            return IsProtected(playerId, pos.x, pos.y);
        }

        public static float? GetRemainingStaySeconds(byte playerId)
        {
            float maxStay = SafeZoneModPlugin.MaxStayDuration.Value;
            if (maxStay <= 0f) return null;
            if (!StayStartTime.TryGetValue(playerId, out var start)) return maxStay;
            float remaining = maxStay - (Time.time - start);
            return remaining > 0f ? remaining : 0f;
        }

        private static PlayerControl? FindPlayer(byte playerId)
        {
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p.PlayerId == playerId) return p;
            }
            return null;
        }
    }
}
