using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Interfaces.Connection
{
    public interface ICacheConnection
    {
        string SetValue(string key, string value);
        string DeleteValue(string key);
        long PushLeft(string listName, string value);
        bool UpsertHash (string hashName, string field, string value);
        string RemoveHashField(string hashName, string field);
        string ValueExists(string listName, string value);
        string RemoveListItem(string listName, string value);
        List<string> GetList(string listName);
    }
}
