using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace BingoGame.Ui.PopupManager
{
    internal class PopupManager : IPopupManager
    {
        private readonly PopupManagerView _popupManagerView;

        public void ShowPopup()
        {
        }
    }
    internal class PopupManagerView : MonoBehaviour
    {
        [SerializeField] private List<PopupView> _popupViews;

        private PopupView _activePopup;

        private void OnEnable()
        {
            foreach (var popupView in _popupViews)
            {
                popupView.Hide();
            }
        }

        public async UniTask ShowAsync(PopupType popupType, CancellationToken cancellationToken)
        {
            if (_activePopup != null)
            {
                throw new System.Exception("Popup is already active");
            }

            var popupView = _popupViews.First(p => p.PopupType == popupType);
            _activePopup = popupView;
            await popupView.ShowAsync(cancellationToken);
        }

        public void Hide()
        {
            if (_activePopup == null)
            {
                throw new System.Exception("No active popup to hide");
            }

            _activePopup.Hide();
            _activePopup = null;
        }
    }
    public abstract class PopupView : MonoBehaviour
    {
        public abstract PopupType PopupType { get; }

        private CancellationTokenSource _lifetimeTokenSource;

        private void OnEnable()
        {
            _lifetimeTokenSource = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        internal void Hide()
        {
            gameObject.SetActive(false);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            gameObject.SetActive(true);
            using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeTokenSource.Token, cancellationToken);
            await FlowAsync(linkedToken.Token);
            Hide();
        }

        protected abstract UniTask FlowAsync(CancellationToken cancellationToken);
    }
}