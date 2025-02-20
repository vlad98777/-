using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Сетевое_программирование.Сокеты
{
        class Program
        {
            private static TcpListener _listener;
            private static bool _isRunning;
        private static TcpClient _client;
        private static NetworkStream _stream;

        static void Main(string[] args)
            {
            Console.WriteLine("Введите IP-адрес сервера:");
            string ipAddress = Console.ReadLine();

            Console.WriteLine("Введите порт:");
            int port = int.Parse(Console.ReadLine());

            Console.WriteLine("Выберите режим общения:");
            Console.WriteLine("0 - Человек (клиент) — Человек (сервер)");
            Console.WriteLine("1 - Человек (клиент) — Компьютер (сервер)");
            Console.WriteLine("2 - Компьютер (клиент) — Человек (сервер)");
            Console.WriteLine("3 - Компьютер (клиент) — Компьютер (сервер)");
            int mode = int.Parse(Console.ReadLine());

            try
            {
                _client = new TcpClient(ipAddress, port);
                _stream = _client.GetStream();
                Console.WriteLine("Подключение успешно!");

                while (true)
                {
                    string message = string.Empty;

                    if (mode == 0 || mode == 1) // человек-клиент
                    {
                        Console.Write("Вы: ");
                        message = Console.ReadLine();
                    }
                    else if (mode == 2 || mode == 3) // компьютер-клиент
                    {
                        message = GenerateComputerMessage(mode);
                        Console.WriteLine("Вы (Компьютер): " + message);
                    }

                    byte[] data = Encoding.ASCII.GetBytes(message);
                    _stream.Write(data, 0, data.Length);

                    if (message.ToLower() == "bye")
                    {
                        Console.WriteLine("Вы отключились.");
                        break;
                    }

                    byte[] buffer = new byte[1024];
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Сервер: " + response);
                }

                _client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        // Генерация сообщения для компьютера
        private static string GenerateComputerMessage(int mode)
        {
            string[] messages;

            if (mode == 1) // Человек (клиент) — Компьютер (сервер)
            {
                messages = new[] { "Здравствуйте!", "Как дела?", "У вас есть вопросы?" };
            }
            else // Компьютер (клиент) — Компьютер (сервер)
            {
                messages = new[] { "Как поживаете?", "Какие у вас планы?", "Всегда рад помочь!" };
            }

            Random rand = new Random();
            return messages[rand.Next(messages.Length)];
        }
            

            private static void StartServer()
            {
                _listener = new TcpListener(IPAddress.Any, 12345);
                _listener.Start();
                _isRunning = true;
                Console.WriteLine("Сервер запущен. Ожидание подключения клиента...");

                while (_isRunning)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Console.WriteLine("Клиент подключен.");
                    Thread clientThread = new Thread(HandleClientComm);
                    clientThread.Start(client);
                }
            }

            private static void HandleClientComm(object client_obj)
            {
                TcpClient tcpClient = (TcpClient)client_obj;
                NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Клиент: " + message);

                    if (message.ToLower().Trim() == "bye")
                    {
                        Console.WriteLine("Клиент отключился.");
                        break;
                    }

                    // Логика для генерации ответа от сервера
                    string response = GenerateResponse();
                    byte[] responseData = Encoding.ASCII.GetBytes(response);
                    stream.Write(responseData, 0, responseData.Length);
                }

                tcpClient.Close();
            }

            private static string GenerateResponse()
            {
                string[] responses = { "Привет!", "Как дела?", "Что нового?", "До свидания!" };
                Random rand = new Random();
                return responses[rand.Next(responses.Length)];
            }
        }
    }