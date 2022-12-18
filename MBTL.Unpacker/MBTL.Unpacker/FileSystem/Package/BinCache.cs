using System;
using System.IO;
using System.Collections.Generic;

namespace MBTL.Unpacker
{
    class BinCache
    {
        private static String m_FullPath = Utils.iGetApplicationPath();

        //FileNames Table
        private static String m_NamesCache = @"\0.10.2.0\Cache_FN.txt";
        private static Dictionary<Int32, String> m_FileNames = new Dictionary<Int32, String>();

        //Offsets Table
        private static String m_OffsetsCache = @"\0.10.2.0\Cache_OT.bin";
        private static Dictionary<Int32, UInt32> m_Offsets = new Dictionary<Int32, UInt32>();

        //Sizes Table
        private static String m_SizesCache = @"\0.10.2.0\Cache_ST.bin";
        private static Dictionary<Int32, Int32> m_Sizes = new Dictionary<Int32, Int32>();

        //Archives map
        private static Dictionary<String, String> m_Archives = new Dictionary<String, String>();

        //Entry cache
        public static List<BinEntry> m_EntryCache = new List<BinEntry>();

        private static void iInitArchiveMap()
        {
            m_Archives.Add("___English", "data000.bin");
            m_Archives.Add("___Korean", "data001.bin");
            m_Archives.Add("___Region", "data002.bin");
            m_Archives.Add("___S_Chinese", "data003.bin");
            m_Archives.Add("___T_Chinese", "data004.bin");
            m_Archives.Add("BattleRes", "data005.bin");
            m_Archives.Add("bg", "data006.bin");
            m_Archives.Add("Bgm", "data007.bin");
            m_Archives.Add("data", "data008.bin");
            m_Archives.Add("DLC", "data009.bin");
            m_Archives.Add("grpdat", "data010.bin");
            m_Archives.Add("script", "data011.bin");
            m_Archives.Add("se", "data012.bin");
            m_Archives.Add("Shader", "data013.bin");
            m_Archives.Add("System", "data014.bin");
            m_Archives.Add("___French", "data015.bin");
            m_Archives.Add("___Portuguese", "data016.bin");
            m_Archives.Add("___Spanish", "data017.bin");
            m_Archives.Add("Append_0", "data018.bin"); // BattleRes
            m_Archives.Add("Append_1", "data019.bin"); // grpdat
        }

        private static Boolean iInitFileNamesCache()
        {
            Int32 i = 0;
            String m_Line = null;
            if (!File.Exists(m_FullPath + m_NamesCache))
            {
                Utils.iSetError("[ERROR]: Unable to load cache file -> " + m_NamesCache);
                return false;
            }

            StreamReader TFileNamesFile = new StreamReader(m_FullPath + m_NamesCache);
            while ((m_Line = TFileNamesFile.ReadLine()) != null)
            {
                m_FileNames.Add(i++, m_Line);
            }

            TFileNamesFile.Close();
            Console.WriteLine("[INFO]: Cache Names is Loaded: {0}!", i);

            return true;
        }

        private static Boolean iInitOffsetsCache()
        {
            if (!File.Exists(m_FullPath + m_OffsetsCache))
            {
                Utils.iSetError("[ERROR]: Unable to load cache file -> " + m_OffsetsCache);
                return false;
            }

            using (FileStream TFileStream = File.OpenRead(m_FullPath + m_OffsetsCache))
            {
                for (Int32 i = 0; i < TFileStream.Length / 4; i++)
                {
                    UInt32 dwOffset = TFileStream.ReadUInt32();
                    m_Offsets.Add(i, dwOffset);
                }
                TFileStream.Dispose();
            }

            Console.WriteLine("[INFO]: Cache Offsets is Loaded!");

            return true;
        }

        private static Boolean iInitSizesCache()
        {
            if (!File.Exists(m_FullPath + m_SizesCache))
            {
                Utils.iSetError("[ERROR]: Unable to load cache file -> " + m_SizesCache);
                return false;
            }

            using (FileStream TFileStream = File.OpenRead(m_FullPath + m_SizesCache))
            {
                for (Int32 i = 0; i < TFileStream.Length / 4; i++)
                {
                    Int32 dwSize = TFileStream.ReadInt32();
                    m_Sizes.Add(i, dwSize);
                }
                TFileStream.Dispose();
            }

            Console.WriteLine("[INFO]: Cache Sizes is Loaded!");
            Console.WriteLine();
            return true;
        }

        private static String iGetArchiveNameFromFileName(String m_FileName)
        {
            String m_Hint = null;
            String m_ArchiveName = null;

            Int32 dwIndex = m_FileName.IndexOf("/");
            if (dwIndex > 0)
                m_Hint = m_FileName.Substring(0, dwIndex);

            if (m_Archives.ContainsKey(m_Hint))
            {
                m_Archives.TryGetValue(m_Hint, out m_ArchiveName);
            }
            else
            {
                Utils.iSetError("[ERROR]: Unable to find archive name for file: " + m_FileName);
                return null;
            }

            return m_ArchiveName;
        }

        private static String iGetFileNameFromID(Int32 dwID)
        {
            String m_FileName = null;
            if (m_FileNames.ContainsKey(dwID))
            {
                m_FileNames.TryGetValue(dwID, out m_FileName);
            }
            else
            {
                Utils.iSetError("[ERROR]: Unable to get file name from ID: " + dwID);
                return null;
            }

            return m_FileName;
        }

        private static UInt32 iGetFileOffsetFromID(Int32 dwID)
        {
            UInt32 dwOffset = 0;
            if (m_Offsets.ContainsKey(dwID))
            {
                m_Offsets.TryGetValue(dwID, out dwOffset);
            }
            else
            {
                Utils.iSetError("[ERROR]: Unable to get offset for file ID: " + dwID);
                return 0xFFFFFFFF;
            }

            return dwOffset;
        }

        private static Int32 iGetFileSizeFromID(Int32 dwID)
        {
            Int32 dwSize = 0;
            if (m_Sizes.ContainsKey(dwID))
            {
                m_Sizes.TryGetValue(dwID, out dwSize);
            }
            else
            {
                Utils.iSetError("[ERROR]: Unable to get size for file ID: " + dwID);
                return -1;
            }

            return dwSize;
        }

        public static Boolean iBuildEntryCache()
        {
            m_EntryCache.Clear();
            m_FileNames.Clear();
            m_Archives.Clear();
            m_Offsets.Clear();
            m_Sizes.Clear();

            iInitArchiveMap();
            if (!iInitFileNamesCache()) { return false; }
            if (!iInitOffsetsCache()) { return false; }
            if (!iInitSizesCache()) { return false; }

            for (Int32 i = 0; i < m_FileNames.Count; i++)
            {
                var m_Entry = new BinEntry();

                m_Entry.m_FileName = iGetFileNameFromID(i);
                m_Entry.m_Archive = iGetArchiveNameFromFileName(m_Entry.m_FileName);
                m_Entry.dwOffset = iGetFileOffsetFromID(i);
                m_Entry.dwSize = iGetFileSizeFromID(i);

                m_EntryCache.Add(m_Entry);
            }

            return true;
        }
    }
}
