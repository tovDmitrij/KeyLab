using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.main.Entities
{
    [Table("switches")]
    public sealed class SwitchEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("sound_path")]
        public string SoundPath { get; set; }

        public SwitchEntity(Guid id, string title, string description, string filePath, string soundPath)
        {
            ID = id;
            Title = title;
            Description = description;
            FilePath = filePath;
            SoundPath = soundPath;
        }

        public SwitchEntity() { }
    }
}