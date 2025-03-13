using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;

namespace BingoGame
{
    [CreateAssetMenu]
    public class BingoCardConfig : ScriptableObject
    {
        public List<string> Elements;

        [Button]
        public void Export()
        {
            var json = JsonUtility.ToJson(this);
            Debug.Log(json);
            File.WriteAllText(Application.streamingAssetsPath + "/bingoCardConfig.json", json);
        }
    }
}