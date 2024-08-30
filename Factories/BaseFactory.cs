using System.Text.RegularExpressions;
using Bogus;
using CTS_BE.DTOs;

namespace CTS_BE.Factories
{
    public abstract partial class BaseFactory<T> : IFactory<T> where T : BaseDTO
    {
        protected Faker<T> _faker;

        public BaseFactory()
        {
            _faker = new Faker<T>();
        }

        public virtual T Create()
        {
            return _faker.Generate();
        }

        public virtual List<T> Make(int count)
        {
            return _faker.Generate(count);
        }

        [GeneratedRegex("(^\\w)|(\\s\\w)")]
        protected static partial Regex CapitalizeFirstLetter();
    }
}