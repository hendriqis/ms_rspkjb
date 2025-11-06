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
    public partial class EPrescription_RSAJ : BaseCustomPharmacyA5Rpt
    {
        public EPrescription_RSAJ()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom entity = BusinessLayer.GetPrescriptionOrderDtCustomList(Convert.ToInt32(param[0])).FirstOrDefault();

            #region Header : Patient Detail
            cPrescriptionNo.Text = entity.TransactionNo + " | " + entity.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entity.TransactionTime;
            cRegistrationNo.Text = entity.RegistrationNo + " | " + entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entity.RegistrationTime;
            cPatientName.Text = entity.PatientName;
            if (entity.OldMedicalNo != "" && entity.OldMedicalNo != null)
            {
                cMedicalNo.Text = string.Format("{0} ({1})", entity.MedicalNo, entity.OldMedicalNo);
            }
            else
            {
                cMedicalNo.Text = string.Format("{0}", entity.MedicalNo);
            }
            cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            cGender.Text = entity.Gender;

            vParamedicMaster ppm = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicName = '{0}'", entity.PrescriptionParamedicName)).FirstOrDefault();
            cPrescriberPhysician.Text = entity.PrescriptionParamedicName + " | " + ppm.SpecialtyName;

            vParamedicMaster rpm = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicName = '{0}'", entity.ParamedicName)).FirstOrDefault();
            cRegisteredPhysician.Text = entity.ParamedicName + " | " + rpm.SpecialtyName;

            cServiceUnitClass.Text = string.Format("{0} | {1}", entity.ServiceUnitName, entity.ClassName);
            cRoomBed.Text = string.Format("{0} | {1}", entity.RoomName, entity.BedCode);
            #endregion

            #region Header : Per Page
            //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
            //    entity.PatientName, entity.Gender, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            //cHeaderRegistration.Text = entity.RegistrationNo;
            //cHeaderMedicalNo.Text = entity.MedicalNo;
            #endregion

            #region Footer
            //cTTDTakenBy.Text = entity.PatientName;
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

        private void xrTableCell47_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        //    e.Cancel = Convert.ToBoolean(GetCurrentColumnValue("IsRFlag")) == false;
        }

        private void xrTableCell17_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Decimal DispenseQty = Convert.ToDecimal(GetCurrentColumnValue("DispenseQtyInString"));
            Decimal TakenQty = Convert.ToDecimal(GetCurrentColumnValue("TakenQty"));

            if (TakenQty > 0)
            {
                xrTableCell17.Text = string.Format("No. {0} \r\n det {1}", DispenseQty, TakenQty.ToString("G29"));
            }
            else
            {
                xrTableCell17.Text = string.Format("No. {0} \r\n nedet", DispenseQty);
            }
        }

    }
}
