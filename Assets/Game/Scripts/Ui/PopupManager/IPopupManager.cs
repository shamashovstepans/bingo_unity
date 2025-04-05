using System.Threading;
using Cysharp.Threading.Tasks;

namespace BingoGame.Ui.PopupManager
{
    public interface IPopupManager
    {
        void ShowPopup(PopupType popupType);
        UniTask ShowPopupAsync(PopupType popupType,CancellationToken cancellationToken);
    }
}