using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;
        private bool _connected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectUser()
        {
            if (_connected) return;

            if (string.IsNullOrWhiteSpace(textBoxUsername.Text))
            {
                MessageBox.Show("Введите имя пользователя!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxIp.Text) || string.IsNullOrWhiteSpace(textBoxPort.Text))
            {
                MessageBox.Show("Введите IP и порт сервера!");
                return;
            }

            try
            {
                _client = new TcpClient();
                _client.Connect(textBoxIp.Text, int.Parse(textBoxPort.Text));
                _stream = _client.GetStream();

                byte[] userNameData = Encoding.UTF8.GetBytes(textBoxUsername.Text);
                _stream.Write(userNameData, 0, userNameData.Length);

                _connected = true;

                _receiveThread = new Thread(ReceiveMessages);
                _receiveThread.IsBackground = true;
                _receiveThread.Start();

                listBoxChat.Items.Add("Вы подключились к чату!");
                buttonConnect.Enabled = false;
                buttonDisConnect.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения!");
            }
        }

        private void DisconnectUser()
        {
            if (!_connected) return;

            try
            {
                byte[] disconnectData = Encoding.UTF8.GetBytes("/disconnect");
                _stream.Write(disconnectData, 0, disconnectData.Length);
            }
            catch
            {
            }

            _connected = false;
            _receiveThread?.Abort();
            _stream?.Close();
            _client?.Close();

            listBoxChat.Items.Add("Вы отключились.");
            buttonConnect.Enabled = true;
            buttonDisConnect.Enabled = false;
        }

        private void SendMessage()
        {
            if (!_connected) return;

            string messageText = textBoxMessage.Text.Trim();
            if (string.IsNullOrEmpty(messageText)) return;

            try
            {
                Message message = new Message(textBoxUsername.Text, messageText);
                string messageData = message.Serialize(); 
                byte[] data = Encoding.UTF8.GetBytes(messageData);
                _stream.Write(data, 0, data.Length);
                textBoxMessage.Clear();
            }
            catch
            {
                MessageBox.Show("Ошибка отправки сообщения!");
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];

            while (_connected)
            {
                try
                {
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (receivedData.StartsWith("USERS_LIST:"))
                    {
                        string usersList = receivedData.Substring(11);
                        UpdateUsersList(usersList);
                    }
                    else
                    {
                        try
                        {
                            Message message = Message.Deserialize(receivedData);
                            Invoke((MethodInvoker)delegate
                            {
                                listBoxChat.Items.Add(message.ToString());
                            });
                        }
                        catch
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                listBoxChat.Items.Add(receivedData);
                            });
                        }
                    }
                }
                catch
                {
                    break;
                }
            }

            if (_connected)
            {
                Invoke((MethodInvoker)delegate
                {
                    DisconnectUser();
                });
            }
        }

        private void UpdateUsersList(string usersList)
        {
            Invoke((MethodInvoker)delegate
            {
                listBoxUsers.Items.Clear();
                string[] users = usersList.Split(',');
                foreach (string user in users)
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        listBoxUsers.Items.Add(user);
                    }
                }
            });
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ConnectUser();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectUser();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void textBoxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DisconnectUser();
            base.OnFormClosing(e);
        }
    }
}