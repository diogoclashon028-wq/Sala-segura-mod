using Hazel;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using SafeZoneMod.Data;
using SafeZoneMod.Managers;

namespace SafeZoneMod.Rpc
{
    public enum ModCalls : uint { ClaimRoom = 1 }

    [RegisterCustomRpc((uint)ModCalls.ClaimRoom)]
    public class ClaimRoomRpc : PlayerCustomRpc<SafeZoneModPlugin, (byte mapId, byte roomIndex)>
    {
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

        public ClaimRoomRpc(SafeZoneModPlugin plugin, uint id) : base(plugin, id) { }

        public override void Write(MessageWriter writer, (byte mapId, byte roomIndex) data)
        {
            writer.Write(data.mapId);
            writer.Write(data.roomIndex);
        }

        public override (byte, byte) Read(MessageReader reader) =>
            (reader.ReadByte(), reader.ReadByte());

        public override void Handle(PlayerControl innerNetObject, (byte mapId, byte roomIndex) data)
        {
            if (innerNetObject == null) return;
            if (!MapRooms.Rooms.TryGetValue(data.mapId, out var rooms)) return;
            if (data.roomIndex >= rooms.Count) return;
            SafeZoneManager.SetClaimedRoom(innerNetObject.PlayerId, rooms[data.roomIndex]);
        }
    }
}
