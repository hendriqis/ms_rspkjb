using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRPlanningNotesRSUKI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRPlanningNotesRSUKI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", visitID));
            this.DataSource = lstChiefComplaint;
        }
    }
}
