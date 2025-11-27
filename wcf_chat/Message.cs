using System;
using System.Text;

namespace ChatService
{
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public Message(string sender, string text, DateTime time)
        {
            Sender = sender;
            Text = text;
            Time = time;
        }

        public Message(string sender, string text) : this(sender, text, DateTime.Now) { }

        public string Serialize()
        {
            return $"{Sender}|{Text}|{Time:yyyy-MM-dd HH:mm:ss}";
        }

        public static Message Deserialize(string data)
        {
            string[] parts = data.Split('|');
            if (parts.Length == 3)
            {
                DateTime time = DateTime.ParseExact(parts[2], "yyyy-MM-dd HH:mm:ss", null);
                return new Message(parts[0], parts[1], time);
            }
            return new Message("System", data);
        }

        public override string ToString()
        {
            return $"[{Time:HH:mm}] {Sender}: {Text}";
        }
    }
}