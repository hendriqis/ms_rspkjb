using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransaksiRawatJalan : BaseDailyPortraitRpt
    {
        public BTransaksiRawatJalan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            lblRegistrationNo.Text = entity.RegistrationNo;
            lblPayer.Text = entity.BusinessPartner;
            lblPatientName.Text = entity.PatientName;
            lblMedicalNo.Text = entity.MedicalNo;
            lblDateOfBirth.Text = entity.DateOfBirthInString;
            lblLastUpdatedBy.Text = appSession.UserFullName;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);

            switch (entity.DepartmentID)
            {
                case Constant.Facility.OUTPATIENT: lblServiceUnit.Text = "Clinic"; break;
                case Constant.Facility.INPATIENT: lblServiceUnit.Text = "Ward"; break;
            }

            base.InitializeReport(param);
        }
    }
}
