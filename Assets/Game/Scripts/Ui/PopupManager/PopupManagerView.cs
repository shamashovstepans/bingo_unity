using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BingoGame.Ui.PopupManager
{
    internal class PopupManagerView : MonoBehaviour
    {
        [SerializeField] private List<PopupView> _popupPrefabs;
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private Camera _popupCamera;

        [Inject] private readonly IInstantiator _instantiator;

        private PopupView _activePopup;

        public async UniTask ShowAsync(PopupType popupType, CancellationToken cancellationToken)
        {
            if (_activePopup != null)
            {
                throw new System.Exception("Popup is already active");
            }

            var popupPrefab = _popupPrefabs.First(p => p.PopupType == popupType);
            var popup = _instantiator.InstantiatePrefabForComponent<PopupView>(popupPrefab, _parentTransform);
            popup.SetCanvasCamera(_popupCamera);
            _activePopup = popup;
            await popup.ShowAsync(cancellationToken);
            Hide();
        }

        private void Hide()
        {
            if (_activePopup == null)
            {
                throw new System.Exception("No active popup to hide");
            }

            Destroy(_activePopup.gameObject);
            _activePopup = null;
        }
    }
}