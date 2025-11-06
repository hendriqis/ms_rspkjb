using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRSurgeryTeam : DevExpress.XtraReports.UI.XtraReport
    {
        public MRSurgeryTeam()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int PatientSurgeryID)
        {
            vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            string filterExpression = string.Format("VisitID IN ({0}) AND PatientSurgeryID = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", VisitID, PatientSurgeryID);
            List<vPatientSurgeryTeam> lstPst = BusinessLayer.GetvPatientSurgeryTeamList(filterExpression);

            if (lstPst.Count > 0)
            {
                xrLabel2.Text = string.Format(":");

                var lst = (from p in lstPst
                           select new
                           {
                               ParamedicName = p.ParamedicName,
                               ParamedicRole = p.ParamedicRole
                           }).ToList();

                this.DataSource = lst;
            }
            else
            {
                xrLabel2.Visible = false;
                this.Visible = false;
            }

            if (entity.ReferralSummary != null && entity.ReferralSummary != "")
            {
                xrLabel6.Text = string.Format("Konsultasi Intra Operatif");
                xrLabel7.Text = string.Format(":");
                lblReferralSummary.Text = string.Format("{0}", entity.ReferralSummary);
            }
            else
            {
                xrLabel6.Visible = false;
                xrLabel7.Visible = false;
                lblReferralSummary.Visible = false;
            }
        }
    }
}
