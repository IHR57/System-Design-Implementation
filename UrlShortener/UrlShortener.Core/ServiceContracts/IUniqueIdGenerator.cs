namespace UrlShortener.Core.ServiceContracts
{
    public interface IUniqueIdGenerator
    {
        public long NextId();
    }
}
