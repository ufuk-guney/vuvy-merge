using UnityEngine;
using UnityEngine.UI;

namespace VuvyMerge.UI
{
    public class HomeScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Button _levelStartButton;

        public ScreenType ScreenType => ScreenType.Home;

        public void Initialize()
        {
            _levelStartButton.onClick.AddListener(StartLevel);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        private void StartLevel() => EventBus.Trigger(EventType.OnLevelStartClick);

        public void Dispose()
        {
            _levelStartButton.onClick.RemoveListener(StartLevel);
        }
    }
}
