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
    public partial class LDaftarListOrderMedicalDiagnostic : BaseCustomDailyLandscapeRpt
    {
        public LDaftarListOrderMedicalDiagnostic()
        {
            InitializeComponent();
        }
        public override void InitializeReport (String[] param)
        {

            String Dept = "%%";
            String Medis = "%%";
            String ParamType = "";
            String Unit = "%%";

            if (param[0] != null && param[0] != "-" && param[0] != "")
            {
                Medis = param[0];

            }

            if (param[1] != null && param[1] != "-" && param[1] != "")
            {
                Dept = param[1];
              
            }
            if (param[2] != null && param[2] != "-" && param[2] != "")
            {
                Unit = param[2];
            }

            String Tgl = param[3];

            String Type = "";
            if (param[4] != null && param[4] != "-" && param[4] != "")
            {
                Type = param[4];
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

            if (Tgl == "-")
            {
                string filter = string.Format("HealthcareServiceUnitID LIKE '{0}' AND DepartmentID LIKE '{1}' AND ServiceUnitCode LIKE '{2}' {3}",Medis, Dept, Unit, temp);
                List<vTestOrderHdVisit> entity = BusinessLayer.GetvTestOrderHdVisitList(filter);
                this.DataSource = entity;
            }
            else
            {                 
                DateTime selectedDate = Helper.GetDatePickerValue(Tgl);
                string filter = string.Format(" HealthcareServiceUnitID LIKE '{0}' AND DepartmentID LIKE '{1}' AND ServiceUnitCode LIKE '{2}' AND ScheduledDate = '{3}' {4}", Medis, Dept, Unit, selectedDate, temp);
                List<vTestOrderHdVisit> entity = BusinessLayer.GetvTestOrderHdVisitList(filter);
                this.DataSource = entity;
            }

            lblParam.Text = Helper.GetDatePickerValue(Tgl).ToString(Constant.FormatString.DATE_FORMAT);

            IsSkipBinding();
            base.InitializeReport(param);       
        }

        protected override bool IsSkipBinding()
        {
            return true;
        }
    
    }
}
