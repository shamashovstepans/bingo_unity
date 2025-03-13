using System.Threading;
using BingoGame.Commands;
using Cysharp.Threading.Tasks;
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
        }
        
        private void OnDisable()
        {
            _cardView.OnCellClicked -= OnCellClicked;
            
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private void OnRestartButtonClicked()
        {
            _configProvider.LoadConfig("eag");
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