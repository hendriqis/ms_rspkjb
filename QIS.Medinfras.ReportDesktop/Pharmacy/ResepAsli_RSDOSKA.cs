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
    public partial class ResepAsli_RSDOSKA : BaseCustomPharmacyA6v4Rpt
    {
        public ResepAsli_RSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom8 entity = BusinessLayer.GetPrescriptionOrderDtCustom8List(Convert.ToInt32(param[0])).FirstOrDefault();

            if (entity != null)
            {
                string[] temp = entity.ReferenceNo.Split('|');

                if (temp.Count() > 1)
                {
                    if (temp[0] != null || temp[1] != null)
                    {
                        cAntrian.Text = string.Format("Nomor Itter : {0} Nomor Antrian : {1}", temp[0], temp[1]);
                    }
                }

                cPrescriber.Text = string.Format("{0}", entity.PrescriptionParamedicName);
                cRegistrationNo.Text = string.Format("{0} | {1} {2}", entity.RegistrationNo, entity.ActualVisitDateInString, entity.ActualVisitTime);
                cPatient.Text = string.Format("{0} | {1} | {2}", entity.PatientName, entity.MedicalNo, entity.Gender);
                cDOB.Text = string.Format("{0} ( {1} yr {2} mnth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                lblTTD.Text = entity.City + ", " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
                if (string.IsNullOrEmpty(entity.NoSEP))
                {
                    cPenjamin.Text = entity.BusinessPartnerName;
                }
                else
                {
                    cPenjamin.Text = string.Format("{0} ({1})", entity.BusinessPartnerName, entity.NoSEP);
                }
                ttdName.Text = entity.PrescriptionParamedicName;
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
                        cSIPNo.Text = "SIP No";
                        cLicenseNo.Text = "-";
                    }
                    else
                    {
                        cSIPNo.Text = "SIP No";
                        cLicenseNo.Text = entity.PrescriptionLicenseNo;
                    }
                }
                else
                {
                    xrTableRow13.Visible = false;
                }

                List<PatientAllergy> entityAllergyLst = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0 AND GCAllergenType = '{1}'", entity.MRN, Constant.AllergenType.DRUG));

                if (entityAllergyLst.Count() > 0)
                {
                    cAllergy.Text = entityAllergyLst.FirstOrDefault().Allergen;
                }
                else
                {
                    cAllergy.Text = "Tidak Ada";
                }

                cDiagnose.Text = entity.PatientDiagnosis;

                cServisUnitName.Text = string.Format("{0} | {1} {2}", entity.ServiceUnitName, entity.cfDateInString, entity.cfRegistrationtTimeInString);
                cPrescription.Text = entity.JenisResep;
                cPrescriptionNo.Text = entity.TransactionNo;
                string Picture = string.Format("{0}{1}{2}.png", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicSignaturePath, entity.PrescriptionParamedicCode);

                ttdParamedic.ImageUrl = Picture;
            }

            base.InitializeReport(param);
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }

        private void lblTest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String cfSignaToReport = Convert.ToString(GetCurrentColumnValue("cfSignaToReport"));
            String MedicationRoute = Convert.ToString(GetCurrentColumnValue("MedicationRoute"));
            String cfIsAsRequired = Convert.ToString(GetCurrentColumnValue("cfIsAsRequired"));
            String MedicationAdministration = Convert.ToString(GetCurrentColumnValue("MedicationAdministration"));
            String MedicationPurpose = Convert.ToString(GetCurrentColumnValue("MedicationPurpose"));
            String DosingUnit = Convert.ToString(GetCurrentColumnValue("DosingUnit"));
            String DispenseQty = Convert.ToString(GetCurrentColumnValue("DispenseQty"));
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));

            string textFull = "";

            if (cfSignaToReport != null && cfSignaToReport != "")
            {
                textFull += string.Format("{0}", cfSignaToReport);
            }

            if (MedicationRoute != null && MedicationRoute != "")
            {
                textFull += string.Format(" {0}", MedicationRoute);
            }

            if (cfIsAsRequired != null && cfIsAsRequired != "")
            {
                textFull += string.Format("\n{0}", cfIsAsRequired);
            }

            if (MedicationAdministration != null && MedicationAdministration != "")
            {
                textFull += string.Format("\n{0}", MedicationAdministration);
            }

            if (MedicationPurpose != null && MedicationPurpose != "")
            {
                textFull += string.Format(" {0}", MedicationPurpose);
            }

            if (IsCompound)
            {
                textFull += string.Format("\nmf la {0} dtd No. {1}", DosingUnit, DispenseQty);
            }

            lblGroupFooter1.Text = textFull;
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsLASA = Convert.ToBoolean(GetCurrentColumnValue("IsLASA"));
            Boolean IsHAM = Convert.ToBoolean(GetCurrentColumnValue("IsHAM"));

            if (IsHAM)
            {
                xrPictureBox1.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Status/ham.png");
                xrPictureBox1.Visible = true;
            }
            else
            {
                xrPictureBox1.Visible = false;
            }

            if (IsLASA)
            {
                xrPictureBox2.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/Status/lasa.png");
                xrPictureBox2.Visible = true;
            }
            else
            {
                xrPictureBox2.Visible = false;
            }
        }

    }
}
