using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BingoGame.Ui.PopupManager
{
    public abstract class PopupView : MonoBehaviour
    {
        public abstract PopupType PopupType { get; }

        private CancellationTokenSource _lifetimeTokenSource;

        protected virtual void OnEnable()
        {
            _lifetimeTokenSource = new CancellationTokenSource();
        }

        protected virtual void OnDisable()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        internal void Hide()
        {
            gameObject.SetActive(false);
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            gameObject.SetActive(true);
            using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeTokenSource.Token, cancellationToken);
            await FlowAsync(linkedToken.Token);
            Hide();
        }

        protected abstract UniTask FlowAsync(CancellationToken cancellationToken);
    }
}