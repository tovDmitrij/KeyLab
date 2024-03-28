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

        [Column("switch_type_id")]
        public Guid SwitchTypeID { get; set; }

        [Column("box_type_id")]
        public Guid BoxTypeID { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("file_name")]
        public string FileName { get; set; } = "";

        [Column("preview_name")]
        public string PreviewName { get; set; } = "";

        [Column("creation_date")]
        public double CreationDate { get; set; }

        public KeyboardEntity(Guid id, Guid ownerID, Guid switchTypeID, Guid boxTypeID, string title, 
                              string fileName, string previewName, double creationDate)
        {
            ID = id;
            OwnerID = ownerID;
            SwitchTypeID = switchTypeID;
            BoxTypeID = boxTypeID;
            Title = title;
            FileName = fileName;
            PreviewName = previewName;
            CreationDate = creationDate;
        }

        public KeyboardEntity(Guid ownerID, Guid switchTypeID, Guid boxTypeID, string title, 
                              string fileName, string previewName, double creationDate)
        {
            OwnerID = ownerID;
            SwitchTypeID = switchTypeID;
            BoxTypeID = boxTypeID;
            Title = title;
            FileName = fileName;
            PreviewName = previewName;
            CreationDate = creationDate;
        }

        public KeyboardEntity() { }
    }
}