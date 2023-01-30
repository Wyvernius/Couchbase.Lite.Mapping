using Couchbase.Lite.Mapping;
using Couchbase.Lite;
using Newtonsoft.Json;

namespace DateOnly_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var test = new DateTimeTest()
            {
                DateTime = DateTime.Now,
                DateTimeOffset = DateTimeOffset.Now,
                TimeOnly = TimeOnly.FromDateTime(DateTime.Now),
                DateOnly = DateOnly.FromDateTime(DateTime.Now)
            };

            var testDoc = test.ToMutableDocument("MyDoc");
            var testObj = testDoc.ToObject<DateTimeTest>();
            var json = JsonConvert.SerializeObject(testObj);
        }
    }


    class DateTimeTest
    {
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public DateOnly DateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
    }
}