﻿namespace Infrastructure.Configurations.RedisConfigurations
{
    public interface IRedisConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}