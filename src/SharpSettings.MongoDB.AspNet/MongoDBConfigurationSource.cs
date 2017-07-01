using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public class MongoDBConfigurationSource<TSettingsObject> : IConfigurationSource where TSettingsObject : MongoWatchableSettings
    {
        private readonly MongoDataStore<TSettingsObject> Store;
        private readonly bool ReloadOnChange;
        private readonly string SettingsId;

        public MongoDBConfigurationSource(MongoDataStore<TSettingsObject> store, string settingsId, bool reloadOnChange)
        {
            Store = store;
            SettingsId = settingsId;
            ReloadOnChange = reloadOnChange;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MongoDBConfigurationProvider<TSettingsObject>(Store, SettingsId, ReloadOnChange);
        }
    }
}