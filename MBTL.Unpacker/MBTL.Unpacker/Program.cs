using System;

namespace MBTL.Unpacker
{
    class Program
    {
        private static String m_Title = "MELTY BLOOD: TYPE LUMINA BIN Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
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
