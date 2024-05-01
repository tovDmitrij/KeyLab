using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace db.v1.keyboards.Entities
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
        public string Title { get; set; } = "";

        [Column("file_name")]
        public string FileName { get; set; } = "";

        [Column("creation_date")]
        public double CreationDate { get; set; }

        public BoxEntity(Guid id, Guid ownerID, Guid typeID, string title, string fileName, double creationDate)
        {
            ID = id;
            OwnerID = ownerID;
            TypeID = typeID;
            Title = title;
            FileName = fileName;
            CreationDate = creationDate;
        }

        public BoxEntity(Guid ownerID, Guid typeID, string title, string fileName, double creationDate)
        {
            OwnerID = ownerID;
            TypeID = typeID;
            Title = title;
            FileName = fileName;
            CreationDate = creationDate;
        }

        public BoxEntity() { }
    }
}