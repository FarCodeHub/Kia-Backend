using Audit.MongoDB.ConfigurationApi;
using Newtonsoft.Json;

namespace Infrastructure.Configurations.MongoConfigurations
{
    public class MongoProviderConfigurator : IMongoProviderConfigurator
    {
        public string _connectionString = "mongodb://localhost:27017";
        public string _database = "Audit";
        public string _collection = "Event";
        public bool _serializeAsBson = false;
        public JsonSerializerSettings _jsonSerializerSettings = null;

        public IMongoProviderConfigurator ConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public IMongoProviderConfigurator Database(string database)
        {
            _database = database;
            return this;
        }

        public IMongoProviderConfigurator Collection(string collection)
        {
            _collection = collection;
            return this;
        }

        public IMongoProviderConfigurator CustomSerializerSettings(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            return this;
        }

        public IMongoProviderConfigurator SerializeAsBson(bool value = true)
        {
            _serializeAsBson = value;
            return this;
        }
    }
}