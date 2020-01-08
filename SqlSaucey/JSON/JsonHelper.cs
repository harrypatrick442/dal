using System.Collections.Generic;
using System.Linq;
using newt = Newtonsoft.Json.Linq;
namespace JSON
{
    public static class JSONHelper
    {
        public static object Deserialize(string json)
        {
            return ToObject(newt.JToken.Parse(json));
        }

        private static object ToObject(newt.JToken token)
        {
            switch (token.Type)
            {
                case newt.JTokenType.Object:
                    Dictionary<string, object> dict = token.Children<newt.JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value));
                    return new JObject(dict);
                case newt.JTokenType.Array:
                    return new JArray(token.Select(ToObject).ToList());

                default:
                    return ((newt.JValue)token).Value;
            }
        }
    }
}