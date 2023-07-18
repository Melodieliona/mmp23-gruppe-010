using Enemy;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Defence
{
    public class KrakenController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tilemap waterTiles;

        [Header("Attribute")]
        [SerializeField] private float damage = 5f;
        [SerializeField] private float damageUntilDestroyed = 25f;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other == null || other.gameObject == null)
            {
                return;
            }

            PirateShipController ship = other.gameObject.GetComponent<PirateShipController>();
            if (ship == null)
            {
                return;
            }

            ship.Damage(damage);
            damageUntilDestroyed -= damage;
            if (damageUntilDestroyed <= 0f)
            {
                Destroy(gameObject);
                Vector3Int position = waterTiles.WorldToCell(gameObject.transform.position);
                waterTiles.SetColliderType(position, Tile.ColliderType.Sprite);
            }
        }
    }
}