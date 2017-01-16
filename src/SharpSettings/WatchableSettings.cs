namespace SharpSettings
{
    public class WatchableSettings<TId>
    {
        public TId Id { get; set; }
        public long LastUpdate { get; set; }
    }
}
