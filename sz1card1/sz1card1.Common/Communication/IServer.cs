using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace sz1card1.Common.Communication
{
    [ServiceContract(Namespace = "sz1card1", SessionMode = SessionMode.Required, CallbackContract = typeof(IClient))]
    public interface IServer
    {
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void Send(System.ServiceModel.Channels.Message msg);
    }
}
