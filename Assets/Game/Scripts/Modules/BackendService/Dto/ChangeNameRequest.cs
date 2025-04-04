using System;

namespace Game.Scenes
{
    [Serializable]
    internal class ChangeNameRequest : Request
    {
        public string user_name;
        public string udid;

        public ChangeNameRequest(string userName, string udid)
        {
            this.user_name = userName;
            this.udid = udid;
        }
    }
}