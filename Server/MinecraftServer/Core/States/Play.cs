using MinecraftServer.Core.Packets.PlayPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Core.States
{
    public class Play : AState
    {
        public Play() : base("Play") { }

        protected override void OnInitializePacket()
        {
            AddSendPacket(0x05, new SpawnPacket());
            AddSendPacket(0x39, new AbilitiesPacket());
            AddSendPacket(0x03, new TimePacket());
            AddSendPacket(0x30, new InventoryPacket());
            AddSendPacket(0x06, new HealthPacket());
            AddSendPacket(0x1F, new ExperiencePacket());
            AddSendPacket(0x08, new PlayerPositionAndLookResponsePacket());
            AddSendPacket(0x21, new ChunkPacket());
            AddSendPacket(0x01, new JoinGamePacket());
            AddSendPacket(0x00, new KeepAlivePacket());
            AddSendPacket(0x15, new EntityRelativeMovePacket());
            AddSendPacket(0x16, new EntityLookPacket());
            AddSendPacket(0x17, new EntityLookAndRelativeMovePacket());
            AddSendPacket(0x0C, new SpawnPlayerPacket());
            AddSendPacket(0x19, new EntityHeadLookPacket());
            AddSendPacket(0x13, new DestroyEntitiesPacket());
            AddSendPacket(0x07, new RespawnPacket());

            AddReceiptPacket(0x03, new PlayerPacket());
            AddReceiptPacket(0x04, new PlayerPositionPacket());
            AddReceiptPacket(0x05, new PlayerLookPacket());
            AddReceiptPacket(0x06, new PlayerPositionAndLookPacket());
            AddReceiptPacket(0x08, new PlayerBlockPlacement());
            AddReceiptPacket(10, new AnimationPacket());
            AddReceiptPacket(0x15, new ClientSettingPacket());
            AddReceiptPacket(0x17, new PluginMessagePacket());
            AddReceiptPacket(11, new EntityActionPacket());
            AddReceiptPacket(0x00, new KeepAlivePacket());
            AddReceiptPacket(0x13, new PlayerAbilitiesPacket());
            AddReceiptPacket(0x07, new PlayerDiggingPacket());;
            AddReceiptPacket(0x09, new HeldItemChangePacket());
            AddReceiptPacket(0x02, new UseEntityPacket());
            AddReceiptPacket(0x16, new ClientStatusPacket());
            AddReceiptPacket(0x10, new CreativeInventoryActionPacket());
            AddReceiptPacket(0x0D, new CloseWindowPacket());
        }
    }
}
