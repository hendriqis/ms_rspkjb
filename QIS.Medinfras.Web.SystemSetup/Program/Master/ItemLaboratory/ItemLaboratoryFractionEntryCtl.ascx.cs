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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ItemLaboratoryFractionEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnItemID.Value = param;
            ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;

            BindGridView(1, true, ref PageCount);

            txtFractionCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtCommunicationCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDisplayOrder.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemLaboratoryFractionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vItemLaboratoryFraction> lstEntity = BusinessLayer.GetvItemLaboratoryFractionList(filterExpression, 8, pageIndex, "FractionName1 ASC");
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

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
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

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemLaboratoryFraction entity)
        {
            entity.FractionID = Convert.ToInt32(hdnFractionID.Value);
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ItemLaboratoryFraction entity = new ItemLaboratoryFraction();
                ControlToEntity(entity);
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertItemLaboratoryFraction(entity);
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
                ItemLaboratoryFraction entity = BusinessLayer.GetItemLaboratoryFraction(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemLaboratoryFraction(entity);
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
                ItemLaboratoryFraction entity = BusinessLayer.GetItemLaboratoryFraction(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemLaboratoryFraction(entity);
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