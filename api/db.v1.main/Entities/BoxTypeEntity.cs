using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities
{
    [Table("box_types")]
    public sealed class BoxTypeEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public BoxTypeEntity(Guid id, string title, string description)
        {
            ID = id;
            Title = title;
            Description = description;
        }

        public BoxTypeEntity() { }
    }
}