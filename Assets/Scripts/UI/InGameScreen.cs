using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : MonoBehaviour, IScreen
{
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _returnHomeButton;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _totalScore;

    public ScreenType ScreenType => ScreenType.InGame;

    public void Initialize()
    {
        _generateButton.onClick.AddListener(OnGenerateClick);
        _returnHomeButton.onClick.AddListener(OnReturnHomeClick);
        EventManager.Subscribe<int>(EventType.OnMerge, OnMerge);
        ResetScore();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnGenerateClick()
    {
        EventManager.Trigger(EventType.OnGenerateClick);
    }

    private void OnReturnHomeClick()
    {
        EventManager.Trigger(EventType.OnReturnHomeClick);
    }

    private void OnMerge(int score)
    {
        _totalScore += score;
        _scoreText.text = "Score : " +_totalScore.ToString();
    }

    private void ResetScore()
    {
        _totalScore = 0;
        _scoreText.text = "Score : " +  "0";
    }

    public void Dispose()
    {
        _generateButton.onClick.RemoveListener(OnGenerateClick);
        _returnHomeButton.onClick.RemoveListener(OnReturnHomeClick);
        EventManager.Unsubscribe<int>(EventType.OnMerge, OnMerge);
    }
}
