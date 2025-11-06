using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryAllergyRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryAllergyRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int MRN)
        {
            List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", MRN));
            this.DataSource = lstAllergy;
        }

    }
}
