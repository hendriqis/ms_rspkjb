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
    public partial class EpisodeResiko : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeResiko()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            List<Registration> entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", RegistrationID));
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", entityCV.VisitID)).FirstOrDefault();

            this.DataSource = entityRegistration;
        }
    }
}
