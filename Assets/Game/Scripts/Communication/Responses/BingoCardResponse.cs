using System;

namespace Game.Scenes
{
    [Serializable]
    public class BingoCardResponse : Response
    {
        public BingoCallDto[] data;
    }
}