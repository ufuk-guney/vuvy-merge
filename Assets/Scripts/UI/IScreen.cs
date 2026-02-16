using System;

public interface IScreen : IDisposable
{
    ScreenType ScreenType { get; }
    void Initialize();
    void Show();
    void Hide();
}
