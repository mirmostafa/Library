using System;
using System.ServiceModel;
using Mohammad.ServiceModel;

namespace TestConsole45
{
    public partial class App
    {
        protected override void Execute()
        {
            var uri = "net.tcp://localhost:1234/test.svc";
            var host = ServiceHost<IChat, Chat>.CreateInstance(uri);
            host.Open();

            var client1 = ServiceClient<IChat>.GetChannel(uri);
            client1.Send("client1");

            var client2 = ServiceClient<IChat>.GetChannel(uri);
            client1.Send("client2");
        }
    }

    [ServiceContract]
    public interface IChat
    {
        [OperationContract]
        void Send(string message);
    }

    public class Chat : IChat
    {
        public void Send(string message) => Console.WriteLine(message);
    }
}