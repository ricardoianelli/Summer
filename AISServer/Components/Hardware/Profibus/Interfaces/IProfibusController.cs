namespace AISServer.Components.Hardware.Profibus.Interfaces
{
    public interface IProfibusController
    {
        short Status { get; }
        
        bool Initialize(ushort cardId);
        bool Shutdown(ushort cardId);
        bool GetBit(ushort deviceId, ushort bitOffset, ref bool bitValue);
        bool SetBit(ushort deviceId, ushort bitOffset, bool bitValue);
        bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue);
        bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue);
    }
}