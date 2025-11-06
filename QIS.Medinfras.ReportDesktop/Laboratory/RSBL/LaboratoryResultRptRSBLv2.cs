using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.IO;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LaboratoryResultRptRSBLv2 : BaseDailyPortrait2Rpt
    {
        public LaboratoryResultRptRSBLv2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            StringBuilder sbFooterNote = new StringBuilder();

            IsNeedVerification = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_RESULT_NEED_VERIFICATION).ParameterValue;
            string defaultDoctor = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LB_KODE_DEFAULT_DOKTER).ParameterValue;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            lblEmail.Text = string.Format("Email : {0}", entityHealthcare.Email);

            string visitID, chargeTransactionID, testOrderID, filterExpression = "";
            List<vLaboratoryResultDt> entity = BusinessLayer.GetvLaboratoryResultDtList(param[0]);
            if (entity.Count > 0)
            {
                visitID = entity[0].VisitID.ToString();
                chargeTransactionID = entity[0].ChargeTransactionID.ToString();
                testOrderID = entity[0].TestOrderID.ToString();
            }
            else
            {
                LaboratoryResultHd entityLabHd = BusinessLayer.GetLaboratoryResultHdList(param[0])[0];
                visitID = entityLabHd.VisitID.ToString();
                chargeTransactionID = entityLabHd.ChargeTransactionID.ToString();
                testOrderID = entityLabHd.TestOrderID.ToString();
            }

            if (testOrderID == "0" || testOrderID == null)
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, chargeTransactionID);
            else
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1} AND TestOrderID = {2}", visitID, chargeTransactionID, testOrderID);
            
            vPatientChargesHdVisit entityPatient = BusinessLayer.GetvPatientChargesHdVisitList(filterExpression)[0];
            vPatientChargesDt entityPatientDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0}", entityPatient.TransactionID))[0];
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

            lblAge.Text = entityPatient.PatientAge;
            lblGender.Text = entityPatient.Gender;
            lblNoReg.Text = entityPatient.RegistrationNo;
            lblNoRM.Text = entityPatient.MedicalNo;
            lblTglLahir.Text = entityPatient.DateOfBirthInString;
            lblParamedic.Text = entityPatient.ParamedicSenderName;
            lblParamedicCheckResult.Text = entityParamedic.FullName;
            lblPatientName.Text = entityPatient.PatientName;
            lblPrintDate.Text = entityPatient.PrintDateInString;
            lblRequestDate.Text = entityPatient.RequestDateInString;
            lblStreet.Text = entityPatient.StreetName;
            lblUnit.Text = entityPatient.VisitServiceUnitName;
            if (entityPatient.TestOrderNo == "" || entityPatient.TestOrderNo == null)
                lblNo.Text = entityPatient.TransactionNo;
            else
                lblNo.Text = entityPatient.TestOrderNo;

            lblPenanggungJawab2.Text = string.Format("{0}", entityParamedic.FullName);

            base.InitializeReport(param);
        }

        private string IsNeedVerification = "0";

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (IsNeedVerification == "1")
            {
                XRTable tbl = (XRTable)sender;
                Boolean isVerified = Convert.ToBoolean(GetCurrentColumnValue("IsVerified"));
                //String value = "";
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    if (!isVerified)
                        tbl.Rows[i].Visible = false;
                }
            }
        }

        //private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (xrLabel7.Text == "")
        //    {
        //        GroupHeader2.Visible = false;
        //    }
        //}

        private void cFractionName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            if (!isNormal)
            {
                cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Bold);
            }
            else
            {
                cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
            }
        }

        private void cLabResult_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            Boolean isPendingResult = Convert.ToBoolean(GetCurrentColumnValue("IsPendingResult"));
            if (!isPendingResult)
            {
                if (!isNormal)
                {
                    cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Bold);
                    string a = Regex.Replace(cell.Text, "<.*?>", " ");
                    cell.Text = a.ToString();
                }
                else
                {
                    cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
                    string a = Regex.Replace(cell.Text, "<.*?>", " ");
                    cell.Text = a.ToString();
                }
            }
            else
            {
                cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Italic);
                cell.Text = "Menyusul";
            }
        }       

        private void cUnitLabel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            Boolean isPendingResult = Convert.ToBoolean(GetCurrentColumnValue("IsPendingResult"));
            if (!isPendingResult)
            {
                if (!isNormal)
                {
                    cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
                }
                else
                {
                    cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
                }
            }
            else
            {
                cell.Text = "";
            }
        }

        private void cSatuan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            Boolean isPendingResult = Convert.ToBoolean(GetCurrentColumnValue("IsPendingResult"));
            if (!isPendingResult)
            {
                if (!isNormal)
                {
                    cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
                }
                else
                {
                    cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
                }
            }
            else
            {
                cell.Text = string.Empty;
            }
        }

        private void cResultFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            if (!isNormal)
            {
                cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
            }
            else
            {
                cell.Font = new Font(cell.Font.FontFamily, cell.Font.Size, FontStyle.Regular);
            }
        }
    }
}
