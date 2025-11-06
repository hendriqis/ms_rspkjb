using System;
using System.Data;
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
    public partial class DiagnosticResultRSSESRptEng : BaseNewCustomDailyPotraitRpt
    {
        private string sv_Email = "";
        private string sv_Sign = "0";

        public DiagnosticResultRSSESRptEng()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('','')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.IS_EMAIL_RADIOLOGI,
                                                        Constant.SettingParameter.IS_NAMA_TANDA_TANGAN_CETAKAN_HASIL_RADIOLOGI
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            sv_Email = lstSetVarDt.FirstOrDefault(a => a.ParameterCode == Constant.SettingParameter.IS_EMAIL_RADIOLOGI).ParameterValue.ToString();
            sv_Sign = lstSetVarDt.FirstOrDefault(a => a.ParameterCode == Constant.SettingParameter.IS_NAMA_TANDA_TANGAN_CETAKAN_HASIL_RADIOLOGI).ParameterValue.ToString();

            vImagingResultReport oResult = BusinessLayer.GetvImagingResultReportList(param[0]).FirstOrDefault();
            if (oResult != null)
            {
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, oResult.TestRealizationPhysicianCode);
                ttdDokter.Visible = true;

                lblSignParamedicName.Text = oResult.TestRealizationPhysician;
            }

            base.InitializeReport(param);
        }

        private void lblEmail_IS_Caption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Email == "")
            {
                lblEmail_IS_Caption.Visible = false;
            }
            else
            {
                lblEmail_IS_Caption.Visible = true;
            }
        }

        private void lblEmail_IS_Symbol_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Email == "")
            {
                lblEmail_IS_Symbol.Visible = false;
            }
            else
            {
                lblEmail_IS_Symbol.Visible = true;
            }
        }

        private void lblEmail_IS_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Email == "")
            {
                lblEmail_IS.Visible = false;
            }
            else
            {
                lblEmail_IS.Visible = true;
                lblEmail_IS.Text = sv_Email;
            }
        }

    }
}
