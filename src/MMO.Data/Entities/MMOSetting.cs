using System.ComponentModel.DataAnnotations;

namespace MMO.Data.Entities
{
    public class MMOSetting
    {
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
