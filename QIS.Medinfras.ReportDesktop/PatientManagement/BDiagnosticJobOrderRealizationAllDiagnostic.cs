using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BDiagnosticJobOrderRealizationAllDiagnostic : BaseCustomDailyPotraitRpt
    {
        public BDiagnosticJobOrderRealizationAllDiagnostic()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            PatientChargesHd entityHD = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", param[0]))[0];
            vPatientChargesDt entity = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0}", param[0]))[0];
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entity.VisitID))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            
            #region Header : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                cv.PatientName, cv.Gender, cv.AgeInYear, cv.AgeInMonth, cv.AgeInDay);
            cHeaderRegistration.Text = cv.RegistrationNo;
            cHeaderMedicalNo.Text = cv.MedicalNo;
            #endregion

            cMedicalNo.Text = cv.MedicalNo;
            cPatientName.Text = string.Format("{0} ( {1} )", cv.PatientName, cv.Gender);
            cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", cv.DateOfBirthInString, cv.AgeInYear, cv.AgeInMonth, cv.AgeInDay);
            cCorporate.Text = cv.BusinessPartnerName;
            cTransactionNo.Text = entity.TransactionNo;
            cTransactionDate.Text = entity.TransactionDateInString + " | " + entity.TransactionTime;
            cRegistrationNo.Text = cv.RegistrationNo;
            cRegisteredPhysician.Text = cv.ParamedicName;
            cDeptServiceUnit.Text = cv.cfServiceUnitRoomBed;

            #region Footer
            lblRemarks.Text = "Remarks : " + "\r\n" + entityHD.Remarks;

            lblMedicalNotesHeader.Text = "Medical Notes : ";

            if (entityHD.TestOrderID != null)
            {
                List<vTestOrderDt> to = BusinessLayer.GetvTestOrderDtList(String.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ID DESC", entityHD.TestOrderID));
                String MedicalNotes = "";
                Int32 Number = 1;
                foreach (vTestOrderDt t in to)
                {
                    string filterOrder = String.Format("TestOrderID = {0} AND ItemID = {1}", t.TestOrderID, t.ItemID);
                    vPatientChargesDtReport3 chargesDt = BusinessLayer.GetvPatientChargesDtReport3List(filterOrder).FirstOrDefault();
                    if (chargesDt != null)                    
                    {
                        if (string.IsNullOrEmpty(MedicalNotes))
                        {
                            MedicalNotes = "(" + Number + ") " + t.ItemName1 + " : " + t.Remarks + Environment.NewLine;
                        }
                        else
                        {
                            MedicalNotes += "; " + "(" + Number + ") " + t.ItemName1 + " : " + t.Remarks + Environment.NewLine;
                        }
                        Number = Number + 1;
                    }
                }
                lblMedicalNotesDetail.Text = MedicalNotes;
            }
            else
            {
                lblMedicalNotesDetail.Visible = false;
            }

            lblLastUpdatedBy.Text = appSession.UserFullName;
            if (entity.LastUpdatedDateInString != "01-Jan-1900")
            {
                lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entity.LastUpdatedDateInString;
            }
            else
            {
                lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entity.CreatedDateInString;
            }

            c1.Text = cv.PatientName;
            c4.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }

        private void xrTableCell45_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String DiagnoseTestOrder = GetCurrentColumnValue("DiagnoseTestOrder").ToString();
            if (DiagnoseTestOrder == "" || DiagnoseTestOrder == null)
            {
                xrTableCell45.Visible = false;
            }
        }

    }
}
