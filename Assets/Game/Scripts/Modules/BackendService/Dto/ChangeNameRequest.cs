using System;

namespace Game.Scenes
{
    [Serializable]
    internal class ChangeNameRequest : Request
    {
        public string newName;

        public ChangeNameRequest(string newName)
        {
            this.newName = newName;
        }
    }
}