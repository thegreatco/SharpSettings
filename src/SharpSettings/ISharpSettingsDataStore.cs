using System.Threading.Tasks;

namespace SharpSettings
{
    public interface ISharpSettingsDataStore<in TId, TSettingsObject>
    {
        Task<TSettingsObject> FindAsync(TId settingsObjectId);
        TSettingsObject Find(TId settingsObjectId);
    }
}