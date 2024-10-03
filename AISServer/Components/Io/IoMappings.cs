using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io
{
    public static class IoMappings
    {
        public static Dictionary<ProfibusInputId, IoData> InputMappings { get; private set; }
        public static Dictionary<ProfibusOutputId, IoData> OutputMappings { get; private set; }

        static IoMappings()
        {
            InputMappings = new Dictionary<ProfibusInputId, IoData>
            {
                { ProfibusInputId.USER_DOOR_NOT_CLOSED, new IoData { Node = 1, Offset = 0} },
            };

            OutputMappings = new Dictionary<ProfibusOutputId, IoData>
            {
                { ProfibusOutputId.IMAGE_LIGHT_ON, new IoData { Node = 1, Offset = 8} },
            };
        }
    }
}