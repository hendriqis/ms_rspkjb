using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemMovementProcessApprovalDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_APPROVAL_COMBINED_ITEM_MOVEMENT;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnMovementID.Value = Page.Request.QueryString["id"];
            vFAItemMovementHd entityItemMovement = BusinessLayer.GetvFAItemMovementHdList(String.Format("MovementID = {0}", Convert.ToInt32(hdnMovementID.Value)))[0];
            EntityToControl(entityItemMovement);
        }

        private void EntityToControl(vFAItemMovementHd entity)
        {
            hdnMovementID.Value = entity.MovementID.ToString();
            txtMovementNo.Text = entity.MovementNo;
            txtMovementDate.Text = entity.MovementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnFromFALocationID.Value = entity.FromFALocationID.ToString();
            txtFromFALocationCode.Text = entity.FromFALocationCode;
            txtFromFALocationName.Text = entity.FromFALocationName;
            hdnToFALocationID.Value = entity.ToFALocationID.ToString();
            txtToFALocationCode.Text = entity.ToFALocationCode;
            txtToFALocationName.Text = entity.ToFALocationName;
            txtMovementType.Text = entity.MovementType;
            txtRemarks.Text = entity.Remarks;
            BindGridView(1, true, ref PageCount);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vFAItemMovementDt entity = e.Row.DataItem as vFAItemMovementDt;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED || lstSelectedMember.Contains(entity.ID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnMovementID.Value != "")
                filterExpression = string.Format("MovementID = {0} AND IsDeleted = 0", hdnMovementID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemMovementDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vFAItemMovementDt> lstEntity = BusinessLayer.GetvFAItemMovementDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FixedAssetName ASC");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemMovementDtDao itemDtDao = new FAItemMovementDtDao(ctx);
            try
            {
                string filterExpressionSetDefaultDt = String.Format("MovementID = {0} AND IsDeleted = 0'", hdnMovementID.Value);
                List<FAItemMovementDt> lstFAItemMovementDtSetDefault = BusinessLayer.GetFAItemMovementDtList(filterExpressionSetDefaultDt);

                string filterExpressionItemMovementDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                List<FAItemMovementDt> lstFAItemMovementDt = BusinessLayer.GetFAItemMovementDtList(filterExpressionItemMovementDt);

                foreach (FAItemMovementDt itemDt in lstFAItemMovementDtSetDefault)
                {
                    if (itemDt.IsDeleted == true && lstFAItemMovementDt.Where(p => p.ID == itemDt.ID).Count() < 1)
                    {
                        itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemDtDao.Update(itemDt);
                    }
                }

                foreach (FAItemMovementDt itemDt in lstFAItemMovementDt)
                {
                    itemDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    itemDtDao.Update(itemDt);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}