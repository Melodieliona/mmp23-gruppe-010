using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class ShopItem
    {
        private readonly GameObject prefab;
        private readonly Button button;

        private readonly int slot;
        private readonly int cost;
        private readonly TileType tileType;
        private readonly float range;

        public ShopItem(GameObject prefab, int slot, int cost, TileType tileType, float range)
        {
            this.prefab = prefab;

            this.slot = slot;
            this.cost = cost;
            this.tileType = tileType;
            this.range = range;
        }

        public GameObject GetPrefab()
        {
            return prefab;
        }

        public int GetSlot()
        {
            return slot;
        }
        
        public int GetCost()
        {
            return cost;
        }

        public TileType GetTileType()
        {
            return tileType;
        }

        public float GetRange()
        {
            return range;        
        }
    }

    public enum TileType
    {
        Land,
        Water
    }
}