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
    public partial class EPrescription2RSSES : BaseCustomPharmacyA5Rpt
    {
        public EPrescription2RSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            GetPrescriptionOrderDtCustom entity = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0])).FirstOrDefault();

            cPrescriber.Text = entity.PrescriptionParamedicName;
            if (entity.GCPrescriptionParamedicMasterType == Constant.ParamedicType.Physician)
            {
                if (entity.PrescriptionLicenseNo == null || entity.PrescriptionLicenseNo == "")
                    cLicenseNo.Text = "-";
                else
                    cLicenseNo.Text = entity.PrescriptionLicenseNo;
            }
            else if (entity.GCPrescriptionParamedicMasterType == Constant.ParamedicType.Bidan)
            {
                if (entity.PrescriptionLicenseNo == null || entity.PrescriptionLicenseNo == "")
                {
                    cSIPNo.Text = "SIPB No";
                    cLicenseNo.Text = "-";
                }
                else
                {
                    cSIPNo.Text = "SIPB No";
                    cLicenseNo.Text = entity.PrescriptionLicenseNo;
                }
            }
            else
            {
                xrTableRow13.Visible = false;
            }

            cRegistrationNo.Text = string.Format("{0} | {1} {2}", entity.RegistrationNo, entity.ActualVisitDateInString, entity.ActualVisitTime);
            cPatient.Text = string.Format("{0} | {1} | {2}", entity.PatientName, entity.MedicalNo, entity.Gender);
            cDOB.Text = string.Format("{0} ( {1} yr {2} mnth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);

            lblTTD.Text = entity.City + ", " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            List<vRegistrationPayer> lstRegPayer = BusinessLayer.GetvRegistrationPayerList(string.Format("RegistrationID='{0}'  AND IsDeleted=0", entity.RegistrationID));
            string payer = string.Empty;
            if (lstRegPayer.Count > 0) 
            {
                if (lstRegPayer.Count == 1)
                {
                    payer = string.Format("{0}", lstRegPayer.FirstOrDefault().BusinessPartnerName);
                }
                else 
                {
                    foreach (vRegistrationPayer rowdata in lstRegPayer)
                    {
                        payer += string.Format("{0} ,", rowdata.CustomerType);
                    }

                    if (!string.IsNullOrEmpty(payer))
                    {
                        payer = payer.Remove(payer.Length - 1, 1);
                    }
                }
            }
            else
            {
                payer = entity.BusinessPartnerName;
            }

            cPenjaminBayar.Text = payer;
            lblTransactionNo.Text = entity.TransactionNo;

            string alergiObat = string.Empty;
            string WEIGHT = string.Empty;
            if (!string.IsNullOrEmpty(entity.MedicalNo))
            {
                Patient oPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo='{0}'", entity.MedicalNo)).FirstOrDefault();
                if (oPatient != null)
                {
                    List<PatientAllergy> lstPatientAlergey = BusinessLayer.GetPatientAllergyList(string.Format("GCAllergenType = '{0}' AND MRN='{1}' AND IsDeleted=0", Constant.AllergenType.DRUG, oPatient.MRN));

                    if (lstPatientAlergey.Count > 0)
                    {
                        foreach (PatientAllergy row in lstPatientAlergey)
                        {
                            alergiObat += string.Format("{0} ,", row.Allergen);
                        }

                        alergiObat = alergiObat.Remove(alergiObat.Length - 1, 1);
                    }

                    vVitalSignDt vt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID='{0}' AND VitalSignLabel='WEIGHT' and IsDeleted=0 order by ID DESC", entity.VisitID)).FirstOrDefault();
                    if (vt != null)
                    {
                        WEIGHT = string.Format("{0} {1}", vt.VitalSignValue, vt.ValueUnit);
                    }

                }

            }
            lblWEIGHT.Text = WEIGHT;
            lblMedicineAllergy.Text = alergiObat;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }

        private void lblSignaLabel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string consumeMethod = "";
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

        private void lblMFLA_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String DosingUnit = Convert.ToString(GetCurrentColumnValue("DosingUnit"));
            String DispenseQty = Convert.ToString(GetCurrentColumnValue("DispenseQty"));

            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (IsCompound)
            {
                lblMFLA.Text = string.Format("mf la {0} dtd No. {1}", DosingUnit, DispenseQty);
            }
            else
            {
                lblMFLA.Text = "";
            }
        }

        private void lbldet_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //Decimal TakenQty = Convert.ToDecimal(GetCurrentColumnValue("ItterText"));
            Decimal TakenQty = Convert.ToDecimal(GetCurrentColumnValue("TakenQty"));

            if (TakenQty > 0)
            {
                lbldet.Text = string.Format("det {0}", TakenQty.ToString("G29"));
            }
            else
            {
                lbldet.Text = "nedet";
            }

            //String itterInfo = Convert.ToString(GetCurrentColumnValue("ItterText"));
            //lbldet.Text = itterInfo;
        }
    }
}
