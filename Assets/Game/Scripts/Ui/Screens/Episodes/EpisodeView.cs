using Game.Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BingoGame.Services.Episodes
{
    internal class EpisodeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _episodeTitleText;
        [SerializeField] private TMP_Text _episodeDescriptionText;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _playersText;
        [SerializeField] private Image _resultBackground;
        [SerializeField] private Color _resultBackgroundColor;
        [SerializeField] private Button _playButton;
        
        public event System.Action<EpisodeDto> OnPlayClicked;
        
        private EpisodeDto _episodeDto;

        public void Setup(EpisodeDto episodeDto)
        {
            _episodeTitleText.text = episodeDto.name;
            _episodeDescriptionText.text = $"season:{episodeDto.season}\nepisode:{episodeDto.episode}";

            if (episodeDto.wasPlayed)
            {
                _resultText.text = $"Result: {episodeDto.averageCallsPerGame}/25";
                _playersText.text = $"Players: {string.Join(", ", episodeDto.playerNames)}";
                _resultBackground.color = _resultBackgroundColor;
            }
            else
            {
                _resultText.text = "Result: Not played";
                _playersText.text = "Players: Not played";
            }

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