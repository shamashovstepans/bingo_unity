using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Module
{
    public class GameModel
    {
        private UniTaskCompletionSource _gameResultCompletionSource;
        
        public GameArgs GameArgs { get; set; }
        
        public async UniTask WaitForGameResultAsync(CancellationToken cancellationToken)
        {
            _gameResultCompletionSource = new UniTaskCompletionSource();
            await using (cancellationToken.Register(() => _gameResultCompletionSource.TrySetCanceled()))
            {
                await _gameResultCompletionSource.Task;
            }
        }

        public void EndGame()
        {
            _gameResultCompletionSource?.TrySetResult();
        }
    }
}