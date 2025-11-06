using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRReferral : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReferral()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vPatientReferral entityPR = BusinessLayer.GetvPatientReferralList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityPR.ToPhysicianID)).FirstOrDefault();

            lblToPhysician.Text = entityPM.FullName;
        }
    }
}
