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
    public partial class BKwitansiPiutangPerPasienRSDOSOBA : BaseRpt
    {
        private string City = "";

        public BKwitansiPiutangPerPasienRSDOSOBA()
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
            vARInvoiceHd2 entityPayment = BusinessLayer.GetvARInvoiceHd2List(string.Format("{0}", param[0])).FirstOrDefault();
            vARInvoiceDt1 entityDt = BusinessLayer.GetvARInvoiceDt1List(string.Format("{0}", param[0])).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            //lblTotalAmountString.Text = "# " + entityPayment.TotalClaimedAmountInStringInd + " #";
            lblRegistration.Text = string.Format("*{0}", entityDt.RegistrationNo);

            string[] temp = param[0].Split(';');
            List<vARInvoiceHd2> lstEntity = BusinessLayer.GetvARInvoiceHd2List(temp[0]);

            lblPrintDate.Text = String.Format("{0} {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

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

        //private void lblPrintNumber_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    SettingParameterDt Param = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_IS_USE_COUNTER_IN_PAYMENT_RECEIPT);
        //    if (Param.ParameterValue == "1")
        //    {
        //        Int32 PrintNumber = Convert.ToInt32((GetCurrentColumnValue("PrintNumber")).ToString());

        //        if (PrintNumber > 0)
        //        {
        //            lblPrintNumber.Text = string.Format("Cetakan ke - {0}", GetCurrentColumnValue("PrintNumber").ToString());
        //        }
        //        else
        //        {
        //            lblPrintNumber.Text = "";
        //        }
        //    }
        //    else
        //    {
        //        lblPrintNumber.Text = "";
        //    }
        //}
    }
}
