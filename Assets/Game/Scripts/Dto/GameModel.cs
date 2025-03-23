using Postgrest.Attributes;
using Postgrest.Models;

namespace BingoGame.Dto
{
    [Table("games")]
    public class GameModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        
        [Column("seed")]
        public short Seed { get; set; }
        
        [Column("episode_id")]
        public long EpisodeId { get; set; }
        
        [Column("player_id")]
        public string PlayerId { get; set; }
        
        [Column("is_winner")]
        public bool IsWinner { get; set; }
        
        [Column("matches")]
        public string Matches { get; set; }
    }
}