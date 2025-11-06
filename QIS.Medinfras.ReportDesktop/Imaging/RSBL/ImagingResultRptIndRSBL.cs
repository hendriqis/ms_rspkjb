using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.IO;
using ThoughtWorks.QRCode.Codec;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class ImagingResultRptIndRSBL : BaseNewCustomDailyPotraitRpt
    {
        private string sv_Email = "";
        private string sv_Sign = "0";

        public ImagingResultRptIndRSBL()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vImagingResultReport entityIR = BusinessLayer.GetvImagingResultReportList(param[0]).FirstOrDefault();

            SettingParameterDt svEmail_IS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IS_EMAIL_RADIOLOGI);
            sv_Email = svEmail_IS.ParameterValue.ToString();

            SettingParameterDt svSign_IS = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IS_KODE_DEFAULT_DOKTER);
            sv_Sign = svSign_IS.ParameterValue.ToString();
            string defaultDoctor = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_KODE_DEFAULT_DOKTER).ParameterValue;

            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", defaultDoctor))[0];

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}",
                entityParamedic.FullName, entityParamedic.LicenseNo);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 0;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ttdDokter.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion

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
