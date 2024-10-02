using AISServer.Components.Hardware.Profibus.Interfaces;
using AISServer.Components.Logging.Interfaces;

namespace AISServer.Components.Hardware.Profibus
{
    public class ProfibusController : IProfibusController
    {
        private short _status; // Update to short to match AppIO method signatures
        private readonly ushort _cardId;
        private readonly ILogger _logger;

        public ProfibusController(ushort cardId, ILogger logger)
        {
            _cardId = cardId;
            _logger = logger;
        }

        public short Status => _status; // Now returns short

        public bool Initialize(ushort cardId)
        {
            _logger.LogInfo($"Initializing Profibus card with ID {cardId}...");
            bool success = AppIO.IO_Init(cardId, ref _status);
            if (!success)
            {
                _logger.LogError($"Profibus initialization failed for card {cardId}. Status code: {_status}");
            }
            return success;
        }

        public bool Shutdown(ushort cardId)
        {
            _logger.LogInfo($"Shutting down Profibus card with ID {cardId}...");
            bool success = AppIO.IO_Exit(cardId, ref _status);
            if (!success)
            {
                _logger.LogError($"Profibus shutdown failed for card {cardId}. Status code: {_status}");
            }
            return success;
        }

        public bool GetBit(ushort deviceId, ushort bitOffset, ref bool bitValue)
        {
            _logger.LogInfo($"Reading bit from device {deviceId} at bit offset {bitOffset}...");
            byte[] bitBuffer = new byte[1];
            bool success = AppIO.IO_ReadIBit(_cardId, deviceId, bitOffset, 1, bitBuffer, ref _status);
            if (!success)
            {
                _logger.LogError($"Failed to read bit from device {deviceId} at offset {bitOffset}. Status code: {_status}");
                return false;
            }

            bitValue = Convert.ToBoolean(bitBuffer[0]);
            _logger.LogInfo($"Successfully read bit value {bitValue} from device {deviceId} at offset {bitOffset}.");
            return true;
        }

        public bool SetBit(ushort deviceId, ushort bitOffset, bool bitValue)
        {
            _logger.LogInfo($"Writing bit value {bitValue} to device {deviceId} at bit offset {bitOffset}...");
            byte[] bitBuffer = new byte[1] { Convert.ToByte(bitValue) };
            bool success = AppIO.IO_WriteQBit(_cardId, deviceId, bitOffset, 1, bitBuffer, ref _status);
            if (!success)
            {
                _logger.LogError($"Failed to write bit value {bitValue} to device {deviceId} at offset {bitOffset}. Status code: {_status}");
            }
            return success;
        }

        public bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue)
        {
            _logger.LogInfo($"Reading byte from device {deviceId} at byte offset {byteOffset}...");
            byte[] byteBuffer = new byte[1];
            bool success = AppIO.IO_ReadIByte(_cardId, deviceId, byteOffset, 1, byteBuffer, ref _status);
            if (!success)
            {
                _logger.LogError($"Failed to read byte from device {deviceId} at offset {byteOffset}. Status code: {_status}");
                return false;
            }

            byteValue = byteBuffer[0];
            _logger.LogInfo($"Successfully read byte value {byteValue} from device {deviceId} at offset {byteOffset}.");
            return true;
        }

        public bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue)
        {
            _logger.LogInfo($"Writing byte value {byteValue} to device {deviceId} at byte offset {byteOffset}...");
            byte[] byteBuffer = new byte[1] { byteValue };
            bool success = AppIO.IO_WriteQByte(_cardId, deviceId, byteOffset, 1, byteBuffer, ref _status);
            if (!success)
            {
                _logger.LogError($"Failed to write byte value {byteValue} to device {deviceId} at offset {byteOffset}. Status code: {_status}");
            }
            return success;
        }
    }
}
