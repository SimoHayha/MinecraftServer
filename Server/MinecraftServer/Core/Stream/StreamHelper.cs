using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Stream
{
    public static class StreamHelper
    {
        public static void WriteTo(System.IO.Stream sourceStream, System.IO.Stream targetStream)
        {
            byte[] buffer = new byte[0x10000];
            int n;
            while ((n = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
                targetStream.Write(buffer, 0, n);
        }
    }
}
