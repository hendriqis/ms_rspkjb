using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HttpServer;
using System.Xml.Linq;

namespace QIS.DesktopTools
{
    public partial class ControlPanel : Form
    {
        XDocument xmlConnection,xmlListenFrom;
        string server,database,uid,pwd,appName;
        static string listenFrom;
        public ControlPanel()
        {
            InitializeComponent();
            xmlConnection = XDocument.Load("ConnectionString.config");
            xmlListenFrom = XDocument.Load("listenFrom.config");
            string[] temp = xmlConnection.Element("connectionStrings").Element("add").Attribute("connectionString").Value.Split(';');
            
            txtServerIP.Text = server = temp[0].Split('=')[1];
            txtDbsName.Text = database = temp[1].Split('=')[1];
            txtUID.Text = uid = temp[2].Split('=')[1];
            txtPassword.Text = pwd = temp[3].Split('=')[1];
            appName = temp[4].Split('=')[1];
            txtListenFrom.Text = listenFrom = xmlListenFrom.Element("listenFrom").Value;   
        }

        private void btnListener_Click(object sender, EventArgs e)
        {
            //if (btnListener.Text.Equals("START"))
            //{
            //    mCmdDispatcher = new HttpCommandDispatcher(Properties.Resources.dummy);
            //    StartHttpCommandDispatcher();
            //    btnListener.Text = "STOP";
            //}
            //else 
            //{
            //    btnListener.Text = "START";
            //    mCmdDispatcher.End();
            //}
        }

        private static void StartHttpCommandDispatcher()
        {
            //mCmdDispatcher.AddResourceLocator(new ImageLocator(Properties.Resources.ResourceManager));
            //mCmdDispatcher.Start(listenFrom);
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            string newConnectionString = "";
            newConnectionString += ("server=" + txtServerIP.Text + ";");
            newConnectionString += ("database=" + txtDbsName.Text + ";");
            newConnectionString += ("uid=" + txtUID.Text + ";");
            newConnectionString += ("pwd=" + txtPassword.Text + ";");
            newConnectionString += ("Application Name = " + appName);
            xmlConnection.Element("connectionStrings").Element("add").Attribute("connectionString").Value = newConnectionString;
            xmlListenFrom.Element("listenFrom").Value = txtListenFrom.Text;
            xmlListenFrom.Save("listenFrom.config");
            xmlConnection.Save("ConnectionString.config");
        }
    }
}
