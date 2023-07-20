using Enemy;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Defence
{
    public class KrakenController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tilemap waterTiles;

        private float damage;
        private int shipsUntilDestroyed = 5;
        
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

            damage = (float)(ship.GetMaxHealth() * 0.3);
            ship.Damage(damage);
            shipsUntilDestroyed--;
            if (shipsUntilDestroyed <= 0)
            {
                Destroy(gameObject);
                Vector3Int position = waterTiles.WorldToCell(gameObject.transform.position);
                waterTiles.SetColliderType(position, Tile.ColliderType.Sprite);
            }
        }
    }
}