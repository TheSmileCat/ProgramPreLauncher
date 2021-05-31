using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramPreLauncher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (!Environment.Is64BitOperatingSystem)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   #####  ####   ####    ###   ####    ");
                Console.WriteLine("   #      #   #  #   #  #   #  #   #   ");
                Console.WriteLine("   #####  ####   ####  #     # ####    ");
                Console.WriteLine("   #      #   #  #   #  #   #  #   #   ");
                Console.WriteLine("   #####  #    # #    #  ###   #    #  ");
                Console.WriteLine("=======================================");
                Console.WriteLine("|| ChobitsMC必须在64位操作系统下运行 ||");
                Console.WriteLine("=======================================");
                Console.ReadKey();
            }
            ChobitsMCLauncher.CheckUpdateThread.Run();
        }
    }
}
