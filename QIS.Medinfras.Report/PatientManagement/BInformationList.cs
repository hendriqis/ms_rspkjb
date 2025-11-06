using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Report
{
    public partial class BInformationList : BaseDailyPortraitRpt
    {
        public BInformationList()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0]))[0];
            lblLastUpdatedBy.Text = entity.LastUpdatedByUserName;
            lblLastUpdatedDate.Text = entity.LastUpdatedDateInString;

            vPatientBill entityBill = BusinessLayer.GetvPatientBillList(string.Format(""))[0];
            lblNoBil.Text = entityBill.PatientBillingNo;


            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            lblRegistrationNo.Text = string.Format("{0} / {1}", entityReg.RegistrationNo, entity.TransactionNo);
            lblPayer.Text = entityReg.BusinessPartner;
            lblPatientName.Text = entityReg.PatientName;
            lblMedicalNo.Text = entityReg.MedicalNo;
            lblDateOfBirth.Text = entityReg.DateOfBirthInString;

            if (entity.DepartmentID == Constant.Facility.INPATIENT)
                lblServiceUnit.Text = GetLabel("Ruang Perawatan");
            else if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
                lblServiceUnit.Text = GetLabel("Klinik");
            else if (entity.DepartmentID == Constant.Facility.EMERGENCY)
                lblServiceUnit.Text = GetLabel("Unit Pelayanan");
            else
                lblServiceUnit.Text = GetLabel("Penunjang Medis");

            base.InitializeReport(param);
        }
    }
}
