using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramPreLauncher
{
    public partial class MainForm : Form
    {
        private static MainForm instance = null;
        public static MainForm GetWindow(bool canCreate = false)
        {
            if (canCreate)
            {
                return instance ?? (instance = new MainForm());
            }
            return instance;
        }

        private MainForm()
        {
            InitializeComponent();
            Load += MainForm_Load;
            Closed += (sender, e) => Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 200;
            timer.Tick += (sender1, e1) =>
            {
                statusbar.Text = Message;
                progress.Value = (int)(Progress * 1000);
            };
            timer.Start();
        }

        public static string Message = "";
        public static double Progress = 1;
        public static void UpdateMessage(string message, double percent = -1)
        {
            Message = message;
            Progress = percent == -1 ? 1 : percent;
        }
    }
}
