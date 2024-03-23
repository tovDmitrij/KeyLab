using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities
{
    [Table("kits")]
    public sealed class KitEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("owner_id")]
        public Guid OwnerID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("creation_date")]
        public double CreationDate { get; set; }



        public KitEntity(Guid id, Guid ownerID, string title, double creationDate)
        {
            ID = id;
            OwnerID = ownerID;
            Title = title;
            CreationDate = creationDate;
        }

        public KitEntity(Guid ownerID, string title, double creationDate)
        {
            OwnerID = ownerID;
            Title = title;
            CreationDate = creationDate;
        }

        public KitEntity() { }
    }
}