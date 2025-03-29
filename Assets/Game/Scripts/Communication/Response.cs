using System;

namespace Game.Scenes
{
    [Serializable]
    public class Response
    {
        public bool success;
        public string message;
        public int code;
    }
}