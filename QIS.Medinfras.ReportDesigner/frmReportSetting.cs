using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace QIS.Medinfras.ReportDesigner
{
    public partial class frmReportSetting : Form
    {
        private string selectFolder;
        public frmReportSetting()
        {
            InitializeComponent();
            selectFolder = "";
            btnSave.Enabled = false;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (fbdRootFolder.ShowDialog() == DialogResult.OK) {
                MessageBox.Show(fbdRootFolder.SelectedPath.ToString());
                selectFolder = fbdRootFolder.SelectedPath.ToString();
                btnSave.Enabled = true;
                txtReportRootFolder.Text = selectFolder;
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!selectFolder.Equals(""))
            {
                StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString())+"\\rpt.conf",false);
                sw.WriteLine(selectFolder);
                sw.Close();
            }
            btnSave.Enabled = false;
        }
    }
}
