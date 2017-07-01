using System.Threading.Tasks;
using MongoDB.Driver;
using SharpSettings;

namespace SharpSettings.MongoDB
{
    public class MongoDataStore<TSettingsObject> : IDataStore<string, TSettingsObject> where TSettingsObject : MongoWatchableSettings
    {
        internal readonly IMongoCollection<TSettingsObject> Store;
        internal readonly ILogger Logger;

        public MongoDataStore(IMongoCollection<TSettingsObject> store, ILogger logger = null)
        {
            Store = store;
            Logger = logger;
        }

        public async Task<TSettingsObject> FindAsync(string settingsId)
        {
            return await Store.Find(Builders<TSettingsObject>.Filter.Eq(x => x.Id, settingsId)).SingleOrDefaultAsync();
        }

        public async Task<TSettingsObject> FindAsync()
        {
            return await Store.Find(Builders<TSettingsObject>.Filter.Empty).FirstOrDefaultAsync();
        }

        public TSettingsObject Find(string settingsId)
        {
            return Store.Find(Builders<TSettingsObject>.Filter.Eq(x => x.Id, settingsId)).SingleOrDefault();
        }

        public TSettingsObject Find()
        {
            return Store.Find(Builders<TSettingsObject>.Filter.Empty).FirstOrDefault();
        }
    }
}