using System.Runtime.InteropServices;

namespace AISServer.Components.Hardware.Profibus
{
    internal static class AppIO
    {
    /// <summary>
    /// This function is used to initialize the IO mode on an applicomIO® interface. It is essential to call
    /// this function for each board configured before any other function in the library. It performs all
    /// operations required to run applicomIO® (checking the mapping, initializing offsets and size of
    /// each device, etc.).
    /// CAUTION: In the case where applicomIO board is not initialized, the IO_Init function returns only
    /// after a time-out time (about 10 seconds)
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_Init(ushort wCard, ref ushort Status);

    /// <summary>
    /// This function must be called at the end of the program using applicomIO® (even if IO_init
    /// returned a bad status).
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer.</param>
    /// <returns>TRUE if OK, FALSE in case of problem.</returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_Exit(ushort wCard, ref ushort Status);

    /// <summary>
    /// This function retrieves from the applicomIO® interfaces all the input data and the statuses of the
    /// devices present on the network. These values are not directly returned but are stored in local
    /// buffers. All read functions of type IO_ReadXXX and the access functions to the device statuses
    /// then use these local buffers.
    /// The function IO_RefreshInput ensures complete consistency of the data read together with
    /// maximum execution speed (the calling application can never be blocked due to synchronization
    /// problems).
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_RefreshInput(ushort wCard, ref ushort Status);

    /// <summary>
    /// This function is used to read the input bits of an device in non-packed format from the local buffer.
    /// N.B.: The local buffer must be refreshed beforehand by the function IO_RefreshInput.
    /// Remark: The offsetbit parameter corresponds to the position in number of bits of the first bit to be read
    /// from the start of the device data area.
    /// Example: if the device contains 3 data bytes, the offset of bit 0 in the third byte will be 16.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offsetbit">
    /// 16 bit integer, address of the first bit to read in the device.
    /// </param>
    /// <param name="NbBit">
    /// 16 bit integer, number of bits to read (1-MAX).
    /// </param>
    /// <param name="TabBite">Bits array, receiving the data read.</param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_ReadIBit(
      ushort wCard,
      ushort wEquip,
      ushort Offsetbit,
      ushort NbBit,
      byte[] TabBite,
      ref ushort Status);

