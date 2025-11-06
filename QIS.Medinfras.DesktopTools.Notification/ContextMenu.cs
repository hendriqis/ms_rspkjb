using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace QIS.Medinfras.DesktopTools.Notification
{
    class ContextMenu
    {

        private static Listener _listener;

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create(Listener listener)
        {
            _listener = listener;

            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep = new ToolStripSeparator();
            //Control Panel
            item = new ToolStripMenuItem() { Text = "Control Panel" };
            item.Click += new EventHandler(mnuOpen_Click);
            menu.Items.Add(item);

            //Report Designer
            //item = new ToolStripMenuItem() { Text = "Report Designer" };
            //item.Click += new EventHandler(mnuAbout_Click);
            //menu.Items.Add(item);

            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        /// <summary>
        /// Processes a menu item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }

        void mnuOpen_Click(object sender, EventArgs e)
        {
            //MainForm form = new MainForm(_listener);
            //form.ShowDialog();
        }
    }
}
