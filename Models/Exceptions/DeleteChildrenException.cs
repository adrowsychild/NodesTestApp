namespace Models.Exceptions
{
    public class DeleteChildrenException : SecureException
    {
        public DeleteChildrenException() : base("You have to delete all children first") { }
    }
}