    /// <summary>
    /// This function is used to read the input bytes of a device from the local buffer.
    /// N.B.: The local buffer must be refreshed beforehand by the function IO_RefreshInput.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offset">
    /// 16 bit integer, address of the first byte to be read in the device.
    /// </param>
    /// <param name="NbByte">
    /// 16 bit integer, number of bytes to be read (1-MAX).
    /// </param>
    /// <param name="TabByte">Bytes array, receiving the data read.</param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_ReadIByte(
      ushort wCard,
      ushort wEquip,
      ushort Offset,
      ushort NbByte,
      byte[] TabByte,
      ref ushort Status);

    /// <summary>
    /// This function is used to read the input words of a device from the local buffer.
    /// N.B.: The local buffer must be refreshed beforehand by the function IO_RefreshInput.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offset">
    /// 16 bit integer, address of the first word to be read in the device.
    /// </param>
    /// <param name="Nb">
    /// 16 bit integer, number of words to be read (1-MAX).
    /// </param>
    /// <param name="TabWord">
    /// 16 bit words array receiving the data read.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_ReadIWord(
      ushort wCard,
      ushort wEquip,
      ushort Offset,
      ushort Nb,
      ushort[] TabWord,
      ref ushort Status);

    /// <summary>
    /// This function is used to read the double input words of a device from the local buffer.
    /// N.B.: The local buffer must be refreshed beforehand by the function IO_RefreshInput.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offset">
    /// 16 bit integer, address of the first double word to be read in the device.
    /// </param>
    /// <param name="Nb">
    /// 16 bit integer, number of double words to be read (1-MAX).
    /// </param>
    /// <param name="TabDWord">
    /// Double words array receiving the data read.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_ReadIDWord(
      ushort wCard,
      ushort wEquip,
      ushort Offset,
      ushort Nb,
      uint[] TabDWord,
      ref ushort Status);

    /// <summary>
    /// - This function can be used to write all the outputs set beforehand in the local write buffer
    /// (with the functions IO_WriteXXX) to the devices present on the network. The function
    /// IO_RefreshOutput ensures complete consistency of the data written. All write functions of
    /// type IO_WriteXXX only access a local buffer, thereby ensuring optimum speed before
    /// physically writing on the network.
    /// - This function returns status 0 even if some devices are not accessible. Statuses of
    /// communication have to be checked if required, via the IO_GetEquipmentStatus function.
    /// If a device becomes again accessible, last written values in the board will be sent to the
    /// device.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_RefreshOutput(ushort wCard, ref ushort Status);

    /// <summary>
    /// This function is used to write output bits of a device in a local write buffer.
    /// N.B.: The local buffer will then be sent on the network with the function IO_RefreshOutput.
    /// The offsetbit parameter corresponds to the position in number of bits of the first bit to be written,
    /// from the start of the data area in the device. Example: if the device contains 3 data bytes, the
    /// offset of bit 0 in the third byte will be 16.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offsetbit">
    /// 16 bit integer, address of the first bit to be written in the device.
    /// </param>
    /// <param name="Nb">
    /// 16 bit integer, number of bits to be written (1-MAX).
    /// </param>
    /// <param name="TabBit">
    /// Bits array, containing the data to be written.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_WriteQBit(
      ushort wCard,
      ushort wEquip,
      ushort Offsetbit,
      ushort Nb,
      byte[] TabBit,
      ref ushort Status);

    /// <summary>
    /// This function is used to write output bytes of an device in a local write buffer.
    /// N.B.: The local buffer will then be sent on the network with the function IO_RefreshOutput.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offset">
    /// 16 bit integer, address of the first bytes to be written in the device.
    /// </param>
    /// <param name="Nb">
    /// 16 bit integer, number of bytes to be written (1-MAX).
    /// </param>
    /// <param name="TabByte">
    /// Bytes array, containing the data to be written.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_WriteQByte(
      ushort wCard,
      ushort wEquip,
      ushort Offset,
      ushort Nb,
      byte[] TabByte,
      ref ushort Status);

    /// <summary>
    /// This function is used to write output words of a device in a local write buffer.
    /// N.B.: The local buffer will then be sent on the network with the function IO_RefreshOutput.
    /// Remark: The values sent take into account the data format configured on the applicomIO® console (little
    /// Indian / Big Indian)
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offset">
    /// 16 bit integer, address of the first word to be written in the device.
    /// </param>
    /// <param name="Nb">
    /// 16 bit integer, number of words to be written (1-MAX).
    /// </param>
    /// <param name="TabWord">
    /// 16 bit words array, containing the data to be written.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_WriteQWord(
      ushort wCard,
      ushort wEquip,
      ushort Offset,
      ushort Nb,
      ushort[] TabWord,
      ref ushort Status);

    /// <summary>
    /// This function is used to write double output words of a device in a local write buffer.
    /// N.B.: The local buffer will then be sent on the network with the function IO_RefreshOutput.
    /// Remark: The values sent take into account the data format configured on the applicomIO® console (littleIndian / Big Indian)
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="Offset">
    /// 16 bit integer, address of the first double word to be written in the device.
    /// </param>
    /// <param name="Nb">
    /// 16 bit integer, number of double words to be written (1-MAX).
    /// </param>
    /// <param name="TabDWord">
    /// Double words array, containing the data to be written.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_WriteQDWord(
      ushort wCard,
      ushort wEquip,
      ushort Offset,
      ushort Nb,
      uint[] TabDWord,
      ref ushort Status);

    /// <summary>
    /// This function retrieves the device status on the network.
    /// N.B.: the function IO_RefreshInput must be called before using IO_GetEquipmentStatus.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="DeviceStatus">
    /// 16 bit integer, device status.
    /// This status corresponds to a communication status. The meaning of the various values returned
    /// in the variable "STATUS" is given in the manual relating to each protocol in the chapter "Function
    /// returned statuses".
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_GetEquipmentStatus(
      ushort wCard,
      ushort wEquip,
      ref ushort DeviceStatus,
      ref ushort Status);

    /// <summary>
    /// This function is used to indicate whether at least one device has an internal problem (short-circuit
    /// on a channel, threshold of an analog variable exceeded, etc.). A precise diagnostic of the faulty
    /// device can be carried out with the utility "DiagDP". This functionality is not supported on the
    /// INTERBUS protocol.
    /// N.B.: the function IO_RefreshInput must be called before using IO_GetGlobalDiag.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="GlobalStatus">
    /// 16 bit integer, Global network diagnostic.
    /// This variable GlobalDiag indicates whether a diagnostic of the network must be carried out.
    /// GlobalDiag = -1 No diagnostic supported by the protocol.
    /// GlobalDiag = 0 No diagnostic to be carried out.
    /// GlobalDiag = 1 Diagnostic on the network.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_GetGlobalStatus(
      ushort wCard,
      ref ushort GlobalStatus,
      ref ushort Status);

    /// <summary>
    /// This function is used to indicate whether the device has an internal problem (short-circuit on a
    /// channel, threshold of an analog variable exceeded, etc.), although the communication with it is
    /// OK (status = 0). During the data exchange, the device indicates an internal malfunction. A precise
    /// diagnostic of the device can be carried out with the utility "DiagDP". This functionality is not
    /// supported on the INTERBUS protocol.
    /// N.B.: the function IO_RefreshInput must be called before using IO_ReadDiag.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured.
    /// </param>
    /// <param name="DeviceDiag">
    /// 16 bit integer, device diagnostic.
    /// this variable indicates whether a diagnostic of the device must be carried out.
    /// DeviceDiag = -1 No diagnostic supported by the device.
    /// DeviceDiag = 0 No diagnostic to be carried out.
    /// DeviceDiag = 1 Diagnostic to be carried out on the targeted device.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_ReadDiag(
      ushort wCard,
      ushort wEquip,
      ref ushort DeviceDiag,
      ref ushort Status);

    /// <summary>
    /// This function is used to indicate whether at least one device has an internal problem (short-circuit
    /// on a channel, threshold of an analog variable exceeded, etc.). A precise diagnostic of the faulty
    /// device can be carried out with the utility "DiagDP". This functionality is not supported on the
    /// INTERBUS protocol.
    /// N.B.: the function IO_RefreshInput must be called before using IO_GetGlobalDiag.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="GlobalDiag">
    /// 16 bit integer, Global network diagnostic.
    /// This variable GlobalDiag indicates whether a diagnostic of the network must be carried out.
    /// GlobalDiag = -1 No diagnostic supported by the protocol.
    /// GlobalDiag = 0 No diagnostic to be carried out.
    /// GlobalDiag = 1 Diagnostic on the network.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_GetGlobalDiag(
      ushort wCard,
      ref ushort GlobalDiag,
      ref ushort Status);

    /// <summary>
    /// This function returns the number of devices present and the list of applicomIO® addresses.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Nb">
    /// 16 bit integer, number of devices present on the network.
    /// </param>
    /// <param name="TabEquip">
    /// 16 bit integer, Array of device present on the network.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_GetEquipmentList(
      ushort wCard,
      ref ushort Nb,
      ushort[] TabEquip,
      ref ushort Status);

    /// <summary>
    /// This function returns the sizes in bytes of device inputs/outputs.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="wEquip">
    /// 16 bit integer, device number (0-255) configured in the Console.
    /// </param>
    /// <param name="InputSize">
    /// 16 bit integer, Sizes of inputs in bytes.
    /// </param>
    /// <param name="OutputSize">
    /// 16 bit integer, Sizes of outputs in bytes.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_GetEquipmentInfo(
      ushort wCard,
      ushort wEquip,
      ref ushort InputSize,
      ref ushort OutputSize,
      ref ushort Status);

    /// <summary>
    /// This function is used to retrieve the status of the digital contact on the applicomIO® board.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern ushort IO_GetDigitalInput(ushort wCard, ref ushort Status);

    /// <summary>
    /// This function is used to define a monitoring time, from which the WatchDog will be
    /// activated if the functions IO__RefreshInput or IO_RefreshOuput are not recalled. The
    /// countdown is reset on each call to one of these functions. The application must ensure
    /// that this monitoring time is sufficient, to avoid untimely triggering. The value 0x0000
    /// deactivates this monitoring.
    /// - By default, the watchdog is inactive.
    /// - Once the watchdog has been activated, it can be reseted by calling again this function with TimeWD = 0.
    /// - Caution, the call to this function is taken account only after the next call to the RefreshInput function.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="TimeWD">
    /// 16 bit integer, time in ms from which the WatchDog is activated. Time base: TimeWD * 100 ms.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_SetWatchDog(ushort wCard, ushort TimeWD, ref ushort Status);

    /// <summary>
    /// This function is used to retrieve the WatchDog output state.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// 1 if the Watchdog output is activated, 0 if the Watchdog output is deactivated
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern ushort IO_GetStateWatchDog(ushort wCard, ref ushort Status);

    /// <summary>
    /// This function is used to retrieve the WatchDog output state.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="TimeReply">
    /// 16 bit integer, time in ms of the fallback time. Time base: TimeReply * 100 ms.
    /// </param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>
    /// TRUE if OK. If this function returns FALSE, the status variable contains the error details
    /// </returns>
    [DllImport("AppIO64.dll")]
    public static extern bool IO_SetReply(ushort wCard, ushort TimeReply, ref ushort Status);

    /// <summary>
    /// This function is used to retrieve the fallback value state.
    /// </summary>
    /// <param name="wCard">16 bit integer, board number.</param>
    /// <param name="Status">16 bit integer, exchange status.</param>
    /// <returns>1 if the fallback values are activated, 0 otherwise.</returns>
    [DllImport("AppIO64.dll")]
    public static extern ushort IO_GetStateReply(ushort wCard, ref ushort Status);
  }
}
