using System.ComponentModel.DataAnnotations;

namespace LogIndexer.Core.Domain
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}