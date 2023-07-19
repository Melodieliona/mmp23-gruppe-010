using UnityEngine;

namespace Defence
{
    public class HeavyCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite level2Sprite;
        [SerializeField] private Sprite level3Sprite;

        private SpriteRenderer spriteRenderer;

        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1, 125, 4f, 0.8f, 350);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            ChangeSprite(2);
            return new CannonStats(2, 175, 4f, 0.9f, 375);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            ChangeSprite(3);
            return new CannonStats(3, 250, 4.5f, 1f, 400);
        }

        private void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/HeavyCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}