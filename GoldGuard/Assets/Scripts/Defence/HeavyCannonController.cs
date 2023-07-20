using TMPro;
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

        protected override void UpdateUpgradeText(CannonLevel currentLevel, CannonLevel nextLevel, int cannonValue)
        {
            Transform panel = gameObject.transform.Find("CannonUI/Panel");
            SetPanelText(panel, "Level Text", nextLevel.GetLevelText(currentLevel));
            SetPanelText(panel, "Stats1 Text", nextLevel.GetUpgradeBpsText(currentLevel));
            SetPanelText(panel, "Stats2 Text", nextLevel.GetUpgradeDamageText(currentLevel));
            SetPanelText(panel, "Stats3 Text", nextLevel.GetUpgradeRangeText(currentLevel));
            SetPanelText(panel, "UpgradeButton/Text", $"UPGRADE {nextLevel.GetCost()} <sprite name=\"coin\">");
            SetPanelText(panel, "SellButton/Text", $"SELL {cannonValue} <sprite name=\"coin\">");
        }

        private static void SetPanelText(Transform panel, string label, string text)
        {
            panel.Find(label).GetComponent<TextMeshProUGUI>().SetText(text);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/HeavyCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}