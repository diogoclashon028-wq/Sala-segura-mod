using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using SafeZoneMod.Data;
using SafeZoneMod.Managers;
using SafeZoneMod.Rpc;
using UnityEngine;

namespace SafeZoneMod.UI
{
    public static class ClaimRoomLogic
    {
        public static bool TryClaimCurrentRoom()
        {
            var player = PlayerControl.LocalPlayer;
            if (player == null || !SafeZoneManager.SelectionPhaseOpen) return false;

            byte mapId = (byte)ShipStatus.Instance.Type;
            if (!MapRooms.Rooms.TryGetValue(mapId, out var rooms)) return false;

            var pos = player.GetTruePosition();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Contains(pos.x, pos.y))
                {
                    SafeZoneManager.SetClaimedRoom(player.PlayerId, rooms[i]);
                    Reactor.Networking.Rpc.Rpc<ClaimRoomRpc>.Instance.Send((mapId, (byte)i));
                    return true;
                }
            }
            return false;
        }
    }

    public class ClaimRoomButton : CustomActionButton
    {
        public override string Name => "Reivindicar Sala";
        public override float Cooldown => 1f;
        public override LoadableAsset<UnityEngine.Sprite> Sprite => new LoadableAssetWrapper<UnityEngine.Sprite>(GenerateSprite());

        public override bool Enabled(RoleBehaviour? role) => SafeZoneManager.SelectionPhaseOpen;

        protected override void OnClick()
        {
            ClaimRoomLogic.TryClaimCurrentRoom();
        }

        private static UnityEngine.Sprite GenerateSprite()
        {
            var tex = new Texture2D(64, 64);
            var pixels = new Color[64 * 64];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.cyan;
            tex.SetPixels(pixels);
            tex.Apply();
            return UnityEngine.Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 100f);
        }
    }
}
