using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace TerminalRobo.Models
{
    class BasicAuthEndpointBehavior : IEndpointBehavior
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthEndpointBehavior(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // No-op
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new BasicAuthMessageInspector(_username, _password));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // No-op
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // No-op
        }
    }
}
