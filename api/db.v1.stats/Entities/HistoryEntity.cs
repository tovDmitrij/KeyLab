using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.stats.Entities
{
    [Table("histories")]
    public sealed class HistoryEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("activity_id")]
        public Guid ActivityID { get; set; }

        [Column("user_id")]
        public Guid UserID { get; set; }

        [Column("date")]
        public double Date { get; set; }



        public HistoryEntity(Guid id, Guid activityID, Guid userID, double date)
        {
            ID = id;
            ActivityID = activityID;
            UserID = userID;
            Date = date;
        }

        public HistoryEntity(Guid activityID, Guid userID, double date)
        {
            ActivityID = activityID;
            UserID = userID;
            Date = date;
        }

        public HistoryEntity() { }
    }
}