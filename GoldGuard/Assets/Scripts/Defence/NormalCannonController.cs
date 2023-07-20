using TMPro;
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
            return new CannonLevel(2, 100, 5.5f, 2f, 5f);
        }

        protected override CannonLevel GetSecondUpgrade()
        {
            return new CannonLevel(3, 150, 6f, 2f, 7.5f);
        }

        protected override void UpdateUpgradeText(CannonLevel currentLevel, CannonLevel nextLevel, int cannonValue)
        {
            Transform panel = gameObject.transform.Find("CannonUI/Panel");
            panel.Find("Level Text").GetComponent<TextMeshProUGUI>().SetText(nextLevel.GetLevelText(currentLevel));
            panel.Find("Stats1 Text").GetComponent<TextMeshProUGUI>().SetText(nextLevel.GetUpgradeRangeText(currentLevel));
            panel.Find("Stats2 Text").GetComponent<TextMeshProUGUI>().SetText(nextLevel.GetUpgradeDamageText(currentLevel));
            panel.Find("UpgradeButton/Text").GetComponent<TextMeshProUGUI>().SetText($"UPGRADE {nextLevel.GetCost()} <sprite name=\"coin\">");
            panel.Find("SellButton/Text").GetComponent<TextMeshProUGUI>().SetText($"SELL {cannonValue} <sprite name=\"coin\">");
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/NormalCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}