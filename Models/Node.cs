using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Node
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string TreeName { get; set; }

        public int? ParentId { get; set; }

        public int TreeId { get; set; }

        public List<Node> Nodes { get; set; } = new List<Node>();
    }
}
