using System;

namespace MBTL.Unpacker
{
    class BinCipher
    {
        public static Byte[] iDecryptData(Byte[] lpBuffer)
        {
            lpBuffer[0] ^= 0xA5;
            lpBuffer[1] ^= 0x18;

            Int32 A = lpBuffer[0] ^ 0xAC;
            Int32 B = lpBuffer[0] ^ 0xAC ^ lpBuffer[1] ^ 0x76381;

            if (lpBuffer.Length > 2)
            {
                for (Int32 i = lpBuffer.Length - 1; i > 1; i--)
                {
                    lpBuffer[i] ^= BinKeys.ENTRY_KEY[A ^ B & 0x3FF];
                    B++;
                }
            }

            return lpBuffer;
        }
    }
}