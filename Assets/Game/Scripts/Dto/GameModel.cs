using Postgrest.Attributes;

namespace BingoGame.Dto
{
    [Table("games")]
    internal class GameModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        
        [Column("seed")]
        public int Seed { get; set; }
        
        [Column("episode_id")]
        public long EpisodeId { get; set; }
        
        [Column("player_id")]
        public string PlayerId { get; set; }
        
        [Column("is_winner")]
        public bool IsWinner { get; set; }
        
        [Column("matches")]
        public int Matches { get; set; }
    }
}