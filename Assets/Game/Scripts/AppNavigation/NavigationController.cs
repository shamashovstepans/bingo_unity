using System;
using System.Threading;
using BingoGame.Episodes;
using BingoGame.Module;
using BingoGame.Ui;
using Cysharp.Threading.Tasks;
using Game.Scripts.Module;
using UnityEngine;
using Zenject;

namespace BingoGame.AppNavigation
{
    internal class NavigationController : IInitializable, IDisposable
    {
        private readonly IScreenManager _screenManager;
        private readonly IBingoModule _bingoModule;
        private readonly EpisodesModel _episodesModel;
        private readonly GameModel _gameModel;

        private readonly CancellationTokenSource _lifetimeTokenSource = new();

        public NavigationController(IScreenManager screenManager,
            IBingoModule bingoModule,
            EpisodesModel episodesModel,
            GameModel gameModel)
        {
            _screenManager = screenManager;
            _bingoModule = bingoModule;
            _episodesModel = episodesModel;
            _gameModel = gameModel;
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
                await _bingoModule.LoginAsync(_lifetimeTokenSource.Token);
                var bingoCard = await _bingoModule.GetBingoCardAsync(_lifetimeTokenSource.Token);
                var episodes = await _bingoModule.GetEpisodesAsync(_lifetimeTokenSource.Token);
                _episodesModel.EpisodesResponse = episodes;

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
                Debug.LogError("Game flow error: " + exception);
            }
        }
    }
}