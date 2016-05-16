using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace sz1card1.Common.Communication
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IClient))]
    public interface IClient
    {
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void Receive(System.ServiceModel.Channels.Message msg);
    }
}
