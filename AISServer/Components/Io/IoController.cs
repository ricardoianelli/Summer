using AISServer.Components.Hardware.Profibus;
using AISServer.Components.Io.Interfaces;
using AISServer.Components.Logging.Interfaces;
using AISServer.Components.Hardware.Profibus.Enums;
using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io
{
    public class IoController : IIoController
    {
        private readonly ushort _cardId = 1;
        private ushort _status;
        private readonly ProfibusIoMappings _ioMappings;
        private readonly ILogger _logger;

        public IoController(ProfibusIoMappings ioMappings, ILogger logger)
        {
            _ioMappings = ioMappings;
            _logger = logger;
        }

        public ProfibusErrorStatusId Initialize()
        {
            _logger.LogInfo($"Initializing IoController card with ID {_cardId}");
            bool result = AppIO.IO_Init(_cardId, ref _status);
            if (!result)
                _logger.LogError($"Initialization failed for card {_cardId}, status: {_status}");
            return ProfibusErrorStatusId.NONE;
        }

        public bool Shutdown(ushort cardId)
        {
            _logger.LogInfo($"Shutting down card with ID {cardId}");
            bool result = AppIO.IO_Exit(cardId, ref _status);
            if (!result)
                _logger.LogError($"Shutdown failed for card {cardId}, status: {_status}");
            return result;
        }

        public bool GetBit(Enum id)
        {
            bool value = false;
            
            if (!RefreshInput())
            {
                _logger.LogError($"Failed to refresh input buffer before reading {id}");
                return false;
            }
            
            if (id is ProfibusInputId inputId && _ioMappings.InputMappings.ContainsKey(inputId))
            {
                _logger.LogInfo($"Getting input bit for {inputId}");
                return GetInputBit(inputId, ref value);
            }

            if (id is ProfibusOutputId outputId && _ioMappings.OutputMappings.ContainsKey(outputId))
            {
                _logger.LogInfo($"Getting output bit for {outputId}");
                return GetOutputBit(outputId, ref value);
            }

            _logger.LogError("Invalid IO ID provided for GetBit.");
            throw new ArgumentException("Invalid IO ID provided");
        }

        public bool SetBit(ProfibusOutputId outputId, bool bitValue)
        {
            _logger.LogInfo($"Setting output bit for {outputId} to {bitValue}");
            
            if (!RefreshOutput())
            {
                _logger.LogError($"Failed to refresh output buffer before writing to {outputId}");
                return false;
            }
            
            if (!_ioMappings.OutputMappings.ContainsKey(outputId))
            {
                _logger.LogError($"Invalid output ID: {outputId}");
                throw new ArgumentException($"Invalid output ID: {outputId}");
            }

            return WriteOutputBit(outputId, bitValue);
        }

        public bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue)
        {
            _logger.LogInfo($"Getting byte for device {deviceId} at offset {byteOffset}");
            byte[] byteBuffer = new byte[1];
            if (AppIO.IO_ReadIByte(_cardId, deviceId, byteOffset, 1, byteBuffer, ref _status))
            {
                byteValue = byteBuffer[0];
                _logger.LogInfo($"Successfully read byte {byteValue} from device {deviceId} at offset {byteOffset}");
                return true;
            }

            _logger.LogError($"Failed to read byte from device {deviceId} at offset {byteOffset}, status: {_status}");
            return false;
        }

        public bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue)
        {
            _logger.LogInfo($"Setting byte for device {deviceId} at offset {byteOffset} to {byteValue}");
            byte[] byteBuffer = new byte[1] { byteValue };
            bool result = AppIO.IO_WriteQByte(_cardId, deviceId, byteOffset, 1, byteBuffer, ref _status);
            if (!result)
                _logger.LogError($"Failed to write byte {byteValue} to device {deviceId} at offset {byteOffset}, status: {_status}");
            return result;
        }

        private bool GetInputBit<T>(ProfibusInputId inputId, ref T value)
        {
            var ioData = _ioMappings.InputMappings[inputId];

            if (ioData.Type == ProfibusIOTypeId.INPUT_BIT)
            {
                byte[] bitBuffer = new byte[1];
                if (AppIO.IO_ReadIBit(_cardId, ioData.NodeId, ioData.Offset, 1, bitBuffer, ref _status))
                {
                    value = (T)(object)Convert.ToBoolean(bitBuffer[0]);
                    _logger.LogInfo($"Successfully read input bit {value} for {inputId}");
                    return true;
                }
            }

            _logger.LogError($"Failed to read input bit for {inputId}, status: {_status}");
            return false;
        }

        private bool GetOutputBit<T>(ProfibusOutputId outputId, ref T value)
        {
            var ioData = _ioMappings.OutputMappings[outputId];

            if (ioData.Type == ProfibusIOTypeId.OUTPUT_BIT)
            {
                byte[] bitBuffer = new byte[1];
                if (AppIO.IO_ReadIBit(_cardId, ioData.NodeId, ioData.Offset, 1, bitBuffer, ref _status))
                {
                    value = (T)(object)Convert.ToBoolean(bitBuffer[0]);
                    _logger.LogInfo($"Successfully read output bit {value} for {outputId}");
                    return true;
                }

                _logger.LogInfo($"IO_ReadIBit didnt work!");
            }
            else
            {
                _logger.LogInfo($"ioData.Type != output_bit");
            }

            _logger.LogError($"Failed to read output bit for {outputId}, status: {_status}");
            return false;
        }

        private bool WriteOutputBit(ProfibusOutputId outputId, bool bitValue)
        {
            var ioData = _ioMappings.OutputMappings[outputId];
            byte[] bitBuffer = new byte[1] { Convert.ToByte(bitValue) };
            bool result = AppIO.IO_WriteQBit(_cardId, ioData.NodeId, ioData.Offset, 1, bitBuffer, ref _status);
            if (!result)
                _logger.LogError($"Failed to write output bit {bitValue} to {outputId}, status: {_status}");
            return result;
        }
        
        public bool RefreshInput()
        {
            _logger.LogInfo($"Refreshing input buffer for card {_cardId}...");
            if (!AppIO.IO_RefreshInput(_cardId, ref _status))
            {
                _logger.LogError($"Failed to refresh input buffer for card {_cardId}. Status code: {_status}");
                return false;
            }
            return true;
        }

        public bool RefreshOutput()
        {
            _logger.LogInfo($"Refreshing output buffer for card {_cardId}...");
            if (!AppIO.IO_RefreshOutput(_cardId, ref _status))
            {
                _logger.LogError($"Failed to refresh output buffer for card {_cardId}. Status code: {_status}");
                return false;
            }
            return true;
        }

    }
}
