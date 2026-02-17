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
        EventManager.Subscribe<int>(EventType.OnMerge, OnMerge);
        EventManager.Subscribe<string>(EventType.OnWarning, ShowWarning);
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
        EventManager.Trigger(EventType.OnGenerateClick);
    }

    private void OnReturnHomeClick()
    {
        EventManager.Trigger(EventType.OnReturnHomeClick);
        _scoreText.text = "" ;
    }

    private void OnMerge(int score)
    {
        _totalScore += score;
        _scoreText.text = _totalScore.ToString();
    }

    private void ResetScore()
    {
        _totalScore = 0;
        _scoreText.text = "";
    }

    private void ShowWarning(string message)
    {
        DOTween.Kill(_warningText.transform);

        _warningText.text = message;
        _warningText.alpha = 0f;
        _warningText.rectTransform.anchoredPosition = _warningOriginalPos;

        var seq = DOTween.Sequence();
        seq.SetTarget(_warningText.transform);
        seq.Append(_warningText.DOFade(1f, 0.25f));
        seq.Append(_warningText.DOFade(0f, 0.5f));
        seq.Join(_warningText.rectTransform.DOAnchorPosY(_warningOriginalPos.y + 50f, 0.5f));
    }

    public void Dispose()
    {
        DOTween.Kill(_warningText.transform);
        _generateButton.onClick.RemoveListener(OnGenerateClick);
        _returnHomeButton.onClick.RemoveListener(OnReturnHomeClick);
        EventManager.Unsubscribe<int>(EventType.OnMerge, OnMerge);
        EventManager.Unsubscribe<string>(EventType.OnWarning, ShowWarning);
    }
}
