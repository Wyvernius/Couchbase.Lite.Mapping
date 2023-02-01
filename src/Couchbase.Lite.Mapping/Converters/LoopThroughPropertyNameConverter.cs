using Couchbase.Lite.Mapping;

namespace Couchbase.Lite.Mapping
{
    public class LoopThroughPropertyNameConverter : IPropertyNameConverter
    {
        public string Convert(string val)
        {
            return val;
        }

        public static void RegisterAsDefault()
        {
            Settings.PropertyNameConverter = new LoopThroughPropertyNameConverter();
        }
    }
}
