using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.stats.Entities
{
    [Table("intervals")]
    public sealed class IntervalEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("seconds")]
        public int Seconds { get; set; }



        public IntervalEntity(Guid id, string title, int seconds)
        {
            ID = id;
            Title = title;
            Seconds = seconds;
        }

        public IntervalEntity() { }
    }
}