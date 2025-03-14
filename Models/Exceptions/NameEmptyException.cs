namespace Models.Exceptions
{
    public class NameEmptyException : SecureException
    {
        public NameEmptyException() : base("Name is empty") { }
    }
}
