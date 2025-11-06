using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcedurePanelEntryQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnProcedureID.Value = param;

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
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

        private string GetFilterExpression()
        {
            string filterExpression = "";

            filterExpression += string.Format("GCItemType = '{0}' AND GCItemStatus != '{1}' AND IsDeleted = 0", Constant.ItemType.PENUNJANG_MEDIS, Constant.ItemStatus.IN_ACTIVE);
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ItemMaster entity = e.Row.DataItem as ItemMaster;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetItemMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<ItemMaster> lstEntity = BusinessLayer.GetItemMasterList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedurePanelDtDao procedurePanelDtDao = new ProcedurePanelDtDao(ctx);
            try
            {
                int procedurePanelID = Convert.ToInt32(hdnProcedureID.Value);
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberPercentage = hdnSelectedMemberPercentage.Value.Split(',');
                string[] lstSelectedMemberOrder = hdnSelectedMemberOrder.Value.Split(',');
                string[] lstSelectedMemberControl = hdnSelectedMemberControl.Value.Split(',');

                List<vItemService> lstvItemService = BusinessLayer.GetvItemServiceList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                int ct = 0;
                foreach (String itemID in lstSelectedMember)
                {
                    ProcedurePanelDt entity = new ProcedurePanelDt();
                    entity.ProcedurePanelID = procedurePanelID;
                    entity.ItemID = Convert.ToInt32(itemID);
                    entity.FormulaPercentage = Convert.ToDecimal(lstSelectedMemberPercentage[ct]);
                    entity.DisplayOrder = Convert.ToInt16(lstSelectedMemberOrder[ct]);
                    entity.IsControlItem = Convert.ToBoolean(lstSelectedMemberControl[ct]);
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    procedurePanelDtDao.Insert(entity);
                }
                ct++;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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