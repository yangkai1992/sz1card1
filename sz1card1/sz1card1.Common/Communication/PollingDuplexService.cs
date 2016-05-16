using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace sz1card1.Common.Communication
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    //[AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Required)]
    public class PollingDuplexService : IServer
    {
        public IProcessor Processor { get; set; }

        public void Send(System.ServiceModel.Channels.Message msg)
        {
            IClient client = OperationContext.Current.GetCallbackChannel<IClient>();
            PollingMessage message = msg.GetBody<PollingMessage>();
            EventHandler<PollingDuplexEventArgs> handler = new EventHandler<PollingDuplexEventArgs>(OnReceived);
            PollingDuplexEventArgs args = new PollingDuplexEventArgs() { Client = client, Message = message };
            IAsyncResult result = handler.BeginInvoke(this, args, new AsyncCallback(OnCompleted), handler);
            if (result.CompletedSynchronously)
                OnCompleted(result);
        }

        private void OnReceived(object sender, PollingDuplexEventArgs e)
        {
            if (Processor != null)
            {
                Processor.Process(e.Message, e.Client);
            }
        }

        private void OnCompleted(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
                return;
            else
            {
                EventHandler<PollingDuplexEventArgs> handler = (EventHandler<PollingDuplexEventArgs>)result.AsyncState;
                handler.EndInvoke(result);
            }
        }
    }
}
