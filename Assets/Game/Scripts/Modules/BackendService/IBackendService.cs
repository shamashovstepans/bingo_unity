using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scenes;

namespace BingoGame.Module
{
    public interface IBackendService
    {
        void SetUdid(string udid);
        UniTask LoginAsync(CancellationToken cancellationToken);
        UniTask<EpisodesResponse> GetEpisodesAsync(CancellationToken cancellationToken);
        UniTask<BingoCardResponse> GetBingoCardAsync(CancellationToken cancellationToken);
        UniTask ConcludeGameAsync(ConcludeGameRequest request, CancellationToken cancellationToken);
        UniTask<ChangeNameResponse> ChangeNameAsync(string newName, CancellationToken cancellationToken);
    }
}