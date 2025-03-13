using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BingoGame
{
    internal class BingoCardsConfigProvider
    {
        private readonly MonoBehaviour _coroutineRunner;
        
        private BingoCardConfig _config;

        public BingoCardsConfigProvider(MonoBehaviour coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public BingoCardConfig GetConfig()
        {
            if (_config == null)
            {
                _config = Resources.Load<BingoCardConfig>("BingoCardConfig");
            }

            return _config;
        }

        public void LoadConfig(string id)
        {
            return;
            var url = "https://bingo-game-bucket-stepans.s3.eu-central-1.amazonaws.com/bingoCardConfig.json";
            var json = DownloadJsonFromUrl(url);
        }

        private string DownloadJsonFromUrl(string url)
        {
            _coroutineRunner.StartCoroutine(GetRequest(url));
            return null;
        }
        
        private IEnumerator GetRequest(string uri) {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.SendWebRequest();
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    throw new Exception("Failed to load uri");
                }

                yield return webRequest.downloadHandler.text;
            }
        }
    }
}