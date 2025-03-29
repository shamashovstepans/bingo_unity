using System;

namespace Game.Scenes
{
    [Serializable]
    public class LoginRequest : Request
    {
        public string udid;
    }
}