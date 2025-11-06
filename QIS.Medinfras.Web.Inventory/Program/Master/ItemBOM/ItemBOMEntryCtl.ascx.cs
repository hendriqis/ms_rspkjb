using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemBOMEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnItemID.Value = param;
            ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemCode.Text = entity.ItemCode;
            txtFormulaItemName.Text = txtItemName.Text = entity.ItemName1;

            BindGridView();

            txtItemBOMCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtSequenceNo.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemQuantity.Attributes.Add("validationgroup", "mpEntryPopup");
            txtBOMQuantity.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        protected string OnGetItemProductFilterExpression()
        {
            string filterExpression = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND GCItemStatus != '{4}'",
                                                            Constant.ItemGroupMaster.SUPPLIES,
                                                            Constant.ItemGroupMaster.DRUGS, 
                                                            Constant.ItemGroupMaster.LOGISTIC,
                                                            Constant.ItemGroupMaster.NUTRITION,
                                                            Constant.ItemStatus.IN_ACTIVE
                                                        );
            filterExpression += string.Format(" AND ItemID NOT IN (SELECT ItemID FROM ItemBOM WHERE ItemID = {0} AND IsDeleted = 0)", hdnItemID.Value );
            filterExpression += string.Format(" AND ItemID NOT IN (SELECT BillOfMaterialID FROM ItemBOM WHERE ItemID = {0} AND IsDeleted = 0)", hdnItemID.Value);

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
            List<vItemBOM> lstEntity = BusinessLayer.GetvItemBOMList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";
            if (param[0] == "save")
            {
                if (hdnIsAdd.Value.ToString() == "0")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemBOM entity)
        {
            entity.BillOfMaterialID = Convert.ToInt32(hdnItemBOMID.Value);
            entity.SequenceNo = Convert.ToInt16(txtSequenceNo.Text);
            entity.ItemQuantity = Convert.ToDecimal(txtItemQuantity.Text);
            entity.BOMQuantity = Convert.ToDecimal(txtBOMQuantity.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ItemBOM entity = new ItemBOM();
                ControlToEntity(entity);
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertItemBOM(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ItemBOM entity = BusinessLayer.GetItemBOM(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemBOM(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ItemBOM entity = BusinessLayer.GetItemBOM(Convert.ToInt32(hdnEntryID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemBOM(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}