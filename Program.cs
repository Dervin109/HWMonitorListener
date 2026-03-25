using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HWMonitorListener
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Directory.CreateDirectory(@"C:\HWMonitor_Events");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HiddenWindow());
        }
    }

    public class HiddenWindow : Form
    {
        const int WM_DEVICECHANGE = 0x0219;
        const int DBT_DEVICEARRIVAL = 0x8000;
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        string eventFile = @"C:\HWMonitor_Events\event.txt";

        public HiddenWindow()
        {
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                if (m.WParam.ToInt32() == DBT_DEVICEARRIVAL)
                {
                    File.WriteAllText(eventFile, "ARRIVAL");
                }
                else if (m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE)
                {
                    File.WriteAllText(eventFile, "REMOVAL");
                }
            }

            base.WndProc(ref m);
        }
    }
}
