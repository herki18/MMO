using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMO.Data.Entities
{
    public class MMOSetting
    {
        public int Id { get; set; }

        [Required, MaxLength(128), Index(IsUnique = true)]
        public string Key { get; set; }

        [Required, MaxLength]
        public string Value { get; set; }
    }
}
