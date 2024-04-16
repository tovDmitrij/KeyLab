using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.keyboards.Entities
{
    [Table("box_types")]
    public sealed class BoxTypeEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        public BoxTypeEntity(Guid id, string title)
        {
            ID = id;
            Title = title;
        }

        public BoxTypeEntity() { }
    }
}