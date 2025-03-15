using System;
using System.Threading;
using BingoGame.Commands;
// using BingoGame.Dto;
using Cysharp.Threading.Tasks;
// using Firebase;
// using Firebase.Extensions;
// using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame
{
    internal class GameController : MonoBehaviour
    {
        [SerializeField] private BingoCardView _cardView;
        [SerializeField] private Button _restartButton;

        private BingoCardsConfigProvider _configProvider;

        private FillBingoCardCommand _fillBingoCardCommand;
        private CalculateBingoCommand _calculateBingoCommand;
        private MarkCellCommand _markCellCommand;
        private InitializeCardViewCommand _initializeCardViewCommand;

        private CancellationTokenSource _cancellationTokenSource;

        private UniTaskCompletionSource<Vector2Int> _userInputCompletionSource;

        // private FirebaseApp app;

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _configProvider = new BingoCardsConfigProvider(this);
            _fillBingoCardCommand = new FillBingoCardCommand(_configProvider);
            _calculateBingoCommand = new CalculateBingoCommand();
            _initializeCardViewCommand = new InitializeCardViewCommand(_cardView);
            _markCellCommand = new MarkCellCommand();

            _restartButton.onClick.AddListener(OnRestartButtonClicked);

            _cardView.OnCellClicked += OnCellClicked;

            GameLoopAsync(_cancellationTokenSource.Token).Forget();

            // FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread<DependencyStatus>(task =>
            // {
            //     var dependencyStatus = task.Result;
            //     if (dependencyStatus == DependencyStatus.Available)
            //     {
            //     }
            //     else
            //     {
            //         Debug.LogError(String.Format(
            //             "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            //         // Firebase Unity SDK is not safe to use here.
            //     }
            // });
            
            // var db = FirebaseFirestore.DefaultInstance;
            // var episodeReference = db.Collection("games_history").Document("episode_1");
            // episodeReference.Listen(snapshot =>
            // {
            //     Debug.LogError($"CHANGED {snapshot.Id}");
            // });
        }

        private void OnDisable()
        {
            _cardView.OnCellClicked -= OnCellClicked;

            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private async void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     var db = FirebaseFirestore.DefaultInstance;
            //     var episodeReference = db.Collection("games_history").Document("episode_1");
            //     var episodeData = new EpisodeData()
            //     {
            //         Name = "polanushka",
            //         Link = "https://www.youtube.com/watch?v=3QHsP9m3G0k"
            //     };
            //     try
            //     {
            //         await episodeReference.SetAsync(episodeData);
            //     }
            //     catch (Exception e)
            //     {
            //         Debug.LogError(e);
            //     }
            // }
            //
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //     var db = FirebaseFirestore.DefaultInstance;
            //     var episodeReference = db.Collection("games_history").Document("episode_1");
            //     episodeReference.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            //     {
            //         try
            //         {
            //             if (task.IsFaulted)
            //             {
            //                 Debug.LogError(task.Exception);
            //                 return;
            //             }
            //
            //             DocumentSnapshot snapshot = task.Result;
            //             Debug.Log($"Id: {snapshot.Id}");
            //             var dictionary = snapshot.ToDictionary();
            //
            //             foreach (var o in dictionary)
            //             {
            //                 Debug.Log($"{o.Key} : {o.Value}");
            //             }
            //         }
            //         catch (Exception e)
            //         {
            //             Debug.LogError(e);
            //         }
            //
            //
            //     });
            // }
        }

        private async void OnRestartButtonClicked()
        {
            // var db = FirebaseFirestore.DefaultInstance;
            // var episodeReference = db.Collection("games_history").Document("episode_2");
            // var episodeData = new EpisodeData()
            // {
            //     Name = "nyamka",
            //     Link = "https://www.youtube.com/watch?v=3QHsP9m3G0k"
            // };
            // try
            // {
            //     await episodeReference.SetAsync(episodeData);
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError(e);
            // }
            Taptic.Light();
            _cardView.Dispose();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            GameLoopAsync(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid GameLoopAsync(CancellationToken token)
        {
            var gameState = new BingoGameState
            {
                Card = new BingoCard()
            };

            await ProcessGameStateAsync(gameState, token);
        }

        private async UniTask ProcessGameStateAsync(BingoGameState gameState, CancellationToken token)
        {
            gameState = _fillBingoCardCommand.Execute(gameState);
            gameState = _initializeCardViewCommand.Execute(gameState);

            while (!token.IsCancellationRequested)
            {
                _userInputCompletionSource?.TrySetCanceled();
                _userInputCompletionSource = new UniTaskCompletionSource<Vector2Int>();
                var action = await _userInputCompletionSource.Task;
                token.ThrowIfCancellationRequested();

                gameState = _markCellCommand.Execute(gameState, action.x, action.y);
                gameState = _calculateBingoCommand.Execute(gameState);
            }
        }

        private void OnCellClicked(Vector2Int coordinates)
        {
            _userInputCompletionSource?.TrySetResult(coordinates);
        }
    }
}