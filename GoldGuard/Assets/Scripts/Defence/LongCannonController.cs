using UnityEngine;

namespace Defence
{
    public class LongCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite cannon3Level1;
        [SerializeField] private Sprite cannon3Level2;

        private SpriteRenderer spriteRenderer;
        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1,75, 7f, 1f, 400f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            ChangeSprite(1);
            return new CannonStats(2,150, 7.5f, 1f, 400f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            ChangeSprite(2);
            return new CannonStats(3,200, 8f, 1f, 400f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/LongCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 1 ? cannon3Level1 : cannon3Level2;
        }
    }
}