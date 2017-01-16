using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public class MongoDBConfigurationProvider<T> : Microsoft.Extensions.Configuration.ConfigurationProvider where T : MongoWatchableSettings
    {
        MongoDataStore<T> Store { get; }
        bool ReloadOnChange { get; }
        public MongoDBConfigurationProvider(MongoDataStore<T> store, bool reloadOnChange)
        {
            Store = store;
            ReloadOnChange = reloadOnChange;
            if (ReloadOnChange)
            {
                throw new NotImplementedException();
            }
        }

        public override void Load()
        {
            var settings = Store.Find();
		    var stuff = GetProperties(settings);
		    Data = stuff.ToDictionary(x => x.Item1, x => x.Item2.ToString());
        }

        public override void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Tuple<string, object>> GetProperties(object obj, string currentFieldName = null)
        {
            if (obj == null) return null;
            var stuffToReturn = new List<Tuple<string, object>>();
            var type = obj.GetType();
            if (type.IsArray)
            {
                var objArray = (IEnumerable)obj;
                var i = 0;
                foreach(var it in objArray)
                {
                    var props = GetProperties(it).ToArray();
                    if (props.Any())
                        stuffToReturn.AddRange(props);
                    else
                        stuffToReturn.Add(new Tuple<string, object>(string.Join(":", currentFieldName, i), it));
                    i++;
                }
            }
            else
            {
                foreach (var property in type.GetTypeInfo().DeclaredProperties.Where(x => x.PropertyType.GetTypeInfo().IsClass))
                {
                    var val = property.GetValue(obj);
                    var props = GetProperties(val, currentFieldName == null ? property.Name : string.Join(":", currentFieldName, property.Name)).ToArray();
                    if (props.Any())
                        stuffToReturn.AddRange(props);
                    else
                        stuffToReturn.Add(new Tuple<string, object>(currentFieldName == null ? property.Name : string.Join(":", currentFieldName, property.Name), val));
                }
            }

            return stuffToReturn.Where(x => x != null);
        }
    }
}