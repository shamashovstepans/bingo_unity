using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BingoGame.Ui.PopupManager
{
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
}