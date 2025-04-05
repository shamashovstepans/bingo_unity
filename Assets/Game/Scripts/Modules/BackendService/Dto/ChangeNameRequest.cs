using System;

namespace Game.Scenes
{
    [Serializable]
    internal class ChangeNameRequest : Request
    {
        public string newName;
        public string userId;

        public ChangeNameRequest(string newName, string userId)
        {
            this.newName = newName;
            this.userId = userId;
        }
    }
}