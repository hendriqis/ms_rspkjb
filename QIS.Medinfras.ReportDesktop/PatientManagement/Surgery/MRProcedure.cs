using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRProcedure : DevExpress.XtraReports.UI.XtraReport
    {
        public MRProcedure()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int PatientSurgeryID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND PatientSurgeryID = '{1}' AND IsDeleted = 0 ORDER BY ID", VisitID, PatientSurgeryID);
            List<vPatientSurgeryProcedureGroup> lstPst = BusinessLayer.GetvPatientSurgeryProcedureGroupList(filterExpression);

            if (lstPst.Count > 0)
            {
                xrLabel1.Text = string.Format("Jenis Operasi");
                xrLabel2.Text = string.Format(":");
                xrLabel4.Text = string.Format("Kategori Operasi");
                xrLabel5.Text = string.Format(":");
                xrLabel3.Text = "Catatan Jenis Operasi";
                xrLabel6.Text = ":";

                var lst = (from p in lstPst
                           select new
                           {
                               ProcedureGroupName = p.ProcedureGroupName,
                               SurgeryClassification = p.SurgeryClassification
                           }).ToList();

                this.DataSource = lst;

                vPatientSurgeryProcedureGroup obj = lstPst.FirstOrDefault();
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.ProcedureGroupRemarks))
                    {
                        this.ReportFooter.Visible = true;
                          lblProcedureGroupRemarks.Text = obj.ProcedureGroupRemarks;
                    }
                    else
                    {
                        //xrLabel3.Visible = false;
                        //xrLabel6.Visible = false;
                        this.ReportFooter.Visible = false;
                    }
                }
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                xrLabel5.Visible = false;
                xrLabel6.Visible = false;
                this.Visible = false;
            }
        }
    }
}
