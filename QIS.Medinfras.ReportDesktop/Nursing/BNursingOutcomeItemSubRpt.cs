using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNursingOutcomeItemSubRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public BNursingOutcomeItemSubRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(string filterExpression)
        {
            List<vNursingTransactionOutcomeDt> lstEntity = BusinessLayer.GetvNursingTransactionOutcomeDtList(filterExpression);
            this.DataSource = lstEntity;

            //lblNOCHeader.Text = String.Format("Setelah dilakukan tindakan keperawatan selama {0} {1}, masalah teratasi dengan kriteria:", GetCurrentColumnValue("NOCInterval"), GetCurrentColumnValue("NOCIntervalPeriod"));
            lblNOCHeader.Text = String.Format("Setelah dilakukan tindakan keperawatan selama {0} {1}", GetCurrentColumnValue("NOCInterval"), GetCurrentColumnValue("NOCIntervalPeriod"));
        }

    }
}
