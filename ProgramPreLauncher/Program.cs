using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramPreLauncher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread.Sleep(500);
            //if (!Environment.Is64BitOperatingSystem)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("   #####  ####   ####    ###   ####    ");
            //    Console.WriteLine("   #      #   #  #   #  #   #  #   #   ");
            //    Console.WriteLine("   #####  ####   ####  #     # ####    ");
            //    Console.WriteLine("   #      #   #  #   #  #   #  #   #   ");
            //    Console.WriteLine("   #####  #    # #    #  ###   #    #  ");
            //    Console.WriteLine("=======================================");
            //    Console.WriteLine("|| ChobitsMC必须在64位操作系统下运行 ||");
            //    Console.WriteLine("=======================================");
            //    Console.ReadKey();
            //}
            Thread thread = new Thread(ChobitsMCLauncher.CheckUpdateThread.Run);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Application.Run(MainForm.GetWindow(true));
            //ChobitsMCLauncher.CheckUpdateThread.Run();
        }
    }
}
