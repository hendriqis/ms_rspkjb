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
    public partial class LDaftarListOrder : BaseCustomDailyLandscapeRpt
    {
        public LDaftarListOrder()
        {
            InitializeComponent();
        }
        public override void InitializeReport (String[] param)
        {

            String Dept = "%%";
            String ParamType = "";
            if (param[0] != null && param[0] != "-" && param[0] != "")
            {
                Dept = param[0];
              
            }

            String Unit = "%%";
            if (param[1] != null && param[1] != "-" && param[1] != "")
            {
                Unit = param[1];
            }
            String Tgl = param[2];

            String Type = "";
            if (param[3] != null && param[3] != "-" && param[3] != "")
            {
                Type = param[3];
            }

            String temp = "";
            if (Type != "")
            {
                ParamType += " AND ";

                ParamType += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);

                if (Type == "0")
                {
                    ParamType += string.Format(" AND GCTransactionStatus IN ('{0}','{1}','{2}')",
                        Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                }
                else if (Type == "1")
                {
                    ParamType += string.Format(" AND GCTransactionStatus = '{0}'",
                        Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                }
                else if (Type == "2")
                {
                    ParamType += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')",
                        Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
                }

                temp = ParamType;

            }
             
            vSettingParameterDt entityVS= BusinessLayer.GetvSettingParameterDtList(String.Format("ParameterCode = '{0}'" , Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM)).FirstOrDefault(); 

            if (Tgl == "-")
            {
                string filter = string.Format("DepartmentID LIKE '{0}' AND ServiceUnitCode LIKE '{1}' AND HealthcareServiceUnitID = {2} {3} ", Dept, Unit, entityVS.ParameterValue, temp);
                List<vTestOrderHdVisit> entity = BusinessLayer.GetvTestOrderHdVisitList(filter);
                this.DataSource = entity;
            }
            else
            {
                DateTime selectedDate = Helper.GetDatePickerValue(Tgl);
                string filter = string.Format("DepartmentID LIKE '{0}' AND ServiceUnitCode LIKE '{1}' AND ScheduledDate = '{2}'  AND HealthcareServiceUnitID = {3} {4}", Dept, Unit, selectedDate, entityVS.ParameterValue, temp);
                List<vTestOrderHdVisit> entity = BusinessLayer.GetvTestOrderHdVisitList(filter);
                this.DataSource = entity;
            }

            lblParam.Text = Helper.GetDatePickerValue(Tgl).ToString(Constant.FormatString.DATE_FORMAT);

            IsSkipBinding();
            base.InitializeReport(param); 
        }

        protected override bool IsSkipBinding() {
            return true;
        }

    }
}
