using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ChatService
{
    internal class ServerUser
    {
        private int _id;
        private string _name;
        private OperationContext operationContext;

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public OperationContext OperationContext { get { return operationContext; } set { operationContext = value; } }
    }
}
