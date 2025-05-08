namespace ArkansasMagic.Core.Alerts.Types
{
    public interface IAlert
    {
        public string Endpoint { get; }
        public string Content { get; }
    }
}
