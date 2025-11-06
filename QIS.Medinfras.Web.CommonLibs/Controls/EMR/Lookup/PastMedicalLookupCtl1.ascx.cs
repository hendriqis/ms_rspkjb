using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PastMedicalLookupCtl1 : BaseProcessPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTemplateGroup.Value = paramInfo[0];

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = string.Format("MRN = {0} AND DepartmentID IN  ('{2}','{3}','{4}') AND VisitID != {1}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID,Constant.Facility.EMERGENCY,Constant.Facility.OUTPATIENT,Constant.Facility.INPATIENT);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreviousMedicalHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreviousMedicalHistory> lstEntity = BusinessLayer.GetvPreviousMedicalHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "VisitDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            retval = hdnSelectedItem.Value;
            return result;
        }
    }
}