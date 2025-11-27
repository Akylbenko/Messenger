using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net.Sockets;

namespace ChatService
{
    internal class ServerUser
    {
        private int _id;
        private string _name;
        private TcpClient _client;
        private NetworkStream _stream;

        public int Id { get { return _id; }  set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public TcpClient Client { get { return _client; } set { _client = value;  } }
        public NetworkStream Stream { get { return _stream; } set { _stream = value; } }
    }
}
