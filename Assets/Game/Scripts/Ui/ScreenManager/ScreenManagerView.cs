using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BingoGame.Ui
{
    internal class ScreenManagerView : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private List<ScreenView> _screenViewPrefabs;
        [SerializeField] private Camera _screenCamera;
        
        private ScreenView _activeScreen;
        
        [Inject] private readonly IInstantiator _instantiator;

        public void ShowScreen(ScreenType screenType)
        {
            if (_activeScreen != null)
            {
                _activeScreen.Hide();
                _activeScreen.gameObject.SetActive(false);
                Destroy(_activeScreen.gameObject);
            }
            
            foreach (var screenPrefab in _screenViewPrefabs)
            {
                if (screenPrefab.ScreenType == screenType)
                {
                    var screen = _instantiator.InstantiatePrefabForComponent<ScreenView>(screenPrefab, _parentTransform);
                    screen.SetCanvasCamera(_screenCamera);
                    _activeScreen = screen;
                    screen.Show();
                }
            }
        }
    }
}