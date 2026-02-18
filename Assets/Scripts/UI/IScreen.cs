using System;

namespace VuvyMerge.UI
{
    public interface IScreen : IDisposable
    {
        ScreenType ScreenType { get; }
        void Initialize();
        void Show();
        void Hide();
    }
}
