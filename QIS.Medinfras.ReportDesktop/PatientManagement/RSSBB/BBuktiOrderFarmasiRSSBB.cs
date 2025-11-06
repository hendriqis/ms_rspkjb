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
    public partial class BBuktiOrderFarmasiRSSBB : BaseDailyPortraitRpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BBuktiOrderFarmasiRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lineNumber = 0;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPrescriptionOrderDt entityDt = BusinessLayer.GetvPrescriptionOrderDtList(param[0]).FirstOrDefault();
            vPrescriptionOrderHd entityHd = BusinessLayer.GetvPrescriptionOrderHdList(param[0]).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("VisitID = {0}", entityHd.VisitID)).FirstOrDefault();
            UserAttribute EntityUA = BusinessLayer.GetUserAttributeList(string.Format("UserID = {0}", entityDt.CreatedBy)).FirstOrDefault();

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
            cPatient.Text = string.Format("{0} | {1} {2}", entityReg.MedicalNo, entityReg.PatientName, gender);
            cDOB.Text = string.Format("{0} ({1})", entityReg.DateOfBirthInString, entityReg.PatientAge);
            cBusinessPartnerName.Text = entityReg.BusinessPartnerName;
            cVisitServiceUnitName.Text = string.Format("{0} ({1})", entityReg.ServiceUnitName, entityReg.ChargeClassName);

            //if (entityPres != null)
            //{
            //    cPreseption.Text = entityPres.PrescriptionType;
            //}
            //else
            //{
            //    cPreseption.Text = "";
            //}

            if (entityReg.BedCode == "")
            {
                cRoom.Text = string.Format("{0}", entityReg.RoomName);
            }
            else
            {
                cRoom.Text = string.Format("{0}|{1}", entityReg.RoomName, entityReg.BedCode);
            }
            cParamedicName.Text = entityReg.ParamedicName;
            cTransaction.Text = string.Format("{0} | {1}", entityHd.PrescriptionOrderNo, entityHd.PrescriptionOrderDateTimeInString);
            cServiceUnitName.Text = entityHd.ServiceUnitName;
            cRemarks.Text = entityHd.Remarks;
            cTglOrder.Text = entityHd.PrescriptionDateInString;
            //if (entity.ScheduledDate.ToString("dd-MM-yyyy") == null || entity.ScheduledDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
            //{
            //    xrTableRow20.Visible = false;
            //}
            //else
            //{
            //    cScheduleDate.Text = entity.ScheduleDateTimeInString;
            //}

            #endregion

            #region Page Header
            //  cHeaderPatient.Text = string.Format("{0}", entityReg.MedicalNo);
            // cHeaderRegTrans.Text = string.Format("{0} | {1}", entityReg.RegistrationNo, entityHd.TestOrderNo);
            #endregion

            #region Report Footer
            //if (entityHd.LastUpdateBy != 0 && entityHd.CreatedBy != null)
            //{
            lblLastUpdatedBy.Text = BusinessLayer.GetUserAttribute(entityDt.CreatedBy).FullName;
            //}
            //else
            //{
            //    lblLastUpdatedBy.Text = BusinessLayer.GetUserAttribute(entityHd.CreatedBy).FullName;
            //}
            //lblLastUpdatedBy.Text = string.Format("{0}", entityDt.CreatedBy);
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entityHd.cfDateForSignInString);
            #endregion

            base.InitializeReport(param);
        }

        //private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    xrTableCell19.Text = (++lineNumber).ToString();
        //    detailID = Convert.ToInt32(GetCurrentColumnValue("ID"));
        //    if (detailID != oldDetailID)
        //    {
        //        totalAmount += Convert.ToDecimal(GetCurrentColumnValue("LineAmount"));
        //    }
        //}

        private void GroupFooter2_AfterPrint(object sender, EventArgs e)
        {
            lineNumber = 0;
            oldDetailID = detailID;
        }

        //private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (GetCurrentColumnValue("TeamDtParamedicName").ToString() != null && GetCurrentColumnValue("TeamDtParamedicName").ToString() != "")
        //    {
        //        e.Cancel = false;
        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }
        //}

        //private void tabParamedicTeam_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (GetCurrentColumnValue("TeamDtParamedicName").ToString() != null && GetCurrentColumnValue("TeamDtParamedicName").ToString() != "")
        //    {
        //        e.Cancel = false;
        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }
        //}

        //private void cTotalLineAmount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    cTotalLineAmount.Text = totalAmount.ToString("N2");
        //}
    }
}
