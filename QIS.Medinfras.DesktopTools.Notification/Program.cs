using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace QIS.Medinfras.DesktopTools.Notification
{
    public static class Program
    {
        public static Icon[] mIcons = new Icon[3];
        public static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-85fd-A8CF-72F04E6BDE8F}");

        public static Listener mListener = new Listener();

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

                GetAppConfiguration();
                mListener.StartListener(mListener.IpAddress, mListener.Port);
                // Show the system tray icon.					
                using (ProcessIcon pi = new ProcessIcon(mListener))
                {
                    // Make sure the application runs!
                    try
                    {
                        pi.Display(mIcons[1], "ACTIVE");
                        Application.Run();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured when starting the notification service", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("MEDINFRAS Notification Service is currently running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }

        private static void GetAppConfiguration()
        {
            mListener.IpAddress = GetLocalIPAddress(); //"172.17.0.154";
            mListener.Port = 6000;
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
