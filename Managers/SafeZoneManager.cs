using System.Collections.Generic;
using UnityEngine;

namespace SafeZoneMod.Managers
{
    public static class SafeZoneManager
    {
        private static readonly Dictionary<byte, float> PlayersInZone = new();

        public static void Reset()
        {
            PlayersInZone.Clear();
        }

        public static void OnPlayerEnter(byte playerId)
        {
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return;

            if (!PlayersInZone.ContainsKey(playerId))
            {
                PlayersInZone[playerId] = Time.time;
            }
        }

        public static void OnPlayerExit(byte playerId)
        {
            PlayersInZone.Remove(playerId);
        }

        public static bool IsProtected(byte playerId)
        {
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return false;
            if (!PlayersInZone.TryGetValue(playerId, out var enteredAt)) return false;

            var maxStay = SafeZoneModPlugin.MaxStayDuration.Value;
            if (maxStay <= 0f) return true;

            return Time.time - enteredAt <= maxStay;
        }

        public static float? GetRemainingTime(byte playerId)
        {
            if (!PlayersInZone.TryGetValue(playerId, out var enteredAt)) return null;

            var maxStay = SafeZoneModPlugin.MaxStayDuration.Value;
            if (maxStay <= 0f) return -1f;

            var remaining = maxStay - (Time.time - enteredAt);
            return remaining > 0f ? remaining : null;
        }

        public static void Tick()
        {
            var maxStay = SafeZoneModPlugin.MaxStayDuration.Value;
            if (maxStay <= 0f || PlayersInZone.Count == 0) return;

            List<byte>? expirados = null;
            foreach (var kvp in PlayersInZone)
            {
                if (Time.time - kvp.Value > maxStay)
                {
                    expirados ??= new List<byte>();
                    expirados.Add(kvp.Key);
                }
            }

            if (expirados == null) return;
            foreach (var id in expirados) PlayersInZone.Remove(id);
        }
    }
}
