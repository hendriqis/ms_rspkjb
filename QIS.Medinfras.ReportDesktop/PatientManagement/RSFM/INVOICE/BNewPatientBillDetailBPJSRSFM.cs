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
    public partial class BNewPatientBillDetailBPJSRSFM : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetailBPJSRSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtAll10> lstHDDT = BusinessLayer.GetPatientChargesHdDtAll10List(entityReg.RegistrationID);
            
            List<PatientBill> lstBill = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}' ORDER BY PaymentDate",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHdDt> lstPaymentHdDt = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            string filterParameter = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", entityReg.RegistrationID, Constant.TransactionStatus.VOID);

            vPatientBill entityBill = BusinessLayer.GetvPatientBillList(filterParameter).FirstOrDefault();

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

            #region Header : Patient Detail
            lblReportTitle.Visible = false;
            lblReportSubTitle.Visible = false;

            cPatientName.Text = entityReg.cfPatientNameSalutation;
            cDOB.Text = entityReg.DateOfBirthInString;
            cRegisteredPhysician.Text = entityReg.ParamedicName;

            cCorporate.Text = corporate;

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
            #endregion

            #region Header : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} th {3} bln {4} hr",
                entityReg.PatientName, entityReg.Gender, entityReg.AgeInYear, entityReg.AgeInMonth, entityReg.AgeInDay);
            cHeaderRegistration.Text = entityReg.RegistrationNo;
            cHeaderMedicalNo.Text = entityReg.MedicalNo;
            #endregion

            #region Transaction
            subTransaction.CanGrow = true;
            billPaymentDetailTransactionRSFM.InitializeReport(lstHDDT);
            #endregion

            #region Footer
            if (entityBill == null)
            {
                lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, entityBill.cfBillCreatedDateTimeInString);
                lblTandaTangan.Text = entityBill.CreatedByFullName;
            }
            else
            {
                lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
                lblTandaTangan.Text = appSession.UserFullName;
            }
            #endregion

            base.InitializeReport(param);
        }
    }
}
