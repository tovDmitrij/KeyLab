using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities
{
    [Table("keyboards")]
    public sealed class KeyboardEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("owner_id")]
        public Guid OwnerID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("creation_date")]
        public double CreationDate { get; set; }

        public KeyboardEntity(Guid id, Guid ownerID, string title, string? description, string filePath, double creationDate)
        {
            ID = id;
            OwnerID = ownerID;
            Title = title;
            Description = description;
            FilePath = filePath;
            CreationDate = creationDate;
        }

        public KeyboardEntity(Guid ownerID, string title, string? description, string filePath, double creationDate)
        {
            OwnerID = ownerID;
            Title = title;
            Description = description;
            FilePath = filePath;
            CreationDate = creationDate;
        }

        public KeyboardEntity() { }
    }
}