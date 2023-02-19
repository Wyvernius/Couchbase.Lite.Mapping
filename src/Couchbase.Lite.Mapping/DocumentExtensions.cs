using System;
using Couchbase.Lite.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Couchbase.Lite
{
    public static class DocumentExtensions
    {
        public static T ToObject<T>(this Document document)
        {
            T obj = default(T);

            try
            {
                if (document != null)
                {
                    if (document.ToDictionary()?.Count > 0)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = new ExcludeStreamPropertiesResolver()
                        };

                        settings.Converters.Add(new BlobToBytesJsonConverter());
                        settings.Converters.Add(new DateTimeOffsetToDateTimeJsonConverter());

                        var dictionary = document.ToMutable()?.ToDictionary();

                        if (dictionary != null)
                        {
                            var json = JsonConvert.SerializeObject(dictionary, settings);


                            if (!string.IsNullOrEmpty(json))
                            {
                                obj = JsonConvert.DeserializeObject<T>(json, settings);
                            }
                        }
                    }
                    else
                    {
                        obj = Activator.CreateInstance<T>();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Couchbase.Lite.Mapper - Error: {ex.Message}");
            }

            return obj;
        }

        public static object ToObject(this Document document, Type type)
        {
            object obj = default;

            try
            {
                if (document != null)
                {
                    if (document.ToDictionary()?.Count > 0)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = new ExcludeStreamPropertiesResolver()
                        };

                        settings.Converters.Add(new BlobToBytesJsonConverter());
                        settings.Converters.Add(new DateTimeOffsetToDateTimeJsonConverter());

                        var dictionary = document.ToMutable()?.ToDictionary();

                        if (dictionary != null)
                        {
                            var json = JsonConvert.SerializeObject(dictionary, type, settings);

                            if (!string.IsNullOrEmpty(json))
                            {
                                obj = JsonConvert.DeserializeObject(json, type, settings);
                            }
                        }
                    }
                    else
                    {
                        obj = Activator.CreateInstance(type);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couchbase.Lite.Mapper - Error: {ex.Message}");
            }

            return obj;
        }
    }
}
