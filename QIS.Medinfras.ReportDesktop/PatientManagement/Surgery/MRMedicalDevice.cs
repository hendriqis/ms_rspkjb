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
    public partial class MRMedicalDevice : DevExpress.XtraReports.UI.XtraReport
    {
        public MRMedicalDevice()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int TestOrderID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND TestOrderID = '{1}' AND IsDeleted = 0 ORDER BY ID", VisitID, TestOrderID);
            List<vPatientMedicalDevice> lstPmd = BusinessLayer.GetvPatientMedicalDeviceList(filterExpression);

            if (lstPmd.Count > 0)
            {
                xrLabel1.Text = string.Format("Tanggal Pemasangan");
                xrLabel2.Text = string.Format(":");
                xrLabel4.Text = string.Format("Nama Item");
                xrLabel5.Text = string.Format(":");
                xrLabel6.Text = string.Format("Serial Number");
                xrLabel7.Text = string.Format(":");

                var lst = (from p in lstPmd
                           select new
                           {
                               ItemName = p.ItemWithCode,
                               SerialNumber = p.SerialNumber,
                               ImplantDate = p.cfImplantDate
                           }).ToList();

                this.DataSource = lst;
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel4.Visible = false;
                xrLabel5.Visible = false;
                xrLabel6.Visible = false;
                xrLabel7.Visible = false;
                this.Visible = false;
            }
        }
    }
}
