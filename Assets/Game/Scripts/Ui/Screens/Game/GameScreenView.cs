using System;
using System.Linq;
using System.Threading;
using BingoGame.Commands;
using BingoGame.Header;
using BingoGame.Module;
using BingoGame.Ui;
using Cysharp.Threading.Tasks;
using Game.Scenes;
using Game.Scripts.Module;
using TMPro;
using UnityEngine;
using Zenject;
using ILogger = BingoGame.Modules.Logger.ILogger;
using Random = UnityEngine.Random;

namespace BingoGame
{
    internal class GameScreenView : ScreenView
    {
        [SerializeField] private GameScreenHeaderView _headerView;
        [SerializeField] private TextMeshProUGUI _episodeText;
        [SerializeField] private BingoCardView _cardView;
        [SerializeField] private GameObject _fullScreenBlocker;
        [SerializeField] private GameObject _gameBlocker;

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
        [Inject] private ILogger _logger;

        public override ScreenType ScreenType => ScreenType.Game;

        public override void Show()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _fillBingoCardCommand = new FillBingoCardCommand();
            _calculateBingoCommand = new CalculateBingoCommand();
            _initializeCardViewCommand = new InitializeCardViewCommand(_cardView);
            _markCellCommand = new MarkCellCommand();

            GameLoopAsync(_cancellationTokenSource.Token).Forget();

            _headerView.RefreshButtonClicked += OnRestartButtonClicked;
            _headerView.BackButtonClicked += OnBackButtonClicked;
            _headerView.SendButtonClicked += OnSendButtonClicked;
            _cardView.OnCellClicked += OnCellClicked;

            _wasSent = false;
            _seed = (short)Random.Range(0, short.MaxValue);

            UpdateHeaderState();
        }

        public override void Hide()
        {
            _headerView.RefreshButtonClicked -= OnRestartButtonClicked;
            _headerView.BackButtonClicked -= OnBackButtonClicked;
            _headerView.SendButtonClicked -= OnSendButtonClicked;
            _cardView.OnCellClicked -= OnCellClicked;

            _cardView.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private void Update()
        {
            UpdateHeaderState();
        }

        private void UpdateHeaderState()
        {

            var headerState = GetHeaderState();
            _headerView.SetState(headerState);
            _gameBlocker.SetActive(headerState == HeaderState.Finished);
        }

        private HeaderState GetHeaderState()
        {
            if (_gameState == null || !_gameState.Card.checkedCells.SelectMany(x => x).Any(x => x))
            {
                return HeaderState.NotStarted;
            }

            return _wasSent ? HeaderState.Finished : HeaderState.InProgress;

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
                _fullScreenBlocker.SetActive(true);
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
                _logger.Info("Successfully sent");
                _wasSent = true;

                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
            catch (Exception e)
            {
                _logger.Error("Failed to send game: " + e.Message);
            }
            finally
            {
                _fullScreenBlocker.SetActive(false);
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