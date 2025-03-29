using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scenes;

namespace BingoGame.Module
{
    public interface IBingoModule
    {
        UniTask LoginAsync(CancellationToken cancellationToken);
        UniTask<EpisodesResponse> GetEpisodesAsync(CancellationToken cancellationToken);
        UniTask<BingoCardResponse> GetBingoCardAsync(CancellationToken cancellationToken);
        UniTask ConcludeGameAsync(ConcludeGameRequest request, CancellationToken cancellationToken);
    }
}