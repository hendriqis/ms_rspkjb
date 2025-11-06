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
    public partial class ProceduresSurgeryDt : DevExpress.XtraReports.UI.XtraReport
    {
        public ProceduresSurgeryDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            vPatientProcedure entity = BusinessLayer.GetvPatientProcedureList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            if (entity != null)
            {

                if (!string.IsNullOrEmpty(entity.ProcedureID))
                {
                    if (entity.ProcedureName != null)
                    {
                        lblTindakan.Text = string.Format("{0}", entity.ProcedureName);
                    }
                    else
                    {
                        lblTindakan.Text = string.Format("{0}", entity.ProcedureText);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(entity.FinalProcedureID))
                    {
                        lblTindakan.Text = string.Format("{0}", entity.FinalProcedureName);
                    }
                    else
                    {
                        lblTindakan.Text = string.Format("{0}", entity.FinalProcedureText);
                    }
                }
            }
            else
            {
                lblTindakan.Visible = false;
            }
        }
    }
}
