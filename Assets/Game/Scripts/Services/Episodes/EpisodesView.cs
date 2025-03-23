using BingoGame.Dto;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BingoGame.Services.Episodes
{
    internal class EpisodesView : MonoBehaviour
    {
        [SerializeField] private EpisodeView _episodePrefab;
        [SerializeField] private Transform _episodesContainer;

        public void CreateEpisode(EpisodeModel episode)
        {
            var episodeView = Instantiate(_episodePrefab, _episodesContainer);
            episodeView.Setup(episode.Name);
            
            episodeView.OnPlayClicked += () =>
            {
                SupabaseService.CurrentEpisode = episode;
                SceneManager.LoadScene("MainScene");
                Debug.Log($"Play episode {episode.Name} {episode.Id}");
            };
        }
    }
}