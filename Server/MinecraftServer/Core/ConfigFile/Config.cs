using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.ConfigFile
{
    public class Config
    {
        #region Variables

        public int port { get; private set; }
        public string name { get; private set; }
        public int maxPlayers { get; private set; }

        #endregion

        #region Constructors

        public Config()
        {
            port = 25565;
            name = "Not a Minecraft server !";
            maxPlayers = 10;
        }

        #endregion

        #region Functions

        public bool init()
        {
            if (System.IO.File.Exists("server.config"))
            {
                System.IO.StreamReader stream = new System.IO.StreamReader("server.config");
                String[] config = stream.ReadToEnd().Split('\n');
                foreach (var param in config)
                    this.appendParams(param.Split('='));
                stream.Close();
                return true;
            }
            else
            {
                MinecraftServer.Core.Network.Server.ViewModel.Warning("server.config doesn't exist! Default parameters will be used.");
                return false;
            }
        }

        private void appendParams(String[] param)
        {
            for (int i = 0; i < param.Length; ++i)
            {
                if (param.ElementAt(i).ToString() == "name")
                    this.name = param.ElementAt(i + 1).ToString().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""); ;
                if (param.ElementAt(i).ToString() == "port")
                    this.port = int.Parse(param.ElementAt(i + 1));
                if (param.ElementAt(i).ToString() == "maxplayers")
                    this.maxPlayers = int.Parse(param.ElementAt(i + 1));
            }
        }
        #endregion
    }
}
