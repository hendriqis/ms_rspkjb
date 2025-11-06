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
    public partial class BNewMCUResultResultDt : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewMCUResultResultDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND Result != 0", visitID);

            filterExpression += filterExpression = string.Format(" AND ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}') ORDER BY CONVERT(INT,TagProperty) ASC",
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_1, //0
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_2, //1
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_3, //2
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_4, //3
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_5, //4
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_6, //5
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_7, //6
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_8, //7
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_9, //8
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_10, //9
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_11, //10
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_12, //11
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_14, //12
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_15, //13
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_16, //14
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_17, //15
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_18, //16
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_19, //17
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_20, //18
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_21, //19
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_22, //20
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_23, //21
                                                    Constant.StandardCode.EXTRENAL_MCU_RESULT_24 //22
                                                );

            List<vMCUResult> lst = BusinessLayer.GetvMCUResultList(filterExpression);
            this.DataSource = lst;
        }
    }
}
