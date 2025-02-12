using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace TerminalRobo.Models
{
    class BasicAuthMessageInspector : IClientMessageInspector
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthMessageInspector(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // No-op
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_password}"));
            var httpRequestMessage = new HttpRequestMessageProperty();
            httpRequestMessage.Headers["Authorization"] = "Basic " + authToken;

            request.Properties[HttpRequestMessageProperty.Name] = httpRequestMessage;
            return null;
        }
    }
}
