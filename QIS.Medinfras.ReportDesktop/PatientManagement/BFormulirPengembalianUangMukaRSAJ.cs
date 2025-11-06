using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BFormulirPengembalianUangMukaRSAJ : BaseDailyPortraitRpt
    {
        public BFormulirPengembalianUangMukaRSAJ()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            
            string jabatan = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.JABATAN_REPORT_PM90032).ParameterValue;
            string[] param2 = jabatan.Split('|'); ;

            lblLastUpdatedBy.Text = string.Format("{0}", param2[0]);
            xrTableCell39.Text = string.Format("{0}", param2[1]);
            xrTableCell40.Text = string.Format("{0}", param2[2]);
            xrTableCell41.Text = string.Format("{0}", param2[3]);

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            
            String TglKeluar = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            if (TglKeluar == null || TglKeluar == "01-Jan-1900")
                {
                    lblDischargeDate.Text = "";
                }
                else
                {
                    lblDischargeDate.Text = TglKeluar;
                }

            base.InitializeReport(param);
        }
    }
}
