using System.Runtime.InteropServices;

namespace AISServer.Components.Hardware.Profibus
{
    internal static class ApplicomIO
    {
        [DllImport("ApplicomIO64.dll", EntryPoint = "AuInitBus_io", CallingConvention = CallingConvention.Winapi)]
        public static extern bool AuInitBus_io(ref uint pdwStatus);

        [DllImport("ApplicomIO64.dll", EntryPoint = "AuExitBus_io", CallingConvention = CallingConvention.Winapi)]
        public static extern bool AuExitBus_io(ref uint pdwStatus);

        [DllImport("ApplicomIO64.dll", EntryPoint = "AuWriteReadMsg_io", CallingConvention = CallingConvention.Winapi)]
        public static extern bool AuWriteReadMsg_io(
            ushort wChan,
            ushort wNes,
            uint dwMsgParam,
            ushort wNbTx,
            byte[] lpbyBufTx,
            ref ushort pwNbRx,
            byte[] lpbyBufRx,
            ref uint pdwStatus);
    }
}