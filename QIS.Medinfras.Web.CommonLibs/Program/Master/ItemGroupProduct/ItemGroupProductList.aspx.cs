using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemGroupProductList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String TypeItem = Request.QueryString["id"];
            switch (TypeItem)
            {
                case Constant.ItemGroupMaster.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.ITEM_GROUP;
                case Constant.ItemGroupMaster.SERVICE: return Constant.MenuCode.Finance.ITEM_GROUP;
                case Constant.ItemGroupMaster.RADIOLOGY: return Constant.MenuCode.Imaging.ITEM_GROUP;
                case Constant.ItemGroupMaster.LABORATORY: return Constant.MenuCode.Laboratory.ITEM_GROUP;
                case Constant.ItemGroupMaster.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.ITEM_GROUP;
                case Constant.ItemGroupMaster.NUTRITION: return Constant.MenuCode.Nutrition.ITEM_GROUP;
                default: return Constant.MenuCode.Inventory.ITEM_GROUP;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            String TypeItem = Request.QueryString["id"];

            hdnTypeItem.Value = TypeItem;

            switch (TypeItem)
            {
                //case Constant.ItemGroup.DIAGNOSTIC: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.DIAGNOSTIC); break;
                //case Constant.ItemGroup.FINANCE: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.SERVICE, Constant.ItemGroupMaster.MEDICAL_CHECKUP); break;
                //case Constant.ItemGroup.IMAGING: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.RADIOLOGY); break;
                //case Constant.ItemGroup.LABORATORY: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.LABORATORY); break;
                //case Constant.ItemGroup.MCU: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.MEDICAL_CHECKUP); break;
                //case Constant.ItemGroup.NUTRIENT: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.NUTRITION); break;
                //default: hdnQueryItem.Value = String.Format("GCItemType IN ('{0}','{1}','{2}')", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC); break;

                case Constant.ItemGroupMaster.DIAGNOSTIC: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.DIAGNOSTIC); break;
                case Constant.ItemGroupMaster.SERVICE: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.SERVICE); break;
                case Constant.ItemGroupMaster.RADIOLOGY: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.RADIOLOGY); break;
                case Constant.ItemGroupMaster.LABORATORY: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.LABORATORY); break;
                case Constant.ItemGroupMaster.MEDICAL_CHECKUP: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.MEDICAL_CHECKUP); break;
                case Constant.ItemGroupMaster.NUTRITION: hdnQueryItem.Value = String.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.NUTRITION); break;
                default: hdnQueryItem.Value = String.Format("GCItemType IN ('{0}','{1}','{2}')", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC); break;
            }

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvItemGroupMasterRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Grup", "Kode Grup" };
            fieldListValue = new string[] { "ItemGroupName1", "ItemGroupCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("{0} AND IsDeleted = 0", hdnQueryItem.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemGroupMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemGroupMaster> lstEntity = BusinessLayer.GetvItemGroupMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemGroupProduct/ItemGroupProductEntry.aspx?id={0}", hdnTypeItem.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemGroupProduct/ItemGroupProductEntry.aspx?id={0}|{1}", hdnTypeItem.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemGroupMaster(entity);
                return true;
            }
            return false;
        }
    }
}