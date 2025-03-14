namespace Models.Dto
{
    public class JournalFilter
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public JournalBodyFilterDto BodyFilter { get; set; }
    }
}
