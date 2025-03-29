using Game.Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Services.Episodes
{
    internal class EpisodeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _episodeTitleText;
        [SerializeField] private Button _playButton;
        
        public event System.Action<EpisodeDto> OnPlayClicked;
        
        private EpisodeDto _episodeDto;

        public void Setup(EpisodeDto episodeDto)
        {
            _episodeTitleText.text = episodeDto.name;
            _episodeDto = episodeDto;
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
            OnPlayClicked?.Invoke(_episodeDto);
        }
    }
}