using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ReorderItemRequest : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        private string[] lstSelectedMember = null;
        private string[] lstQtyItemRequest = null;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.REORDER_ITEM_REQUEST;
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
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_REQUEST);
            filterExpressionLocationTo = string.Format("{0};0;{1},{2};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.PURCHASE_REQUEST, Constant.TransactionCode.ITEM_DISTRIBUTION);

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");
            }


            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
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
            SetControlEntrySetting(rblROPDynamic, new ControlEntrySetting(true, true, false, "false"));

            if (hdnIsUsedProductLine.Value == "1")
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, false));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, true));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (rblROPDynamic.SelectedValue.ToLower().Equals("false"))
            {
                grdView.Columns[9].Visible = false;
            }
            else
            {
                grdView.Columns[9].Visible = true;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetReorderItemRequest entity = e.Row.DataItem as GetReorderItemRequest;
                TextBox txtItemRequest = e.Row.FindControl("txtItemRequest") as TextBox;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");
                Decimal autoQty = (entity.QuantityMAX - entity.QuantityEND - entity.ItemRequestQtyOnOrder);

                if (rblROPDynamic.SelectedValue.ToLower().Equals("true"))
                {
                    if (entity.BackwardDays > 0)
                    {
                        autoQty = (entity.ItemMovementPerDate / entity.BackwardDays * entity.ForwardDays) - entity.QuantityEND - entity.ItemRequestQtyOnOrder;
                    }
                }

                if (autoQty < 0)
                {
                    autoQty = 0;
                }

                txtItemRequest.Text = autoQty.ToString(Constant.FormatString.NUMERIC_2);

                if (lstSelectedMember.Contains(entity.ID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.ID.ToString());
                    chkIsSelected.Checked = true;
                    txtItemRequest.ReadOnly = false;
                    txtItemRequest.Text = lstQtyItemRequest[idx];
                }

                if (autoQty <= 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }
            }
        }

        public void SaveItemRequestHd(IDbContext ctx, ref int itemReqID, ref string retval)
        {
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            ItemRequestHd entityHd = new ItemRequestHd();
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.TransactionDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.TransactionTime = txtItemOrderTime.Text;
            entityHd.Remarks = txtNotes.Text;
            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            entityHd.ItemRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_REQUEST, entityHd.TransactionDate, ctx);
            retval = entityHd.ItemRequestNo;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            itemReqID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int oIsFilterLocationFrom = 0;
            int oLocationIDFrom = 0;
            int oLocationIDTo = 0;
            int oProductLineID = 0;
            int oIsFilterQtyOnHand = 0;

            int oIsROPDynamic = 0;
            int oDisplayMinimum = 0;
            int oDisplayOption = 0;

            if (rbFilterItemLocation.SelectedValue.Equals("from"))
            {
                oIsFilterLocationFrom = 1;
            }
            else
            {
                oIsFilterLocationFrom = 0;
            }
            
            if (hdnLocationIDFrom.Value != "")
            {
                oLocationIDFrom = Convert.ToInt32(hdnLocationIDFrom.Value);
            }

            if (hdnLocationIDTo.Value != "")
            {
                oLocationIDTo = Convert.ToInt32(hdnLocationIDTo.Value);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                if (hdnProductLineID.Value != "")
                {
                    oProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                }
            }

            if (rblROPDynamic.SelectedValue.ToLower().Equals("true"))
            {
                oIsROPDynamic = 1;
            }

            if (rblDisplayMinimum.SelectedValue.ToLower().Equals("true"))
            {
                oDisplayMinimum = 1;
            }
            else
            {
                oDisplayMinimum = 0;
            }

            if (chkIsFilterQtyOnHand.Checked)
            {
                oIsFilterQtyOnHand = 1;
            }

            if (rblDisplayOption.SelectedValue.ToLower().Equals("1"))
            {
                oDisplayOption = 1;
            }

            if (isCountPageCount)
            {
                int rowCount = 0;
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split('|');
            lstQtyItemRequest = hdnItemRequest.Value.Split('|');

            List<GetReorderItemRequest> lstEntity = BusinessLayer.GetReorderItemRequestList(oIsFilterLocationFrom, oLocationIDFrom, oLocationIDTo, oProductLineID, oIsFilterQtyOnHand, oIsROPDynamic, oDisplayMinimum, oDisplayOption);
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] paramID = hdnSelectedMember.Value.Substring(1).Split('|');
            String[] paramItemRequest = hdnItemRequest.Value.Substring(1).Split('|');
            IDbContext ctx = DbFactory.Configure(true);
            int itemRequestID = 0;
            ItemRequestDtDao entityItemRequestDtDao = new ItemRequestDtDao(ctx);
            try
            {
                SaveItemRequestHd(ctx, ref itemRequestID, ref retval);
                for (int ct = 0; ct < paramID.Length; ct++)
                {
                    vItemBalance entityItemBalance = BusinessLayer.GetvItemBalanceList(string.Format("ID = {0}", paramID[ct]), ctx)[0];
                    ItemRequestDt entityItemReqDt = new ItemRequestDt();
                    entityItemReqDt.ItemRequestID = itemRequestID;
                    entityItemReqDt.ItemID = entityItemBalance.ItemID;
                    entityItemReqDt.Quantity = Convert.ToDecimal(paramItemRequest[ct]);
                    entityItemReqDt.GCItemUnit = entityItemBalance.GCItemUnit;
                    entityItemReqDt.GCBaseUnit = entityItemBalance.GCItemUnit;
                    entityItemReqDt.ConversionFactor = Convert.ToDecimal("1.00");
                    entityItemReqDt.GCItemRequestType = entityItemBalance.GCItemRequestType;
                    entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityItemReqDt.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityItemRequestDtDao.Insert(entityItemReqDt);
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