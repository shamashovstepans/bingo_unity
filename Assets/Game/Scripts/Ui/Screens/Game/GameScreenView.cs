using System;
using System.Linq;
using System.Threading;
using BingoGame.Commands;
using BingoGame.Module;
using BingoGame.Ui;
using Cysharp.Threading.Tasks;
using Game.Scenes;
using Game.Scripts.Module;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace BingoGame
{
    internal class GameScreenView : ScreenView
    {
        [SerializeField] private BingoCardView _cardView;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backButton;

        [SerializeField] private TextMeshProUGUI _episodeText;

        [SerializeField] private GameObject _blocker;

        [SerializeField] private Image _sentSuccessfullyIcon;
        [SerializeField] private Button _sendButton;

        private FillBingoCardCommand _fillBingoCardCommand;
        private CalculateBingoCommand _calculateBingoCommand;
        private MarkCellCommand _markCellCommand;
        private InitializeCardViewCommand _initializeCardViewCommand;

        private CancellationTokenSource _cancellationTokenSource;

        private UniTaskCompletionSource<Vector2Int> _userInputCompletionSource;

        private short _seed;

        private BingoGameState _gameState;
        private bool _wasSent;

        [Inject] private GameModel _gameModel;
        [Inject] private IBackendService _backendService;

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _fillBingoCardCommand = new FillBingoCardCommand();
            _calculateBingoCommand = new CalculateBingoCommand();
            _initializeCardViewCommand = new InitializeCardViewCommand(_cardView);
            _markCellCommand = new MarkCellCommand();

            _restartButton.onClick.AddListener(OnRestartButtonClicked);

            _cardView.OnCellClicked += OnCellClicked;

            GameLoopAsync(_cancellationTokenSource.Token).Forget();

            _sendButton.onClick.AddListener(OnSendButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);

            _wasSent = false;

            _sentSuccessfullyIcon.gameObject.SetActive(false);

            _seed = (short)Random.Range(0, short.MaxValue);
        }

        private void Update()
        {
            if (_gameState != null)
            {
                var isAnyCall = _gameState.Card.checkedCells.SelectMany(x => x).Any(x => x);
                _backButton.gameObject.SetActive(isAnyCall);
                _restartButton.gameObject.SetActive(!isAnyCall);

                if (_wasSent)
                {
                    _restartButton.gameObject.SetActive(false);
                }

                if (!_wasSent)
                {
                    _sendButton.gameObject.SetActive(isAnyCall);
                }
            }

            if (_wasSent)
            {
                _sentSuccessfullyIcon.gameObject.SetActive(true);
                _sendButton.gameObject.SetActive(false);
            }

            _blocker.gameObject.SetActive(_wasSent);
        }

        private void OnDisable()
        {
            _cardView.OnCellClicked -= OnCellClicked;

            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _cardView.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            _sendButton.onClick.RemoveListener(OnSendButtonClicked);
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            Taptic.Light();
            _gameModel.EndGame();
        }

        private async void OnSendButtonClicked()
        {
            try
            {
                _sendButton.interactable = false;
                Taptic.Light();

                var request = new ConcludeGameRequest
                {
                    bingoCardId = 1,
                    seed = _seed,
                    bingoCount = _gameState.Bingo.Count(x => x.IsBingo),
                    episodeId = _gameModel.GameArgs.episode.id,
                    calls = _gameState.Card.GetBitmask()
                };

                await _backendService.ConcludeGameAsync(request, _cancellationTokenSource.Token);
                Debug.Log("Successfully sent");
                _wasSent = true;

                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to send: " + e.Message);
            }
            finally
            {
                _sendButton.interactable = true;
            }
        }

        private void OnRestartButtonClicked()
        {
            Taptic.Light();
            _cardView.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            GameLoopAsync(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid GameLoopAsync(CancellationToken token)
        {
            _seed = (short)Random.Range(0, short.MaxValue);
            var gameState = new BingoGameState
            {
                Card = new BingoCard()
            };

            await ProcessGameStateAsync(gameState, token);
        }

        private async UniTask ProcessGameStateAsync(BingoGameState gameState, CancellationToken token)
        {
            _episodeText.text = _gameModel.GameArgs.episode.name;
            gameState = _fillBingoCardCommand.Execute(gameState, _gameModel.GameArgs.bingoCard.data, _seed);
            gameState = _initializeCardViewCommand.Execute(gameState);

            _gameState = gameState;

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