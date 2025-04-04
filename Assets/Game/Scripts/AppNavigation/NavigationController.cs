using System;
using System.Threading;
using BingoGame.Episodes;
using BingoGame.Module;
using BingoGame.Ui;
using Cysharp.Threading.Tasks;
using Game.Scripts.Module;
using UnityEngine;
using Zenject;
using ILogger = BingoGame.Modules.Logger.ILogger;

namespace BingoGame.AppNavigation
{
    internal class NavigationController : IInitializable, IDisposable
    {
        private readonly IScreenManager _screenManager;
        private readonly IBackendService _backendService;
        private readonly EpisodesModel _episodesModel;
        private readonly GameModel _gameModel;
        private readonly ILogger _logger;

        private readonly CancellationTokenSource _lifetimeTokenSource = new();

        public NavigationController(IScreenManager screenManager,
            IBackendService backendService,
            EpisodesModel episodesModel,
            GameModel gameModel,
            ILogger logger)
        {
            _screenManager = screenManager;
            _backendService = backendService;
            _episodesModel = episodesModel;
            _gameModel = gameModel;
            _logger = logger;
        }

        public void Initialize()
        {
            GameFlowAsync().Forget();
        }

        public void Dispose()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        private async UniTaskVoid GameFlowAsync()
        {
            try
            {
                
                _screenManager.ShowScreen(ScreenType.Loading);
                
                await _backendService.LoginAsync(_lifetimeTokenSource.Token);
                
                var bingoCardTask = _backendService.GetBingoCardAsync(_lifetimeTokenSource.Token);
                var episodesTask = _backendService.GetEpisodesAsync(_lifetimeTokenSource.Token);

                var (bingoCard, episodesResponse) = await UniTask.WhenAll(bingoCardTask, episodesTask);
                _episodesModel.EpisodesResponse = episodesResponse;

                while (!_lifetimeTokenSource.Token.IsCancellationRequested)
                {
                    _screenManager.ShowScreen(ScreenType.Episodes);

                    var selectedEpisode = await _episodesModel.WaitForSelectedEpisode(_lifetimeTokenSource.Token);

                    var gameArgs = new GameArgs
                    {
                        bingoCard = bingoCard,
                        episode = selectedEpisode
                    };

                    _gameModel.GameArgs = gameArgs;

                    _screenManager.ShowScreen(ScreenType.Game);

                    await _gameModel.WaitForGameResultAsync(_lifetimeTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                _logger.Error("Game flow failed", exception);
            }
        }
    }
}