using System.Threading;
using BingoGame.Module;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BingoGame.Ui.Common
{
    internal class ConfirmationPromptView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _userInputField;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        [Inject] private readonly IBackendService _backendService;

        private void OnEnable()
        {
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
            _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        }

        private void OnConfirmButtonClicked()
        {
            string userInput = _userInputField.text;
            if (!string.IsNullOrEmpty(userInput))
            {
                _backendService.ChangeNameAsync(userInput, CancellationToken.None);
                gameObject.SetActive(false);
            }
        }

        private void OnCancelButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}