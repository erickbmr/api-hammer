using Lotest.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace Lotest.Worker
{
    public static class InputWorker
    {
        public static void Start(ServiceProvider serviceProvider)
        {
            string option = string.Empty;

            while (!option.Equals("0"))
            {
                PrintOptions();
                option = Console.ReadLine() ?? string.Empty;

                Console.Clear();

                switch (option)
                {
                    case "1":
                        Console.Write("JSON payload (single line): ");
                        var json = Console.ReadLine() ?? string.Empty;
                        var properties = JSON.GetProperties(json);
                        var count1 = GetRequestCountFromUser();
                        var payloads1 = ContentWorker.GetPayloads(properties, count1);
                        RequestWorker.Start(serviceProvider, payloads1);
                        break;

                    case "2":
                        var count2 = GetRequestCountFromUser();
                        var payloads2 = ContentWorker.GenerateRandomPayloads(count2);
                        RequestWorker.Start(serviceProvider, payloads2);
                        break;

                    default:
                        Console.WriteLine(option);
                        break;
                }

                Console.WriteLine("\n\n\n");
            }
        }

        private static void PrintOptions()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("1 - input a simple JSON content");
            Console.WriteLine("2 - generate a random JSON content");
            Console.WriteLine("0 - exit");
            Console.Write("(1/2/0): ");
        }

        private static int GetRequestCountFromUser()
        {
            int count;
            countInput:
            {
                Console.Write("number of requests: ");
                var input = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(input, out count) || count <= 0) goto countInput;
            }
            return count;
        }
    }
}
