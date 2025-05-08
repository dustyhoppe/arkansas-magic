namespace ArkansasMagic.Core.Entities
{
    public interface IHasConcurrencyToken
    {
        byte[] ConcurrencyToken { get; set; }
    }
}
