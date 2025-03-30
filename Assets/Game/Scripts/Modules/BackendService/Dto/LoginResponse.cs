using System;

namespace Game.Scenes
{
    [Serializable]
    public class LoginResponse : Response
    {
        public LoginDto data;
    }
}