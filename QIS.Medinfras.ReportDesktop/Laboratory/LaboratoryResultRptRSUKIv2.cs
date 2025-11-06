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

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LaboratoryResultRptRSUKIv2 : BaseDailyPortraitRpt
    {
        StringBuilder sbFooterNote = new StringBuilder();

        public LaboratoryResultRptRSUKIv2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            IsNeedVerification = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_RESULT_NEED_VERIFICATION).ParameterValue;
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

            foreach (vLaboratoryResultDt resultDt in entity)
            {
                if (resultDt.IsUsingFooterNote)
                {
                    sbFooterNote.AppendLine(resultDt.FooterNote);
                    if (resultDt.IsNormal == true)
                    {
                        lblCatatan.Text = "Catatan : ";
                    }
                    else
                    {
                        lblCatatan.Text = "Saran : ";
                    }
                }
                else
                {
                    lblCatatan.Visible = false;
                }
            }

            if (testOrderID == "0" || testOrderID == null)
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, chargeTransactionID);
            else
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1} AND TestOrderID = {2}", visitID, chargeTransactionID, testOrderID);
            
            vPatientChargesHdVisit entityPatient = BusinessLayer.GetvPatientChargesHdVisitList(filterExpression)[0];
            vPatientChargesDt entityPatientDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0}", entityPatient.TransactionID))[0];
            LaboratoryResultHd entityHd = BusinessLayer.GetLaboratoryResultHdList(param[0])[0];
            UserAttribute entityUA = BusinessLayer.GetUserAttributeList(string.Format("UserID = {0}", entityHd.CreatedBy))[0];

            lblAge.Text = entityPatient.PatientAgeInd;
            lblGender.Text = entityPatient.cfSex;
            lblNoReg.Text = entityPatient.RegistrationNo;
            lblNoRM.Text = entityPatient.MedicalNo;
            lblTglLahir.Text = entityPatient.DateOfBirthInString;
            lblParamedic.Text = entityPatient.ParamedicSenderName;
            lblParamedicCheckResult.Text = entityPatientDt.ParamedicName;
            lblPatientName.Text = entityPatient.PatientName;
            lblPrintDate.Text = entityPatient.PrintDateInString;
            lblRequestDate.Text = entityPatient.RequestDateInString;
            lblStreet.Text = entityPatient.StreetName;
            lblUnit.Text = entityPatient.VisitServiceUnitName;
            lblBusinessPartner.Text = entityPatient.BusinessPartnerName;
            lblParamediName1.Text = "1. dr. Erida Manalu, Sp.Pk";
            lblParamediName2.Text = "2. dr. Danny Ernest Jonas Luhulima, Sp.Pk";
            if (entityPatient.TestOrderNo == "" || entityPatient.TestOrderNo == null)
                lblNo.Text = entityPatient.TransactionNo;
            else
                lblNo.Text = entityPatient.TestOrderNo;
            lblCreatedByName.Text = entityUA.FullName;
            base.InitializeReport(param);
        }

        private string IsNeedVerification = "0";

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (IsNeedVerification == "1")
            {
                XRTable tbl = (XRTable)sender;
                Boolean isVerified = Convert.ToBoolean(GetCurrentColumnValue("IsVerified"));
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

            }
        }

        private void cFractionName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

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

        }

        private void cSatuan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

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

        }
    }
}
