namespace SharpSettings.MongoDB.AspNet.Test
{
    public class TestSettings : MongoWatchableSettings
    {
        public TestSettings()
        {
            Bob = new TestSettings2();
        }
        
        public TestSettings2 Bob { get; set; }
    }
    
    public class TestSettings2
    {
        public TestSettings2()
        {
            Foo = "Bar";
            Bobs = new[] { 4, 3, 2, 1 };
        }
        public string Foo { get; set; }
        public int[] Bobs { get; set; }
    }
}