using UnityEngine;

namespace Defence
{
    public class HeavyCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite level2Sprite;
        [SerializeField] private Sprite level3Sprite;

        private SpriteRenderer spriteRenderer;

        protected override CannonLevel GetBaseStats()
        {
            return new CannonLevel(1, 125, 4.5f, 0.5f, 20f);
        }

        protected override CannonLevel GetFirstUpgrade()
        {
            return new CannonLevel(2, 175, 5f, 0.75f, 25f);
        }

        protected override CannonLevel GetSecondUpgrade()
        {
            return new CannonLevel(3, 250, 5.5f, 1f, 30f);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/HeavyCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}