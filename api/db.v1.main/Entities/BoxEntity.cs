using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace db.v1.main.Entities
{
    [Table("boxes")]
    public sealed class BoxEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("owner_id")]
        public Guid OwnerID { get; set; }

        [Column("type_id")]
        public Guid TypeID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("creation_date")]
        public double CreationDate { get; set; }

        public BoxEntity(Guid ID, Guid ownerID, Guid typeID, string title, string? description, string filePath, double creationDate)
        {
            this.ID = ID;
            OwnerID = ownerID;
            TypeID = typeID;
            Title = title;
            Description = description;
            FilePath = filePath;
            CreationDate = creationDate;
        }

        public BoxEntity(Guid ownerID, Guid typeID, string title, string? description, string filePath, double creationDate)
        {
            OwnerID = ownerID;
            TypeID = typeID;
            Title = title;
            Description = description;
            FilePath = filePath;
            CreationDate = creationDate;
        }

        public BoxEntity() { }
    }
}
