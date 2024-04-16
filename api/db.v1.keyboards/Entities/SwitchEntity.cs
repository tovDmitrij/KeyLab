using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db.v1.keyboards.Entities
{
    [Table("switches")]
    public sealed class SwitchEntity
    {
        [Key]
        [Column("id")]
        public Guid ID { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("file_name")]
        public string FileName { get; set; } = "";

        [Column("preview_name")]
        public string PreviewName { get; set; } = "";

        [Column("sound_name")]
        public string SoundName { get; set; } = "";

        public SwitchEntity(Guid id, string title, string fileName, string previewName, string soundName)
        {
            ID = id;
            Title = title;
            FileName = fileName;
            PreviewName = previewName;
            SoundName = soundName;
        }

        public SwitchEntity() { }
    }
}