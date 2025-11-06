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
    public partial class BDiagnosticJobOrderRealizationRSDOSKA : BaseCustomDailyPotrait3Rpt
    {
        public BDiagnosticJobOrderRealizationRSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            PatientChargesHd entityHD = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            PatientChargesDt entityDT = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entityHD.VisitID)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            
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
            cTransactionNo.Text = entityHD.TransactionNo;
            cTransactionDate.Text = entityHD.TransactionDateInString + " | " + entityHD.TransactionTime;
            cRegistrationNo.Text = cv.RegistrationNo;
            cRegisteredPhysician.Text = cv.ParamedicName;
            cDeptServiceUnit.Text = cv.cfServiceUnitRoomBed;
            //if (entityHD.TestOrderID != null)
            //{
            //    vTestOrderHd vTestOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", entityHD.TestOrderID)).FirstOrDefault();
            //    List<vPatientChargesDt> to = BusinessLayer.GetvPatientChargesDtList(String.Format("TransactionID = {0} ", entityHD.TransactionID));
            //    String MedicalNotes = "";
            //    Int32 Number = 1;

            //    if (vTestOrderHd.IsCITO == true)
            //    {
            //        IsCito.Text = "CITO";
            //    }
            //    else
            //    {
            //        IsCito.Text = " ";
            //    }

            //    if (entityDT.ReferenceDtID == null)
            //    {
            //        foreach (vPatientChargesDt t in to)
            //        {

            //            if (string.IsNullOrEmpty(MedicalNotes))
            //            {
            //                MedicalNotes = "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + " * " + Environment.NewLine;
            //            }
            //            else
            //            {
            //                MedicalNotes += "; " + "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + " * " + Environment.NewLine;
            //            }
            //            Number = Number + 1;

            //            txtRemarks.Text = MedicalNotes;
            //        }
            //    }
            //    else
            //    {
            //        foreach (vPatientChargesDt t in to)
            //        {

            //            if (string.IsNullOrEmpty(MedicalNotes))
            //            {
            //                MedicalNotes = "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + Environment.NewLine;
            //            }
            //            else
            //            {
            //                MedicalNotes += "; " + "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + Environment.NewLine;
            //            }
            //            Number = Number + 1;

            //            txtRemarks.Text = MedicalNotes;
            //        }
            //    }

            //}
            //else
            //{
            //    txtRemarks.Visible = false;
            //}
            txtPemeriksaan.Text = string.Format("{0} | {1}",entityHD.TransactionDateInString, entityHD.TransactionTime);

            #region Footer
            lblRemarks.Text = "Remarks : " + "\r\n" + entityHD.Remarks;

            //lblMedicalNotesHeader.Text = "Medical Notes : ";

            if (entityHD.TestOrderID != null)
            {
                vTestOrderHd vTestOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", entityHD.TestOrderID)).FirstOrDefault();
                List<vPatientChargesDt> to = BusinessLayer.GetvPatientChargesDtList(String.Format("TransactionID = {0} ", entityHD.TransactionID));
                //String MedicalNotes = "";
                //Int32 Number = 1;

                txtRemarks.Text = vTestOrderHd.Remarks;
                if (vTestOrderHd.IsCITO == true)
                {
                    IsCito.Text = "CITO";
                }
                else
                {
                    IsCito.Text = " ";
                }

                //if (entityDT.ReferenceDtID == null)
                //{
                //    foreach (vPatientChargesDt t in to)
                //    {

                //        if (string.IsNullOrEmpty(MedicalNotes))
                //        {
                //            MedicalNotes = "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + " * " + Environment.NewLine;
                //        }
                //        else
                //        {
                //            MedicalNotes += "; " + "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + " * " + Environment.NewLine;
                //        }
                //        Number = Number + 1;

                //        lblMedicalNotesDetail.Text = MedicalNotes;
                //    }
                //}
                //else
                //{
                //    foreach (vPatientChargesDt t in to)
                //    {

                //        if (string.IsNullOrEmpty(MedicalNotes))
                //        {
                //            MedicalNotes = "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + Environment.NewLine;
                //        }
                //        else
                //        {
                //            MedicalNotes += "; " + "(" + Number + ") " + t.ItemName1 + " : " + vTestOrderHd.Remarks + Environment.NewLine;
                //        }
                //        Number = Number + 1;

                //        lblMedicalNotesDetail.Text = MedicalNotes;
                //    }
                //}
            }
            else
            {
                txtRemarks.Text = entityHD.Remarks;
                //lblMedicalNotesDetail.Visible = false;
            }

            lblLastUpdatedBy.Text = appSession.UserFullName;
            if (entityHD.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityHD.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityHD.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
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
