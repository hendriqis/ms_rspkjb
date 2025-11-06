using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PhysicalCountUncheckedDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PHYSICAL_COUNT_UNCHECKED;
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

            hdnReceiveID.Value = Page.Request.QueryString["id"];
            vPurchaseReceiveHd1 entityItemReceive = BusinessLayer.GetvPurchaseReceiveHd1List(String.Format("PurchaseReceiveID = '{0}'", Convert.ToInt32(hdnReceiveID.Value)))[0];
            EntityToControl(entityItemReceive);

            GetSettingParameterDt();
        }

        private void GetSettingParameterDt()
        {
            string filterExpression = String.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_REVISI_PEMESANAN_KETIKA_KONFIRMASI_PENERIMAAN);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            if (lstParam.Count > 0)
            {
                hdnIsAutoUpdatePO.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_REVISI_PEMESANAN_KETIKA_KONFIRMASI_PENERIMAAN).FirstOrDefault().ParameterValue;
            }
        }

        private void EntityToControl(vPurchaseReceiveHd1 entity)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                //watermarkText = entity.TransactionStatusWatermark;
            }
            hdnReceiveID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReceiveDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseReceiveTime.Text = entity.ReceivedTime;
            txtDateReferrence.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.SupplierID.ToString();
            txtSupplierCode.Text = entity.SupplierCode;
            txtSupplierName.Text = entity.SupplierName;
            txtFacturNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtTerm.Text = entity.TermName;
            txtCurrency.Text = entity.CurrencyCode;
            txtKurs.Text = entity.CurrencyRate.ToString();

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnReceiveID.Value != "")
                filterExpression = string.Format("PurchaseReceiveID = {0} AND IsPhysicalCountChecked = 1", hdnReceiveID.Value);


            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseReceiveDt entity = e.Item.DataItem as vPurchaseReceiveDt;

                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                HtmlGenericControl lblQuantity = (HtmlGenericControl)e.Item.FindControl("lblPurchaseReceiveQty");
                HtmlGenericControl lblUnitPrice = (HtmlGenericControl)e.Item.FindControl("lblPurchaseReceivePrice");
                HtmlGenericControl lblDisc1 = (HtmlGenericControl)e.Item.FindControl("lblDiscountAmount1Pct");
                HtmlGenericControl lblDisc2 = (HtmlGenericControl)e.Item.FindControl("lblDiscountAmount2Pct");

                if (entity.UnitPrice != entity.OrderUnitPrice)
                {
                    if (lblUnitPrice != null)
                        lblUnitPrice.Style.Add("color", "Red");
                }

                if (entity.DiscountPercentage1 != entity.OrderDisc1)
                {
                    if (lblDisc1 != null)
                        lblDisc1.Style.Add("color", "Red");
                }

                if (entity.DiscountPercentage2 != entity.OrderDisc2)
                {
                    if (lblDisc2 != null)
                        lblDisc2.Style.Add("color", "Red");
                }

                if (entity.Quantity != entity.OrderQuantity)
                {
                    if (lblQuantity != null)
                        lblQuantity.Style.Add("color", "Red");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveDtDao itemDtDao = new PurchaseReceiveDtDao(ctx);
            try
            {
                string filterExpression = String.Format("PurchaseReceiveID = {0} AND IsPhysicalCountChecked = 1", hdnReceiveID.Value);
                List<PurchaseReceiveDt> lstPurchaseReceive = BusinessLayer.GetPurchaseReceiveDtList(filterExpression, ctx);

                List<PurchaseReceiveDt> lstPurchaseReceiveDt = null;
                if (hdnSelectedMember.Value != "")
                {
                    string filterExpressionPurchaseReceiveDt = String.Format("ID IN ({0})", hdnSelectedMember.Value.Substring(1));
                    lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(filterExpressionPurchaseReceiveDt, ctx);
                }

                foreach (PurchaseReceiveDt itemDt in lstPurchaseReceiveDt)
                {
                    itemDt.IsPhysicalCountChecked = false;
                    itemDt.PhysicalCountUncheckedBy = AppSession.UserLogin.UserID;
                    itemDt.PhysicalCountUncheckedDate = DateTime.Now;
                    itemDtDao.Update(itemDt);
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