using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class JournalItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EventId { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; }

        public string? QueryParameters { get; set; }

        public string? BodyParameters { get; set; }

        public string? StackTrace { get; set; }
    }
}
