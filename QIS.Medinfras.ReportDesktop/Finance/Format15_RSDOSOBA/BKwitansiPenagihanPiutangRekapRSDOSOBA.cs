using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKwitansiPenagihanPiutangRekapRSDOSOBA : BaseRpt
    {
        private string City = "";

        public BKwitansiPenagihanPiutangRekapRSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String HealthcareID = AppSession.UserLogin.HealthcareID;

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.DIREKTUR_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);
            vARInvoiceHd1 entityPayment = BusinessLayer.GetvARInvoiceHd1List(string.Format("{0}", param[0])).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblTotalAmountString.Text = "# " + entityPayment.TotalClaimedAmountInStringInd + " #";

            string[] temp = param[0].Split(';');
            List<vARInvoiceHd1> lstEntity = BusinessLayer.GetvARInvoiceHd1List(temp[0]);

            this.DataSource = lstEntity;

            if (entityPayment.LastUpdatedBy != 0 && entityPayment.LastUpdatedBy != null)
            {
                lblLastUpdatedBy.Text = AppSession.UserLogin.UserFullName;
            }
            else
            {
                lblLastUpdatedBy.Text = AppSession.UserLogin.UserFullName;
            }
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ARReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", City, createdDate);
            }
        }
    }
}
