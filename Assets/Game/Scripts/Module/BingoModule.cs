using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scenes;
using Game.Scenes.HttpClient;
using UnityEngine;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace BingoGame.Module
{
    internal class BingoModule : IBingoModule
    {
        private const string DEV_URL = "http://localhost:3000";
        private const string PROD_URL = "https://bingo-server-production-4652.up.railway.app";

        private readonly IHttpClient _httpClient;

        private string _userId;
        private BingoCardResponse _bingoCard;

        public BingoModule(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async UniTask LoginAsync(CancellationToken cancellationToken)
        {
            var request = new LoginRequest
            {
                udid = SystemInfo.deviceUniqueIdentifier
            };

            var url = GetUrl("/api/login");

            var response = await _httpClient.Post<LoginRequest, LoginResponse>(url, request, cancellationToken);

            Debug.Log("User name: " + response.data.user_name);
            _userId = response.data.id;
        }

        public UniTask<EpisodesResponse> GetEpisodesAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new Exception("User is not logged in");
            }

            var request = new Request();
            var url = GetUrl("/api/episodes");

            return _httpClient.Get<Request, EpisodesResponse>(url, request, cancellationToken);
        }

        public async UniTask<BingoCardResponse> GetBingoCardAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                throw new Exception("User is not logged in");
            }

            var request = new Request();
            var url = GetUrl("/api/bingo-card");

            _bingoCard = await _httpClient.Get<Request, BingoCardResponse>(url, request, cancellationToken);
            return _bingoCard;
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