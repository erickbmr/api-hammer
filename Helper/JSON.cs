using Newtonsoft.Json;

namespace Lotest.Helper
{
    public class JSON
    {
        public static Dictionary<string, object> GetProperties(string input)
        {
            if (string.IsNullOrEmpty(input)) return new();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(input) ?? new();
        }

        public static string GetDictionaryAsJson(Dictionary<string, object> jsonProperties)
        {
            return JsonConvert.SerializeObject(jsonProperties);
        }
    }
}
