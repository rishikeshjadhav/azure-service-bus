
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Nitor.OSS.ServiceBusAPI.Helpers
{
    public static class CommonHelper
    {
        /// <summary>
        /// Method to convert string json to entity formatted json
        /// </summary>
        /// <param name="json">Input string json</param>
        /// <returns>Entity formatted json</returns>
        public static string ToEnityFormattedJSON(this string json)
        {
            return Convert.ToString(json, CultureInfo.InvariantCulture)
                    .Replace("\"[", "[")
                    .Replace("]\"", "]")
                    .Replace("\\\"", "\"")
                    .Replace("\"{", "{")
                    .Replace("}\"", "}");
        }

        /// <summary>
        /// Method to convert input dicionary to specific entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="dictionary">Input dictionary object</param>
        /// <param name="replaceDoubleQuote">If double quotes to be replaced</param>
        /// <returns>Entity converted from input json</returns>
        public static T ToEntity<T>(this IDictionary<string, object> dictionary, bool replaceDoubleQuote = false)
        {
            if (replaceDoubleQuote)
            {
                List<string> keys = new List<string>(dictionary.Keys);
                foreach (string key in keys)
                {
                    string tempValue = Convert.ToString(dictionary[key]);
                    if (!string.IsNullOrWhiteSpace(tempValue))
                    {
                        dictionary[key] = Convert.ToString(dictionary[key]).Replace("\"", "'");
                    }
                }
            }
            string json = ToEnityFormattedJSON(JsonConvert.SerializeObject(dictionary));
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
