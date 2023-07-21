using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

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
            return new CannonLevel(1, 75, 8f, 0.75f, 7.5f);
        }

        protected override CannonLevel GetFirstUpgrade()
        {
            return new CannonLevel(2, 100, 9f, 1f, 10f);
        }

        protected override CannonLevel GetSecondUpgrade()
        {
            return new CannonLevel(3, 150, 10f, 1.25f, 12.5f);
        }

        protected override void UpdateUpgradeText(CannonLevel currentLevel, CannonLevel nextLevel, int gold, int cannonValue)
        {
            Transform panel = gameObject.transform.Find("CannonUI/Panel");
            SetPanelText(panel, "Level Text", nextLevel.GetLevelText(currentLevel));
            SetPanelText(panel, "Stats1 Text", nextLevel.GetUpgradeBpsText(currentLevel));
            SetPanelText(panel, "Stats2 Text", nextLevel.GetUpgradeDamageText(currentLevel));
            SetPanelText(panel, "Stats3 Text", nextLevel.GetUpgradeRangeText(currentLevel));
            SetPanelText(panel, "UpgradeButton/Text", $"UPGRADE {nextLevel.GetCost()} <sprite name=\"coin\">", nextLevel.GetCost() <= gold ? Color.white : new Color(1f, 0.44f, 0.44f));
            SetPanelText(panel, "SellButton/Text", $"SELL {cannonValue} <sprite name=\"coin\">");
        }

        private static void SetPanelText(Transform panel, string label, string text)
        {
            SetPanelText(panel, label, text, Color.white);
        }

        private static void SetPanelText(Transform panel, string label, string text, Color color)
        {
            TextMeshProUGUI textMeshPro = panel.Find(label).GetComponent<TextMeshProUGUI>();
            textMeshPro.color = color;
            textMeshPro.SetText(text);
        }

        protected override void ChangeSprite(int level)
        {
            spriteRenderer = gameObject.transform.Find("RotationPoint/LongCannonSprite").GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = level == 2 ? level2Sprite : level3Sprite;
        }
    }
}