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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class ItemLaboratoryFractionEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnItemLabID.Value = param;
            ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemLabID.Value));
            txtItemLabCode.Text = entity.ItemCode;
            txtItemLabName.Text = entity.ItemName1;

            BindGridView(1, true, ref PageCount);
            txtItemLaboratoryCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtFractionCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDisplayOrder.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnItemLabID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemLaboratoryFractionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vItemLaboratoryFraction> lstEntity = BusinessLayer.GetvItemLaboratoryFractionList(filterExpression, 8, pageIndex, "DisplayOrder ASC");
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
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemLaboratoryFraction entity)
        {
            entity.DisplayOrder = Convert.ToByte(txtDisplayOrder.Text);
            entity.IsTestItem = chkIsTestItem.Checked;
            if (chkIsTestItem.Checked)
            {
                entity.DetailItemID = Convert.ToInt32(hdnItemLaboratoryID.Value);
                entity.FractionID = null;
            }
            else
            {
                entity.FractionID = Convert.ToInt32(hdnFractionID.Value);
                entity.DetailItemID = null;
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ItemLaboratoryFraction entity = new ItemLaboratoryFraction();
                ControlToEntity(entity);
                entity.ItemID = Convert.ToInt32(hdnItemLabID.Value);
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