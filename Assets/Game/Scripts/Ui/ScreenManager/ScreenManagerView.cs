using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BingoGame.Ui
{
    internal class ScreenManagerView : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private List<ScreenView> _screenViewPrefabs;
        
        private ScreenView _activeScreen;

        public void ShowScreen(ScreenType screenType)
        {
            if (_activeScreen != null)
            {
                _activeScreen.Hide();
                _activeScreen.gameObject.SetActive(false);
                // Destroy(_activeScreen.gameObject);
            }
            
            foreach (var screenPrefab in _screenViewPrefabs)
            {
                if (screenPrefab.ScreenType == screenType)
                {
                    // var screen = Instantiate(screenPrefab, _parentTransform);
                    screenPrefab.SetCamera(Camera.main);
                    _activeScreen = screenPrefab;
                    screenPrefab.gameObject.SetActive(true);
                    screenPrefab.Show();
                }
            }
        }
    }
}