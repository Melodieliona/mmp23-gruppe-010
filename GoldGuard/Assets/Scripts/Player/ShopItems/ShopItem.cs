using Defence;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.ShopItems
{
    [Serializable]
    public class ShopItem
    {
        private readonly int slot;
        private readonly int cost;
        private readonly TileType tileType;
        private float range;
        
        private readonly GameObject normalPrefab;
        private readonly GameObject transparentPrefab;
        
        protected ShopItem(string weaponName, int slot, int cost, TileType tileType)
        {
            this.slot = slot;
            this.cost = cost;
            this.tileType = tileType;

            normalPrefab = GameObject.Find(weaponName);
            transparentPrefab = GameObject.Find("Transparent" + weaponName);
        }

        public GameObject GetPrefab()
        {
            return normalPrefab;
        }

        public GameObject GetTransparent()
        {
            return transparentPrefab;
        }

        public SpriteRenderer GetTransparentRenderer()
        {
            return transparentPrefab.GetComponent<SpriteRenderer>();
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
            range = normalPrefab.GetComponent<CannonController>().GetRange();
            return range;
        }
    }

    public enum TileType
    {
        Land,
        Water
    }
}