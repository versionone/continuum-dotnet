using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Connection;
using StackExchange.Redis;

namespace ContinuumDotNet.Connections
{
    public class RedisCacheConnection : ICacheConnection
    {
        private IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;

        public RedisCacheConnection(string connectionString)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            _database = _connectionMultiplexer.GetDatabase();
        }

        public string SetValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        public string DeleteValue(string key)
        {
            throw new NotImplementedException();
        }

        public long PushLeft(string listName, string value)
        {
            return _database.ListLeftPush(listName, value);
        }

        public bool UpsertHash(string hashName, string field, string value)
        {
            return _database.HashSet(hashName, field, value);
        }

        public string RemoveHashField(string hashName, string field)
        {
            throw new NotImplementedException();
        }

        public string ValueExists(string listName, string value)
        {
            throw new NotImplementedException();
        }

        public string RemoveListItem(string listName, string value)
        {
            throw new NotImplementedException();
        }

        public List<string> GetList(string listName)
        {
            throw new NotImplementedException();
        }
    }
}
