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
    public partial class LaboratoryPAResultRpt : BaseDailyPortraitRpt
    {
        public LaboratoryPAResultRpt()
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

            vPatientChargesLabPAResult entityHd = BusinessLayer.GetvPatientChargesLabPAResultList(filterExpression)[0];

            lblAge.Text = entityHd.PatientAge;
            lblGender.Text = entityHd.Gender;
            lblNoReg.Text = entityHd.RegistrationNo;
            lblNoRM.Text = entityHd.MedicalNo;
            lblParamedic.Text = entityHd.ParamedicSenderName;
            lblPatientName.Text = entityHd.PatientName;
            lblPrintDate.Text = entityHd.PrintDateInString;
            lblRequestDate.Text = entityHd.RequestDateInString;
            string address = string.Format("{0}", entityHd.StreetName);
            string RT = string.Empty;
            string RW = string.Empty;
            if (!string.IsNullOrEmpty(entityHd.RT))
            {
                RT = string.Format("RT{0}", entityHd.RT);
                address += string.Format(" {0}", RT);
            }
            if (!string.IsNullOrEmpty(entityHd.RW))
            {
                RW = string.Format("/RW{0}", entityHd.RW);
                address += string.Format(" {0}", RW);
            }
            address += string.Format(" {0} {1}", entityHd.County, entityHd.City);
            lblStreet.Text = address;
            ////string.Format("{0} RT{1}/{2} {3} {4}", entityHd.StreetName, string.IsNullOrEmpty(entityHd.RT) ? "-" : entityHd.RT, string.IsNullOrEmpty(entityHd.RW) ? "-" : entityHd.RW, entityHd.County, entityHd.City);
            lblUnit.Text = entityHd.VisitServiceUnitName;
            //if (entityHd.IsLaboratoryUnit) 
            //{
            //    if (entityHd.GCReferrerGroup == Constant.Referrer.DOKTER_RS)
            //    {
            //        ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(entityHd.ReferrerID);
            //        if (oParamedic != null)
            //        {
            //            lblUnit.Text = oParamedic.FullName;
            //        }
            //    }
            //    else {
            //        vReferrer oReferrer = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID='{0}'", entityHd.ReferrerID))[0];
            //        if (oReferrer != null) {
            //            lblUnit.Text = oReferrer.BusinessPartnerName;
            //        }
            //    }
            //}
            if (entityHd.TestOrderNo == "" || entityHd.TestOrderNo == null)
                lblNo.Text = entityHd.TransactionNo;
            else
                lblNo.Text = entityHd.TestOrderNo;
            lblParamedicCheckResult.Text = entityHd.ParamedicDetail;
            lblMengetahui.Text = entityHd.ParamedicDetail;

            if (!string.IsNullOrEmpty(chargeTransactionID))
            {
                PatientChargesHdInfo oPHDInfo = BusinessLayer.GetPatientChargesHdInfo(Convert.ToInt32(chargeTransactionID));
                if (oPHDInfo != null)
                {
                    lblPANo.Text = oPHDInfo.PAReferenceNo;
                }
            }
            lblMacroscopicRemarks.Text = string.Format("{0}", entityHd.MacroscopicRemarks);
            lblSpecimenLocation.Text = string.Format("Lokasi Spesimen : {0}", entityHd.SpecimenLocation);

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
            String textValue = GetCurrentColumnValue("TextValue").ToString();
            if (isPendingResult)
            {
                xrRichText1.Font = new Font(xrRichText1.Font.FontFamily, xrRichText1.Font.Size, FontStyle.Italic);
                xrRichText1.Html = "Menyusul";
            }
            else
            {
                xrRichText1.Html = textValue; //ini wir
            }

        }
    }
}
