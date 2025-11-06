using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HttpServer;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;

namespace QIS.DesktopTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //Initialize the icons
                mIcons[0] = new Icon(string.Format(@"{0}\ico\{1}", Application.StartupPath, "off.ico"));
                mIcons[1] = new Icon(string.Format(@"{0}\ico\{1}", Application.StartupPath, "start.ico"));
                mIcons[2] = new Icon(string.Format(@"{0}\ico\{1}", Application.StartupPath, "stop.ico"));

       

                // Show the system tray icon.					
                using (ProcessIcon pi = new ProcessIcon())
                {
                    // Make sure the application runs!
                    try
                    {
                        StartHttpCommandDispatcher();
                        pi.Display(mIcons[1], "ACTIVE");
                        Application.Run();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured when starting the listener service", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("MEDINFRAS Desktop Service is currently running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }

        private static void StartHttpCommandDispatcher()
        {
            mCmdDispatcher.AddResourceLocator(new ImageLocator(Properties.Resources.ResourceManager));
            mCmdDispatcher.Start("http://localhost:60025/");
        }

        public static HttpCommandDispatcher mCmdDispatcher = new HttpCommandDispatcher(Properties.Resources.dummy);
        public static Icon[] mIcons = new Icon[3];
        public static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
    }
}
