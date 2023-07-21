using UnityEngine;

namespace Defence
{
    public class NormalCannonController : CannonController
    {
        [Header("Images")]
        [SerializeField] private Sprite level2Sprite;
        [SerializeField] private Sprite level3Sprite;

        private SpriteRenderer spriteRenderer;

        protected override CannonLevel GetBaseStats()
        {
            return new CannonLevel(1, 50, 5f, 2f, 2.5f);
        }

        protected override CannonLevel GetFirstUpgrade()
        {
            return new CannonLevel(2, 75, 5.5f, 2f, 5f);
        }

        protected override CannonLevel GetSecondUpgrade()
        {
            return new CannonLevel(3, 100, 6f, 2f, 7.5f);
        }

        protected override void UpdateUpgradeText(CannonLevel currentLevel, CannonLevel nextLevel, int gold, int cannonValue)
        {
            Transform panel = gameObject.transform.Find("CannonUI/Panel");
            SetPanelText(panel, "Level Text", nextLevel.GetLevelText(currentLevel), Color.white);
            SetPanelText(panel, "Stats1 Text", nextLevel.GetUpgradeRangeText(currentLevel), Color.white);
            SetPanelText(panel, "Stats2 Text", nextLevel.GetUpgradeDamageText(currentLevel), Color.white);
            SetPanelText(panel, "UpgradeButton/Text", $"UPGRADE {nextLevel.GetCost()} <sprite name=\"coin\">", nextLevel.GetCost() <= gold ? Color.white : new Color(1f, 0.44f, 0.44f));
            SetPanelText(panel, "SellButton/Text", $"SELL {cannonValue} <sprite name=\"coin\">", Color.white);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/NormalCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}