using System.Threading;
using Cysharp.Threading.Tasks;

namespace BingoGame.Services
{
    public interface ILoginService
    {
        bool IsLoggedIn { get; }
        UniTask<UserData> GetUserDataAsync(CancellationToken cancellationToken);
        UniTask SignInAsync(Credentials credentials, CancellationToken cancellationToken);
        UniTask SignUpAsync(Credentials credentials, CancellationToken cancellationToken);
    }
}