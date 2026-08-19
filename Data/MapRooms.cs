using System.Collections.Generic;

namespace SafeZoneMod.Data
{
    // MapId: 0=Skeld 1=MiraHQ 2=Polus 3=Airship 4=Fungle
    // TODO: só Skeld tem coordenadas reais. Os outros ainda precisam ser
    // preenchidos testando dentro do jogo.
    public static class MapRooms
    {
        public static readonly Dictionary<byte, List<RoomZone>> Rooms = new()
        {
            [0] = new List<RoomZone>
            {
                new("Cafeteria", -3f, 3f, 5f, 9f),
                new("Weapons", 6f, 4f, 10f, 8f),
                new("Navigation", 12f, 1f, 16f, 5f),
                new("Shields", 10f, -6f, 15f, -2f),
                new("Communications", 2f, -10f, 7f, -6f),
                new("Storage", -2f, -9f, 3f, -4f),
                new("Electrical", -8f, -6f, -3f, -2f),
                new("Medbay", -9f, 1f, -4f, 5f),
                new("Security", -12f, -2f, -8f, 1f),
                new("Reactor", -18f, -3f, -13f, 2f),
                new("Admin", -1f, -2f, 3f, 1f),
            },
            [1] = new List<RoomZone>(),
            [2] = new List<RoomZone>(),
            [3] = new List<RoomZone>(),
            [4] = new List<RoomZone>(),
        };
    }
}
