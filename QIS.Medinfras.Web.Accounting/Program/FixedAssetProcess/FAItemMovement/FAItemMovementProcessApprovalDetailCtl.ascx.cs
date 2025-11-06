using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemMovementProcessApprovalDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnMovementID.Value = lstParam[0];

            vFAItemMovementHd entity = BusinessLayer.GetvFAItemMovementHdList(String.Format("MovementID = {0}", Convert.ToInt32(hdnMovementID.Value)))[0];
            txtMovementNo.Text = entity.MovementNo;
            txtMovementDate.Text = entity.MovementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtFromFALocationCode.Text = entity.FromFALocationCode;
            txtFromFALocationName.Text = entity.FromFALocationName;
            txtToFALocationCode.Text = entity.ToFALocationCode;
            txtToFALocationName.Text = entity.ToFALocationName;
            txtMovementType.Text = entity.MovementType;
            txtRemarks.Text = entity.Remarks;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MovementID = {0} AND IsDeleted = 0", hdnMovementID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemMovementDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFAItemMovementDt> lstEntity = BusinessLayer.GetvFAItemMovementDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "MovementNo DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
    }
}