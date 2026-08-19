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

            var room = SafeZoneManager.GetClaimedRoom(local.PlayerId);
            if (room == null)
            {
                _text.text = SafeZoneManager.SelectionPhaseOpen ? "Escolha uma sala!" : "";
                return;
            }

            var pos = local.GetTruePosition();
            _text.text = room.Value.Contains(pos.x, pos.y)
                ? $"PROTEGIDO ({room.Value.Name})"
                : $"Sala: {room.Value.Name}";
        }
    }
}
