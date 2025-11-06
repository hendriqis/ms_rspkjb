using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class ImagingResultTemporaryPhysicianList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.IMAGING_RESULT_TEMPORARY_PHYSICIAN;
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            txtEndDateContract.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (e.Parameter != null && e.Parameter != "")
            {
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected string GetFilterExpression()
        {
            string filterExpression = "IsTemporaryPhysician = 1";

            if (!chkEndDate.Checked)
            {
                filterExpression += string.Format(" AND ResultDate = '{0}'", Helper.GetDatePickerValue(txtEndDateContract.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvImagingResultDt1CustomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 15);
            }
            List<vImagingResultDt1> lstEntity = BusinessLayer.GetvImagingResultDt1List(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "TransactionNo");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}