using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace ChatClient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox textBoxMessage;
        private TextBox textBoxIp;
        private TextBox textBoxPort;
        private TextBox textBoxUsername;
        private ListBox listBoxChat;
        private ListBox listBoxUsers;
        private Button buttonConnect;
        private Button buttonDisConnect;
        private Button buttonSend;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelChat;
        private System.Windows.Forms.Label labelUsers;
        private System.Windows.Forms.Label labelMessage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            textBoxMessage = new TextBox();
            textBoxIp = new TextBox();
            textBoxPort = new TextBox();
            textBoxUsername = new TextBox();
            listBoxChat = new ListBox();
            listBoxUsers = new ListBox();
            buttonConnect = new Button();
            buttonDisConnect = new Button();
            buttonSend = new Button();
            labelIp = new System.Windows.Forms.Label();
            labelPort = new System.Windows.Forms.Label();
            labelUsername = new System.Windows.Forms.Label();
            labelChat = new System.Windows.Forms.Label();
            labelUsers = new System.Windows.Forms.Label();
            labelMessage = new System.Windows.Forms.Label();

            textBoxMessage.Location = new Point(12, 245);
            textBoxMessage.Size = new Size(329, 20);
            textBoxMessage.KeyDown += textBoxMessage_KeyDown;

            textBoxIp.Location = new Point(45, 285);
            textBoxIp.Size = new Size(100, 20);

            textBoxPort.Location = new Point(45, 311);
            textBoxPort.Size = new Size(100, 20);

            textBoxUsername.Location = new Point(45, 337);
            textBoxUsername.Size = new Size(100, 20);

            listBoxChat.Location = new Point(12, 25);
            listBoxChat.Size = new Size(419, 199);

            listBoxUsers.Location = new Point(518, 25);
            listBoxUsers.Size = new Size(150, 199);

            buttonConnect.Location = new Point(151, 283);
            buttonConnect.Size = new Size(85, 23);
            buttonConnect.Text = "Подключиться";
            buttonConnect.Click += buttonConnect_Click;

            buttonDisConnect.Location = new Point(151, 312);
            buttonDisConnect.Size = new Size(85, 23);
            buttonDisConnect.Text = "Отключиться";
            buttonDisConnect.Click += buttonDisConnect_Click;

            buttonSend.Location = new Point(345, 245);
            buttonSend.Size = new Size(85, 23);
            buttonSend.Text = "Отправить";
            buttonSend.Click += buttonSend_Click;

            labelChat.Location = new Point(12, 9);
            labelChat.Size = new Size(125, 13);
            labelChat.Text = "История чата:";

            labelUsers.Location = new Point(515, 9);
            labelUsers.Size = new Size(125, 13);
            labelUsers.Text = "Подключенные пользователи:";
            labelUsers.AutoSize = true;

            labelMessage.Location = new Point(12, 229);
            labelMessage.Size = new Size(68, 13);
            labelMessage.Text = "Сообщение:";
            labelMessage.AutoSize = true;

            labelIp.Location = new Point(12, 288);
            labelIp.Size = new Size(20, 13);
            labelIp.Text = "IP:";
            labelIp.AutoSize = true;

            labelPort.Location = new Point(12, 314);
            labelPort.Size = new Size(35, 13);
            labelPort.Text = "Порт:";
            labelPort.AutoSize = true;

            labelUsername.Location = new Point(12, 340);
            labelUsername.Size = new Size(32, 13);
            labelUsername.Text = "Имя:";

            Controls.AddRange(new Control[] {
                textBoxMessage, textBoxIp, textBoxPort, textBoxUsername, listBoxChat, listBoxUsers, 
                buttonConnect, buttonDisConnect, buttonSend,labelChat, labelIp, labelPort, labelUsername, labelUsers, labelMessage
            });

            this.Size = new Size(700, 450);
        }

    }
}

