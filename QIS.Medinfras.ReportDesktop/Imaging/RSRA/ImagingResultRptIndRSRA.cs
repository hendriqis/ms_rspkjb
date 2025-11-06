using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class ImagingResultRptIndRSRA : BaseNewCustomDailyPotraitRpt
    {
        private string sv_Email = "";
        private string sv_Sign = "0";

        public ImagingResultRptIndRSRA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            SettingParameterDt svEmail_IS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IS_EMAIL_RADIOLOGI);
            sv_Email = svEmail_IS.ParameterValue.ToString();

            SettingParameterDt svSign_IS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IS_NAMA_TANDA_TANGAN_CETAKAN_HASIL_RADIOLOGI);
            sv_Sign = svSign_IS.ParameterValue.ToString();

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

        private void lblSignCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (sv_Sign == "1")
            {
                lblSignCaption.Text = "Pemeriksa,";
            }
            else
            {
                lblSignCaption.Text = "Dokter Pemeriksa,";
            }
        }

        //private void lblSignName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (sv_Sign == "1")
        //    {
        //        lblSignName.Text = appSession.UserFullName;
        //    }
        //    else
        //    {
        //        lblSignName.Text = GetCurrentColumnValue("TestRealizationPhysician").ToString();
        //    }
        //}

    }
}
