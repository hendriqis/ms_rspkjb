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
    public partial class BNew8SuratPenagihanIng : BaseCustomDailyPotraitRpt
    {
        public BNew8SuratPenagihanIng()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string ARInvoiceID = param[0];
            string type = param[2];
            string diskon = param[3];
            decimal exchange = Convert.ToDecimal(param[4]);

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            List<SettingParameterDt> setvarAR = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR));
            lblEmailPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;
            lblContactPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;
            lblDate.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            List<vARInvoiceDt> lstInvoice = BusinessLayer.GetvARInvoiceDtList(string.Format("{0}", ARInvoiceID));

            #region Transaction
            subTransaction.CanGrow = true;
            detailSuratPenagihan.InitializeReport(lstInvoice, exchange);
            #endregion

            base.InitializeReport(param);
        }
    }
}
