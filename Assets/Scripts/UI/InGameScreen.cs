using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : MonoBehaviour, IScreen
{
    [SerializeField] private Button _generateButton;
    [SerializeField] private Button _returnHomeButton;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public ScreenType ScreenType => ScreenType.InGame;

    public void Initialize()
    {
        _generateButton.onClick.AddListener(OnGenerateClick);
        _returnHomeButton.onClick.AddListener(OnReturnHomeClick);
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

    public void Dispose()
    {
        _generateButton.onClick.RemoveListener(OnGenerateClick);
        _returnHomeButton.onClick.RemoveListener(OnReturnHomeClick);
    }
}
