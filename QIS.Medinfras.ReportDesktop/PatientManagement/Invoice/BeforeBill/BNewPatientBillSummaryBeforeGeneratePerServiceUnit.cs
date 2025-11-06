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
    public partial class BNewPatientBillSummaryBeforeGeneratePerServiceUnit : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillSummaryBeforeGeneratePerServiceUnit()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] newParam = param[0].Split(';');
            string[] reg = new string[] { string.Format("RegistrationID = {0}", newParam[0]) };
            string hsu = string.Format("ItemHealthcareServiceUnitID = {0}", newParam[1]);

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(reg[0]).FirstOrDefault();
            Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            Healthcare entityHealthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);

            List<GetPatientChargesHdDtAllBeforeBillPerServiceUnit> lst;
            if (Convert.ToInt32(newParam[1]) != 0)
            {
                lst = BusinessLayer.GetPatientChargesHdDtAllBeforeBillPerServiceUnitList(entityReg.RegistrationID, Convert.ToInt32(newParam[1]));
            }
            else
            {
                lst = BusinessLayer.GetPatientChargesHdDtAllBeforeBillPerServiceUnitList(entityReg.RegistrationID, 0);
            }

            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

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

            cServiceUnitClass.Text = string.Format("{0} | {1}", entityReg.ServiceUnitName, entityReg.ClassName);
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

            if (entityRegLinked != null)
            {
                cNotes.Text = string.Format("Transfer Rawat Inap ({0})", entityRegLinked.RegistrationNo);
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
            billPaymentSummaryTransactionBeforeGenerateByServiceUnit.InitializeReport(lst);
            #endregion

            #region Footer
            cTTDPatient.Text = entityReg.PatientName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(reg);
        }
    }
}
