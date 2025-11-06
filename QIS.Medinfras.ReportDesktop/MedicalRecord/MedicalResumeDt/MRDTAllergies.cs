using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRDTAllergies : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDTAllergies()
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
