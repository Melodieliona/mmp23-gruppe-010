using UnityEngine;

namespace Defence
{
    public class NormalCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite level2Sprite;
        [SerializeField] private Sprite level3Sprite;

        private SpriteRenderer spriteRenderer;

        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1, 50, 5f, 2f, 2.5f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            return new CannonStats(2, 100, 5.5f, 2f, 5f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            return new CannonStats(3, 150, 6f, 2f, 7.5f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/NormalCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}