using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNursingDiagnoseItemSubRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public BNursingDiagnoseItemSubRpt()
        {
            InitializeComponent();
            
        }
        List<vNursingTransactionDt> lstEntity = new List<vNursingTransactionDt>();
        public void InitializeReport(string filterExpression)
        {
            lstEntity = BusinessLayer.GetvNursingTransactionDtList(String.Format("{0} AND IsShowNursingItemGroupSubGroup = 1", filterExpression));
            this.DataSource = lstEntity;

            if (lstEntity.Count > 0)
            {
                lblDiagnoseName.Text = GetCurrentColumnValue("NursingDiagnoseName").ToString();
            }
            else
            {
                lblDiagnoseName.Text = string.Empty;
            }
            lblItemSubGroup.BeforePrint += new System.Drawing.Printing.PrintEventHandler(lblItemSubGroup_BeforePrint);
        }

        protected void lblItemSubGroup_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lstEntity.Count > 0)
            {
                string temp = GetCurrentColumnValue("NursingItemGroupSubGroupText").ToString();
                if (temp.Trim() == "Keterangan")
                    e.Cancel = true;
            }
        }

    }
}
