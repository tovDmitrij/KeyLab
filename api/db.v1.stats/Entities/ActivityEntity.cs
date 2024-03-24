using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.stats.Entities
{
    [Table("activities")]
    public sealed class ActivityEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("tag")]
        public string Tag { get; set; }



        public ActivityEntity(Guid id, string title, string tag)
        {
            ID = id;
            Title = title;
            Tag = tag;
        }

        public ActivityEntity() { }
    }
}