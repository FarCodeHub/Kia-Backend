using Infrastructure.Configurations.MongoConfigurations;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Persistence.AuditProvider
{
    public interface IAuditMontiroringRepository
    {
        public IMongoDataProvider MongoDataProvider
        {
            get; set;
        }

        public IMongoCollection<BsonDocument> Table();
    }
}