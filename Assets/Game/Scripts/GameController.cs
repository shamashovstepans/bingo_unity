using System.Threading;
using BingoGame.Commands;
using BingoGame.Dto;
using BingoGame.Services;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame
{
    internal class GameController : MonoBehaviour
    {
        [SerializeField] private BingoCardView _cardView;
        [SerializeField] private Button _restartButton;
        
        [SerializeField] private TextMeshProUGUI _episodeText;

        [SerializeField] private Button _sendButton;

        private BingoCardsConfigProvider _configProvider;

        private FillBingoCardCommand _fillBingoCardCommand;
        private CalculateBingoCommand _calculateBingoCommand;
        private MarkCellCommand _markCellCommand;
        private InitializeCardViewCommand _initializeCardViewCommand;

        private CancellationTokenSource _cancellationTokenSource;

        private UniTaskCompletionSource<Vector2Int> _userInputCompletionSource;

        private short _seed;

        private BingoGameState _gameState;

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

            _episodeText.text = SupabaseService.CurrentEpisode.Name;
            
            _sendButton.onClick.AddListener(OnSendButtonClicked);
            
            _seed = (short)Random.Range(0, short.MaxValue);
        }

        private void OnDisable()
        {
            _cardView.OnCellClicked -= OnCellClicked;

            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            
            _sendButton.onClick.RemoveListener(OnSendButtonClicked);
        }

        private void OnSendButtonClicked()
        {
            Taptic.Light();
            var game = new GameModel();
            game.Seed = _seed;
            game.EpisodeId = SupabaseService.CurrentEpisode.Id;
            game.PlayerId = SupabaseService.Auth.CurrentUser.Id;
            game.Matches = _gameState.Card.GetBitmask();
            SupabaseService.SupabaseInstance.InsertNewGame(game, _cancellationTokenSource.Token).Forget(Debug.LogError);
        }

        private void OnRestartButtonClicked()
        {
            Taptic.Light();
            _cardView.Dispose();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
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
            gameState = _fillBingoCardCommand.Execute(gameState, _seed);
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