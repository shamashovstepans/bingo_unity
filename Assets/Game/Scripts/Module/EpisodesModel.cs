using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scenes;

namespace BingoGame.Episodes
{
    public class EpisodesModel
    {
        private UniTaskCompletionSource<EpisodeDto> _selectedEpisodeCompletionSource;
        
        public EpisodesResponse EpisodesResponse { get; set; }

        public async UniTask<EpisodeDto> WaitForSelectedEpisode(CancellationToken cancellationToken)
        {
            _selectedEpisodeCompletionSource = new UniTaskCompletionSource<EpisodeDto>();
            await using (cancellationToken.Register(() => _selectedEpisodeCompletionSource.TrySetCanceled()))
            {
                return await _selectedEpisodeCompletionSource.Task;
            }
        }
        
        public void SelectEpisode(EpisodeDto episode)
        {
            _selectedEpisodeCompletionSource?.TrySetResult(episode);
        }
    }
}