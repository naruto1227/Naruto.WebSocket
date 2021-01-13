using Microsoft.Extensions.DependencyInjection;
using Naruto.WebSocket.Interface;
using Naruto.WebSocket.Interface.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.WebSocket.Internal
{
    public sealed class ClientSend<TService> : IClientSend<TService> where TService : NarutoWebSocketService
    {

        private readonly IServiceProvider serviceProvider;


        private ICurrentClient currentClient;

        private IAllClient allClient;

        private IOtherClient otherClient;

        private IGroupClient groupClient;

        public ClientSend(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }


        ICurrentClient IClientSend.Current
        {
            get
            {
                if (currentClient != null)
                {
                    return currentClient;
                }
                currentClient = serviceProvider.GetRequiredService<ICurrentClient<TService>>();
                return currentClient;
            }
        }


        IAllClient IClientSend.All
        {
            get
            {
                if (allClient != null)
                {
                    return allClient;
                }
                allClient = serviceProvider.GetRequiredService<IAllClient<TService>>();
                return allClient;
            }
        }

        IGroupClient IClientSend.Group
        {
            get
            {
                if (groupClient != null)
                {
                    return groupClient;
                }
                groupClient = serviceProvider.GetRequiredService<IGroupClient<TService>>();
                return groupClient;
            }
        }

        IOtherClient IClientSend.Other
        {
            get
            {
                if (otherClient != null)
                {
                    return otherClient;
                }
                otherClient = serviceProvider.GetRequiredService<IOtherClient<TService>>();
                return otherClient;
            }
        }
    }
}
