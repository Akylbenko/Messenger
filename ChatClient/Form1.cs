using ChatClient.ServiceChat;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form, IService1Callback
    {
        Service1Client client;
        int id;
        bool connected = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void MessageCallback(string message)
        {
            Invoke((MethodInvoker)delegate
            {
                listBoxChat.Items.Add(message);
            });
        }

        private void ConnectUser()
        {
            if (connected) return;

            if (string.IsNullOrWhiteSpace(textBoxUsername.Text))
            {
                MessageBox.Show("Введите имя пользователя!");
                return;
            }

            try
            {
                var context = new InstanceContext(this);
                client = new Service1Client(context);

                id = client.Connect(textBoxUsername.Text);
                connected = true;

                listBoxChat.Items.Add("Вы подключились к чату!");
                buttonConnect.Enabled = false;
                buttonDisConnect.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }

        private void DisconnectUser()
        {
            if (!connected) return;

            try
            {
                client.Disonnect(id);
                client = null;
                connected = false;

                listBoxChat.Items.Add("Вы отключились.");
                buttonConnect.Enabled = true;
                buttonDisConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отключения: " + ex.Message);
            }
        }

        private void SendMessage()
        {
            if (!connected) return;

            string message = textBoxMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            client.SendMessage(message, id);
            textBoxMessage.Clear();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ConnectUser();
        }

        private void buttonDisConnect_Click(object sender, EventArgs e)
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
