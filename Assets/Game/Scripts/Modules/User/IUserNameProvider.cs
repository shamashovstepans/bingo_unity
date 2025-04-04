using Utils.ReactiveProperty;

namespace BingoGame.Modules.User
{
    public interface IUserNameProvider
    {
        IReadonlyReactiveProperty<string> UserName { get; }
    }
}