using System;
using System.Collections.Generic;

namespace MBTL.Unpacker
{
    class BinEntry
    {
        public String m_Archive { get; set; }
        public String m_FileName { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
    }
}