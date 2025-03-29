using System;

namespace Game.Scenes
{
    [Serializable]
    public class EpisodeDto
    {
        public int id;
        public string name;
        public string link;
        public int season;
        public int episode_number;
    }
}