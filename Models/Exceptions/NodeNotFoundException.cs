namespace Models.Exceptions
{
    public class NodeNotFoundException : SecureException
    {
        public NodeNotFoundException() : base("Node was not found") { }
    }
}
