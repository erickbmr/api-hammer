using Lotest.Clients.Interfaces;
using Lotest.DTOs;
using Lotest.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Lotest.Worker
{
    public static class RequestWorker
    {
        public static void Start(ServiceProvider serviceProvider, string[] jsonPayloads)
        {
            Console.Clear();
            var loadType = GetRequestLoadTypeFromUser();

            Console.Clear();
            var headers = GetRequestHeadersFromUser();
            
            var clientService = serviceProvider.GetService<IClient>();
            if (clientService == null) return;

            //TODO - add the validation for Authentication type (none, basic, token)

            switch (loadType)
            {
                case LoadType.Concurrent:
                    DoConcurrentRequests(clientService, headers, jsonPayloads);
                    break;

                case LoadType.Sequential:
                    DoSequentialRequests(clientService, headers, jsonPayloads);
                    break;

                case LoadType.WaitForResponse:
                    DoRequestsWaitingForResponse(clientService, headers, jsonPayloads);
                    break;

                default:
                    Console.WriteLine("unknown handle requests mode");
                break;    
            }
        }

        private static LoadType GetRequestLoadTypeFromUser()
        {
            string option = string.Empty;

            while (!option.Equals("-1"))
            {
                PrintLoadTypeOptions();
                option = Console.ReadLine() ?? string.Empty;

                Console.Clear();

                switch (option)
                {
                    case "0": return LoadType.Concurrent;
                    case "1": return LoadType.Sequential;
                    case "2": return LoadType.WaitForResponse;

                    default:
                        Console.WriteLine("choose a valid option!");
                    break;
                }

                Console.WriteLine("\n\n");
            }

            return LoadType.WaitForResponse;
        }

        private static void PrintLoadTypeOptions()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("0 - requests made at the same time");
            Console.WriteLine("1 - requests made one after another (fire and forget)");
            Console.WriteLine("2 - requests that wait for a response before the next");
            Console.Write("(0/1/2): ");
        }

        private static RequestHeaders GetRequestHeadersFromUser()
        {
            Console.WriteLine("now, add the POST requests info...");

            var target = GetInputFromUser("target");
            var authType = GetAuthenticationTypeFromUser();

            switch (authType)
            {
                case AuthenticationType.None: return new (AuthenticationType.None, target);
                
                case AuthenticationType.Basic:
                    var userName = GetInputFromUser("userName");
                    var password = GetInputFromUser("password");
                return new(AuthenticationType.Basic, target, userName, password);

                case AuthenticationType.Token:
                    var token = GetInputFromUser("token");
                return new (AuthenticationType.Token, target, token);

                default: throw new NotImplementedException();
            }
        }

        private static string GetInputFromUser(string inputName)
        {
            string value;
            
            valueInput:
            {
                Console.Write(inputName + ": ");
                value = Console.ReadLine() ?? string.Empty;
            }
            if (string.IsNullOrEmpty(value)) goto valueInput;
            
            return value;
        }

        private static AuthenticationType GetAuthenticationTypeFromUser()
        {
            string authTypeInput;
            authenticationInput:
            {
                PrintAuthenticationOptions();
                authTypeInput = Console.ReadLine() ?? string.Empty;
            }
            if (string.IsNullOrEmpty(authTypeInput)) goto authenticationInput;
            if (!authTypeInput.Contains('0') && !authTypeInput.Contains('1') && !authTypeInput.Contains('2')) goto authenticationInput;
            if (!int.TryParse(authTypeInput, out int authenticationType)) goto authenticationInput;

            return (AuthenticationType)authenticationType;
        }

        private static void PrintAuthenticationOptions()
        {
            Console.WriteLine("authentication...");
            Console.WriteLine("0 - none");
            Console.WriteLine("1 - token");
            Console.WriteLine("2 - basic");
            Console.Write("(0/1/2): ");
        }

        private static async void DoConcurrentRequests(IClient client, RequestHeaders headers, string[] jsonPayloads)
        {
            var tasks = new List<Task<HttpResponseMessage>>();
            foreach (var payload in jsonPayloads)
            {
                tasks.Add(headers.AuthType == AuthenticationType.Basic 
                    ? client.PostWithBasicAuthAsync(headers.Target, payload, headers.UserName, headers.Password)
                    : client.PostWithTokenAsync(headers.Target, payload, headers.Token)
                );
            }

            var responses = await Task.WhenAll(tasks);
            foreach (var response in responses)
            {
                Console.WriteLine(response?.Content?.ToString());
            }
        }

        private static void DoSequentialRequests(IClient client, RequestHeaders headers, string[] jsonPayloads)
        {
            foreach (var payload in jsonPayloads)
            {
                _ = headers.AuthType == AuthenticationType.Basic
                    ? client.PostWithBasicAuthAsync(headers.Target, payload, headers.UserName, headers.Password)
                    : client.PostWithTokenAsync(headers.Target, payload, headers.Token);
            }
            Console.WriteLine("done!");
        }

        private static async void DoRequestsWaitingForResponse(IClient client, RequestHeaders headers, string[] jsonPayloads)
        {
            foreach (var payload in jsonPayloads)
            {
                var response = headers.AuthType == AuthenticationType.Basic
                    ? await client.PostWithBasicAuthAsync(headers.Target, payload, headers.UserName, headers.Password)
                    : await client.PostWithTokenAsync(headers.Target, payload, headers.Token);
                
                Console.WriteLine(response?.Content?.ToString());
            }
        }
    }
}
