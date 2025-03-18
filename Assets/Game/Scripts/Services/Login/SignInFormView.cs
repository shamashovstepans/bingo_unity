using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Services
{
    internal class SignInFormView : MonoBehaviour
    {
        public event Action<Credentials> OnSignInClicked;

        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _signInButton;

        private void OnEnable()
        {
            _signInButton.onClick.AddListener(OnSignInButtonClicked);
        }

        private void OnDisable()
        {
            _signInButton.onClick.RemoveListener(OnSignInButtonClicked);
        }

        private void OnSignInButtonClicked()
        {
            Taptic.Light();
            var credentials = new Credentials
            {
                Email = _emailInputField.text,
                Password = _passwordInputField.text
            };
            OnSignInClicked?.Invoke(credentials);
        }
    }
}