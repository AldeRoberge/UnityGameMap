namespace Migration.Packets
{
    

    public class RotateObjectPacket : Packet {
        public int objectUID;
        public int newRotation;
    }

    
}