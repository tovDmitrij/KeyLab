using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.keyboards.Entities
{
    [Table("kits")]
    public sealed class KitEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("owner_id")]
        public Guid OwnerID { get; set; }

        [Column("box_type_id")]
        public Guid BoxTypeID { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("preview_name")]
        public string PreviewName { get; set; } = "";

        [Column("creation_date")]
        public double CreationDate { get; set; }



        public KitEntity(Guid id, Guid ownerID, Guid boxTypeID, string title, string previewName, double creationDate)
        {
            ID = id;
            OwnerID = ownerID;
            BoxTypeID = boxTypeID;
            Title = title;
            PreviewName = previewName;
            CreationDate = creationDate;
        }

        public KitEntity(Guid ownerID, Guid boxTypeID, string title, string previewName, double creationDate)
        {
            OwnerID = ownerID;
            BoxTypeID = boxTypeID;
            Title = title;
            PreviewName = previewName;
            CreationDate = creationDate;
        }

        public KitEntity() { }
    }
}