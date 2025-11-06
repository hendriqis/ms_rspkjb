using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using System.Drawing.Printing;

namespace QIS.Medinfras.PrescriptionPrinting
{
    public partial class Form1 : Form
    {
        int MaxStringPerLine;
        Boolean isTicking;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void tmrPolling_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = "Fetching Data....";
            List<vOrderResep> list = BusinessLayer.GetvOrderResepList(String.Format("IsOrderTextSentToPrinter = 0 ORDER BY TglUpdate"));
            if (list.Count > 0)
            {
                vOrderResep entityOrder = list.First();
                EntityToControl(entityOrder);
                lblStatus.Text = "Printing....";


                printDocument.Print();
                lblStatus.Text = "Updating....";
                UpdateWorklistDT();
                lblStatus.Text = "Finished";
            }
            else
            {
                lblStatus.Text = "Data Not Found....";
                PrepareScreen();
            }
        }

        private void EntityToControl(vOrderResep entity)
        {
            txtOrder.Text = String.Format("{0} / {1}",entity.NoJO,entity.TglUpdate.ToString("dd-MM-yyyy HH:mm"));
            txtRegistrasi.Text = String.Format("{0} / {1}", entity.NoReg, entity.NoRM);
            txtNamaPasien.Text = entity.NamaPasien;
            txtDokter.Text = entity.NmDokter;
            txtUnit.Text = entity.NmUnit;
            txtResep.Text = entity.Resep;
            lblPrintCount.Text = String.Format("Print Count : {0} - {1}", entity.PrintCount + 1,entity.UsrUpdate);
            lblKdLayan.Text = entity.KdLayan;
            lblNoJO.Text = entity.NoJO;
        }

        private void PrepareScreen()
        {
            txtOrder.Text = String.Empty;
            txtRegistrasi.Text = String.Empty;
            txtNamaPasien.Text = String.Empty;
            txtDokter.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtResep.Text = String.Empty;
            lblPrintCount.Text = String.Empty;
            lblKdLayan.Text = String.Empty;
        }

        private void UpdateWorklistDT()
        {
            MD_WorklistDT entity = BusinessLayer.GetMD_WorklistDT(lblNoJO.Text, lblKdLayan.Text);
            entity.PrintCount++;
            entity.IsOrderTextSentToPrinter = true;
            BusinessLayer.UpdateMD_WorklistDT(entity);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon.BalloonTipText = "Medinfras Prescription Print Tool is running...";
            notifyIcon.BalloonTipTitle = "Medinfras Prescription Print Tool";
            //notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            
            isTicking = true;
            Int32 interval = Convert.ToInt32(BusinessLayer.GetSetVar("FM_", "PrescriptionPollingInterval").Nilai);
            tmrPolling.Interval = interval;
            tmrPolling.Start();
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font printFont;
            printFont = new Font("Draft", 8);
            PrinterSettings printSetting = new PrinterSettings();
            //printSetting.PrinterName = GetPrinterName();            
            
            e.Graphics.DrawString(DirectPrint(), printFont, Brushes.Black, 10, 10);
        }

        private string DirectPrint()
        {
            SetVar setvar = BusinessLayer.GetSetVar("FM_", "PrescriptionCharPerLine");
            MaxStringPerLine = Convert.ToInt32(setvar.Nilai);

            string txtprn;
            txtprn = GeneratePrescriptionString();

            return txtprn;
        }

        private string GeneratePrescriptionString()
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine(String.Format("Order : {0}", txtOrder.Text));
            strBuild.AppendLine(String.Format("{0}", txtRegistrasi.Text));
            strBuild.AppendLine(String.Format("{0}", txtNamaPasien.Text));
            strBuild.AppendLine(String.Format("{0}", txtDokter.Text));
            strBuild.AppendLine(String.Format("{0}", txtUnit.Text));
            strBuild.AppendLine(String.Format("{0}",GenerateLineBreak()));
            strBuild.AppendLine(String.Format("{0}", txtResep.Text));
            strBuild.AppendLine(String.Format("{0}", GenerateLineBreak()));
            strBuild.AppendLine(String.Format("{0}", lblPrintCount.Text));
            return strBuild.ToString();
        }

        private string GenerateLineBreak()
        {
            string lineBreak = string.Empty;

            //_lineBreak
            for (int lb = 0; lb < MaxStringPerLine; lb++)
            {
                lineBreak += "-";
            }
            return lineBreak;
        }

        private string GetPrinterName()
        {
            string printerName = String.Empty;
            StdFieldtDT obj = BusinessLayer.GetStdFieldtDT("DIRECTPRINT", "");


            return printerName;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (isTicking)
            {
                tmrPolling.Stop();
                btnStartStop.Text = "Start Fetching";
                lblStatus.Text = "Stopped";
            }
            else
            {
                tmrPolling.Start();
                btnStartStop.Text = "Stop Fetching";
                lblStatus.Text = "Started";
            }
            isTicking = !isTicking;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.Hide();
            }

            //else if (FormWindowState.Normal == this.WindowState)
            //{
            //    notifyIcon.Visible = false;
            //}
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void closeProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
