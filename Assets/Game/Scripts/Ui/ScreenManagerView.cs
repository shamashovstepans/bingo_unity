using System.Collections.Generic;
using UnityEngine;

namespace BingoGame.Ui
{
    internal class ScreenManagerView : MonoBehaviour
    {
        [SerializeField] private List<ScreenView> _screenViews;

        private void OnEnable()
        {
            foreach (var screenView in _screenViews)
            {
                screenView.Hide();
            }
        }

        public void ShowScreen(ScreenType screenType)
        {
            foreach (var screenView in _screenViews)
            {
                if (screenView.ScreenType == screenType)
                {
                    screenView.Show();
                }
                else
                {
                    screenView.Hide();
                }
            }
        }
    }
}