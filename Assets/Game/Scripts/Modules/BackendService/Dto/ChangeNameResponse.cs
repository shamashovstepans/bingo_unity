using System;

namespace Game.Scenes
{
    [Serializable]
    public class ChangeNameResponse : Response
    {
        public LoginDto data;
    }
}