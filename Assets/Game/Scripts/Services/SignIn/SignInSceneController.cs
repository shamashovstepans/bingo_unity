using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BingoGame.Services
{
    internal class SignInSceneController : IInitializable, IDisposable
    {
        private readonly SignInFormView _signInFormView;
        private readonly SignUpSceneView _signUpSceneView;
        private readonly SupabaseService _supabaseService;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public SignInSceneController(SignInFormView signInFormView, SupabaseService supabaseService, SignUpSceneView signUpSceneView)
        {
            _signInFormView = signInFormView;
            _supabaseService = supabaseService;
            _signUpSceneView = signUpSceneView;

            _signInFormView.OnSignInClicked += OnSignInClicked;
            _signInFormView.OnSignUpClicked += OnSignUpClicked;
        }

        public void Initialize()
        {
            Debug.Log("SignInSceneController initialized");
            
        }

        public void Dispose()
        {
            _signInFormView.OnSignInClicked -= OnSignInClicked;
            _signInFormView.OnSignUpClicked -= OnSignUpClicked;
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private async void OnSignInClicked(Credentials credentials)
        {
            try
            {
                var instance = await _supabaseService.LoginAsync(credentials, _cancellationTokenSource.Token);

                if (instance != null)
                {
                    SceneManager.LoadScene("EpisodesScene", LoadSceneMode.Single);
                }
                else
                {
                    throw new Exception("Failed to login");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        private void OnSignUpClicked()
        {
            _signUpSceneView.gameObject.SetActive(true);
            _signUpSceneView.OnSignUpClicked += credentials =>
            {
                _supabaseService.SignUpAsync(credentials, _cancellationTokenSource.Token).Forget(Debug.LogError);
                _signUpSceneView.gameObject.SetActive(false);
            };

            _signUpSceneView.OnCloseClicked += () =>
            {
                _signInFormView.gameObject.SetActive(false);
            };
        }
    }
}