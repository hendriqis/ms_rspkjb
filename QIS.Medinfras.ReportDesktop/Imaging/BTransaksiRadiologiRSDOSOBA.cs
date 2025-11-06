using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransaksiRadiologiRSDOSOBA : BaseA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BTransaksiRadiologiRSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lineNumber = 0;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vConsultVisit entityCv = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entity.VisitID)).FirstOrDefault();
            vPrescriptionOrderHd entityPres = null;
            if (entity.PrescriptionOrderID != null)
            {
                entityPres = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", entity.PrescriptionOrderID)).FirstOrDefault();
            }

            string gender = "";
            if (entityReg.GCGender == Constant.Gender.MALE)
            {
                gender = "(L)";
            }
            else
            {
                gender = "(P)";
            }

            #region Header
            cRegistrationNo.Text = entityReg.RegistrationNo;
            cPatient.Text = string.Format("{0} | {1} {2} {3}", entityReg.MedicalNo, entityReg.PatientName, entityReg.Salutation, gender);
            cDOB.Text = string.Format("{0} ({1})", entityReg.DateOfBirthInString, entityReg.PatientAge);
            cBusinessPartnerName.Text = entityReg.BusinessPartnerName;
            cVisitServiceUnitName.Text = string.Format("{0} ({1})", entityReg.ServiceUnitName, entityReg.ChargeClassName);
            
            if (entityReg.BedCode == "")
            {
                cRoom.Text = string.Format("{0}", entityReg.RoomName);
            }
            else
            {
                cRoom.Text = string.Format("{0}|{1}", entityReg.RoomName, entityReg.BedCode);
            }
            cParamedicName.Text = entityCv.ParamedicName;
            cTransaction.Text = string.Format("{0} | {1}", entity.TransactionNo, entity.TransactionDateInString);
            cServiceUnitName.Text = entity.ServiceUnitName;
            cReferenceNo.Text = entity.ReferenceNo;
            if (entity.ScheduledDate.ToString("dd-MM-yyyy") == null || entity.ScheduledDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
            {
                xrTableRow20.Visible = false;
            }
            else
            {
                cScheduleDate.Text = entity.ScheduleDateTimeInString;
            }

            #endregion

            #region Page Header
            cHeaderPatient.Text = string.Format("{0}", entityReg.MedicalNo);
            cHeaderRegTrans.Text = string.Format("{0} | {1}", entity.RegistrationNo, entity.TransactionNo);
            #endregion

            #region Report Footer
            if (entity.LastUpdatedBy != 0 && entity.LastUpdatedBy != null)
            {
                lblLastUpdatedBy.Text = BusinessLayer.GetUserAttribute(entity.LastUpdatedBy).FullName;
            }
            else
            {
                lblLastUpdatedBy.Text = BusinessLayer.GetUserAttribute(entity.CreatedBy).FullName;
            }
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.cfDateForSignInString);
            #endregion

            base.InitializeReport(param);
        }

        private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell19.Text = (++lineNumber).ToString();
            detailID = Convert.ToInt32(GetCurrentColumnValue("ID"));
            if (detailID != oldDetailID)
            {
                totalAmount += Convert.ToDecimal(GetCurrentColumnValue("LineAmount"));
            }
        }

        private void GroupFooter2_AfterPrint(object sender, EventArgs e)
        {
            lineNumber = 0;
            oldDetailID = detailID;
        }

        private void cTotalLineAmount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cTotalLineAmount.Text = totalAmount.ToString("N2");
        }
    }
}
