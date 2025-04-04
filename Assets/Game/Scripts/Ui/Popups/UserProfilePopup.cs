using System.Threading;
using BingoGame.Ui.PopupManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BingoGame.Ui.Popups
{
    internal class UserProfilePopup : PopupView
    {
        [SerializeField]
        
        public override PopupType PopupType => PopupType.UserProfile;

        protected override UniTask FlowAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}