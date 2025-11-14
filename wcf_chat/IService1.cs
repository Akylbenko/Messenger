using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    [ServiceContract(CallbackContract = typeof(Iservice1Callback))]
    public interface IService1
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disonnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMessage(string message, int id);
    }

    public interface Iservice1Callback
    {
        [OperationContract(IsOneWay = true)]
        void MessageCallback(string message);
    }
}
