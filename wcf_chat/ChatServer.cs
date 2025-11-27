using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatService
{
    public class ChatServer
    {
        private TcpListener _listener;
        private List<ServerUser> _users = new List<ServerUser>();
        private int _nextId = 1;
        private bool _isRunning;

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 8888);
            _listener.Start();
            _isRunning = true;

            Console.WriteLine("Сервер запущен на порту 8888...");

            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();

            foreach (var user in _users)
            {
                user.Client?.Close();
            }
            _users.Clear();
        }

        private void HandleClient(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();
            ServerUser user = null;

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string userName = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                user = new ServerUser()
                {
                    Id = _nextId++,
                    Name = userName,
                    Client = client,
                    Stream = stream
                };

                _users.Add(user);

                Message connectMessage = new Message("Система", $"{user.Name} подключился!");
                BroadcastMessage(connectMessage);
                UpdateUsersList();

                Console.WriteLine($"Пользователь {user.Name} подключился (ID: {user.Id})");

                while (client.Connected)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (receivedData == "/disconnect")
                        break;

                    try
                    {
                        Message receivedMessage = Message.Deserialize(receivedData);
                        BroadcastMessage(receivedMessage);
                    }
                    catch
                    {
                        Message errorMessage = new Message(user.Name, receivedData);
                        BroadcastMessage(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка с клиентом {user?.Name}: {ex.Message}");
            }
            finally
            {
                if (user != null)
                {
                    _users.Remove(user);
                    Message disconnectMessage = new Message("Система", $"{user.Name} отключился!");
                    BroadcastMessage(disconnectMessage);
                    UpdateUsersList();
                    Console.WriteLine($"Пользователь {user.Name} отключился");
                }

                client.Close();
            }
        }

        private void BroadcastMessage(Message message)
        {
            string messageData = message.Serialize();
            byte[] data = Encoding.UTF8.GetBytes(messageData);

            List<ServerUser> disconnectedUsers = new List<ServerUser>();

            foreach (var user in _users)
            {
                try
                {
                    if (user.Client.Connected)
                    {
                        user.Stream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        disconnectedUsers.Add(user);
                    }
                }
                catch
                {
                    disconnectedUsers.Add(user);
                }
            }

            foreach (var disconnectedUser in disconnectedUsers)
            {
                _users.Remove(disconnectedUser);
            }
        }

        private void UpdateUsersList()
        {
            string usersList = "USERS_LIST:" + string.Join(",", _users.ConvertAll(u => u.Name));
            byte[] data = Encoding.UTF8.GetBytes(usersList);

            foreach (var user in _users)
            {
                try
                {
                    if (user.Client.Connected)
                    {
                        user.Stream.Write(data, 0, data.Length);
                    }
                }
                catch
                {
                }
            }
        }
    }
}