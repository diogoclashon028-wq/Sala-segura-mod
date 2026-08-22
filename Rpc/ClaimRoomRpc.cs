using Hazel;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using SafeZoneMod.Managers;

namespace SafeZoneMod.Rpc
{
    public enum ModCalls : uint { ClaimRoom = 1 }

    [RegisterCustomRpc((uint)ModCalls.ClaimRoom)]
    public class ClaimRoomRpc : PlayerCustomRpc<SafeZoneModPlugin, byte>
    {
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.Before;

        public ClaimRoomRpc(SafeZoneModPlugin plugin, uint id) : base(plugin, id) { }

        public override void Write(MessageWriter writer, byte data)
        {
            writer.Write(data);
        }

        public override byte Read(MessageReader reader) => reader.ReadByte();

        public override void Handle(PlayerControl innerNetObject, byte data)
        {
            if (innerNetObject == null) return;
            SafeZoneManager.SetClaimedRoom(innerNetObject.PlayerId, (SystemTypes)data);
        }
    }
}
