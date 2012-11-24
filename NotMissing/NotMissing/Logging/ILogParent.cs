namespace NotMissing.Logging
{
    public interface ILogParent
    {
        void Register(ILogListener listener);
        void Unregister(ILogListener listener);
    }
}
