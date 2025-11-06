using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodePemeriksaanFisik : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodePemeriksaanFisik()
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
