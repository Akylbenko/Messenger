using System;

namespace ChatService
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();

            Console.WriteLine("Запуск чат-сервера...");
            Console.WriteLine("Для остановки нажмите Ctrl+C...");

            try
            {
                server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }

            Console.WriteLine("Сервер остановлен. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}