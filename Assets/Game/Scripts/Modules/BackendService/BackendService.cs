using System;
using System.Threading;
using BingoGame.Modules.Logger;
using BingoGame.Modules.User;
using Cysharp.Threading.Tasks;
using Game.Scenes;
using Game.Scenes.HttpClient;
using Utils.ReactiveProperty;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace BingoGame.Module
{
    internal class BackendService : IBackendService, IUserNameProvider
    {
        private const string DEV_URL = "http://localhost:3000";
        private const string PROD_URL = "https://bingo-server-production-4652.up.railway.app";

        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly ReactiveProperty<string> _userName = new();
        
        private string _userId;

        public IReadonlyReactiveProperty<string> UserName => _userName;

        public BackendService(IHttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async UniTask LoginAsync(CancellationToken cancellationToken)
        {
            var request = new LoginRequest
            {
                udid = SystemInfo.deviceUniqueIdentifier
            };

            var url = GetUrl("/api/login");

            var response = await _httpClient.Post<LoginRequest, LoginResponse>(url, request, cancellationToken);

            _logger.Info("Logged in. User name: " + response.data.user_name);
            _userId = response.data.id;
            _userName.Value = response.data.user_name;
        }

        public UniTask<EpisodesResponse> GetEpisodesAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new Exception("User is not logged in");
            }

            var url = GetUrl("/api/episodes");

            return _httpClient.Get<EpisodesResponse>(url, cancellationToken);
        }

        public async UniTask<BingoCardResponse> GetBingoCardAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new Exception("User is not logged in");
            }

            var request = new Request();
            var url = GetUrl("/api/bingo-card");

            return await _httpClient.Get<BingoCardResponse>(url, cancellationToken);
        }

        public UniTask ConcludeGameAsync(ConcludeGameRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new Exception("User is not logged in");
            }

            request.userId = _userId;

            var url = GetUrl("/api/games");

            return _httpClient.Post<ConcludeGameRequest, Response>(url, request, cancellationToken);
        }

        public async UniTask<ChangeNameResponse> ChangeNameAsync(string newName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new Exception("User is not logged in");
            }

            var request = new ChangeNameRequest(newName, _userId);

            var url = GetUrl("/api/user/change-name");
            var result = await _httpClient.Post<ChangeNameRequest, ChangeNameResponse>(url, request, cancellationToken);
            _userName.Value = result.data.user_name;
            _logger.Info("Changed name to: " + result.data);
            return result;
        }

        private static string GetUrl(string endpoint)
        {
        #if PRODUCTION
            var url = PROD_URL + endpoint;
        #else
            var url = DEV_URL + endpoint;
        #endif
            return url;
        }
    }
}