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

        public void ShowPopup(PopupType popupType)
        {
            ShowPopupAsync(popupType, CancellationToken.None).Forget();
        }

        public UniTask ShowPopupAsync(PopupType popupType, CancellationToken cancellationToken)
        {
            var token = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeTokenSource.Token, cancellationToken).Token;
            return _popupManagerView.ShowAsync(popupType, token);
        }

        public void Dispose()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }
    }
}