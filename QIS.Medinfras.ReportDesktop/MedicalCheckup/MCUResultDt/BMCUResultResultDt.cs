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
    public partial class BMCUResultResultDt : DevExpress.XtraReports.UI.XtraReport
    {
        public BMCUResultResultDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND Result != 0", visitID);

            filterExpression += filterExpression = string.Format(" AND ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ORDER BY CONVERT(INT,TagProperty) ASC",
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_6, //0
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_7, //1
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_8, //2
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_9, //3
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_10, //4
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_11, //5
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_12 //6
                                                );

            List<vMCUResult> lst = BusinessLayer.GetvMCUResultList(filterExpression);
            this.DataSource = lst;
        }
    }
}
