using System.ComponentModel;

namespace AISServer.Components.Hardware.Profibus.Enums;

public enum ProfibusErrorStatusId
  {
    /// <summary>No Fault Detected.</summary>
    [Description("No fault detected.")] NONE = 0,
    /// <summary>Unknown function.  The function requested is not supported.</summary>
    [Description("Unknown function.  The function requested is not supported.")] UNKNOWN_FUNCTION = 1,
    /// <summary>Negative acknowledge in layer 2 from device (NACK).  UE(User Error), remote device error.</summary>
    [Description("Negative acknowledge in layer 2 from device (NACK).  UE(User Error), remote device error.")] NACK_USER_ERROR = 10, // 0x0000000A
    /// <summary>Negative acknowledge in layer 2 from device (NACK).  RR(Remote Resource), insufficient resources in the remote device or invalid initialization parameters.</summary>
    [Description("Negative acknowledge in layer 2 from device (NACK).  RR(Remote Resource), insufficient resources in the remote device or invalid initialization parameters.")] NACK_RESOURCE = 11, // 0x0000000B
    /// <summary>Negative acknowledge in layer 2 from device (NACK).  RDL(Response FDL/FMA/1/2 Data Low), insufficient resources in the remote device to process the received data, low priority response.</summary>
    [Description("Negative acknowledge in layer 2 from device (NACK).  RDL(Response FDL/FMA/1/2 Data Low), insufficient resources in the remote device to process the received data, low priority response.")] NACK_DATA_LOW = 13, // 0x0000000D
    /// <summary>Negative acknowledge in layer 2 from device (NACK).  RDL(Response FDL/FMA/1/2 Data High), insufficient resoures in the remote device to process the received data, high priority response.</summary>
    [Description("Negative acknowledge in layer 2 from device (NACK).  RDL(Response FDL/FMA/1/2 Data High), insufficient resoures in the remote device to process the received data, high priority response.")] NACK_DATA_HIGH = 14, // 0x0000000E
    /// <summary>Indicates that the parameters passed to the functions are not correct.  (Eg. Number of Variables too large)</summary>
    [Description("Indicates that the parameters passed to the functions are not correct.  (Eg. Number of Variables too large)")] PARAMETERS_INCORRECT = 32, // 0x00000020
    /// <summary>Response time failure (Time-Out)</summary>
    [Description("Response time failure (Time-Out)")] TIME_OUT = 33, // 0x00000021
    /// <summary>Device not configured.  Define the device configuration with the applicomIO® Console and re-initiate the initialization of the applicomIO® product by running the PcInitIO</summary>
    [Description("Device not configured.  Define the device configureation with the applicomIO Console and re-initiate the initialization of the applicomIO product by running PcInitIO.exe.")] DEVICE_NOT_CONFIGURED = 36, // 0x00000024
    /// <summary>Non-resident dialogue software.  Initialize the applicomIO interface before use by running the PcInitIO.</summary>
    [Description("Non-resident dialogue software.  Initialize the applicomIO interface before use by running the PcInitIO.")] NOT_INITIALIZED = 45, // 0x0000002D
    /// <summary>Targeted applicomIO card configuration not valid</summary>
    [Description("Targeted applicomIO card configuration not valid")] DEVICE_CONFIGURATION_INVALID = 46, // 0x0000002E
    /// <summary>Targeted applicomIO card invalid or incorrectly initialized by the function IO_Init.</summary>
    [Description("Targeted applicomIO card invalid or incorrectly initialized by the function IO_Init.")] INCORRECT_INTIALIZATION = 47, // 0x0000002F
    /// <summary>Indicates that a communication error has been encountered on the serial port.</summary>
    [Description("Indicates that a communication error has been encountered on the serial port.")] SERIAL_PORT = 63, // 0x0000003F
    /// <summary>Not enough applicomIO interface memory.</summary>
    [Description("Not enough applicomIO interface memory.")] INSUFFCIENT_MEMORY = 66, // 0x00000042
    /// <summary>The array of bytes received is larger than the number of bytes requested.</summary>
    [Description("The array of bytes received is larger than the number of bytes requested.")] BYTE_COUNT_MISMATCH = 74, // 0x0000004A
    /// <summary>Driver cannot be accessed</summary>
    [Description("Driver cannot be accessed")] DRIVER_INACCESSIBLE = 93, // 0x0000005D
    /// <summary>ApplicomIO solution already running.</summary>
    [Description("ApplicomIO solution already running.")] ALREADY_RUNNING = 99, // 0x00000063
    /// <summary>The local input buffer was not updated beforehand by the function IO_RefreshInput.</summary>
    [Description("The local input buffer was not updated beforehand by the function IO_RefreshInput.")] INPUT_BUFFER = 255, // 0x000000FF
    /// <summary>Unknown Error Status.</summary>
    [Description("Unknown Error Status.")] UNKNOWN_STATUS = 1000, // 0x000003E8
    /// <summary>Already connected</summary>
    [Description("Already connected")] ALREADY_CONNECTED = 1001, // 0x000003E9
    /// <summary>The controller error handler thread failed to start.</summary>
    [Description("The controller error handler thread failed to start.")] ERROR_HANDLER_START = 1002, // 0x000003EA
    /// <summary>The message queue thread for the controller failed to start properly.</summary>
    [Description("The message queue thread for the controller failed to start properly.")] MESSAGE_QUEUE = 1003, // 0x000003EB
  }