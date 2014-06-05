using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network.Clients
{
    class EmptyStateException : Exception
    {
        public EmptyStateException() { }
        public EmptyStateException(string message) : base(message) { }
        public EmptyStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
