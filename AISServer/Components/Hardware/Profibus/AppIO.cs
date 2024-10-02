using System.Runtime.InteropServices;

namespace AISServer.Components.Hardware.Profibus
{
    internal static class AppIO
    {
        [DllImport("appIO64.dll", EntryPoint = "IO_Init", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_Init(ushort wCard, ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_Exit", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_Exit(ushort wCard, ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_RefreshInput", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_RefreshInput(ushort wCard, ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_RefreshOutput", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_RefreshOutput(ushort wCard, ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_WriteQBit", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_WriteQBit(
            ushort wCard,
            ushort wEquip,
            ushort Offsetbit,
            ushort Nb,
            byte[] TabBit,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_ReadIBit", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_ReadIBit(
            ushort wCard,
            ushort wEquip,
            ushort Offsetbit,
            ushort Nb,
            byte[] TabBit,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_WriteQByte", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_WriteQByte(
            ushort wCard,
            ushort wEquip,
            ushort Offset,
            ushort Nb,
            byte[] TabByte,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_ReadIByte", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_ReadIByte(
            ushort wCard,
            ushort wEquip,
            ushort Offset,
            ushort Nb,
            byte[] TabByte,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_WriteQWord", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_WriteQWord(
            ushort wCard,
            ushort wEquip,
            ushort Offset,
            ushort Nb,
            ushort[] TabWord,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_ReadIWord", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_ReadIWord(
            ushort wCard,
            ushort wEquip,
            ushort Offset,
            ushort Nb,
            ushort[] TabWord,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_WriteQDWord", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_WriteQDWord(
            ushort wCard,
            ushort wEquip,
            ushort Offset,
            ushort Nb,
            uint[] TabDWord,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_ReadIDWord", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_ReadIDWord(
            ushort wCard,
            ushort wEquip,
            ushort Offset,
            ushort Nb,
            uint[] TabDWord,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetEquipmentList", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_GetEquipmentList(
            ushort wCard,
            ref ushort Nb,
            ushort[] TabEquip,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetEquipmentInfo", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_GetEquipmentInfo(
            ushort wCard,
            ushort wEquip,
            ref ushort InputSize,
            ref ushort OutputSize,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetEquipmentStatus", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_GetEquipmentStatus(
            ushort wCard,
            ushort wEquip,
            ref ushort EquipmentStatus,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_SetWatchDog", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_SetWatchDog(
            ushort wCard,
            ushort TimeWD,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetStateWatchDog", CallingConvention = CallingConvention.Winapi)]
        public static extern ushort IO_GetStateWatchDog(
            ushort wCard,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_SetReply", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_SetReply(
            ushort wCard,
            ushort TimeReply,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetStateReply", CallingConvention = CallingConvention.Winapi)]
        public static extern ushort IO_GetStateReply(
            ushort wCard,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_ReadDiag", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_ReadDiag(
            ushort wCard,
            ushort wEquip,
            ref ushort DeviceDiag,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetGlobalDiag", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_GetGlobalDiag(
            ushort wCard,
            ref ushort GlobalDiag,
            ref short pStatus);

        [DllImport("appIO64.dll", EntryPoint = "IO_GetGlobalStatus", CallingConvention = CallingConvention.Winapi)]
        public static extern bool IO_GetGlobalStatus(
            ushort wCard,
            ref ushort GlobalStatus,
            ref short pStatus);
    }
}
