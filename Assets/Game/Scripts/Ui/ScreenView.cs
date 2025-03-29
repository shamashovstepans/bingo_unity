using UnityEngine;

namespace BingoGame.Ui
{
    public class ScreenView : MonoBehaviour
    {
        [SerializeField] private ScreenType _screenType;
        
        public ScreenType ScreenType => _screenType;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}