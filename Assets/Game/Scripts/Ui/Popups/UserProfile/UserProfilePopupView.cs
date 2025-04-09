using System.Threading;
using BingoGame.Common;
using BingoGame.Module;
using BingoGame.Ui.Common;
using BingoGame.Ui.PopupManager;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BingoGame.Ui.Popups
{
    internal class UserProfilePopupView : PopupView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _changeNameButton;
        [SerializeField] private ConfirmationPromptView _confirmationPromptView;

        [SerializeField] private TMP_InputField _setUdidInput;
        [SerializeField] private Button _setUdidButton;

        [Inject] private readonly IBackendService _backendService;

        private UniTaskWithCancellationToken<object> _closeButtonClickedTcs;

        public override PopupType PopupType => PopupType.UserProfile;

        protected override void OnStart()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _changeNameButton.onClick.AddListener(OnChangeNameButtonClicked);
            _setUdidButton.onClick.AddListener(OnSetUdidButtonClicked);
        }

        protected override void OnStop()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _changeNameButton.onClick.RemoveListener(OnChangeNameButtonClicked);
            _setUdidButton.onClick.RemoveListener(OnSetUdidButtonClicked);
        }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            _closeButtonClickedTcs = new UniTaskWithCancellationToken<object>(cancellationToken);

            await _closeButtonClickedTcs.GetTask();
        }

        private void OnCloseButtonClicked()
        {
            _closeButtonClickedTcs.Complete(null);
        }

        private void OnChangeNameButtonClicked()
        {
            _confirmationPromptView.gameObject.SetActive(true);
        }

        private void OnSetUdidButtonClicked()
        {
            string udidInput = _setUdidInput.text;
            if (!string.IsNullOrEmpty(udidInput))
            {
                _backendService.SetUdid(udidInput);
                gameObject.SetActive(false);
            }
        }
    }
}