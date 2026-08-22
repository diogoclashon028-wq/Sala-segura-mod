using SafeZoneMod.Managers;
using TMPro;
using UnityEngine;

namespace SafeZoneMod.Components
{
    public class SafeZoneIndicator : MonoBehaviour
    {
        private TextMeshPro _text = null!;

        private void Awake()
        {
            _text = gameObject.AddComponent<TextMeshPro>();
            _text.fontSize = 2.2f;
            _text.color = Color.cyan;
            _text.alignment = TextAlignmentOptions.TopLeft;
            var rect = _text.rectTransform;
            rect.SetParent(HudManager.Instance.transform, false);
            rect.anchoredPosition = new Vector2(-3.7f, 2.6f);
        }

        private void Update()
        {
            var local = PlayerControl.LocalPlayer;
            if (local == null) { _text.text = ""; return; }

            var claimed = SafeZoneManager.GetClaimedRoom(local.PlayerId);
            var pos = local.GetTruePosition();

            if (claimed == null)
            {
                if (!SafeZoneManager.SelectionPhaseOpen) { _text.text = ""; return; }

                var here = RoomRegistry.FindRoomAt(pos.x, pos.y);
                if (here != null)
                {
                    bool full = SafeZoneManager.IsRoomFull(here.RoomId, local.PlayerId);
                    _text.text = full ? $"Sala: {here.RoomId} X LOTADA" : $"Sala: {here.RoomId}";
                    return;
                }

                _text.text = "Escolha uma sala!";
                return;
            }

            var room = RoomRegistry.FindRoomById(claimed.Value);
            bool inside = room != null && room.roomArea.OverlapPoint(new Vector2(pos.x, pos.y));

            if (!inside)
            {
                _text.text = $"Sala: {claimed.Value}";
                return;
            }

            var remaining = SafeZoneManager.GetRemainingStaySeconds(local.PlayerId);
            _text.text = remaining == null
                ? $"PROTEGIDO ({claimed.Value})"
                : $"PROTEGIDO ({claimed.Value}) - {remaining:F0}s";
        }
    }
}
