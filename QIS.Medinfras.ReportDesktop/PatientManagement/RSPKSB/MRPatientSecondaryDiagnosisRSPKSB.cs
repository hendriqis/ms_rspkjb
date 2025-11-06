using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRPatientSecondaryDiagnosisRSPKSB : DevExpress.XtraReports.UI.XtraReport
    {
        public MRPatientSecondaryDiagnosisRSPKSB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            List<vPatientDiagnosis> entityD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", VisitID, Constant.DiagnoseType.COMPLICATION));

            if (entityD.Count() > 0)
            {
                var diagnosis = (from d in entityD
                                 select new
                                 {
                                     DiagnoseText = !string.IsNullOrEmpty(d.DiagnoseID) ? d.DiagnoseName : d.DiagnosisText,
                                     DiagnosisID = d.DiagnoseID
                                 }).ToList();

                this.DataSource = diagnosis;
            }
            else
            {
                List<String> empty = new List<String>();
                empty.AddRange(new List<String> {"", "" });

                this.tbDiagnoseText.Text = "";
                this.tbDiagnoseText.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
                this.tbDiagnoseText.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;

                this.tbDiagnosisID.Text = "";
                this.tbDiagnosisID.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
                this.tbDiagnosisID.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;

                this.DataSource = empty;
            }
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

    }
}
