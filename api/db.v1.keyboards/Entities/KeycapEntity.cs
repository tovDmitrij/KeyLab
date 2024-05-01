using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.keyboards.Entities
{
    [Table("keycaps")]
    public sealed class KeycapEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("kit_id")]
        public Guid KitID { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("file_name")]
        public string FileName { get; set; } = "";

        [Column("creation_date")]
        public double CreationDate { get; set; }



        public KeycapEntity(Guid id, Guid kitID, string title, string fileName, double creationDate)
        {
            ID = id;
            KitID = kitID;
            Title = title;
            FileName = fileName;
            CreationDate = creationDate;
        }

        public KeycapEntity(Guid kitID, string title, string fileName, double creationDate)
        {
            KitID = kitID;
            Title = title;
            FileName = fileName;
            CreationDate = creationDate;
        }

        public KeycapEntity() { }
    }
}