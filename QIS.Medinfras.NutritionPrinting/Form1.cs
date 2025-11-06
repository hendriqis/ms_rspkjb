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
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.NutritionPrinting
{
    public partial class Form1 : Form
    {
        int MaxStringPerLine;
        Boolean isTicking;
        
        
        public Form1()
        {
            InitializeComponent();
        }

        private void tmPolling_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = "Fetching Data....";
            List<NutritionOrderDt> list = BusinessLayer.GetNutritionOrderDtList(String.Format("IsReadyToPrint = 1 ORDER BY LastUpdatedDate"));
            if (list.Count > 0)
            {
                try
                {
                    NutritionOrderDt entityOrder = list.First();
                    vNutritionOrderDtCustom entity = BusinessLayer.GetvNutritionOrderDtCustomList(String.Format("NutritionOrderDtID = {0}", entityOrder.NutritionOrderDtID)).FirstOrDefault();
                    EntityToControl(entity);
                    lblStatus.Text = "Printing....";


                    printDocument.Print();
                    lblStatus.Text = "Updating....";
                    UpdateNutritionOrderDT(entityOrder.NutritionOrderDtID);
                    lblStatus.Text = "Finished";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                lblStatus.Text = "Data Not Found....";
                PrepareScreen();
            }
        }

        private void EntityToControl(vNutritionOrderDtCustom entity)
        {   
            txtRegistrasi.Text = String.Format("{0}/{1}", entity.RegistrationNo.Trim(), entity.MedicalNo);
            txtNoTTKelas.Text = String.Format("{0} / {1}",entity.BedCode.Trim(),entity.ClassName);
            txtNamaPasien.Text = entity.PatientName;
            txtAgama.Text = entity.Religion;
            txtNamaDiit.Text = String.Format("{0} - {1}",entity.MealPlanCode, entity.MealPlanName);
            txtMealTime.Text = entity.MealTime;
            txtRemarks.Text = entity.Remarks;
            txtRuang.Text = entity.ServiceUnitName;
            txtNoOrder.Text = entity.NutritionOrderNo;
            txtTglJamOrder.Text = String.Format("{0}", entity.NutritionOrderDate);
            if (entity.DiagnoseID == null)
            {
                if (entity.DiagnoseText != null)
                {
                    txtDiagnosa.Text = entity.DiagnoseText;
                }
                else txtDiagnosa.Text = "";
            }

            else if (entity.DiagnoseText == null)
            {
                if (entity.DiagnoseID != null)
                {
                    txtDiagnosa.Text = entity.DiagnoseName;
                }
                else txtDiagnosa.Text = "";
            }
            else
            {
                txtDiagnosa.Text = String.Format("{0}, {1}", entity.DiagnoseName, entity.DiagnoseText);
            }
                
                
            lblPrintCount.Text = String.Format("Last Printed By : {0}",entity.LastPrintedByName);
            lblDtID.Text = entity.NutritionOrderDtID.ToString();
        }

        private void PrepareScreen()
        {
            txtRegistrasi.Text = String.Empty;
            txtNoTTKelas.Text = String.Empty;
            txtNamaPasien.Text = String.Empty;
            txtAgama.Text = String.Empty;
            txtNamaDiit.Text = String.Empty;
            txtMealTime.Text = String.Empty;
            txtRemarks.Text = String.Empty;
            txtRuang.Text = String.Empty;
            lblPrintCount.Text = String.Empty;
            
        }

        private void UpdateNutritionOrderDT(int orderDetailID)
        {
            NutritionOrderDt entity = BusinessLayer.GetNutritionOrderDt(orderDetailID);
            entity.IsReadyToPrint = false;
            BusinessLayer.UpdateNutritionOrderDt(entity);
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font printFont;
            printFont = new Font("BatangChe", 8);
            PrinterSettings printSetting = new PrinterSettings();
            //printSetting.PrinterName = GetPrinterName();            

            e.Graphics.DrawString(DirectPrint(), printFont, Brushes.Black, 10, 10);
        }

        private string DirectPrint()
        {
            SettingParameter setvar = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DIRECT_PRINT_STRING_PER_LINE);
            MaxStringPerLine = Convert.ToInt32(setvar.ParameterValue);

            string txtprn;
            txtprn = GeneratePrescriptionString();
            return txtprn;
        }

        private string GeneratePrescriptionString()
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine(String.Format("NoReg/No.RM : {0}", txtRegistrasi.Text));
            strBuild.AppendLine(String.Format("No.TT/Kelas : {0}", txtNoTTKelas.Text));
            strBuild.AppendLine(String.Format("Nama        : {0}", txtNamaPasien.Text));
            strBuild.AppendLine(String.Format("Agama       : {0}", txtAgama.Text));
            strBuild.AppendLine(String.Format("Nama Diit   : {0}", txtNamaDiit.Text));
            strBuild.AppendLine(String.Format("Jam Makan   : {0}", txtMealTime.Text));
            strBuild.AppendLine(String.Format("Keterangan  : {0}", txtRemarks.Text));
            strBuild.AppendLine(String.Format("Ruang       : {0}", txtRuang.Text));
            strBuild.AppendLine(String.Format("No Order    : {0}", txtNoOrder.Text));
            strBuild.AppendLine(String.Format("Tgl Order   : {0}", txtTglJamOrder.Text));
            strBuild.AppendLine(String.Format("Diagnosa    : {0}", txtDiagnosa.Text));
            strBuild.AppendLine(String.Format(""));
            strBuild.AppendLine(String.Format("Tgl Cetak   : {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm")));
            strBuild.AppendLine(String.Format(""));
            strBuild.AppendLine(String.Format("  HA     LH     LN     SY     BH   Cairan"));
            strBuild.AppendLine(String.Format("[    ] [    ] [    ] [    ] [    ] [    ]"));
            strBuild.AppendLine(String.Format(""));
            strBuild.AppendLine(String.Format("HABIS    1   2   3   4   5    UTUH"));
            strBuild.AppendLine(String.Format("      <- - - - - - - - - - -> "));
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

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon.BalloonTipText = "Medinfras Nutrition Print Tool is running...";
            notifyIcon.BalloonTipTitle = "Medinfras Nutrition Print Tool";
            
            isTicking = true;
            //Int32 interval = Convert.ToInt32(BusinessLayer.GetSetVar("FM_", "PrescriptionPollingInterval").Nilai);
            tmrPolling.Interval = 10000;
            tmrPolling.Start();
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
        }

        private void closeProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        
    }
}
