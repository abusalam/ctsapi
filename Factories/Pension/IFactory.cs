namespace CTS_BE.Factories.Pension
{
    public interface IFactory<T>
    {
        public T Create();
    }
}