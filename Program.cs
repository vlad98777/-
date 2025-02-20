using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        private static TcpClient _client;
        private static NetworkStream _stream;

        static void Main(string[] args)
        {
            Console.WriteLine("Введите IP-адрес сервера:");
            string ipAddress = Console.ReadLine();

            Console.WriteLine("Введите порт:");
            int port = int.Parse(Console.ReadLine());

            try
            {
                _client = new TcpClient(ipAddress, port);
                _stream = _client.GetStream();
                Console.WriteLine("Подключение успешно!");

                while (true)
                {
                    Console.Write("Вы: ");
                    string message = Console.ReadLine();

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
    }
}