using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LaboratoryResultRpt3 : BaseDailyPortraitRpt
    {
        private string city = "";
        private string penanggungJawab = "";
        public LaboratoryResultRpt3()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            city = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault().City;
            IsNeedVerification = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_RESULT_NEED_VERIFICATION).ParameterValue;
            penanggungJawab = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LB_KODE_DEFAULT_DOKTER).ParameterValue;
            vParamedicMaster entityParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", penanggungJawab))[0];

            string visitID, chargeTransactionID, testOrderID, filterExpression = "";
            LaboratoryResultHd entityLabHd = BusinessLayer.GetLaboratoryResultHdList(param[0])[0];
            List<vLaboratoryResultDt> entity = BusinessLayer.GetvLaboratoryResultDtList(param[0]);
            if (entity.Count > 0)
            {
                visitID = entity[0].VisitID.ToString();
                chargeTransactionID = entity[0].ChargeTransactionID.ToString();
                testOrderID = entity[0].TestOrderID.ToString();
            }
            else
            {
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
            lblParamedic.Text = entityPatient.ParamedicSenderName;
            lblPatientName.Text = entityPatient.PatientName;
            lblPrintDate.Text = entityPatient.PrintDateInString;
            lblRequestDate.Text = entityPatient.RequestDateInString;
            lblStreet.Text = entityPatient.StreetName;
            lblUnit.Text = entityPatient.VisitServiceUnitName;
            if (entityPatient.TestOrderNo == "" || entityPatient.TestOrderNo == null)
                lblNo.Text = entityPatient.TransactionNo;
            else
                lblNo.Text = entityPatient.TestOrderNo;
            lblCatatan.Text = string.Format("{0}", entityLabHd.Remarks);

            lblPenanggungJawab.Text = entityParamedic.ParamedicName;
            ttdPenanggungJawab.Text = entityParamedic.ParamedicName;

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

        private void lblTanggal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string createdDate = Convert.ToDateTime(GetCurrentColumnValue("CreatedDate")).ToString(Constant.FormatString.DATE_FORMAT);
            lblTanggal.Text = string.Format("{0}, {1}", city, createdDate);
        }
    }
}
