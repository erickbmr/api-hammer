using Lotest.Helper;

namespace Lotest.Worker
{
    public static class ContentWorker
    {
        public static string[] GetRequests(Dictionary<string, object> properties, int requestCount)
        {
            var result = new string[requestCount];

            for (int i = 0; i < requestCount; i++)
            {
                result[i] = GetRequestJSON(properties, i);
            }

            return result;
        }

        public static string[] GenerateRandomRequests(int requestCount)
        {
            //TODO
            throw new NotImplementedException();
        }

        private static string GetRequestJSON(Dictionary<string, object> properties, int index)
        {
            var random = new Random();
            var jsonProperties = new Dictionary<string, object>();
            
            foreach (var property in properties)
            {
                if (property.Value == null) continue;
                var propertyType = property.Value.GetType();
                
                if (propertyType.Equals(typeof(int)))
                {
                    jsonProperties.Add(property.Key, random.Next());
                    continue;
                }

                if (propertyType.Equals(typeof(string)))
                {
                    jsonProperties.Add(property.Key, (string)property.Value + index.ToString());
                    continue;
                }

                if (propertyType.Equals(typeof(bool)))
                {
                    jsonProperties.Add(property.Key, index % 2 == 0 ? true : false);
                    continue;
                }

                jsonProperties.Add(property.Key, property.Value);
            }

            return JSON.GetDictionaryAsJson(jsonProperties);
        }
    }
}
