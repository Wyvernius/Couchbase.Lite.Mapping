using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Threading.Tasks;
using System.Linq;
using Couchbase.Lite.Mapping;
using CouchbaseDB.Couchbase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Couchbase.Lite
{
    public static class ResultSetExtensions
    {
        public static TInterface ToInterface<TInterface>(this Query.Result result, Type objectType)
        {
            TInterface obj = default;

            if (result != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new ExcludeStreamPropertiesResolver()
                };

                settings.Converters?.Add(new BlobToBytesJsonConverter());

                JObject rootJObj = new JObject();

                foreach (var key in result.Keys)
                {
                    var value = result[key]?.Value;

                    if (value != null)
                    {
                        JObject jObj = null;

                        if (value.GetType() == typeof(DictionaryObject))
                        {
                            var json = JsonConvert.SerializeObject(value, settings);

                            if (!string.IsNullOrEmpty(json))
                            {
                                jObj = JObject.Parse(json);
                            }
                        }
                        else
                        {
                            jObj = new JObject
                            {
                                new JProperty(key, value)
                            };
                        }

                        if (jObj != null)
                        {
                            rootJObj.Merge(jObj, new JsonMergeSettings
                            {
                                // Union array values together to avoid duplicates (e.g. "id")
                                MergeArrayHandling = MergeArrayHandling.Union
                            });
                        }

                        if (rootJObj != null)
                        {
                            obj = (TInterface)rootJObj.ToObject(objectType);
                        }
                    }
                }
            }

            return obj;
        }

        public static T ToObject<T>(this Query.Result result)
        {
            T obj = default;

            if (result != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new ExcludeStreamPropertiesResolver()
                };

                settings.Converters?.Add(new BlobToBytesJsonConverter());

                JObject rootJObj = new JObject();

                foreach (var key in result.Keys)
                {
                    var value = result[key]?.Value;

                    if (value != null)
                    {
                        JObject jObj = null;

                        if (value.GetType() == typeof(DictionaryObject))
                        {
                            var json = JsonConvert.SerializeObject(value, settings);

                            if (!string.IsNullOrEmpty(json))
                            {
                                jObj = JObject.Parse(json);
                            }
                        }
                        else
                        {
                            jObj = new JObject
                            {
                                new JProperty(key, value)
                            };
                        }

                        if (jObj != null)
                        {
                            rootJObj.Merge(jObj, new JsonMergeSettings
                            {
                                // Union array values together to avoid duplicates (e.g. "id")
                                MergeArrayHandling = MergeArrayHandling.Union
                            });
                        }

                        if (rootJObj != null)
                        { 
                            obj = rootJObj.ToObject<T>();
                        }
                    }
                }
            }

            return obj;
        }

        public static IEnumerable<T> ToObjects<T>(this List<Query.Result> results)
        {
            List<T> objects = Array.Empty<T>().ToList();

            if (results?.Count > 0)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new ExcludeStreamPropertiesResolver()
                };

                settings.Converters?.Add(new BlobToBytesJsonConverter());

                objects = new List<T>();

                foreach (var result in results)
                {
                    var obj = ToObject<T>(result);

                    if (obj != null)
                    {
                        objects.Add(obj);
                    }
                }
            }

            return objects;
        }

        public static IEnumerable<TInterface> ToInterfaces<TInterface>(this List<Query.Result> results)
        {
            List<TInterface> objects = default;

            if (results?.Count > 0)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new ExcludeStreamPropertiesResolver()
                };

                settings.Converters?.Add(new BlobToBytesJsonConverter());

                objects = new List<TInterface>();

                foreach (var result in results)
                {
                    var typeString = result.GetDictionary(0).GetString("Type"); 
                    var type = CouchbaseTypes.Get(typeString);
                    var obj = result.ToInterface<TInterface>(type);


                    if (obj != null)
                    {
                        objects.Add(obj);
                    }
                }
            }

            return objects;
        }

        public static IEnumerable<TInterface> ToInterfacesParallel<TInterface>(this List<Query.Result> results)
        {
            List<TInterface> objects = default;

            if (results?.Count > 0)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new ExcludeStreamPropertiesResolver()
                };

                settings.Converters?.Add(new BlobToBytesJsonConverter());

                objects = new List<TInterface>();

                Parallel.ForEach(results, (s, e) =>
                {
                    var typeString = s.GetDictionary(0).GetString("Type");
                    var type = CouchbaseTypes.Get(typeString);
                    var obj = s.ToInterface<TInterface>(type);
                    if (obj != null)
                    {
                        objects.Add(obj);
                    }
                });
            }
            return objects;
        }
    }
}
