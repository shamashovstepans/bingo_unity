using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Services
{
    internal class SignUpSceneView : MonoBehaviour
    {
        public event Action OnCloseClicked;
        public event Action<Credentials> OnSignUpClicked;

        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _signUpButton;
        [SerializeField] private Button _closeButton;

        private void OnEnable()
        {
            _signUpButton.onClick.AddListener(OnSignUpButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnDisable()
        {
            _signUpButton.onClick.RemoveListener(OnSignUpButtonClicked);
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
        
        private void OnCloseButtonClicked()
        {
            Taptic.Light();
            OnCloseClicked?.Invoke();
        }

        private void OnSignUpButtonClicked()
        {
            Debug.LogError("OnSignInButtonClicked");
            Taptic.Light();
            var credentials = new Credentials
            {
                Email = _emailInputField.text,
                Password = _passwordInputField.text
            };
            OnSignUpClicked?.Invoke(credentials);
        }
    }
}