using Bogus;
using Lotest.DTOs;
using Lotest.Helper;

namespace Lotest.Worker
{
    public static class ContentWorker
    {
        public static string[] GetPayloads(Dictionary<string, object> properties, int requestCount)
        {
            var result = new string[requestCount];

            for (int i = 0; i < requestCount; i++)
            {
                result[i] = GetPayloadJSON(properties, i);
            }

            return result;
        }

        public static string[] GenerateRandomPayloads(int requestCount)
        {
            var result = new string[requestCount];

            for (int i = 0; i < requestCount; i++)
            {
                result[i] = GetRandomPayloadJSON();
            }

            return result;
        }

        private static string GetRandomPayloadJSON()
        {
            var payload = new Faker<PayloadExample>()
                .RuleFor(u => u.Id, f => f.Random.Int())
                .RuleFor(u => u.Code, f => f.Random.Guid().ToString())
                .RuleFor(u => u.City, f => f.Address.City())
                .RuleFor(u => u.Country, f => f.Address.Country())
                .RuleFor(u => u.Name, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName);

            return JSON.GetObjectAsJson(payload.Generate());
        }

        private static string GetPayloadJSON(Dictionary<string, object> properties, int index)
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
