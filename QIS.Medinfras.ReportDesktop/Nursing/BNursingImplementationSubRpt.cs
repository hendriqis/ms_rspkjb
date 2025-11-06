using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNursingImplementationSubRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public BNursingImplementationSubRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(string filterExpression)
        {
            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(String.Format("{0} AND NursingTransactionInterventionDtID IS NOT NULL ORDER BY JournalDate, JournalTime", filterExpression));
            this.DataSource = lstEntity;
        }

    }
}
