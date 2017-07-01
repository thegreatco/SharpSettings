using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public static class MongoDBConfigurationExtensions
    {
        public static IConfigurationBuilder AddMongoDBConfigProvider<TSettingsObject>(this IConfigurationBuilder builder, MongoDataStore<TSettingsObject> store, string settingsId, bool reloadOnChange = true) where TSettingsObject : MongoWatchableSettings
        {
            return builder.Add(new MongoDBConfigurationSource<TSettingsObject>(store, settingsId, reloadOnChange));
        }
    }
}