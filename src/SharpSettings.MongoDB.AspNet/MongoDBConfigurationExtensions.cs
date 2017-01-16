using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace SharpSettings.MongoDB.AspNet
{
    public static class MongoDBConfigurationExtensions
    {
        public static IConfigurationBuilder AddMongoDBConfigProvider<T>(this IConfigurationBuilder builder, MongoDataStore<T> store, bool reloadOnChange = false) where T : MongoWatchableSettings
        {
            return builder.Add(new MongoDBConfigurationSource<T>(store, reloadOnChange));
        }
    }
}