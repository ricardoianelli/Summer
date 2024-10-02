using AISServer.Components.Hardware.Profibus.Enums;

namespace AISServer.Components.Hardware.Profibus;

public class ProfibusIOData
{
    public ushort NodeId { get; set; }
    public ushort Offset { get; set; }
    public ProfibusIOTypeId Type { get; set; }
}