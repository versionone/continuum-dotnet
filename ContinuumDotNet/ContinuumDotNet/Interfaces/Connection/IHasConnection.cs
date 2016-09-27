namespace ContinuumDotNet.Interfaces.Connection
{
    public interface IHasConnection
    {
        IContinuumConnection Connection { get; set; }
    }
}
