using System.Threading.Tasks;

namespace SharpSettings
{
    public interface IDataStore<TId, TSettingsObject>
    {
        Task<TSettingsObject> FindAsync(TId settingsObjectId);
        TSettingsObject Find(TId settingsObjectId);
    }
}