using System.Threading;
using BingoGame.Common;
using BingoGame.Ui.Common;
using BingoGame.Ui.PopupManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Ui.Popups
{
    internal class UserProfilePopupView : PopupView
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _changeNameButton;
        [SerializeField] private ConfirmationPromptView _confirmationPromptView;

        private UniTaskWithCancellationToken<object> _closeButtonClickedTcs;

        public override PopupType PopupType => PopupType.UserProfile;

        protected override void OnStart()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _changeNameButton.onClick.AddListener(OnChangeNameButtonClicked);
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _changeNameButton.onClick.AddListener(OnChangeNameButtonClicked);
        }

        protected override void OnStop()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _changeNameButton.onClick.RemoveListener(OnChangeNameButtonClicked);
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
    }
}