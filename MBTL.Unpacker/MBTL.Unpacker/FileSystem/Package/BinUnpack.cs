using System;
using System.IO;

namespace MBTL.Unpacker
{
    class BinUnpack
    {
        public static void iDoIt(String m_SrcFolder, String m_DstFolder)
        {
            foreach (BinEntry m_Entry in BinCache.m_EntryCache)
            {
                Console.WriteLine("[UNPACKING]: {0}", m_Entry.m_FileName);

                if (m_Entry.dwSize != 0)
                {
                    using (FileStream TFileStream = File.OpenRead(m_SrcFolder + m_Entry.m_Archive))
                    {
                        TFileStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                        var lpBuffer = TFileStream.ReadBytes(m_Entry.dwSize);
                        lpBuffer = BinCipher.iDecryptData(lpBuffer);

                        String m_FullPath = m_DstFolder + m_Entry.m_FileName.Replace("/", @"\");
                        Utils.iCreateDirectory(m_FullPath);

                        File.WriteAllBytes(m_FullPath, lpBuffer);

                        TFileStream.Dispose();
                    }
                }
            }
        }
    }
}
