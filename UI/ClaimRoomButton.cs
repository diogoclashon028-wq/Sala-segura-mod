using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
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
            if (player == null) return false;

            if (!SafeZoneManager.SelectionPhaseOpen)
            {
                SafeZoneModPlugin.Log.LogInfo("Reivindicar Sala: clique ignorado, fora da janela de seleção.");
                return false;
            }

            var pos = player.GetTruePosition();
            var room = RoomRegistry.FindRoomAt(pos.x, pos.y);
            if (room == null) return false;

            if (SafeZoneManager.IsRoomFull(room.RoomId, player.PlayerId)) return false;

            SafeZoneManager.SetClaimedRoom(player.PlayerId, room.RoomId);
            Reactor.Networking.Rpc.Rpc<ClaimRoomRpc>.Instance.Send((byte)room.RoomId);
            return true;
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
