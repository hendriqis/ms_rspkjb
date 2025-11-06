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
    public partial class BNewPatientBillDetailRSSBBComp2 : BaseCustomDailyPotraitRpt
    {
        public BNewPatientBillDetailRSSBBComp2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0]).FirstOrDefault();
            //Registration entityRegLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            Registration entityRegLinkedTo = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityReg.LinkedToRegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            List<GetPatientChargesHdDtAll2Comp2> lstHDDTComp2 = BusinessLayer.GetPatientChargesHdDtAll2Comp2List(entityReg.RegistrationID);

            string corporate = "";
            if (entityReg.BusinessPartnerID != 1)
            {
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

            cServiceUnitClass.Text = string.Format("{0} | {1}", entityReg.ServiceUnitName, entityReg.ClassName);
            cRoomBed.Text = string.Format("{0} | {1}", entityReg.RoomName, entityReg.BedCode);

            string planDischarge = entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string discharge = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            string dischargeDateInfo = "-";

            if (entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
            {
                dischargeDateInfo = discharge;
            }
            else
            {
                if (entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_FORMAT_112) != "19000101")
                {
                    dischargeDateInfo = planDischarge + "(*)";
                }
            }

            cDischargeDate.Text = dischargeDateInfo;

            if (entityRegLinkedTo != null)
            {
                cNotes.Text = string.Format("Transfer Rawat Inap ({0})", entityRegLinkedTo.RegistrationNo);
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

            #region Doctor Fee Information

            if (lstHDDTComp2.Count() > 0)
            {
                subTariffComp2.CanGrow = true;
                billPaymentDetailTransactionComp2RSSBB.InitializeReport(lstHDDTComp2);
            }
            else
            {
                lblTariffComp2Caption.Visible = false;
                subTariffComp2.Visible = false;
            }

            #endregion

            base.InitializeReport(param);
        }
    }
}
