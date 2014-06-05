using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network.Clients
{
    public class ClientList : ObservableCollection<ObservableClient>
    {
        public ClientList() : base()
        {

        }
    }

    public class ObservableClient
    {
        private Client client;

        public ObservableClient(Client c)
        {
            client = c;
        }

        public Client Client
        {
            get { return client; }
            private set { client = value; }
        }
    }
}
