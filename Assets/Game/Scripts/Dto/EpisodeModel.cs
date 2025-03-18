using Postgrest.Attributes;
using Postgrest.Models;

namespace BingoGame.Dto
{
    [Table("episodes")]
    internal class EpisodeModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("link")]
        public string Link { get; set; }
    }
}