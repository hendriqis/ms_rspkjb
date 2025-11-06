using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRReferralRSBL : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReferralRSBL()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            List<vPatientReferral> lstPR = BusinessLayer.GetvPatientReferralList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID));

            var lst = (from p in lstPR
                       select new
                       {
                           ParamedicName = p.ToPhysicianName,
                           PatientReferralDts = lstPR.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "PatientReferralDts";
        }
    }
}
