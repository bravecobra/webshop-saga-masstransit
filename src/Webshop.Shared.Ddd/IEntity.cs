namespace Webshop.Shared.Ddd
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}