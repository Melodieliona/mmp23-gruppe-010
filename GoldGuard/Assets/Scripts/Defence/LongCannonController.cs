using UnityEngine;

namespace Defence
{
    public class LongCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite level2Sprite;
        [SerializeField] private Sprite level3Sprite;

        private SpriteRenderer spriteRenderer;

        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1, 75, 7f, 0.5f, 7.5f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            return new CannonStats(2, 150, 8f, 0.75f, 10f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            return new CannonStats(3, 200, 9f, 1f, 12.5f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/LongCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}