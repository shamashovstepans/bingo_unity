using System;
using System.Collections.Generic;

namespace Game.Scenes
{
    [Serializable]
    public class EpisodesResponse : Response
    {
        public List<EpisodeDto> data;
    }
}