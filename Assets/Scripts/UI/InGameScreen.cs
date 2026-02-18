using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VuvyMerge.UI
{
    public class InGameScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Button _generateButton;
        [SerializeField] private Button _returnHomeButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _warningText;

        private int _totalScore;
        private RectTransform _warningRect;
        private Vector2 _warningOriginPos;

        public ScreenType ScreenType => ScreenType.InGame;

        public void Initialize()
        {
            _warningRect = _warningText.rectTransform;
            _warningOriginPos = _warningRect.anchoredPosition;
            _warningText.alpha = 0f;

            _generateButton.onClick.AddListener(OnGenerateClick);
            _returnHomeButton.onClick.AddListener(OnReturnHomeClick);
            EventBus.Subscribe<int>(EventType.OnMerge, OnMerge);
            EventBus.Subscribe<string>(EventType.OnWarning, ShowWarning);

            ResetScore();
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void Dispose()
        {
            DOTween.Kill(_warningRect);
            _generateButton.onClick.RemoveListener(OnGenerateClick);
            _returnHomeButton.onClick.RemoveListener(OnReturnHomeClick);
            EventBus.Unsubscribe<int>(EventType.OnMerge, OnMerge);
            EventBus.Unsubscribe<string>(EventType.OnWarning, ShowWarning);
        }

        private void OnGenerateClick() => EventBus.Trigger(EventType.OnGenerateClick);

        private void OnReturnHomeClick()
        {
            EventBus.Trigger(EventType.OnReturnHomeClick);
            ResetScore();
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
            DOTween.Kill(_warningRect);

            _warningText.text = message;
            _warningText.alpha = 0f;
            _warningRect.anchoredPosition = _warningOriginPos;

            var targetY = _warningOriginPos.y + Constants.Animation.WarningFloatDistance;

            DOTween.Sequence()
                .SetTarget(_warningRect)
                .Append(_warningText.DOFade(1f, Constants.Animation.WarningFadeInDuration))
                .Append(_warningText.DOFade(0f, Constants.Animation.WarningFadeOutDuration))
                .Join(_warningRect.DOAnchorPosY(targetY, Constants.Animation.WarningFadeOutDuration));
        }
    }
}
