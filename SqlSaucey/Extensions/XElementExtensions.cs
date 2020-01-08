using System;
using System.Xml;
using System.Xml.Linq;

namespace SqlSaucey.Extensions
{
    public static class XElementExtensions
    {
        public static T GetContent<T>(this XElement xElement)
        {
            if (xElement.FirstNode == null) return default(T);
            return (T)Convert.ChangeType(((XText)xElement.FirstNode).Value, typeof(T));
        }
    }
}
