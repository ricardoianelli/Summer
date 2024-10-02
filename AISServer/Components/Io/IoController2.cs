using AISServer.Components.Hardware.Profibus;
using AISServer.Components.Io.Interfaces;
using AISServer.Components.Logging.Interfaces;
using System;
using AISServer.Components.Hardware.Profibus.Enums;
using AISServer.Components.Io.Enums;

namespace AISServer.Components.Io
{
    public class IoController2 : IIoController
    {
        public ushort CardID = 1;
        public ushort Channel = 0;
        public bool Connected = false;
        
        private readonly ProfibusIoMappings _ioMappings;
        private readonly ILogger _logger;
        private ushort _status;

        public IoController2(ProfibusIoMappings ioMappings, ILogger logger)
        {
            _ioMappings = ioMappings;
            _logger = logger;
        }


        public ProfibusErrorStatusId Initialize()
        {
            try
            {
                _logger.LogInfo($"Initializing IoController2 card with ID {CardID}");

                ushort appIoInitializationStatus = 0;
                bool appIoInitialization = AppIO.IO_Init(CardID, ref appIoInitializationStatus);
                var errorStatusId = GetErrorStatusId(appIoInitializationStatus);
                _logger.LogInfo($"appIoInitialization: {appIoInitializationStatus} - Status: {errorStatusId}");
                
                if (!appIoInitialization)
                {
                    _logger.LogInfo($"appIoInitialization failed. Status: {errorStatusId}");
                    int num = (int) Disconnect();
                    return errorStatusId;
                }
                
                _logger.LogInfo("appIoInitialization finished!");
                
                uint applicomIOInitializationStatus = 0;
                bool applicomIOInitialization = ApplicomIO.AuInitBus_io(ref applicomIOInitializationStatus);
                var applicomErrorStatusId = GetErrorStatusId(applicomIOInitializationStatus);
                _logger.LogInfo($"applicomIOInitialization: {applicomIOInitialization} - Status: {applicomErrorStatusId}");
                
                
                if (!applicomIOInitialization)
                {
                    _logger.LogInfo($"applicomIOInitialization failed. Status: {applicomErrorStatusId}");
                    AppIO.IO_Exit(CardID, ref appIoInitializationStatus);
                    return applicomErrorStatusId;
                }
            
                Connected = true;
                return ProfibusErrorStatusId.NONE;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public ProfibusErrorStatusId Disconnect()
        {
            ushort Status1 = 0;
            uint Status2 = 0;
            if (!AppIO.IO_Exit(CardID, ref Status1))
            {
                ApplicomIO.AuExitBus_io(ref Status2);
                return GetErrorStatusId(Status1);
            }
            if (!ApplicomIO.AuExitBus_io(ref Status2))
                return GetErrorStatusId(Status2);
            
            Connected = false;
            return ProfibusErrorStatusId.NONE;
        }

        public bool Shutdown(ushort cardId)
        {
            return Disconnect() != ProfibusErrorStatusId.NONE;
        }

        public bool GetBit(Enum id)
        {
            try
            {
                switch (id)
                {
                    case ProfibusInputId inputId when _ioMappings.InputMappings.ContainsKey(inputId):
                    {
                        _logger.LogInfo($"Getting input bit for {inputId}");
                        return GetInputBit(inputId);
                    }
                    case ProfibusOutputId outputId when _ioMappings.OutputMappings.ContainsKey(outputId):
                        _logger.LogInfo($"Getting output bit for {outputId}");
                        return GetOutputBit(outputId);
                    default:
                        _logger.LogError("Invalid IO ID provided for GetBit.");
                        throw new ArgumentException("Invalid IO ID provided");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        
        public bool GetInputBit(ProfibusInputId inputId)
        {
            if (!RefreshInput())
            {
                _logger.LogError($"Failed to refresh input buffer before reading {inputId}");
                return false;
            }
            
            var ioData = _ioMappings.InputMappings[inputId];

            bool[] boolData = new bool[1];
            int inputBits = (int) GetInputBits(ioData.NodeId, ioData.Offset, boolData);
            var status = (ProfibusErrorStatusId) inputBits;
            
            var result = boolData[0];

            _logger.LogError($"Read input bit for {inputId}, status: {status}, result: {result}");
            return false;
        }

        public bool GetOutputBit(ProfibusOutputId outputId)
        {
            if (!RefreshOutput())
            {
                _logger.LogError($"Failed to refresh output buffer before reading {outputId}");
                return false;
            }
            
            var ioData = _ioMappings.OutputMappings[outputId];

            ushort usInputByteCount = 0;
            ushort usOutputByteCount = 0;
            int nodeByteCountResult = (int) GetNodeByteCount(ioData.NodeId, ref usInputByteCount, ref usOutputByteCount);
            _logger.LogInfo("nodeByteCountResult: " + nodeByteCountResult + ", usOutputByteCount: " + usOutputByteCount);
            byte[] byteOutput = new byte[usOutputByteCount];
            int outputBytesResult = (int) GetOutputBytes(ioData.NodeId, byteOutput);
            _logger.LogInfo("outputBytesResult: " + outputBytesResult + ", byteOutput: " + byteOutput);
            int index = ioData.Offset / 8;
            int num2 = ioData.Offset % 8;
            
            _logger.LogInfo("index: " + index);
            _logger.LogInfo("num2: " + num2);
            var result = (byteOutput[index] & (uint) (1 << num2)) > 0U;

            _logger.LogError($"Read output bit for {outputId},  result: {result}");
            return result;
        }

        public ProfibusErrorStatusId GetInputBits(ushort usNode, ushort usOffset, bool[] boolData)
        {
            ushort length = (ushort) boolData.Length;
            byte[] TabBite = new byte[length];
            ushort Status = 0;
            AppIO.IO_ReadIBit(CardID, usNode, usOffset, length, TabBite, ref Status);
            ProfibusErrorStatusId errorStatusId = this.GetErrorStatusId(Status);
            if (errorStatusId == ProfibusErrorStatusId.NONE)
            {
                for (int index = 0; index < TabBite.Length && index < length; ++index)
                    boolData[index] = Convert.ToBoolean(TabBite[index]);
            }
            return errorStatusId;
        }
        

        public bool SetBit(ProfibusOutputId outputId, bool bitValue)
        {
            try
            {
                ushort Status = 0;
                var byteValues = new byte[1];
                byteValues[0] = Convert.ToByte(bitValue);
                
                var ioData = _ioMappings.OutputMappings[outputId];
                return AppIO.IO_WriteQBit(CardID, ioData.NodeId, ioData.Offset, 1, byteValues,
                    ref Status);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public bool GetByte(ushort deviceId, ushort byteOffset, ref byte byteValue)
        {
            throw new NotImplementedException();
        }

        public bool SetByte(ushort deviceId, ushort byteOffset, byte byteValue)
        {
            throw new NotImplementedException();
        }

        private ProfibusErrorStatusId GetErrorStatusId(uint uiValue)
        {
            return Enum.IsDefined(typeof (ProfibusErrorStatusId), (int) uiValue) ? (ProfibusErrorStatusId) uiValue : ProfibusErrorStatusId.UNKNOWN_STATUS;
        }
        
        private ProfibusErrorStatusId GetErrorStatusId(ushort usValue)
        {
            return Enum.IsDefined(typeof (ProfibusErrorStatusId), (int) usValue) ? (ProfibusErrorStatusId) usValue : ProfibusErrorStatusId.UNKNOWN_STATUS;
        }
        
        public bool RefreshInput()
        {
            _logger.LogInfo($"Refreshing input buffer for card {CardID}...");
            if (!AppIO.IO_RefreshInput(CardID, ref _status))
            {
                _logger.LogError($"Failed to refresh input buffer for card {CardID}. Status code: {_status}");
                return false;
            }
            return true;
        }

        public bool RefreshOutput()
        {
            _logger.LogInfo($"Refreshing output buffer for card {CardID}...");
            if (!AppIO.IO_RefreshOutput(CardID, ref _status))
            {
                _logger.LogError($"Failed to refresh output buffer for card {CardID}. Status code: {_status}");
                return false;
            }
            return true;
        }
        
        public ProfibusErrorStatusId GetNodeByteCount(
            ushort usNode,
            ref ushort usInputByteCount,
            ref ushort usOutputByteCount)
        {
            ushort Status = 0;
            AppIO.IO_GetEquipmentInfo(CardID, usNode, ref usInputByteCount, ref usOutputByteCount, ref Status);
            _logger.LogError("AppIO.IO_GetEquipmentInfo - usInputByteCount: " + usInputByteCount + " usOutputByteCount: " + usOutputByteCount + " Status: " + Status);
            return GetErrorStatusId(Status);
        }
        
        public ProfibusErrorStatusId GetOutputBytes(ushort usNode, byte[] byteOutput)
        {
            ushort wNbTx = 0;
            byte[] byBufTx = new byte[1];
            ushort length = (ushort) byteOutput.Length;
            uint Status = 0;
            ApplicomIO.AuWriteReadMsg_io(Channel, usNode, 57U, wNbTx, byBufTx, ref length, byteOutput, ref Status);
            return this.GetErrorStatusId(Status);
        }

    }
}
