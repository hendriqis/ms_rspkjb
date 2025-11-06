using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class StandardCodeList : BasePageList
    {
        protected int PageCount = 1;
        protected int PageCount1 = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.STANDARD_CODE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            BindGridView(1, true, ref PageCount);
            BindGridView1(1, true, ref PageCount1);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Standard Code ID", "Standard Code Name" };
            fieldListValue = new string[] { "StandardCodeID", "StandardCodeName" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsDeleted = 0 AND IsHeader = 1";

            if (!chkIsShowAll.Checked)
            {
                filterExpression += " AND IsEditableByUser = 1";
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetStandardCodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex,"StandardCodeID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("IsDeleted = 0 AND ParentID = '{0}'",hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetStandardCodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex,"StandardCodeID");
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();
        }

        protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            if (hdnIsEditableByUser.Value == "1")
            {
                url = ResolveUrl(String.Format("~/Program/ControlPanel/StandardCode/StandardCodeEntry.aspx?par={0}", hdnID.Value));
                return true;
            }
            else
            {
                errMessage = "Cannot Add Data To This Record";
                return false;
            }
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnIsEditableByUser.Value == "1")
            {
                if (hdnID.Value.ToString() != "")
                {
                    url = ResolveUrl(string.Format("~/Program/ControlPanel/StandardCode/StandardCodeEntry.aspx?par={0}&id={1}", hdnID.Value, hdnID1.Value));
                    return true;
                }
                return false;
            }
            else
            {
                errMessage = "Cannot Edit This Record";
                return false;
            }
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnIsEditableByUser.Value == "1")
            {
                if (hdnID.Value.ToString() != "")
                {
                    StandardCode entity = BusinessLayer.GetStandardCode(hdnID1.Value);
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateStandardCode(entity);
                    return true;
                }
                return false;
            }
            else
            {
                errMessage = "Cannot Delete This Record";
                return false;
            }
        }
    }
}