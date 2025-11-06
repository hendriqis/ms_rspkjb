using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNursingInterventionItemSubRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public BNursingInterventionItemSubRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(string filterExpression)
        {
            List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);
            this.DataSource = lstEntity;
        }

    }
}
