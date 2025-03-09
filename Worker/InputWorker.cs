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
                        //TODO
                        //get JSON input
                        //get requests from content worker
                        //start request worker
                    break;

                    case "2":
                        //TODO
                        //generate random JSON on content worker
                        //start request worker
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
    }
}
