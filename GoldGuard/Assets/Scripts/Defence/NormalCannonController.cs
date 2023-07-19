using UnityEngine;

namespace Defence
{
    public class NormalCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite cannon1Level1;
        [SerializeField] private Sprite cannon1Level2;

        private SpriteRenderer spriteRenderer;

        protected override CannonStats GetBaseStats()
        {
            return new CannonStats(1, 50, 5f, 1f, 400f);
        }

        protected override CannonStats GetFirstUpgrade()
        {
            ChangeSprite(1);
            return new CannonStats(2, 100, 5f, 1.5f, 450f);
        }

        protected override CannonStats GetSecondUpgrade()
        {
            ChangeSprite(2);
            return new CannonStats(3, 150, 5f, 2f, 500f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/NormalCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 1 ? cannon1Level1 :cannon1Level2;
        }
    }
}