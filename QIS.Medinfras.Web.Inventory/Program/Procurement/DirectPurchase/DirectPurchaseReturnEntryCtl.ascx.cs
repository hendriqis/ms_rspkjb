using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DirectPurchaseReturnEntryCtl : BaseEntryPopupCtl
    {
        private DirectPurchaseReturn DetailPage
        {
            get { return (DirectPurchaseReturn)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnDirectPurchaseID.Value = temp[0];
            hdnDirectPurchaseReturnID.Value = temp[1];

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnDirectPurchaseID.Value, Constant.TransactionStatus.VOID);
            if (hdnDirectPurchaseReturnID.Value != "0" && hdnDirectPurchaseReturnID.Value != "")
                filterExpression += string.Format(" AND ItemID NOT IN (SELECT ItemID FROM DirectPurchaseReturnDt WHERE DirectPurchaseReturnID = {0})", hdnDirectPurchaseID.Value);
            
            List<vDirectPurchaseDt> lstEntityDt = BusinessLayer.GetvDirectPurchaseDtList(filterExpression);
            lvwView.DataSource = lstEntityDt;
            lvwView.DataBind();
        }

        #endregion

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vDirectPurchaseDt entity = (vDirectPurchaseDt)e.Item.DataItem;
                TextBox txtQtyRetur = (TextBox)e.Item.FindControl("txtQtyRetur");
                txtQtyRetur.Attributes.Add("max", entity.Quantity.ToString());
                Helper.SetControlEntrySetting(txtQtyRetur, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            try
            {
                int PRID = 0;
                string purchaseReturnNo = "";
                DetailPage.SavePurchaseReturnHd(ctx, ref PRID, ref purchaseReturnNo);

                string[] lstSelectedItem = hdnSelectedItem.Value.Split(',');
                string[] lstSelectedQty = hdnSelectedQtyRetur.Value.Split(',');
                List<DirectPurchaseDt> lstDirectPurchaseDt = BusinessLayer.GetDirectPurchaseDtList(string.Format("ID IN ({0})", hdnSelectedItem.Value), ctx);
                for (int i = 0; i < lstSelectedItem.Length; ++i)
                {
                    DirectPurchaseDt directPurchaseDt = lstDirectPurchaseDt.FirstOrDefault(p => p.ID == Convert.ToInt32(lstSelectedItem[i]));
                    DirectPurchaseReturnDt entityDt = new DirectPurchaseReturnDt();
                    entityDt.ItemID = directPurchaseDt.ItemID;
                    entityDt.ConversionFactor = directPurchaseDt.ConversionFactor;
                    entityDt.DiscountPercentage1 = directPurchaseDt.DiscountPercentage;
                    entityDt.DiscountPercentage2 = 0;
                    entityDt.GCBaseUnit = directPurchaseDt.GCBaseUnit;
                    entityDt.GCItemUnit = directPurchaseDt.GCItemUnit;
                    entityDt.Quantity = Convert.ToDecimal(lstSelectedQty[i]);
                    entityDt.UnitPrice = directPurchaseDt.UnitPrice;
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityDt.DirectPurchaseReturnID = PRID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                }

                DirectPurchaseHd entity = entityHdDao.Get(Convert.ToInt32(hdnDirectPurchaseID.Value));
                entity.IsHasPurchaseReturn = true;
                entity.DirectPurchaseReturnID = Convert.ToInt32(PRID);
                entityHdDao.Update(entity);
                retval = purchaseReturnNo;
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