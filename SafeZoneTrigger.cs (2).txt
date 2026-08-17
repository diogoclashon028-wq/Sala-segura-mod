using SafeZoneMod.Managers;
using UnityEngine;

namespace SafeZoneMod.Components
{
    public class SafeZoneTrigger : MonoBehaviour
    {
        private CircleCollider2D _collider = null!;

        private void Awake()
        {
            _collider = gameObject.GetComponent<CircleCollider2D>()
                        ?? gameObject.AddComponent<CircleCollider2D>();

            _collider.isTrigger = true;
            _collider.radius = SafeZoneModPlugin.SafeZoneRadius.Value;
        }

        private void Update()
        {
            if (!Mathf.Approximately(_collider.radius, SafeZoneModPlugin.SafeZoneRadius.Value))
                _collider.radius = SafeZoneModPlugin.SafeZoneRadius.Value;

            SafeZoneManager.Tick();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerControl>();
            if (player == null || player.Data == null) return;

            SafeZoneManager.OnPlayerEnter(player.PlayerId);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerControl>();
            if (player == null || player.Data == null) return;

            SafeZoneManager.OnPlayerExit(player.PlayerId);
        }
    }
}
