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
    public partial class BKwitansiKolektifPiutangRSDOSKA : BaseRpt
    {
        public BKwitansiKolektifPiutangRSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            string[] temp = param[0].Split(';');
            List<vARReceiptHd> lstEntity = BusinessLayer.GetvARReceiptHdList(temp[0]);

            this.DataSource = lstEntity;
        }

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            string receiptDate = Convert.ToDateTime(GetCurrentColumnValue("ARReceiptDate")).ToString(Constant.FormatString.DATE_FORMAT);
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);

            if (receiptDate != "01-Jan-1900")
            {
                lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, receiptDate);
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, createdDate);
            }
        }
    }
}
