using System.Threading;
using Cysharp.Threading.Tasks;

namespace BingoGame.Ui.PopupManager
{
    public interface IPopupManager
    {
        void ShowPopup();
        UniTask ShowPopupAsync(CancellationToken cancellationToken);
    }
}