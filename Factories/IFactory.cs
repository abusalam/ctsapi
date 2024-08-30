namespace CTS_BE.Factories
{
    public interface IFactory<T>
    {
        public T Create();
    }
}