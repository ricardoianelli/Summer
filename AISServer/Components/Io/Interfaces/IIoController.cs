using AISServer.Components.Hardware.Profibus.Enums;
using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io.Interfaces
{
    public interface IIoController
    {
        ProfibusErrorStatusId Initialize();
        bool Shutdown(ushort cardId);

        bool GetBit(Enum id);
        bool SetBit(ProfibusOutputId outputId, bool bitValue);

        bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue);
        bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue);
    }
}