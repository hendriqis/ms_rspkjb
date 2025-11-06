using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QISEncryption;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class DiagnosticResultRSSESRptInd : BaseNewCustomDailyPotraitRpt
    {
        private string sv_Email = "";
        private string sv_Sign = "0";

        public DiagnosticResultRSSESRptInd()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
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

                #region QR Codes Image
                string filter = string.Format("VisitID = {0}", oResult.VisitID);
                vConsultVisit9 entityCV = BusinessLayer.GetvConsultVisit9List(filter).FirstOrDefault();

                string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.RM0072, Constant.SettingParameter.RM0073);
                List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

                string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                    entityCV.MedicalNo, entityCV.RegistrationNo, entityCV.FirstName, entityCV.MiddleName, entityCV.LastName, entityCV.cfPatientLocation);

                if (lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM0072).FirstOrDefault().ParameterValue == "1")
                {
                    string url = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM0073).FirstOrDefault().ParameterValue;
                    string contenPlain = string.Format(@"{0}|{1}|{2}", reportMaster.ReportCode, entityCV.VisitID, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"));
                    string ecnryptText = Encryption.EncryptString(contenPlain);
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ecnryptText);
                    contents = string.Format("{0}/{1}", url, System.Convert.ToBase64String(plainTextBytes));
                }

                QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qRCodeEncoder.QRCodeScale = 4;
                qRCodeEncoder.QRCodeVersion = 0;
                qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                MemoryStream memoryStream = new MemoryStream();
                //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                //imgBarCode.Height = 400;
                //imgBarCode.Width = 400;

                using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        //byte[] byteImage = ms.ToArray();
                        //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                    }
                }
                #endregion
            }

            base.InitializeReport(param);
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

    }
}
