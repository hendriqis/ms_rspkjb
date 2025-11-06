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
    public partial class InformationItemSupplierCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string queryString)
        {
            string[] param = queryString.Split('|');
            hdnParam.Value = param[0];

            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnParam.Value));
            txtItem.Text = string.Format("{0} - {1}", im.ItemCode, im.ItemName1);

            BindGridView(1, true, ref PageCount);
        }
        
        protected void cbpViewItemSupplierCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else if (param[0] == "save")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
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
                    result = false;
                    errMessage = "Supplier Item dengan satuan ini sudah tersedia.";
                    ctx.RollBackTransaction();
                }
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
            string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0", hdnParam.Value);
            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSupplierItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vSupplierItem> lstEntity = BusinessLayer.GetvSupplierItemList(filterExpression, 8, pageIndex, "BusinessPartnerID ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}