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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SupplierFinanceItemEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnBusinessPartnerID.Value = param;

            BusinessPartners hsu = BusinessLayer.GetBusinessPartners(Convert.ToInt32(param));
            txtSupplierName.Text = string.Format("{0} - {1}", hsu.BusinessPartnerCode, hsu.BusinessPartnerName);

            BindGridView(1, true, ref PageCount);

            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLeadTime.Attributes.Add("validationgroup", "mpEntryPopup");

            hdnDateTodayInDatePicker.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void cboPurchaseUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(hdnItemID.Value))
            {
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
                Methods.SetComboBoxField<StandardCode>(cboPurchaseUnit, lst, "StandardCodeName", "StandardCodeID");
                cboPurchaseUnit.SelectedIndex = 0;
            }
            else cboPurchaseUnit.Items.Clear();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("BusinessPartnerID = {0} AND IsDeleted = 0", hdnBusinessPartnerID.Value);

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSupplierItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vSupplierItem> lstEntity = BusinessLayer.GetvSupplierItemList(filterExpression, 8, pageIndex, "ItemName1 ASC");
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

        private void ControlToEntity(SupplierItem entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.SupplierItemCode = txtSupplierItemCode.Text;
            entity.SupplierItemName = txtSupplierItemName.Text;
            entity.LeadTime = Convert.ToByte(txtLeadTime.Text);
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entity.GCPurchaseUnit = cboPurchaseUnit.Value.ToString();
            entity.Price = Convert.ToDecimal(txtPrice.Text);
            entity.PurchaseUnitPrice = Convert.ToDecimal(txtPurchaseUnitPrice.Text);
            entity.ConversionFactor = Convert.ToDecimal(Request.Form[txtConversionFactor.UniqueID]);
            entity.DiscountPercentage = Convert.ToDecimal(txtDiscountPercentage1.Text);
            entity.DiscountPercentage2 = Convert.ToDecimal(txtDiscountPercentage2.Text);
            entity.Remarks = txtRemarks.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierItemDao entitySupDao = new SupplierItemDao(ctx);
            try
            {
                SupplierItem entity = new SupplierItem();
                ControlToEntity(entity);
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;

                string filter = string.Format("BusinessPartnerID = {0} AND ItemID = {1} AND GCPurchaseUnit = '{2}' AND IsDeleted = 0",
                                                entity.BusinessPartnerID, entity.ItemID, entity.GCPurchaseUnit);
                List<SupplierItem> lst = BusinessLayer.GetSupplierItemList(filter);
                if (lst.Count() == 0)
                {
                    entitySupDao.Insert(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Supplier Item dengan satuan ini sudah tersedia.";
                    result = false;
                    ctx.RollBackTransaction();
                }
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierItemDao entitySupDao = new SupplierItemDao(ctx);
            try
            {
                SupplierItem entity = entitySupDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                string filter = string.Format("BusinessPartnerID = {0} AND ItemID = {1} AND GCPurchaseUnit = '{2}' AND IsDeleted = 0 AND ID != {3}",
                                                entity.BusinessPartnerID, entity.ItemID, entity.GCPurchaseUnit, entity.ID);
                List<SupplierItem> lst = BusinessLayer.GetSupplierItemList(filter);
                if (lst.Count() == 0)
                {
                    entitySupDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Supplier Item dengan satuan ini sudah tersedia.";
                    result = false;
                    ctx.RollBackTransaction();
                }
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                SupplierItem entity = BusinessLayer.GetSupplierItem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSupplierItem(entity);
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