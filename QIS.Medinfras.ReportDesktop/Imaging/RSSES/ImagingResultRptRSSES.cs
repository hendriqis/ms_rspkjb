using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QISEncryption;
using System.Linq;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class ImagingResultRptRSSES : BaseDailyPortraitRpt
    {
        public ImagingResultRptRSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region QR Codes Image
            vImagingResultReport oResult = BusinessLayer.GetvImagingResultReportList(param[0]).FirstOrDefault();
            if (oResult != null)
            {
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
            }
            #endregion

            base.InitializeReport(param);
        }

        private void lblTestOrderInfo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblTestOrderInfo.Text.Contains(";"))
            {
                lblOrderParamedic.Text = lblTestOrderInfo.Text.Split(';')[3];
                string TestOrderNo = lblTestOrderInfo.Text.Split(';')[0];
                string ParamedicName = string.Empty;
                if (!string.IsNullOrEmpty(TestOrderNo)) {
                    string expression = string.Format("TestOrderNo='{0}'", TestOrderNo);
                    vImagingResultReport oResult = BusinessLayer.GetvImagingResultReportList(expression).FirstOrDefault();
                    if (oResult != null) 
                    {
                        vPatientChargesHd9 oTransaction = BusinessLayer.GetvPatientChargesHd9List(string.Format("TransactionID='{0}'", oResult.TransactionID)).FirstOrDefault();
                        if (oTransaction != null) {

                            if (oTransaction.IsOrderCreatedBySystem)
                            {
                                vConsultVisit5 oVisit = BusinessLayer.GetvConsultVisit5List(string.Format("VisitID='{0}'", oTransaction.VisitID)).FirstOrDefault();
                                if (oVisit.DepartmentID == Constant.Facility.IMAGING || oVisit.DepartmentID == Constant.Facility.DIAGNOSTIC)
                                {
                                    vRegistration3 registration = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", oVisit.RegistrationID)).FirstOrDefault();
                                    if (registration.ReferrerParamedicID > 0)
                                    {
                                        string strFilterExpression = string.Format("ParamedicID = '{0}'", registration.ReferrerParamedicID);
                                        ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(strFilterExpression).FirstOrDefault();
                                        if (pm != null)
                                        {
                                            ParamedicName = pm.FullName;
                                        }
                                    }
                                    else
                                    {
                                        ParamedicName = registration.ReferrerName;
                                    }

                                    lblOrderParamedic.Text = ParamedicName;
                                    vPatientDiagnosis oDiagnosa = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID='{0}' and GCDiagnoseType='{1}' AND IsDeleted=0", oVisit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
                                    if (oDiagnosa != null)
                                    {
                                        if (!string.IsNullOrEmpty(oDiagnosa.DiagnoseName))
                                        {
                                            lblCatatanKlinis.Text = oDiagnosa.DiagnoseName;
                                        }
                                        else
                                        {
                                            lblCatatanKlinis.Text = oDiagnosa.DiagnosisText;
                                        }

                                    }
                                }
                            }
                            else 
                            {
                              lblCatatanKlinis.Text  = oResult.Remarks;
                            }
                           
                        }


                        ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, oResult.TestRealizationPhysicianCode);
                        ttdDokter.Visible = true;

                        lblSignParamedicName.Text = oResult.TestRealizationPhysician;                      
                    }
                }
               
            }
        }

    }
}
