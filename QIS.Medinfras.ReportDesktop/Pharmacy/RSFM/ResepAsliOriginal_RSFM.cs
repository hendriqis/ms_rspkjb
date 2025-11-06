using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class ResepAsliOriginal_RSFM : BaseDailyPortraitRpt
    {
        public ResepAsliOriginal_RSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom8 entity = BusinessLayer.GetPrescriptionOrderDtCustom8List(Convert.ToInt32(param[0])).FirstOrDefault();
            ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entity.PrescriptionParamedicID)).FirstOrDefault();

            if (entity != null)
            {
                string[] temp = entity.ReferenceNo.Split('|');

                cParamedic.Text = string.Format("{0}", entity.PrescriptionParamedicName);
                cNoSIP.Text = entityParamedic.LicenseNo;
                cRegistrationNo.Text = string.Format("{0} | {1} {2}", entity.RegistrationNo, entity.ActualVisitDateInString, entity.ActualVisitTime);
                cPatient.Text = string.Format("{0}", entity.PatientName);
                cGender.Text = entity.Gender;
                cMedicalNo.Text = entity.MedicalNo;
                cDOB.Text = string.Format("{0} ( {1} yr {2} mnth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                cPenjamin.Text = entity.BusinessPartnerName;

                cServiceUnitName.Text = string.Format("{0} | {1} {2}", entity.ServiceUnitName, entity.cfDateInString, entity.cfRegistrationtTimeInString);
                cPrescriptionType.Text = entity.JenisResep;
                cPrescriptionNo.Text = entity.TransactionNo;
                string Picture = string.Format("{0}{1}{2}.png", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISParamedicSignaturePath, entity.PrescriptionParamedicCode);

                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entity.PrescriptionParamedicCode);
                ttdDokter.Visible = true;

                base.InitializeReport(param);
            }
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
    }
}
