using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Services.Episodes
{
    internal class EpisodeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _episodeTitleText;
        [SerializeField] private Button _playButton;
        
        public event System.Action OnPlayClicked;

        public void Setup(string episodeTitle)
        {
            _episodeTitleText.text = episodeTitle;
        }
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        
        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
        
        private void OnPlayButtonClicked()
        {
            OnPlayClicked?.Invoke();
        }
    }
}