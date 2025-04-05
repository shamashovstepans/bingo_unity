using System;
using System.Threading;
using BingoGame.Ui.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BingoGame.Ui.PopupManager
{
    public abstract class PopupView : MonoBehaviour, ICanvasHolder
    {
        [SerializeField] private Canvas _canvas;
        public abstract PopupType PopupType { get; }

        private CancellationTokenSource _lifetimeTokenSource;

        public void SetCanvasCamera(Camera uiCamera)
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = uiCamera;
        }

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            _lifetimeTokenSource = new CancellationTokenSource();
            using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeTokenSource.Token, cancellationToken);

            try
            {
                OnStart();
                await OnFlowAsync(linkedToken.Token);
            }
            finally
            {
                OnStop();
            }

            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        protected abstract void OnStart();
        protected abstract UniTask OnFlowAsync(CancellationToken cancellationToken);
        protected abstract void OnStop();
    }
}