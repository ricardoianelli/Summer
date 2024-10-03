using AISServer.Components.Io.Enums;
using AISServer.Components.Io.Interfaces;
using MonsantoAutomation.Devices.Applicom;

namespace AISServer.Components.Io;

public class IoController : IIoController
{
    private const ushort CardId = 1;
    private Profibus _profibus;
    
    public void Initialize()
    {
        if (_profibus?.Connected == true) return;
        
        _profibus = new Profibus();
        var error = _profibus.Connect(CardId);
        Console.WriteLine($"Connect - error: {_profibus.GetErrorDescription(error)}");
    }

    public void Shutdown()
    {
        if (_profibus?.Connected != true) return;

        _profibus.Disconnect();
    }

    public List<ushort> GetNodes()
    {
        var nodes = new List<ushort>();
        _profibus.GetNodes(nodes);
        return nodes;
    }

    public bool GetBit(ProfibusInputId inputId)
    {
        var bitValue = false;
        var ioData = IoMappings.InputMappings[inputId];
        var error = _profibus.GetInputBit(ioData.Node, ioData.Offset, ref bitValue);
        Console.WriteLine($"Get bit - InputId: {inputId}, bitValue: {bitValue}, error: {_profibus.GetErrorDescription(error)}");
        return bitValue;
    }
    
    public bool GetBit(ProfibusOutputId outputId)
    {
        var bitValue = false;
        var ioData = IoMappings.OutputMappings[outputId];
        var error = _profibus.GetInputBit(ioData.Node, ioData.Offset, ref bitValue);
        Console.WriteLine($"Get bit - OutputId: {outputId}, bitValue: {bitValue}, error: {_profibus.GetErrorDescription(error)}");
        return bitValue;
    }

    public bool SetBit(ProfibusOutputId outputId, bool bitValue)
    {
        var ioData = IoMappings.OutputMappings[outputId];
        _profibus.SetBit(ioData.Node, ioData.Offset, bitValue);
        Console.WriteLine($"Set bit - OutputId: {outputId}, bitValue: {bitValue}");
        
        return GetBit(outputId);
    }

    public bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue)
    {
        throw new NotImplementedException();
    }

    public bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue)
    {
        throw new NotImplementedException();
    }
}