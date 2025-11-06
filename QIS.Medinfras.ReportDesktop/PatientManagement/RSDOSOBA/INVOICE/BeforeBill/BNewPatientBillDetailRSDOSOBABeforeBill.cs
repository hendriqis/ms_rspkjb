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
    public partial class BNewPatientBillDetailRSDOSOBABeforeBill : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetailRSDOSOBABeforeBill()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtAll7BeforeBill> lstHDDT = BusinessLayer.GetPatientChargesHdDtAll7BeforeBillList(entityReg.RegistrationID);
            
            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}' ORDER BY PaymentDate",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            string corporate = "";
            string filterRegPayer = string.Format("RegistrationID = {0} AND IsDeleted = 0 ORDER BY IsPrimaryPayer DESC, BusinessPartnerID ASC", entityReg.RegistrationID);
            List<vRegistrationPayer> lstRegPayer = BusinessLayer.GetvRegistrationPayerList(filterRegPayer);
            foreach (vRegistrationPayer rp in lstRegPayer)
            {
                if (rp.BusinessPartnerOldCode != null && rp.BusinessPartnerOldCode != "")
                {
                    if (corporate != "")
                    {
                        corporate += ", " + "(" + rp.BusinessPartnerOldCode + "*) " + rp.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + rp.BusinessPartnerOldCode + "*) " + rp.BusinessPartnerName;
                    }
                }
                else
                {
                    if (corporate != "")
                    {
                        corporate += ", " + "(" + rp.BusinessPartnerCode + ") " + rp.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + rp.BusinessPartnerCode + ") " + rp.BusinessPartnerName;
                    }
                }
            }

            if (corporate == "")
            {
                if (entityReg.BusinessPartnerID != 1)
                {
                    if (entityReg.BusinessPartnerOldCode != null && entityReg.BusinessPartnerOldCode != "")
                    {
                        corporate = "(" + entityReg.BusinessPartnerOldCode + "*) " + entityReg.BusinessPartnerName;
                    }
                    else
                    {
                        corporate = "(" + entityReg.BusinessPartnerCode + ") " + entityReg.BusinessPartnerName;
                    }
                }
                else
                {
                    corporate = entityReg.BusinessPartnerName;
                }
            }

            lblCompany.Text = string.Format("{0} ( NPWP : {1} )", entityHealthcare.CompanyName, entityHealthcare.TaxRegistrantNo);

            #region Header : Patient Detail
            cPatientName.Text = entityReg.cfPatientNameSalutation;
            cDOB.Text = entityReg.DateOfBirthInString;
            cRegisteredPhysician.Text = entityReg.ParamedicName;

            cCorporate.Text = corporate;

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

            string planDischarge = entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string discharge = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string dischargeDateInfo = "-";

            if (entityReg.DischargeDate != null && discharge != "01-Jan-1900")
            {
                dischargeDateInfo = discharge + " " + entityReg.DischargeTime;
            }
            else
            {
                if (entityReg.PlanDischargeDate != null && planDischarge != "01-Jan-1900")
                {
                    dischargeDateInfo = planDischarge + "(*)";
                }
            }

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
            billPaymentDetailTransactionRSDOSOBABeforeBill.InitializeReport(lstHDDT);
            #endregion

            #region Footer
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
            lblTandaTangan.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }
    }
}
