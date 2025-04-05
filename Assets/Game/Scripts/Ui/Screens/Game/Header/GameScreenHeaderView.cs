using System;
using BingoGame.Ui.PopupManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BingoGame.Header
{
    internal class GameScreenHeaderView : MonoBehaviour
    {
        public event Action BackButtonClicked; 
        public event Action RefreshButtonClicked;
        public event Action SendButtonClicked;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _sendButton;
        [SerializeField] private GameObject _sendSuccessfullyIcon;
        [SerializeField] private Button _profileButton;

        [Inject] private readonly IPopupManager _popupManager;
        
        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _refreshButton.onClick.AddListener(OnRefreshButtonClicked);
            _sendButton.onClick.AddListener(OnSendButtonClicked);
            _profileButton.onClick.AddListener(OnProfileButtonClicked);
        }
        
        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _refreshButton.onClick.RemoveListener(OnRefreshButtonClicked);
            _sendButton.onClick.RemoveListener(OnSendButtonClicked);
            _profileButton.onClick.RemoveListener(OnProfileButtonClicked);
        }
        
        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }
        
        private void OnRefreshButtonClicked()
        {
            RefreshButtonClicked?.Invoke();
        }
        
        private void OnSendButtonClicked()
        {
            SendButtonClicked?.Invoke();
        }
        
        private void OnProfileButtonClicked()
        {
            _popupManager.ShowPopup(PopupType.UserProfile);
        }

        public void SetState(HeaderState state)
        {
            switch (state)
            {
                case HeaderState.NotStarted:
                    _backButton.gameObject.SetActive(true);
                    _refreshButton.gameObject.SetActive(true);
                    _sendButton.gameObject.SetActive(false);
                    _sendSuccessfullyIcon.SetActive(false);
                    break;
                case HeaderState.InProgress:
                    _backButton.gameObject.SetActive(true);
                    _refreshButton.gameObject.SetActive(false);
                    _sendButton.gameObject.SetActive(true);
                    _sendSuccessfullyIcon.SetActive(false);
                    break;
                case HeaderState.Finished:
                    _backButton.gameObject.SetActive(true);
                    _refreshButton.gameObject.SetActive(false);
                    _sendButton.gameObject.SetActive(false);
                    _sendSuccessfullyIcon.SetActive(true);
                    break;
            }
        }
    }
}