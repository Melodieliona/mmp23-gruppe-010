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
            return new CannonStats(1, 125, 3.5f, 0.25f, 20f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            return new CannonStats(2, 175, 4f, 0.5f, 25f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            return new CannonStats(3, 250, 4.5f, 0.75f, 30f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/HeavyCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}