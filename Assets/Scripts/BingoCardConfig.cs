using System.Collections.Generic;
using UnityEngine;

namespace BingoGame
{
    [CreateAssetMenu]
    public class BingoCardConfig : ScriptableObject
    {
        public List<string> Elements;
    }
}