using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public class MongoDBConfigurationSource<T> : IConfigurationSource where T : MongoWatchableSettings
    {
        private readonly MongoDataStore<T> Store;
        private readonly bool ReloadOnChange;

        public MongoDBConfigurationSource(MongoDataStore<T> store, bool reloadOnChange)
        {
            Store = store;
            ReloadOnChange = reloadOnChange;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoDBConfigurationProvider<T>(Store, ReloadOnChange);
        }
    }
}