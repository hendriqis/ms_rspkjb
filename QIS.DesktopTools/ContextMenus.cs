using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using QIS.DesktopTools.Properties;
using System.Diagnostics;

namespace QIS.DesktopTools
{
    class ContextMenus
    {

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripMenuItem mnUpdate; 
            //ToolStripSeparator sep = new ToolStripSeparator();
            ////Control Panel
            //item = new ToolStripMenuItem() { Text = "Control Panel" };
            //item.Click += new EventHandler(mnuAbout_Click);
            //menu.Items.Add(item);

            //Report Designer
            //item = new ToolStripMenuItem() { Text = "Report Designer" };
            //item.Click += new EventHandler(mnuAbout_Click);
            //menu.Items.Add(item);

            //menu.Items.Add(sep);

            //update desktoptools

            mnUpdate = new ToolStripMenuItem();
            mnUpdate.Text = "Update Desktoptools";
            mnUpdate.Click += new System.EventHandler(menuUpdate_Click);
            menu.Items.Add(mnUpdate); 


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
    
        void mnuAbout_Click(object sender, EventArgs e)
        {
            ControlPanel form = new ControlPanel();
            form.ShowDialog();
        }


        void menuUpdate_Click(object sender, EventArgs e)
        {
            FormUpdateInfo iform = new FormUpdateInfo();
            iform.Show();
        }

    }
}
