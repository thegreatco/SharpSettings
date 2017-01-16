using System.Threading.Tasks;
using SharpSettings;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SharpSettings.MongoDB
{
    public class MongoDataStore<TSettingsObject> : IDataStore<TSettingsObject, ObjectId> where TSettingsObject : MongoWatchableSettings
    {
        private readonly IMongoCollection<TSettingsObject> _store;

        public MongoDataStore(IMongoCollection<TSettingsObject> store)
        {
            _store = store;
        }

        public async Task<TSettingsObject> FindAsync(ObjectId settingsId)
        {
            return await _store.Find(Builders<TSettingsObject>.Filter.Eq(x => x.Id, settingsId)).SingleOrDefaultAsync();
        }

        public async Task<TSettingsObject> FindAsync()
        {
            return await _store.Find(Builders<TSettingsObject>.Filter.Empty).FirstOrDefaultAsync();
        }

        public TSettingsObject Find(ObjectId settingsId)
        {
            return _store.Find(Builders<TSettingsObject>.Filter.Eq(x => x.Id, settingsId)).SingleOrDefault();
        }

        public TSettingsObject Find()
        {
            return _store.Find(Builders<TSettingsObject>.Filter.Empty).FirstOrDefault();
        }
    }
}