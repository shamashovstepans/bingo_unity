using Firebase.Firestore;

namespace BingoGame.Dto
{
    [FirestoreData]
    internal struct EpisodeData
    {
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public string Link { get; set; }
    }
}