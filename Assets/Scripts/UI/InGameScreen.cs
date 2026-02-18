using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : MonoBehaviour, IScreen
{
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _returnHomeButton;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _warningText;

    private int _totalScore;
    private Vector2 _warningOriginalPos;

    public ScreenType ScreenType => ScreenType.InGame;

    public void Initialize()
    {
        _generateButton.onClick.AddListener(OnGenerateClick);
        _returnHomeButton.onClick.AddListener(OnReturnHomeClick);
        EventBus.Subscribe<int>(EventType.OnMerge, OnMerge);
        EventBus.Subscribe<string>(EventType.OnWarning, ShowWarning);
        _warningOriginalPos = _warningText.rectTransform.anchoredPosition;
        _warningText.alpha = 0f;
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
        EventBus.Trigger(EventType.OnGenerateClick);
    }

    private void OnReturnHomeClick()
    {
        EventBus.Trigger(EventType.OnReturnHomeClick);
        _scoreText.text = Constants.Text.EmptyScore;
    }

    private void OnMerge(int score)
    {
        _totalScore += score;
        _scoreText.text = _totalScore.ToString();
    }

    private void ResetScore()
    {
        _totalScore = 0;
        _scoreText.text = Constants.Text.EmptyScore;
    }

    private void ShowWarning(string message)
    {
        DOTween.Kill(_warningText.transform);

        _warningText.text = message;
        _warningText.alpha = 0f;
        _warningText.rectTransform.anchoredPosition = _warningOriginalPos;

        var seq = DOTween.Sequence();
        seq.SetTarget(_warningText.transform);
        seq.Append(_warningText.DOFade(1f, Constants.Animation.WarningFadeInDuration));
        seq.Append(_warningText.DOFade(0f, Constants.Animation.WarningFadeOutDuration));
        seq.Join(_warningText.rectTransform.DOAnchorPosY(_warningOriginalPos.y + Constants.Animation.WarningFloatDistance, Constants.Animation.WarningFadeOutDuration));
    }

    public void Dispose()
    {
        DOTween.Kill(_warningText.transform);
        _generateButton.onClick.RemoveListener(OnGenerateClick);
        _returnHomeButton.onClick.RemoveListener(OnReturnHomeClick);
        EventBus.Unsubscribe<int>(EventType.OnMerge, OnMerge);
        EventBus.Unsubscribe<string>(EventType.OnWarning, ShowWarning);
    }
}
