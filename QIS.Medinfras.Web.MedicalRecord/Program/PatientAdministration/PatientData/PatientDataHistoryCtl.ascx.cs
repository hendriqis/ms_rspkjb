using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PatientDataHistoryCtl : BaseViewPopupCtl
    {
        public class CPatient
        {
            public String FullName  { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string CustomDateOfBirth
            {
                get
                {
                    if (DateOfBirth.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
                        return DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
                    else return DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
                }
            }
        }

        public class CPatientHistory
        {
            public String UserName { get; set; }
            public String FullName { get; set; }
            public DateTime LogDate { get; set; }
            public CPatient Old { get; set; }
            public CPatient New { get; set; }
            public string CustomLogDate
            {
                get
                {
                    if (LogDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
                        return DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_6);
                    else return LogDate.ToString(Constant.FormatString.DATE_TIME_FORMAT_6);
                }
            }
        }

        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnFilterExpressionCtl.Value = param.ToString();
            Patient entityPatient = BusinessLayer.GetPatient(Convert.ToInt32(param));
            txtPatientName.Text = String.Format("{0}", entityPatient.FullName);
            BindGridView(1, true, ref PageCount);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0}", hdnFilterExpressionCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAuditLogHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPatientAuditLogHistory> lstEntity = BusinessLayer.GetvPatientAuditLogHistoryList(filterExpression, 8, pageIndex, "ID ASC");
            List<CPatientHistory> lstPatientHistory = new List<CPatientHistory>();
            foreach (vPatientAuditLogHistory entity in lstEntity)
            {
                CPatient oldPatient = JsonConvert.DeserializeObject<CPatient>(entity.OldValues);
                CPatient newPatient = JsonConvert.DeserializeObject<CPatient>(entity.NewValues);
                CPatientHistory entityPatientHistory = new CPatientHistory();
                entityPatientHistory.Old = oldPatient;
                entityPatientHistory.New = newPatient;
                entityPatientHistory.UserName = entity.UserName;
                entityPatientHistory.FullName = entity.FullName;
                entityPatientHistory.LogDate = entity.LogDate;
                lstPatientHistory.Add(entityPatientHistory);
            }

            lvwDetail.DataSource = lstPatientHistory;
            lvwDetail.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}