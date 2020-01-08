using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GenericPlanitHandler
/// </summary>
/// 
namespace JSON
{
    public class JObject : IDictionary<string, object>
    {
        protected Dictionary<string, object> _Payload;

        public JObject(JObject other)
        {
            _Payload = other._Payload;
        }
        public JObject()
        {
            _Payload = new Dictionary<string, object>();
        }
        public JObject(Dictionary<string, object> payload)
        {
            _Payload = payload;
        }
        public bool Has(string name)
        {
            try
            {
                return _Payload[name] != null;
            }
            catch
            {
                return false;
            }
        }
        public object this[string i]
        {
            get { return _Payload[i]; }
            set { _Payload[i] = value; System.Diagnostics.Debug.WriteLine(i); }
        }

        public ICollection<string> Keys { get { return _Payload.Keys; } }

        public ICollection<object> Values { get { return _Payload.Values; } }

        public int Count { get { return _Payload.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool ContainsKey(string key)
        {
            return _Payload.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _Payload.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _Payload.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _Payload.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _Payload.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _Payload.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _Payload.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _Payload.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _Payload.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Payload.GetEnumerator();
        }
        public string Serialize()
        {
            return JsonConvert.SerializeObject(_Payload);
        }
    }
}