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
            return new CannonStats(1, 75, 7f, 1f, 400f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            ChangeSprite(2);
            return new CannonStats(2, 150, 7.5f, 1f, 400f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            ChangeSprite(3);
            return new CannonStats(3, 200, 8f, 1f, 400f);
        }

        private void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/LongCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}