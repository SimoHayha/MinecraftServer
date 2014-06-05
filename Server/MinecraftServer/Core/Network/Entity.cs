using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.Network
{
    public class Entity
    {
        public struct Position
        {
            public double X;
            public double Z;
            public double FeetY;
            public double HeadY;
            public double Yaw;
            public double Pitch;
            public bool OnGround;
        }

        public Position EntityPosition;

        public int EntityId;

        public Entity(int entityId)
        {
            EntityId = entityId;
        }

        public bool Update()
        {
            return true;
        }
    }
}
