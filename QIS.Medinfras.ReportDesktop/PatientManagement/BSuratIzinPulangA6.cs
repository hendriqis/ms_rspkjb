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
    public partial class BSuratIzinPulangA6 : BaseCustomA6Rpt
    {
        private int lineNumber = 0;
        private int detailID = 0;
        private int oldDetailID = 0;
        private decimal totalAmount = 0;

        public BSuratIzinPulangA6()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Registration entity = BusinessLayer.GetRegistrationList(param[0])[0];
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
            cHealthcareName.Text = oHealthcare.HealthcareName;
            cHealthcareAddress.Text = string.Format("{0} {1} {2}", oHealthcare.StreetName, oHealthcare.City, oHealthcare.ZipCode);

            lblNamaPasien.Text = entityP.PatientName;
            lblNoRM.Text = entityP.MedicalNo;
            lblNoReg.Text = entity.RegistrationNo;
            lblServiceUnit.Text = entityVisit.ServiceUnitName;

            lblPenanggungJawab.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

       
    }
}
