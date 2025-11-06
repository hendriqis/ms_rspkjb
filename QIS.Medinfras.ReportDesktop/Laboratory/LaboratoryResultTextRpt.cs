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
    public partial class LaboratoryResultTextRpt : BaseDailyPortraitRpt
    {
        public LaboratoryResultTextRpt()
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

            if (testOrderID == "0" || testOrderID == null)
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1}", visitID, chargeTransactionID);
            else
                filterExpression = string.Format("VisitID = {0} AND TransactionID = {1} AND TestOrderID = {2}", visitID, chargeTransactionID, testOrderID);

            vPatientChargesHdVisit entityChargesHdVisit = BusinessLayer.GetvPatientChargesHdVisitList(filterExpression)[0];

            lblAge.Text = entityChargesHdVisit.PatientAge;
            lblGender.Text = entityChargesHdVisit.Gender;
            lblNoReg.Text = entityChargesHdVisit.RegistrationNo;
            lblNoRM.Text = entityChargesHdVisit.MedicalNo;
            lblParamedic.Text = entityChargesHdVisit.ParamedicSenderName;
            lblPatientName.Text = entityChargesHdVisit.PatientName;
            lblPrintDate.Text = entityChargesHdVisit.PrintDateInString;
            lblRequestDate.Text = entityChargesHdVisit.RequestDateInString;
            lblStreet.Text = entityChargesHdVisit.StreetName;
            lblUnit.Text = entityChargesHdVisit.VisitServiceUnitName;
            if (entityChargesHdVisit.TestOrderNo == "" || entityChargesHdVisit.TestOrderNo == null)
                lblNo.Text = entityChargesHdVisit.TransactionNo;
            else
                lblNo.Text = entityChargesHdVisit.TestOrderNo;
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

        private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrLabel7.Text == "")
            {
                GroupHeader2.Visible = false;
            }
        }

        private void cHasilLab_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void cUnitLabel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void cSatuan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void cResultFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void xrRichText1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean isPendingResult = Convert.ToBoolean(GetCurrentColumnValue("IsPendingResult"));
            if (isPendingResult)
            {
                xrRichText1.Font = new Font(xrRichText1.Font.FontFamily, xrRichText1.Font.Size, FontStyle.Italic);
                xrRichText1.Text = "Menyusul";
            }
        }
    }
}
