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
    public partial class BKwitansiPenagihanPiutangDetailRSSBB : BaseCustomDailyPotraitA5Rpt
    {
        public BKwitansiPenagihanPiutangDetailRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string HealthcareID = "001"; //patok dulu karna di console ga bs baca appsession
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(HealthcareID, setvar.ParameterCode);
            
            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/LOGO_TEXT.png");
            cReceiptHealthcareName.Text = h.HealthcareName;
            cReceiptHealthcareAddress.Text = string.Format("{0} {1} {2}", h.StreetName, h.City, h.ZipCode);
            cReceiptHealthcarePhone.Text = string.Format("{0} {1}", h.PhoneNo1, h.FaxNo1);

            string ARInvoiceID = param[0]; 
                /////param[0].Replace("ARInvoiceID = ", "");
            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHdList(ARInvoiceID).FirstOrDefault(); /////////BusinessLayer.GetARInvoiceHd(Convert.ToInt32(ARInvoiceID));
            vARInvoiceDt entity = BusinessLayer.GetvARInvoiceDtList(ARInvoiceID).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", h.City, entity.SignatureDateInString);
            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = setvar.ParameterName;

            base.InitializeReport(param);
        }

    }
}
