using UnityEngine;
using UnityEngine.UIElements;

public class GUIManager : MonoBehaviour
{
    [Header("References")]
    private PlayerManager playerManager;

    private Label hpLabel;
    private Label goldLabel;
    private Label waveLabel;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        UpdateHp();
        UpdateScore();
        UpdateGold();
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        hpLabel = root.Q<Label>("HP_Label");
        goldLabel = root.Q<Label>("Gold_Label");
        waveLabel = root.Q<Label>("Wave_Label");
    }

    public void UpdateHp()
    {
        hpLabel.text = "HP: " + playerManager.GetHealthPoints();
    }

    public void UpdateScore()
    {
        //scoreLabel.text = "Score: " + playerManager.GetScore();
    }

    public void UpdateGold()
    {
        goldLabel.text = "Gold: " + playerManager.GetGold();
    }
}