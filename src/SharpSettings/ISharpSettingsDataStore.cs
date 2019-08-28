using System.Threading.Tasks;

namespace SharpSettings
{
    public interface ISharpSettingsDataStore<in TId, TSettingsObject>
    {
        ValueTask<TSettingsObject> FindAsync(TId settingsObjectId);
        TSettingsObject Find(TId settingsObjectId);
    }
}