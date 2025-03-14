namespace Models.Dto
{
    public class JournalBodyFilterDto
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string Search {  get; set; }
    }
}
