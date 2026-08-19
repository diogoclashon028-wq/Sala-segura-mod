using System.Collections.Generic;
using HarmonyLib;
using SafeZoneMod.Managers;
using UnityEngine;

namespace SafeZoneMod.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class PlayerOverlayPatches
    {
        private static readonly Dictionary<byte, SpriteRenderer> ShieldByPlayer = new();
        private static Sprite? _shieldSprite;

        public static void Postfix()
        {
            if (!SafeZoneModPlugin.SafeZoneEnabled.Value) return;

            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p == null || p.Data == null) continue;
                UpdateNameTag(p);
                UpdateShield(p);
            }
        }

        private static void UpdateNameTag(PlayerControl p)
        {
            if (p.cosmetics?.nameText == null) return;
            if (p.Data.Role == null) return;

            string baseName = p.Data.PlayerName;
            string roleLine = p.Data.Role.IsImpostor
                ? "<color=#FF3333>IMPOSTOR</color>"
                : "<color=#3399FF>TRIPULANTE</color>";

            p.cosmetics.nameText.text = $"{baseName}\n{roleLine}";
        }

        private static void UpdateShield(PlayerControl p)
        {
            if (!ShieldByPlayer.TryGetValue(p.PlayerId, out var shield) || shield == null)
            {
                shield = CreateShield(p);
                ShieldByPlayer[p.PlayerId] = shield;
            }

            var pos = p.GetTruePosition();
            shield.enabled = SafeZoneManager.IsProtected(p.PlayerId, pos.x, pos.y);
        }

        private static SpriteRenderer CreateShield(PlayerControl p)
        {
            var go = new GameObject("SafeZoneMod_Shield");
            go.transform.SetParent(p.transform, false);
            go.transform.localPosition = new Vector3(0f, 0f, -1f);
            go.transform.localScale = new Vector3(1.4f, 1.4f, 1f);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = GetShieldSprite();
            sr.enabled = false;
            return sr;
        }

        private static Sprite GetShieldSprite()
        {
            if (_shieldSprite != null) return _shieldSprite;

            const int size = 128;
            var tex = new Texture2D(size, size);
            var pixels = new Color[size * size];
            Vector2 center = new(size / 2f, size / 2f);
            float outerR = size / 2f - 2f;
            float innerR = outerR - 8f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    pixels[y * size + x] = (dist <= outerR && dist >= innerR)
                        ? new Color(0.3f, 0.85f, 1f, 0.9f)
                        : Color.clear;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            _shieldSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
            return _shieldSprite;
        }
    }
}
