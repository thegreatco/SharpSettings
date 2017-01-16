using System.Threading.Tasks;

namespace SharpSettings
{
    public interface IDataStore<TSettingsObject, TId>
    {
        Task<TSettingsObject> FindAsync(TId settingsObjectId);
    }
}