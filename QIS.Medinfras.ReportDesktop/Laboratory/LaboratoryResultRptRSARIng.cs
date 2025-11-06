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

namespace QIS.Medinfras.ReportDesktop   
{
    public partial class LaboratoryResultRptRSARIng : BaseDailyPortraitRpt
    {
        StringBuilder sbFooterNote = new StringBuilder();

        public LaboratoryResultRptRSARIng()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            IsNeedVerification = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_RESULT_NEED_VERIFICATION).ParameterValue;
            string defaultDoctor = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LB_KODE_DEFAULT_DOKTER).ParameterValue;

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

            //string oFooterNote = "";
            foreach (vLaboratoryResultDt resultDt in entity)
            {
                if (resultDt.IsUsingFooterNote)
                {
                    //if (oFooterNote != "")
                    //{
                    //    oFooterNote += "/r/n";
                    //}
                    //oFooterNote += resultDt.FooterNote;

                    sbFooterNote.AppendLine(resultDt.FooterNote);
                }                
            }
            //rtFooterNote.Text = oFooterNote;
            //rtFooterNote.Text = sbFooterNote.ToString();

            if (testOrderID == "0" || testOrderID == null)
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, chargeTransactionID);
            else
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1} AND TestOrderID = {2}", visitID, chargeTransactionID, testOrderID);

            vPatientChargesHdVisit entityPatient = BusinessLayer.GetvPatientChargesHdVisitList(filterExpression)[0];
            vPatientChargesDt entityPatientDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0}", entityPatient.TransactionID))[0];
            LaboratoryResultHd entityResultLabHd = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", entityPatient.TransactionID))[0];
            PatientDiagnosis entityPatientDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} ORDER BY ID DESC", visitID)).FirstOrDefault();
            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", defaultDoctor)).FirstOrDefault();

            lblAge.Text = entityPatient.PatientAge;
            lblGender.Text = entityPatient.Gender;
            lblNoReg.Text = entityPatient.RegistrationNo;
            lblNoRM.Text = entityPatient.MedicalNo;
            lblTglLahir.Text = entityPatient.DateOfBirthInString;
            lblParamedic.Text = entityPatient.ParamedicSenderName;
            lblParamedicCheckResult.Text = entityPatientDt.ParamedicName;
            lblPatientName.Text = entityPatient.PatientName;
            lblPrintDate.Text = entityPatient.PrintDateInString;
            lblRequestDate.Text = entityPatient.RequestDateInString;
            lblLabDate.Text = string.Format("{0}, {1}", entityResultLabHd.ResultDate.ToString(Constant.FormatString.DATE_FORMAT), entityResultLabHd.ResultTime);
            lblStreet.Text = entityPatient.StreetName;
            lblUnit.Text = entityPatient.VisitServiceUnitName;
            if (entityPatient.TestOrderNo == "" || entityPatient.TestOrderNo == null)
                lblNo.Text = entityPatient.TransactionNo;
            else
                lblNo.Text = entityPatient.TestOrderNo;
            base.InitializeReport(param);
            if (entityPatientDiagnose != null)
            {
                lblDiagnose.Text = entityPatientDiagnose.DiagnosisText;
            }
            else
            {
                lblDiagnose.Text = string.Format(" ");
            }
            lblParamedicDPJP.Text = entityParamedic.FullName;
            lblParamedicLab.Text = entityParamedic.FullName;
            
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

        private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrLabel7.Text == "")
            {
                //GroupHeader2.Visible = false;
            }
        }

        //private void xrRichText1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (cIsNormal.Checked)
        //    {
        //        xrRichText1.Font = new Font("Tahoma", 10, FontStyle.Regular);
        //    }
        //    else
        //    {
        //        xrRichText1.Font = new Font("Tahoma", 10, FontStyle.Bold);
        //    }
        //}

        private void cFractionName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //XRTableCell cell = (XRTableCell)sender;
            //Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            //if (!isNormal)
            //{
            //    cell.Font = new Font(Font.FontFamily, cell.Font.Size, FontStyle.Bold);
            //}
            //else
            //{
            //    cell.Font = new Font(Font.FontFamily, cell.Font.Size, FontStyle.Regular);
            //}
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
            //XRTableCell cell = (XRTableCell)sender;
            //Boolean MetrixUnitLabel = Convert.ToBoolean(GetCurrentColumnValue("MetrixUnitLabel"));
            //Boolean MetrixUnitLabel = Convert.ToBoolean(GetCurrentColumnValue("MetrixUnitLabel2"));
            //Boolean isPendingResult = Convert.ToBoolean(GetCurrentColumnValue("IsPendingResult"));
            //if (!isPendingResult)
            //{
            //    if (!isNormal)
            //    {
            //        cell.Text = MetrixUnitLabel;
            //    }
            //    else
            //    {
            //        cell.Text = MetrixUnitLabel2;
            //    }
            //}
            //else
            //{
            //    cell.Text = "";
            //}
        }

        private void cSatuan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //XRTableCell cell = (XRTableCell)sender;
            //Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            //Boolean isPendingResult = Convert.ToBoolean(GetCurrentColumnValue("IsPendingResult"));
            //if (!isPendingResult)
            //{
            //    if (!isNormal)
            //    {
            //        cell.Font = new Font(cell.Font.FontFamily, 9, FontStyle.Bold);
            //    }
            //    else
            //    {
            //        cell.Font = new Font(cell.Font.FontFamily, 9, FontStyle.Regular);
            //    }
            //}
            //else
            //{
            //    cell.Text = string.Empty;
            //}
        }

        private void cResultFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            if (!isNormal)
            {
                cell.Font = new Font(cell.Font.FontFamily, 9, FontStyle.Bold);
            }
            else
            {
                cell.Font = new Font(cell.Font.FontFamily, 9, FontStyle.Regular);
            }
        }

        private void rtFooterNote_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //rtFooterNote.Text = sbFooterNote.ToString();
            //rtFooterNote.Text = "aaaaaaaaaaaa"; //sbFooterNote.ToString();
        }
    }
}
