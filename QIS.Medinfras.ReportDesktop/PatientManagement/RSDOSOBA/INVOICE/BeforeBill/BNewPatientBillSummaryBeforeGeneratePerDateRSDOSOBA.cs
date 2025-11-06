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
    public partial class BNewPatientBillSummaryBeforeGeneratePerDateRSDOSOBA : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillSummaryBeforeGeneratePerDateRSDOSOBA()
        {
            InitializeComponent();
        }
        
        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            List<GetPatientChargesHdDtAllBeforeBillPerDate2> lstHDDT = BusinessLayer.GetPatientChargesHdDtAllBeforeBillPerDate2List(Convert.ToInt32(param[0]), param[1]);
            lblCompany.Text = string.Format("{0} ( NPWP : {1} )", entityHealthcare.CompanyName, entityHealthcare.TaxRegistrantNo);

            #region Header : Patient Detail
            cPatientName.Text = entityReg.PatientName;
            cDOB.Text = entityReg.DateOfBirthInString;
            cRegisteredPhysician.Text = entityReg.ParamedicName;

            if (entityReg.ReferrerParamedicID != null && entityReg.ReferrerParamedicID != 0)
            {
                cReferrerPhysician.Text = entityReg.ReferrerParamedicName;
            }
            else
            {
                if (entityReg.ReferrerID != null && entityReg.ReferrerID != 0)
                {
                    cReferrerPhysician.Text = entityReg.ReferrerName;
                }
                else
                {
                    cReferrerPhysician.Text = "";
                }
            }

            if (entityReg.OldMedicalNo != "" && entityReg.OldMedicalNo != null)
            {
                cMedicalNo.Text = string.Format("{0} ({1})", entityReg.MedicalNo, entityReg.OldMedicalNo);
            }
            else
            {
                cMedicalNo.Text = string.Format("{0}", entityReg.MedicalNo);
            }

            cServiceUnitClass.Text = string.Format("{0} | {1}", entityReg.ServiceUnitName, entityReg.ChargeClassName);
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);

            string dischargeDateInfo = entityReg.cfDischargeDateInString;
            //if (dischargeDateInfo != "")
            //{
            //    if (entityReg.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entityReg.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            //    {
            //        dischargeDateInfo = dischargeDateInfo + " (" + "RIP" + ")";
            //    }
            //}
            cDischargeDate.Text = dischargeDateInfo;

            if (entityReg.LinkedToRegistrationID != null && entityReg.LinkedToRegistrationID != 0)
            {
                Registration entityRegLinkedTo = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.LinkedToRegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();

                cNotes.Text = string.Format("Transfer ke Registrasi {0}", entityRegLinkedTo.RegistrationNo);
            }
            else
            {
                cNotes.Text = "-";
            }
            #endregion

            #region Header : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityReg.PatientName, entityReg.Gender, entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);
            cHeaderRegistration.Text = entityReg.RegistrationNo;
            cHeaderMedicalNo.Text = entityReg.MedicalNo;
            #endregion

            #region Transaction
            subTransaction.CanGrow = true;
            billPaymentSummaryTransactionBeforeGeneratePerDate1RSDOSOBA.InitializeReport(lstHDDT);
            #endregion

            #region Footer
            cTTDPatient.Text = entityReg.PatientName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            string tempParam = string.Format("RegistrationID = {0}", param[0]);
            string[] lstTempParam = tempParam.Split('|');
            base.InitializeReport(lstTempParam);
        }
    }
}
