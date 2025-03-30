using System;

namespace Game.Scenes
{
    [Serializable]
    public class ConcludeGameRequest : Request
    {
        public string userId;
        public int seed;
        public int bingoCount;
        public int[] calls;
        public int episodeId;
        public int bingoCardId = 1;
    }
}