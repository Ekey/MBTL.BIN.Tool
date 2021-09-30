using System;

namespace MBTL.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MELTY BLOOD: TYPE LUMINA BIN Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    MBTL.Unpacker <m_BinDirectory> <m_OutDirectory>");
                Console.WriteLine("    m_BinDirectory - BINs directory");
                Console.WriteLine("    m_OutDirectory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    MBTL.Unpacker E:\\Games\\MBTL D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Input = Utils.iCheckArgumentsPath(args[0]);
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!BinCache.iBuildEntryCache())
            {
                Utils.iSetError("[ERROR]: Unable to build entry cache");
                return;
            }

            BinUnpack.iDoIt(m_Input, m_Output);
        }
    }
}
