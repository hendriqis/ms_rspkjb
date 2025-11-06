using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKajianIGD_ResikoJatuh_RSDOSKA : DevExpress.XtraReports.UI.XtraReport
    {
        public BKajianIGD_ResikoJatuh_RSDOSKA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            List<Registration> entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", RegistrationID));
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            vVitalSignDt entityVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            if (entityVitalSignDt != null)
            {
                xrLabel2.Text = string.Format("Skala Nyeri : {0}", entityVitalSignDt.VitalSignValue);
            }
            else
            {
                xrLabel2.Text = string.Empty;
            }

            this.DataSource = entityRegistration;
        }
    }
}
