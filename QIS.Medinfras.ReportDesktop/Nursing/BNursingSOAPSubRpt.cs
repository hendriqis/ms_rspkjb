using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNursingSOAPSubRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public BNursingSOAPSubRpt()
        {
            InitializeComponent();
        }
        List<vNursingSOAPEvaluation> lstEntity = new List<vNursingSOAPEvaluation>();
        public void InitializeReport(string filterExpression)
        {
            lstEntity = BusinessLayer.GetvNursingSOAPEvaluationList(filterExpression);
            this.DataSource = lstEntity;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lstEntity.Count > 0)
            {
                if (GetCurrentColumnValue("SOAPGroup2").ToString() == "")
                    e.Cancel = true;
            }
        }

        private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lstEntity.Count > 0)
            {
                if (GetCurrentColumnValue("SOAPType").ToString() != Constant.NursingEvaluation.ASSESSMENT)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (Convert.ToBoolean(GetCurrentColumnValue("IsProblemSolved")))
                    {
                        xrLabel4.Text = "Masalah teratasi dengan indikator";
                    }
                    else
                    {
                        xrLabel4.Text = "Masalah belum teratasi dengan indikator";
                    }
                }
            }

        }

    }
}
