using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.States
{
    public class StateManager
    {
        private Dictionary<string, AState> _states;

        public static StateManager Instance;

        public StateManager() { }

        public void Init()
        {
            _states = new Dictionary<string, AState>();

            _states.Add("handshake", new Handshake());
            _states.Add("login", new Login());
            _states.Add("play", new Play());
            _states.Add("status", new Status());

            Instance = this;
        }

        public static AState FindState(string state)
        {
            AState s = Instance._states[state];

            if (s == null)
                throw new NullReferenceException("State " + s + " doesn't exist, default are handshake, login, play, status");

            return s;
        }
    }
}
