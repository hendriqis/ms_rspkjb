using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BRekapRevenueSharingAdjPlus : DevExpress.XtraReports.UI.XtraReport
    {
        public BRekapRevenueSharingAdjPlus()
        {
            InitializeComponent();
        }

        public void InitializeReport(string param)
        {
            string filterExpression = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND GCRSAdjustmentGroup = '{1}'",
                                            AppSession.ParamedicID, Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN);

            filterExpression += string.Format(" AND RSTransactionID IN (SELECT RSTransactionID FROM TransRevenueSharingHd WHERE GCTransactionStatus != '{0}' AND {1})",
                                            Constant.TransactionStatus.VOID, param);

            List<vTransRevenueSharingAdj> lst = BusinessLayer.GetvTransRevenueSharingAdjList(filterExpression);

            this.DataSource = lst;
        }

    }
}
