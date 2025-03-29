using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Scenes.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        public UniTask<TResponse> Get<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken) where TResponse : Response where TRequest : Request
        {
            return SendRequestAsync<TRequest, TResponse>(url, "GET", request, cancellationToken);
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
            // Serialize the request object to JSON
            var jsonPayload = JsonConvert.SerializeObject(request);
            var jsonToSend = Encoding.UTF8.GetBytes(jsonPayload);

            using var webRequest = new UnityWebRequest(url, httpMethod);

            if (httpMethod != "GET")
            {
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            var asyncOp = webRequest.SendWebRequest();
            await asyncOp.WithCancellation(cancellationToken);

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                throw new Exception(webRequest.error);
            }

            var jsonResponse = webRequest.downloadHandler.text;
            var response = JsonUtility.FromJson<TResponse>(jsonResponse);
            return response;
        }
    }
}