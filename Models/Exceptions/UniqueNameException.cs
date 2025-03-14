namespace Models.Exceptions
{
    public class UniqueNameException : SecureException
    {
        public UniqueNameException() : base("Item with the same name already exists") { }
    }
}
