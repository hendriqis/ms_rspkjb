using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using QIS.DesktopTools.Properties;
using System.Diagnostics;
using System.Drawing;

namespace QIS.DesktopTools
{
    class ProcessIcon : IDisposable
    {
        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        NotifyIcon ni;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public ProcessIcon()
        {
            // Instantiate the NotifyIcon object.
            ni = new NotifyIcon();
        }

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display(Icon ico, string status)
        {
            // Put the icon in the system tray and allow it react to mouse clicks.			
            ni.Icon = ico;
            ni.Text = string.Format("{0} - {1}","Medinfras Desktop",status);
            ni.Visible = true;

            // Attach a context menu.
            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }
    }
}
