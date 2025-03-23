using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Services
{
    internal class SignInFormView : MonoBehaviour
    {
        public event Action<Credentials> OnSignInClicked;
        public event Action OnSignUpClicked;

        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _signInButton;
        [SerializeField] private Button _signUpButton;

        private void OnEnable()
        {
            _signInButton.onClick.AddListener(OnSignInButtonClicked);
            _signUpButton.onClick.AddListener(OnSignUpButtonClicked);
        }

        private void OnDisable()
        {
            _signInButton.onClick.RemoveListener(OnSignInButtonClicked);
            _signUpButton.onClick.RemoveListener(OnSignUpButtonClicked);
        }

        private void OnSignInButtonClicked()
        {
            Debug.LogError("OnSignInButtonClicked");
            Taptic.Light();
            var credentials = new Credentials
            {
                Email = _emailInputField.text,
                Password = _passwordInputField.text
            };
            OnSignInClicked?.Invoke(credentials);
        }
        
        private void OnSignUpButtonClicked()
        {
            Taptic.Light();
            OnSignUpClicked?.Invoke();
        }
    }
}