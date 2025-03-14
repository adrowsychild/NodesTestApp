namespace Models.Exceptions
{
    public class TreeNotFoundException : SecureException
    {
        public TreeNotFoundException() : base("Tree was not found") { }
    }
}
