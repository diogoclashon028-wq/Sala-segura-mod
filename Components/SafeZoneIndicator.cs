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
            if (local == null)
            {
                _text.text = "";
                return;
            }

            var remaining = SafeZoneManager.GetRemainingTime(local.PlayerId);

            _text.text = remaining switch
            {
                null => "",
                < 0f => "PROTEGIDO",
                _ => $"PROTEGIDO ({remaining:0}s)"
            };
        }
    }
}
