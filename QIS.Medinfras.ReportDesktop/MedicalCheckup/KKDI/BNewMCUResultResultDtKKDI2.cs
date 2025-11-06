using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewMCUResultResultDtKKDI2 : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewMCUResultResultDtKKDI2()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND Result != 0", visitID);

            filterExpression += filterExpression = string.Format(" AND ParentID IN ('{0}') ORDER BY CONVERT(INT,TagProperty) ASC",
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_22
                                                );

            List<vMCUResult> lst = BusinessLayer.GetvMCUResultList(filterExpression);
            this.DataSource = lst;
        }
    }
}
