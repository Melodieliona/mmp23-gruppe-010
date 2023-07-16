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

        private void FindTarget()
        {
            //RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, transform.position, 0f, enemyMask);
            //
            //if (hits.Length > 0)
            //{
            //    target = hits[0].transform;
            //}
        }

        private bool CheckTargetIsInRange()
        {
            //return Vector2.Distance(target.position, transform.position) <= targetingRange;
            return true;
        }
    }
}