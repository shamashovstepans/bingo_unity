using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using ILogger = BingoGame.Modules.Logger.ILogger;

namespace Game.Scenes.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        private readonly ILogger _logger;

        public HttpClient(ILogger logger)
        {
            _logger = logger;
        }

        public UniTask<TResponse> Get<TResponse>(string url, CancellationToken cancellationToken) where TResponse : Response
        {
            return SendRequestAsync<Request, TResponse>(url, "GET", null, cancellationToken);
        }

        public UniTask<TResponse> Post<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken) where TResponse : Response where TRequest : Request
        {
            return SendRequestAsync<TRequest, TResponse>(url, "POST", request, cancellationToken);
        }

        /// <summary>
        /// Sends an asynchronous POST request.
        /// Serializes the TRequest object as JSON in the request body.
        /// </summary>
        public async UniTask<TResponse> SendRequestAsync<TRequest, TResponse>(
            string url,
            string httpMethod,
            TRequest request,
            CancellationToken cancellationToken)
            where TResponse : Response
            where TRequest : Request
        {
            using var webRequest = new UnityWebRequest(url, httpMethod);

            if (request != null)
            {
                var jsonPayload = JsonConvert.SerializeObject(request);
                var jsonToSend = Encoding.UTF8.GetBytes(jsonPayload);
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            var asyncOp = webRequest.SendWebRequest();
            await asyncOp.WithCancellation(cancellationToken);

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                _logger.Error($"Request failed: {webRequest.error}");
                throw new Exception(webRequest.error);
            }

            var jsonResponse = webRequest.downloadHandler.text;
            var response = JsonUtility.FromJson<TResponse>(jsonResponse);
            return response;
        }
    }
}