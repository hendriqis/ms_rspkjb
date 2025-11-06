using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Report
{
    public partial class LaboratoryResultRpt : QIS.Medinfras.Report.BaseDailyPortraitRpt
    {
        public LaboratoryResultRpt()
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
            vPatientChargesHdVisit entityPatient = BusinessLayer.GetvPatientChargesHdVisitList(filterExpression)[0];

            lblAge.Text = entityPatient.PatientAge;
            lblGender.Text = entityPatient.Gender;
            lblNoReg.Text = entityPatient.RegistrationNo;
            lblNoRM.Text = entityPatient.MedicalNo;
            lblParamedic.Text = entityPatient.ParamedicName;
            lblPatientName.Text = entityPatient.PatientName;
            lblPrintDate.Text = entityPatient.PrintDateInString;
            lblRequestDate.Text = entityPatient.RequestDateInString;
            lblStreet.Text = entityPatient.StreetName;
            lblUnit.Text = entityPatient.ServiceUnitName;
            if (entityPatient.TestOrderNo == "" || entityPatient.TestOrderNo == null)
                lblNo.Text = entityPatient.TransactionNo;
            else
                lblNo.Text = entityPatient.TestOrderNo;
            base.InitializeReport(param);
        }

        private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel lbl = (XRLabel)sender;
            Boolean isNormal = Convert.ToBoolean(GetCurrentColumnValue("IsNormal"));
            String value = GetCurrentColumnValue("MetricResultValue").ToString();
            if (!isNormal)
            {
                lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size, FontStyle.Bold);
                value = String.Format("{0} *", value);
            }
            else
            {
                lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size, FontStyle.Regular);
            }
            lbl.Text = value;
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
    }
}
