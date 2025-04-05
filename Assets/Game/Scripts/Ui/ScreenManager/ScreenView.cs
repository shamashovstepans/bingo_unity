using BingoGame.Ui.Common;
using UnityEngine;

namespace BingoGame.Ui
{
    public abstract class ScreenView : MonoBehaviour, ICanvasHolder
    {
        [SerializeField] private Canvas _canvas;
        public abstract ScreenType ScreenType { get; }

        public void SetCanvasCamera(Camera uiCamera)
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = uiCamera;
        }

        public abstract void Show();

        public abstract void Hide();
    }
}