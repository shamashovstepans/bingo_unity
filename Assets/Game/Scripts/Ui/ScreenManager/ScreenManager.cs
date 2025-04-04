namespace BingoGame.Ui
{
    internal class ScreenManager : IScreenManager
    {
        private readonly ScreenManagerView _screenManagerView;

        public ScreenManager(ScreenManagerView screenManagerView)
        {
            _screenManagerView = screenManagerView;
        }

        public void ShowScreen(ScreenType screenType)
        {
            _screenManagerView.ShowScreen(screenType);
        }
    }
}