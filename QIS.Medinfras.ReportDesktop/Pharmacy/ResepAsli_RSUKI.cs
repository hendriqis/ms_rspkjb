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
    public partial class ResepAsli_RSUKI : BaseA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;
        private int isCompound = 0;

        public ResepAsli_RSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom5 entity = BusinessLayer.GetPrescriptionOrderDtCustom5List(Convert.ToInt32(param[0])).FirstOrDefault();
            PatientChargesHd entityPcd = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(param[0]));
            vPatientChargesHd entityVp = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vVitalSignDt VitalSignW = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'WEIGHT' AND IsDeleted = 0", entity.RegistrationID)).FirstOrDefault();

            List<PatientAllergy> entityAllergyLst = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0 AND GCAllergenType = '{1}'", entity.MRN, Constant.AllergenType.DRUG));

            if (entityAllergyLst.Count() > 0)
            {
                cAlergi.Text = entityAllergyLst.FirstOrDefault().Allergen;
            }
            else
            {
                cAlergi.Text = "Tidak Ada";
            }

            if (entity.GCPrescriptionParamedicMasterType == Constant.ParamedicType.Physician)
            {
                if (entity.PrescriptionLicenseNo == null || entity.PrescriptionLicenseNo == "")
                    lblSIPNo.Text = "-";
                else
                    lblSIPNo.Text = entity.PrescriptionLicenseNo;
            }
            else
            {
                lblSIPNo.Text = "-";
            }

            if (VitalSignW == null)
            {
                lblWeight.Text = string.Format("-");
            }
            else
            {
                lblWeight.Text = string.Format("{0} kg", VitalSignW.VitalSignValue);
            }


            pctReceiptLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            lblPatientInfo.Text =string.Format("{0}", entity.PatientName);
            lblRegistrationInfo.Text = string.Format("{0} / {1}", entity.RegistrationNo, entity.MedicalNo);
            lblBusinessPartnerName.Text = entity.BusinessPartnerName;
            lblTglLahir.Text = string.Format("{0} / ({1}yr)", entity.DateOfBirthInString, entity.AgeInYear);
            lblPatientLocation.Text = string.Format("{0} {1}", entity.ServiceUnitName, entity.BedCode);
            lblPhysicianName.Text = entity.PrescriptionParamedicName;
            lblServiceUnit.Text = string.Format("{0} | {1} {2}", entity.LocationName, entity.cfDateInString, entity.cfRegistrationtTimeInString);
            lblPreception.Text = entity.JenisResep;
            cTransaction.Text = entity.TransactionNo;

            if (entity.IsCorrectPatient == true)
            {
                cPx.Text = "V";
            }
            if (entity.IsCorrectMedication == true)
            {
                cOB.Text = "V";
            }
            if (entity.IsCorrectStrength == true)
            {
                cKE.Text = "V";
            }
            if (entity.IsCorrectFrequency == true)
            {
                cFRE.Text = "V";
            }
            if (entity.IsCorrectDosage == true)
            {
                cDO.Text = "V";
            }
            if (entity.IsCorrectRoute == true)
            {
                cRP.Text = "V";
            }
            if (entity.IsHasDrugInteraction == true)
            {
                cIO.Text = "V";
            }
            if (entity.IsHasDuplication == true)
            {
                cDUP.Text = "V";
            }
            if (entity.IsCorrectTimeOfGiving == true)
            {
                cWP.Text = "V";
            }

            List<GetPrescriptionOrderDtCustom1> entityC = BusinessLayer.GetPrescriptionOrderDtCustom1List(Convert.ToInt32(param[0]));
            foreach (GetPrescriptionOrderDtCustom1 e in entityC)
            {
                if (e.IsCompound)
                {
                    isCompound += 1;
                }
            }
            base.InitializeReport(param);
        }

        private void cIsRFlag_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void lblNotesCaption_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            if (isCompound > 0)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void cTakenQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cDispenseQty_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void cItemName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ItemName = Convert.ToString(GetCurrentColumnValue("ItemName"));
            String CompoundQtyInString = Convert.ToString(GetCurrentColumnValue("CompoundQtyInString"));
            String CompoundUnit = Convert.ToString(GetCurrentColumnValue("CompoundUnit"));
            String ChargeQty = Convert.ToDecimal(GetCurrentColumnValue("ChargeQty")).ToString("G29");
            String ResultQty = Convert.ToDecimal(GetCurrentColumnValue("ResultQty")).ToString("G29");
            String ChargedQuantity = Convert.ToDecimal(GetCurrentColumnValue("ChargedQuantity")).ToString("G29");
            String CoenamRule = Convert.ToString(GetCurrentColumnValue("CoenamRule"));
            String SignaLabel = Convert.ToString(GetCurrentColumnValue("SignaLabel"));
            String BaseUnit = Convert.ToString(GetCurrentColumnValue("BaseUnit"));
            String DosingUnit = Convert.ToString(GetCurrentColumnValue("DosingUnit"));
            String ItemUnit = Convert.ToString(GetCurrentColumnValue("ItemUnit"));
            
            if (CompoundQtyInString == "")
            {
                if (CoenamRule != "" && SignaLabel != "")
                {
                    cItemName.Text = string.Format("{0} {1} {2} ({3} {4})", ItemName, ChargeQty, ResultQty, CoenamRule, SignaLabel);
                }
                else
                {
                    cItemName.Text = string.Format("{0}", ItemName);
                }
            }
            else
            {
                if (CoenamRule != "" && SignaLabel != "")
                {
                    cItemName.Text = string.Format("{0} ({1} {2}) {3} {4} ({5} {6})",
                            ItemName, ChargeQty, CompoundQtyInString, CompoundUnit, ResultQty, CoenamRule, SignaLabel);
                }
                else
                {
                    cItemName.Text = string.Format("{0} ({1} {2} - {3} {4})",
                           ItemName, CompoundQtyInString, CompoundUnit, ChargeQty, ItemUnit);
                }

            }
        }

        private void cFlagCaption_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
            e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void lblSignaLabel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String consumeMethod = "";
            switch (GetCurrentColumnValue("GCDosingFrequency").ToString())
            {
                case Constant.DosingFrequency.DAY:
                    consumeMethod = string.Format("{0} x sehari {1} {2} {3}", Convert.ToDecimal(GetCurrentColumnValue("Frequency")).ToString("G29"), Convert.ToDecimal(GetCurrentColumnValue("NumberOfDosage")).ToString("G29"), GetCurrentColumnValue("DosingUnit"), GetCurrentColumnValue("CoenamRule"));
                    break;
                case Constant.DosingFrequency.WEEK:
                    consumeMethod = string.Format("{0} x seminggu {1} {2} {3}", Convert.ToDecimal(GetCurrentColumnValue("Frequency")).ToString("G29"), Convert.ToDecimal(GetCurrentColumnValue("NumberOfDosage")).ToString("G29"), GetCurrentColumnValue("DosingUnit"), GetCurrentColumnValue("CoenamRule"));
                    break;
                case Constant.DosingFrequency.HOUR:
                    consumeMethod = string.Format("setiap {0} jam {1} {2} {3}", Convert.ToDecimal(GetCurrentColumnValue("Frequency")).ToString("G29"), Convert.ToDecimal(GetCurrentColumnValue("NumberOfDosage")).ToString("G29"), GetCurrentColumnValue("DosingUnit"), GetCurrentColumnValue("CoenamRule"));
                    break;
                default:
                    break;
            }
            lblSignaLabel.Text = consumeMethod;
        }
    }
}
