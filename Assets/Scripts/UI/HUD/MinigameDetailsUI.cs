using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameDetailsUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text timeLimitText;
    [SerializeField] private Button startButton;
    
    private MinigamePopupBehaviour _owner;
    
    public void Initialize(MinigamePopupBehaviour owner)
    {
        _owner = owner;
        nameText.text = owner.Data.Name;
        descriptionText.text = owner.Data.Description;
        rewardText.text = $"Reward: {owner.Data.Reward}€";
        timeLimitText.text = $"Time Limit: {owner.Data.TimeLimit} seconds";
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(OnStartButtonClicked);
    }
    
    public void OnStartButtonClicked()
    {
        ServiceLocator.Instance.CursorManager.SelectObject(null);
        ServiceLocator.Instance.MinigamesManager.StartMinigame(_owner);
    }
}