using AISServer.Components.Hardware.Profibus.Enums;
using AISServer.Components.Io.Enums;

namespace AISServer.Components.Hardware.Profibus
{
    public class ProfibusIoMappings
    {
        public Dictionary<ProfibusInputId, ProfibusIOData> InputMappings { get; private set; }
        public Dictionary<ProfibusOutputId, ProfibusIOData> OutputMappings { get; private set; }

        public ProfibusIoMappings()
        {
            InputMappings = new Dictionary<ProfibusInputId, ProfibusIOData>
            {
                { ProfibusInputId.USER_DOOR_NOT_CLOSED, new ProfibusIOData { NodeId = 1, Offset = 0, Type = ProfibusIOTypeId.INPUT_BIT } },
                // Add other inputs...
            };

            OutputMappings = new Dictionary<ProfibusOutputId, ProfibusIOData>
            {
                { ProfibusOutputId.IMAGE_LIGHT_ON, new ProfibusIOData { NodeId = 1, Offset = 8, Type = ProfibusIOTypeId.OUTPUT_BIT } },
                // Add other outputs...
            };
        }
    }
}