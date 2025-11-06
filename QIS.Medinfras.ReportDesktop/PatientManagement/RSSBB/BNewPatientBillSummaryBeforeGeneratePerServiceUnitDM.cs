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
    public partial class BNewPatientBillSummaryBeforeGeneratePerServiceUnitDM : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillSummaryBeforeGeneratePerServiceUnitDM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] newParam = param[0].Split(';');
            string[] reg = new string[] { string.Format("RegistrationID = {0}", newParam[0]) };
            string hsu = string.Format("ItemHealthcareServiceUnitID = {0}", newParam[1]);

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(reg[0]).FirstOrDefault();
            RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(String.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
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
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            string corporate = "";
            string filterRegPayer = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY IsPrimaryPayer DESC, BusinessPartnerID ASC", entityReg.RegistrationID);
            List<vRegistrationPayer> lstRegPayer = BusinessLayer.GetvRegistrationPayerList(filterRegPayer);
            foreach (vRegistrationPayer rp in lstRegPayer)
            {
                if (corporate != "")
                {
                    corporate += ", " + rp.BusinessPartnerName;
                }
                else
                {
                    corporate = rp.BusinessPartnerName;
                }
            }

            if (corporate == "")
            {
                corporate = entityReg.BusinessPartnerName;
            }

            lblCompany.Text = string.Format("{0} ( NPWP : {1} )", entityHealthcare.CompanyName, entityHealthcare.TaxRegistrantNo);

            #region Header : Patient Detail
            if (entityReg.Salutation != "")
            {
                cPatientName.Text = entityReg.Salutation + " " + entityReg.PatientName;
            }
            else
            {
                cPatientName.Text = entityReg.PatientName;
            }
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

            cServiceUnitClass.Text = string.Format("{0}", entityReg.ServiceUnitName);
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);

            cCorporate.Text = corporate;

            //string dischargeDateInfo = entityReg.cfDischargeDateInString;

            //cDischargeDate.Text = dischargeDateInfo;
            string planDischarge = entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string discharge = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string dischargeDateInfo = "-";

            if (entityReg.DischargeDate.Date.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
            {
                dischargeDateInfo = discharge;
            }
            else
            {
                if (entityReg.PlanDischargeDate.Date.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
                {
                    dischargeDateInfo = planDischarge + "(*)";
                }
            }
            cDischargeDate.Text = dischargeDateInfo;

            if (entityRegBPJS != null)
            {
                cSEPNoCaption.Text = "SEP No";
                cSEPNo.Text = entityRegBPJS.NoSEP;
            }
            else if (entityReg.ReferralNo != null && entityReg.ReferralNo != "")
            {
                cSEPNoCaption.Text = "Referral No";
                cSEPNo.Text = entityReg.ReferralNo;
            }
            else
            {
                cSEPNoCaption.Visible = false;
                cSEPNo.Text = "";
                cSEPNo.Visible = false;
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
            billPaymentSummaryTransactionBeforeGenerateByServiceUnitDM.InitializeReport(lst);
            #endregion

            base.InitializeReport(reg);
        }
    }
}
