using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace BingoGame.Ui.PopupManager
{
    internal class PopupManager : IPopupManager, IDisposable
    {
        private readonly PopupManagerView _popupManagerView;
        private readonly CancellationTokenSource _lifetimeTokenSource = new();

        public PopupManager(PopupManagerView popupManagerView)
        {
            _popupManagerView = popupManagerView;
        }

        public void ShowPopup()
        {
            ShowPopupAsync(CancellationToken.None).Forget();
        }

        public UniTask ShowPopupAsync(CancellationToken cancellationToken)
        {
            var token = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeTokenSource.Token, cancellationToken).Token;
            return _popupManagerView.ShowAsync(PopupType.UserProfile, token);
        }

        public void Dispose()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }
    }
}