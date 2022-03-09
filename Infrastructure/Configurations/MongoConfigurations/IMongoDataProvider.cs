using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Configurations.MongoConfigurations
{
    public interface IMongoDataProvider
    {
        IMongoCollection<BsonDocument> Table();
    }
}