namespace Models.Dto
{
    public class AddNodeDto
    {
        public string TreeName { get; set; }

        public int? ParentNodeId { get; set; }

        public string NodeName { get; set; }
    }
}
