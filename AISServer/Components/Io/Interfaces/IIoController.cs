using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io.Interfaces
{
    public interface IIoController
    {
        bool Initialize(ushort cardId);
        bool Shutdown(ushort cardId);

        bool GetBit<T>(Enum id, ref T value);
        bool SetBit(ProfibusOutputId outputId, bool bitValue);

        bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue);
        bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue);
    }
}