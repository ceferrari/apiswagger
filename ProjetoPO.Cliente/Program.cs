using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace ProjetoPO.Cliente
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            const string apiUrl = "http://localhost:12345/api";
            var accessToken = string.Empty;
            var serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            var opcao = -1;
            while (opcao != 0)
            {
                Stopwatch watch;
                Task<dynamic> request;
                string id;
                string username;
                string password;

                Console.WriteLine("[ 1 ] Obter Token de Acesso");
                Console.WriteLine("[ 2 ] Listar Todos Usuários");
                Console.WriteLine("[ 3 ] Obter Usuário por Id");
                Console.WriteLine("[ 4 ] Criar Novo Usuário");
                Console.WriteLine("[ 5 ] Atualizar Usuário");
                Console.WriteLine("[ 6 ] Excluir Usuário");
                Console.WriteLine("[ 0 ] Sair do Programa");
                Console.WriteLine("----------------------------------");
                Console.Write("Escolha uma opção: ");
                Console.WriteLine();

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    opcao = -1;
                }

                switch (opcao)
                {
                    case 1:
                        watch = Stopwatch.StartNew();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'username':");
                        username = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'password':");
                        password = Console.ReadLine();

                        request = apiUrl
                            .AppendPathSegment("token")
                            .AllowAnyHttpStatus()
                            .PostUrlEncodedAsync(new { username, password })
                            .ReceiveJson();

                        if (((IDictionary<string, object>)request.Result).ContainsKey("access_token"))
                        {
                            accessToken = request.Result?.access_token;
                        }
                        //accessToken = request.Result?.data?.access_token;

                        Console.WriteLine();
                        Console.WriteLine("Reposta:");
                        Console.Write(JsonConvert.SerializeObject(request.Result, serializerSettings));
                        Console.WriteLine();
                        break;

                    case 2:
                        watch = Stopwatch.StartNew();

                        request = apiUrl
                            .WithOAuthBearerToken(accessToken)
                            .AppendPathSegment("users")
                            .AllowAnyHttpStatus()
                            .GetJsonAsync();

                        Task.WhenAll(request);

                        Console.WriteLine();
                        Console.WriteLine("Reposta:");
                        Console.Write(JsonConvert.SerializeObject(request.Result, serializerSettings));
                        Console.WriteLine();
                        break;

                    case 3:
                        watch = Stopwatch.StartNew();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'id':");
                        id = Console.ReadLine();

                        request = apiUrl
                            .WithOAuthBearerToken(accessToken)
                            .AppendPathSegment("users")
                            .AppendPathSegment(id)
                            .AllowAnyHttpStatus()
                            .GetJsonAsync();

                        Task.WhenAll(request);

                        Console.WriteLine();
                        Console.WriteLine("Reposta:");
                        Console.Write(JsonConvert.SerializeObject(request.Result, serializerSettings));
                        Console.WriteLine();
                        break;

                    case 4:
                        watch = Stopwatch.StartNew();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'username':");
                        username = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'password':");
                        password = Console.ReadLine();

                        request = apiUrl
                            .WithOAuthBearerToken(accessToken)
                            .AppendPathSegment("users")
                            .PostJsonAsync(new { username, password })
                            .ReceiveJson();

                        Task.WhenAll(request);

                        Console.WriteLine();
                        Console.WriteLine("Reposta:");
                        Console.Write(JsonConvert.SerializeObject(request.Result, serializerSettings));
                        Console.WriteLine();
                        break;

                    case 5:
                        watch = Stopwatch.StartNew();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'id':");
                        id = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Informe o novo 'username':");
                        username = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Informe o novo 'password':");
                        password = Console.ReadLine();

                        request = apiUrl
                            .WithOAuthBearerToken(accessToken)
                            .AppendPathSegment("users")
                            .AppendPathSegment(id)
                            .PutJsonAsync(new { username, password })
                            .ReceiveJson();

                        Task.WhenAll(request);

                        Console.WriteLine();
                        Console.WriteLine("Reposta:");
                        Console.Write(JsonConvert.SerializeObject(request.Result, serializerSettings));
                        Console.WriteLine();
                        break;

                    case 6:
                        watch = Stopwatch.StartNew();

                        Console.WriteLine();
                        Console.WriteLine("Informe o 'id':");
                        id = Console.ReadLine();

                        request = apiUrl
                            .WithOAuthBearerToken(accessToken)
                            .AppendPathSegment("users")
                            .AppendPathSegment(id)
                            .AllowAnyHttpStatus()
                            .DeleteAsync()
                            .ReceiveJson();

                        Task.WhenAll(request);

                        Console.WriteLine();
                        Console.WriteLine("Reposta:");
                        Console.Write(JsonConvert.SerializeObject(request.Result, serializerSettings));
                        Console.WriteLine();
                        break;

                    case 0:
                        watch = null;
                        Console.WriteLine();
                        Console.WriteLine("Pressione uma tecla para finalizar...");
                        break;

                    default:
                        watch = null;
                        Console.WriteLine();
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                if (watch != null)
                {
                    watch.Stop();
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine("Tempo decorrido: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).ToString(@"hh\h\:mm\m\:ss\s\:fff\m\s"));
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
