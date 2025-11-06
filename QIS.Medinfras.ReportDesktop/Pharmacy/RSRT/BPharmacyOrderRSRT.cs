using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPharmacyOrderRSRT : BaseCustomPharmacyA5Rpt
    {
        public BPharmacyOrderRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom entity = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0])).FirstOrDefault();

            #region Header : Patient Detail
            cPrescriptionNo.Text = entity.TransactionNo + " | " + entity.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
            cOrderNo.Text = entity.PrescriptionOrderNo + " | " + entity.PrescriptionDateInString;
            cPatientName.Text = entity.PatientName;
            cRegistrationNo.Text = entity.RegistrationNo;
            cRegistrationDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);
            cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            cRegisteredPhysician.Text = entity.ParamedicName;
            cPrescriberPhysician.Text = entity.PrescriptionParamedicName;

            if (entity.OldMedicalNo != "" && entity.OldMedicalNo != null)
            {
                cMedicalNo.Text = string.Format("{0} ({1})", entity.MedicalNo, entity.OldMedicalNo);
            }
            else
            {
                cMedicalNo.Text = string.Format("{0}", entity.MedicalNo);
            }
            cServiceUnitClass.Text = string.Format("{0} | {1}", entity.ServiceUnitName, entity.ClassName);
            cRoomBed.Text = string.Format("{0} | {1}", entity.RoomName, entity.BedCode);
            cCorporate.Text = entity.BusinessPartnerName;
            cDischargeDate.Text = entity.DischargeDateInString;
            #endregion

            #region Header : Per Page
            //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
            //    entity.PatientName, entity.Gender, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            //cHeaderRegistration.Text = entity.RegistrationNo;
            //cHeaderMedicalNo.Text = entity.MedicalNo;
            #endregion

            #region Footer
            cTTDTakenBy.Text = entity.PatientName;
            //cTTDEtiketBy.Text = entityPresHD.ParamedicName;
            //cTTDCompoundBy.Text = entityPresHD.ParamedicName;
            //cTTDVerificationBy.Text = entityPresHD.ParamedicName;
            #endregion

            base.InitializeReport(param);
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }

        private void xrTableCell56_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsUseSweetener = Convert.ToBoolean(GetCurrentColumnValue("IsUseSweetener"));
            if (IsUseSweetener)
            {
                xrTableCell56.Text = "Yes";
            }
            else
            {
                xrTableCell56.Text = "No";
            }
        }

        private void xrTableCell47_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean isRFlag = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag"));
            if (!isRFlag) e.Cancel = true;
        }

    }
}
