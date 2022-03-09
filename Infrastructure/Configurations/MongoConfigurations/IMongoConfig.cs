﻿namespace Infrastructure.Configurations.MongoConfigurations
{
    public interface IMongoConfig
    {
        protected internal string ConnectionString { get; set; }

        protected internal string Database { get; set; }

        protected internal string Collection { get; set; }
    }
}