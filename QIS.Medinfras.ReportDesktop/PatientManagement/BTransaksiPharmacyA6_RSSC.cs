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
    public partial class BTransaksiPharmacyA6_RSSC : BaseA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;
        private int isCompound = 0;

        public BTransaksiPharmacyA6_RSSC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            GetPrescriptionOrderDtCustom1 entity = BusinessLayer.GetPrescriptionOrderDtCustom1List(Convert.ToInt32(param[0])).FirstOrDefault();
            PatientChargesHd entityPcd = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(param[0]));
            GetPrescriptionPrice entityPrice = BusinessLayer.GetPrescriptionPrice(Convert.ToInt32(param[0]), Convert.ToInt32(entityPcd.PrescriptionOrderID)).FirstOrDefault();
            vPatientChargesHd entityVp = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            lblPatientInfo.Text =string.Format("{0} / {1}", entity.PatientName, entity.cfGenderInitial);
            lblRegistrationInfo.Text = string.Format("{0} / {1}", entity.RegistrationNo, entity.MedicalNo);
            lblBusinessPartnerName.Text = entity.BusinessPartnerName;
            lblTglLahir.Text = string.Format("{0} / ({1}yr)", entity.DateOfBirthInString, entity.AgeInYear);
            lblPatientLocation.Text = string.Format("{0} {1}", entity.ServiceUnitName, entity.BedCode);
            lblPhysicianName.Text = entity.PrescriptionParamedicName;
            lblServiceUnit.Text = string.Format("{0}({1} {2})", entity.LocationName, entity.cfDateInString, entity.cfRegistrationtTimeInString);
            lblPreception.Text = entity.JenisResep;
            cTransaction.Text = entity.TransactionNo;

            List<GetPrescriptionOrderDtCustom1> entityC = BusinessLayer.GetPrescriptionOrderDtCustom1List(Convert.ToInt32(param[0]));
            foreach (GetPrescriptionOrderDtCustom1 e in entityC)
            {
                if (e.IsCompound)
                {
                    isCompound += 1;
                }
            }

            if (entityVp.LastUpdatedBy != 0 && entityVp.LastUpdatedBy != null)
            {
                lblLastUpdatedBy.Text = BusinessLayer.GetUserAttribute(entityVp.LastUpdatedBy).FullName;
            }
            else
            {
                lblLastUpdatedBy.Text = BusinessLayer.GetUserAttribute(entityVp.CreatedBy).FullName;
            }
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entityVp.cfDateForSignInString);

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
    }
}
