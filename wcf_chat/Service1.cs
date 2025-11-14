using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;
using System.Text;

namespace ChatService

{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]   
    
    public class Service1 : IService1
    {   
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                Id = nextId,
                Name = name,
                OperationContext = OperationContext.Current
            };
            nextId++;
            SendMessage(user.Name + " подключился к чату!", 0);
            users.Add(user);
            return user.Id;
        }

        public void Disonnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage(user.Name + " покинул чат!", 0);
            }
        }

        public void SendMessage(string message, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    answer += ": " + user.Name + "";
                }

                answer += message;

                item.OperationContext.GetCallbackChannel<Iservice1Callback>().MessageCallback(answer);
            }
        }
    }
}

