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
    public partial class BSuratKeteranganRawatInapRSCK : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganRawatInapRSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Registration entity = BusinessLayer.GetRegistrationList(param[0])[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare entityH1 = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];

            lblName.Text = entityP.PatientName;
            lblAge.Text = string.Format("{0} Tahun {1} Bulan", entityP.AgeInYear, entityP.AgeInMonth);
            lblEmploye.Text = entityP.Occupation;
            lblAddress.Text = entityP.StreetName;

            lblRegistrationDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);

            if (entityCV.DischargeDate != null && entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
            {
                lblDischargeDate.Text = entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                lblDischargeDate.Text = "";
            }

            lblPrintDate.Text = string.Format("{0}, {1}", entityH1.City, DateTime.Today.ToString(Constant.FormatString.DATE_FORMAT));
            lblHealthcareName.Text = string.Format("{0}.", entityH1.HealthcareName);
            lblUserName.Text = string.Format("({0})", AppSession.UserLogin.UserFullName);
            
            base.InitializeReport(param);
        }

    }
}
