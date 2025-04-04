using System.Collections.Generic;
using BingoGame.Episodes;
using BingoGame.Services.Episodes;
using Game.Scenes;
using UnityEngine;
using Zenject;

namespace BingoGame.Ui.Screens.Episodes
{
    internal class EpisodesScreenView : ScreenView
    {
        [SerializeField] private RectTransform _episodesContainer;
        [SerializeField] private EpisodeView _episodeViewPrefab;

        [Inject] private EpisodesModel _episodesModel;

        private readonly List<EpisodeView> _episodeViews = new();

        public override ScreenType ScreenType => ScreenType.Episodes;

        public override void Show()
        {
            foreach (var episodeDto in _episodesModel.EpisodesResponse.data)
            {
                var episodeView = Instantiate(_episodeViewPrefab, _episodesContainer);
                episodeView.Setup(episodeDto);
                _episodeViews.Add(episodeView);
                episodeView.OnPlayClicked += OnEpisodeViewClicked;
            }
        }

        public override void Hide()
        {
            foreach (var episodeView in _episodeViews)
            {
                episodeView.OnPlayClicked -= OnEpisodeViewClicked;
                Destroy(episodeView.gameObject);
            }
            _episodeViews.Clear();
        }
        
        private void OnEpisodeViewClicked(EpisodeDto episodeDto)
        {
            _episodesModel.SelectEpisode(episodeDto);
        }
    }
}