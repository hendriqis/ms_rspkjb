using System;
using System.Collections.Generic;
using System.Data;
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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ReorderItemDistributionList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocationFrom = "";
        protected string filterExpressionLocationTo = "";
        private string[] lstSelectedMember = null;
        private string[] lstQtyItemRequest = null;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.REORDER_ITEM_DISTRIBUTION;
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
            filterExpressionLocationFrom = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_DISTRIBUTION);
            filterExpressionLocationTo = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_REQUEST);
            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.IM_DISTRIBUTION_ALLOWED_WITHOUT_REQUEST
                                                    );
            List<SettingParameterDt> setvarList = BusinessLayer.GetSettingParameterDtList(filterSetVar);
            hdnIsDistributionAllowedWithoutRequest.Value = setvarList.Where(a => a.ParameterCode == Constant.SettingParameter.IM_DISTRIBUTION_ALLOWED_WITHOUT_REQUEST).FirstOrDefault().ParameterValue;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnLocationIDFrom, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtLocationCodeTo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationNameTo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        public void SaveItemDistributionHd(IDbContext ctx, ref int distributionID, ref string retval)
        {
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionHd entityHd = new ItemDistributionHd();
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.DeliveryDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.DeliveryTime = txtItemOrderTime.Text;
            entityHd.DeliveryRemarks = txtNotes.Text;
            entityHd.TransactionDate = DateTime.Now;
            entityHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entityHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_DISTRIBUTION, entityHd.DeliveryDate, ctx);
            retval = entityHd.DistributionNo;
            entityHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            distributionID = BusinessLayer.GetItemDistributionHdMaxID(ctx);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            string isUsingDynamic = "0";
            if (rblROPDynamic.SelectedValue.ToLower().Equals("true")) isUsingDynamic = "1";

            if (hdnLocationIDTo.Value != "" && hdnLocationIDFrom.Value != "")
                filterExpression = string.Format("LocationID = {0} AND IsUsingDynamicROP = {2} AND QuantityEND < QuantityMIN AND IsDeleted = 0 AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = {1} AND QuantityEND > 0 AND IsDeleted = 0)", hdnLocationIDTo.Value, hdnLocationIDFrom.Value, isUsingDynamic);

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemBalance> lstEntity = BusinessLayer.GetvItemBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            string lstItemID = "";
            foreach (vItemBalance entity in lstEntity)
            {
                if (lstItemID != "")
                    lstItemID += ",";
                lstItemID += entity.ItemID.ToString();
            }

            filterExpression = "1 = 0";
            if (hdnLocationIDFrom.Value != "" && lstItemID != "")
                filterExpression = string.Format("LocationID = {0} AND ItemID IN ({1}) AND IsDeleted = 0", hdnLocationIDFrom.Value, lstItemID);

            //if (rblROPDynamic.SelectedValue.ToLower().Equals("false"))
            //    filterExpression += " AND (QuantityMIN > 0 AND QuantityMax > 0)";

            lstSelectedMember = hdnSelectedMember.Value.Split('|');
            lstQtyItemRequest = hdnItemDistribution.Value.Split('|');

            lstItemBalanceFromLocation = BusinessLayer.GetItemBalanceList(filterExpression);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        List<ItemBalance> lstItemBalanceFromLocation = null;
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (rblROPDynamic.SelectedValue.ToLower().Equals("false"))
            {
                grdView.Columns[8].Visible = false;
            }
            else grdView.Columns[8].Visible = true;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalance entity = e.Row.DataItem as vItemBalance;
                HtmlInputText txtItemRequest = e.Row.FindControl("txtItemRequest") as HtmlInputText;
                TextBox txtItemRequesta = e.Row.FindControl("txtItemRequest") as TextBox;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");

                ItemBalance itemBalanceFromLocation = lstItemBalanceFromLocation.FirstOrDefault(p => p.ItemID == entity.ItemID);
                if (itemBalanceFromLocation != null)
                {
                    decimal qty = (entity.QuantityMAX - entity.QuantityEND);
                    if (rblROPDynamic.SelectedValue.ToLower().Equals("true"))
                    {
                        if (entity.BackwardDays > 0) qty = (entity.ItemMovementPerDate / entity.BackwardDays * entity.ForwardDays) - entity.QuantityEND;
                    }
                    if (qty > itemBalanceFromLocation.QuantityEND)
                        qty = itemBalanceFromLocation.QuantityEND;

                    txtItemRequest.Attributes.Add("max", itemBalanceFromLocation.QuantityEND.ToString());
                    decimal distQty = qty - entity.ItemDistributionQtyOnOrder;
                    txtItemRequest.Value = (qty - entity.ItemDistributionQtyOnOrder).ToString("N2");

                    if (lstSelectedMember.Contains(entity.ID.ToString()))
                    {
                        int idx = Array.IndexOf(lstSelectedMember, entity.ID.ToString());
                        chkIsSelected.Checked = true;
                        //txtItemRequesta.ReadOnly = false;
                        //txtItemRequesta.Text = lstQtyItemRequest[idx];
                    }

                    HtmlGenericControl divFromLocationQty = (HtmlGenericControl)e.Row.FindControl("divFromLocationQty");
                    divFromLocationQty.InnerHtml = itemBalanceFromLocation.QuantityEND.ToString();

                    if (entity.ItemDistributionQtyOnOrder > 0)
                        e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    else
                        e.Row.Cells[9].ForeColor = System.Drawing.Color.Black;
                }
            }
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] paramID = hdnSelectedMember.Value.Substring(1).Split('|');
            String[] paramItemDistribution = hdnItemDistribution.Value.Substring(1).Split('|');
            IDbContext ctx = DbFactory.Configure(true);
            int distributionID = 0;
            ItemDistributionDtDao entityItemDistributionDtDao = new ItemDistributionDtDao(ctx);
            try
            {
                SaveItemDistributionHd(ctx, ref distributionID, ref retval);
                for (int ct = 0; ct < paramID.Length; ct++)
                {
                    vItemBalance entityItemBalance = BusinessLayer.GetvItemBalanceList(string.Format("ID = {0}", paramID[ct]), ctx)[0];
                    ItemDistributionDt entityItemDistributionDt = new ItemDistributionDt();
                    entityItemDistributionDt.DistributionID = distributionID;
                    entityItemDistributionDt.ItemID = entityItemBalance.ItemID;
                    entityItemDistributionDt.Quantity = Convert.ToDecimal(paramItemDistribution[ct]);
                    entityItemDistributionDt.GCItemUnit = entityItemBalance.GCItemUnit;
                    entityItemDistributionDt.GCBaseUnit = entityItemBalance.GCItemUnit;
                    entityItemDistributionDt.ConversionFactor = Convert.ToDecimal("1.00");
                    entityItemDistributionDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                    entityItemDistributionDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityItemDistributionDtDao.Insert(entityItemDistributionDt);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
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