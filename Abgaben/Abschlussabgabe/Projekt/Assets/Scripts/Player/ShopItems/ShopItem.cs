using Defence;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Player.ShopItems
{
    [Serializable]
    public class ShopItem
    {
        private readonly int slot;
        private readonly int cost;
        private readonly TileType tileType;

        private readonly GameObject normalPrefab;
        private readonly GameObject transparentPrefab;

        private Sprite affordableSprite;
        private Sprite tooExpensiveSprite;

        protected ShopItem(string weaponName, int slot, int cost, TileType tileType)
        {
            this.slot = slot;
            this.cost = cost;
            this.tileType = tileType;

            normalPrefab = GameObject.Find(weaponName);
            transparentPrefab = GameObject.Find("Transparent" + weaponName);

            affordableSprite = Resources.Load<Sprite>(weaponName + "Affordable");
            tooExpensiveSprite = Resources.Load<Sprite>(weaponName + "TooExpensive");
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
            CannonController controller = normalPrefab.GetComponent<CannonController>();
            if (controller == null)
            {
                // If item is not a cannon (i.e. Kraken), Range is always 1
                return 1;
            }

            return controller.GetCurrentLevel().GetRange();
        }

        public Sprite GetSprite(int playerGold)
        {
            return cost > playerGold ? tooExpensiveSprite : affordableSprite;
        }
    }

    public enum TileType
    {
        Land,
        Water
    }
}