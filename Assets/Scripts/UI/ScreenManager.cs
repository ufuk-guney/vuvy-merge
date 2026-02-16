using System;
using System.Collections.Generic;
using VContainer.Unity;

public class ScreenManager : IInitializable, IDisposable
{
    private readonly Dictionary<ScreenType, IScreen> _screens = new();
    private IScreen _currentScreen;

    public ScreenManager(IReadOnlyList<IScreen> screens)
    {
        foreach (var screen in screens)
            _screens[screen.ScreenType] = screen;
    }

    public void Initialize()
    {
        foreach (var screen in _screens.Values)
        {
            screen.Initialize();
            screen.Hide();
        }

        EventManager.Subscribe(EventType.OnLevelStartClick, OnLevelStartClick);
        EventManager.Subscribe(EventType.OnReturnHomeClick, OnReturnHomeClick);

        ShowScreen(ScreenType.Home);
    }

    public void ShowScreen(ScreenType screenType)
    {
        if (_screens.TryGetValue(screenType, out var next) && next != _currentScreen)
        {
            _currentScreen?.Hide();
            next.Show();
            _currentScreen = next;
        }
    }

    private void OnLevelStartClick()
    {
        ShowScreen(ScreenType.InGame);
    }

    private void OnReturnHomeClick()
    {
        ShowScreen(ScreenType.Home);
    }

    public void Dispose()
    {
        EventManager.Unsubscribe(EventType.OnLevelStartClick, OnLevelStartClick);
        EventManager.Unsubscribe(EventType.OnReturnHomeClick, OnReturnHomeClick);

        foreach (var screen in _screens.Values)
            screen.Dispose();
    }
}
