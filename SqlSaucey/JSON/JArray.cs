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
    public class JArray : IList<object>
    {
        private List<object> _Payload;
        public JArray(List<object> payload){
            _Payload = payload;
        }
        public object this[int index] { get {
                return
_Payload[index];
                    }
        set { _Payload[index] = value;
            }
        }
        

        public int Count { get { return _Payload.Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Add(object item)
        {
            _Payload.Add(item);
        }

        public void Clear()
        {
            _Payload.Clear();
        }

        public bool Contains(object item)
        {
            return _Payload.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            _Payload.CopyTo(array, arrayIndex);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _Payload.GetEnumerator();
        }

        public int IndexOf(object item)
        {
            return _Payload.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            _Payload.Insert(index, item);
        }

        public bool Remove(object item)
        {
            return _Payload.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _Payload.RemoveAt(index);
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