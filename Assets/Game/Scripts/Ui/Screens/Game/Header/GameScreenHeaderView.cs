using System;
using UnityEngine;
using UnityEngine.UI;

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
        
        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _refreshButton.onClick.AddListener(OnRefreshButtonClicked);
            _sendButton.onClick.AddListener(OnSendButtonClicked);
        }
        
        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _refreshButton.onClick.RemoveListener(OnRefreshButtonClicked);
            _sendButton.onClick.RemoveListener(OnSendButtonClicked);
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

        public void SetState(HeaderState state)
        {
            switch (state)
            {
                case HeaderState.NotStarted:
                    _backButton.gameObject.SetActive(true);
                    _refreshButton.gameObject.SetActive(false);
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