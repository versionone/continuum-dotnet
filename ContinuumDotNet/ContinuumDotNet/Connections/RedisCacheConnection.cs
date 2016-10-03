using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Utilities;
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
            for (var i = 0; i < _database.ListLength(listName) - 1; i++)
            {
                if (_database.ListGetByIndex(listName, i) == value) return 0;
            }
            return _database.ListLeftPush(listName, value);
        }

        public void UpsertHash(string hashName, object value)
        {
            _database.HashSet(hashName, value.ToHashEntries());
        }

        public object GetHash(string hashName)
        {
            return _database.HashGetAll(hashName);
        }

        public string RemoveHashField(string hashName, string field)
        {
            throw new NotImplementedException();
        }

        public string ValueExists(string listName, string value)
        {
            throw new NotImplementedException();
        }

        public void RemoveListItem(string listName, string value)
        {
            var listLength = _database.ListLength(listName);
            for (var i = 0; i < listLength - 1; i++)
            {
                if (_database.ListGetByIndex(listName, i) == value)
                {
                    _database.ListRemove(listName, value);
                }
            }
        }

        public List<string> GetList(string listName)
        {
            var listItems = new List<string>();
            var listLength = _database.ListLength(listName);
            for (var i = 0; i < listLength-1; i++)
            {
                listItems.Add(_database.ListGetByIndex(listName,i).ToString());
            }
            return listItems;
        }

        public void RemoveHash(string hashName)
        {
            _database.HashDelete(hashName, _database.HashKeys(hashName));
        }
    }
}
