using Migration.Packets.DontTransfer;

namespace Migration.Packets
{

    
    public class MoveObjectPacket : Packet {
        public int objectUID;
        public WorldPosData newPos;
    }
}