using UnityEngine;

namespace BingoGame.Ui
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        public abstract ScreenType ScreenType { get; }

        public void SetCamera(Camera uiCamera)
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = uiCamera;
        }

        public abstract void Show();

        public abstract void Hide();
    }
}