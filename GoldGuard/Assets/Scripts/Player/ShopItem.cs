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

        public ShopItem(GameObject prefab, int slot, int cost, TileType tileType)
        {
            this.prefab = prefab;

            this.slot = slot;
            this.cost = cost;
            this.tileType = tileType;
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
    }

    public enum TileType
    {
        Land,
        Water
    }
}