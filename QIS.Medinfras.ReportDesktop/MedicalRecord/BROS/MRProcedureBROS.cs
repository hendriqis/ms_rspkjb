using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRProcedureBROS : DevExpress.XtraReports.UI.XtraReport
    {
        public MRProcedureBROS()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            PatientProcedure entityP = BusinessLayer.GetPatientProcedureList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();

            lblTindakan.Text = string.Format("{0} ({1})", entityP.ProcedureText, entityP.ProcedureID);
        }
    }
}
