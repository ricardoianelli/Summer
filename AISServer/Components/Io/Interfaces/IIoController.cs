using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io.Interfaces
{
    public interface IIoController
    {
        void Initialize();
        void Shutdown();

        List<ushort> GetNodes();

        bool GetBit(ProfibusInputId inputId);
        bool GetBit(ProfibusOutputId outputId);
        bool SetBit(ProfibusOutputId outputId, bool bitValue);

        bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue);
        bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue);
    }
}