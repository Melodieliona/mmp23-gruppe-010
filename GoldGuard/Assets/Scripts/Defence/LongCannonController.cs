using UnityEngine;

namespace Defence
{
    public class LongCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite level2Sprite;
        [SerializeField] private Sprite level3Sprite;

        private SpriteRenderer spriteRenderer;

        protected override CannonLevel GetBaseStats()
        {
            return new CannonLevel(1, 75, 7f, 0.5f, 7.5f);
        }

        protected override CannonLevel GetFirstUpgrade()
        {
            return new CannonLevel(2, 150, 8f, 0.75f, 10f);
        }

        protected override CannonLevel GetSecondUpgrade()
        {
            return new CannonLevel(3, 200, 9f, 1f, 12.5f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/LongCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}