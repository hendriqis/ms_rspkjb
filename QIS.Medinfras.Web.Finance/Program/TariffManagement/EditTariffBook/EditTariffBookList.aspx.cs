using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class EditTariffBookList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.EDIT_TARIFF_BOOK;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvTariffBookHdRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Healthcare", "Status" };
            fieldListValue = new string[] { "HealthcareName", "TransactionStatus" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND IsDeleted = 0", Constant.TransactionStatus.CLOSED);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTariffBookHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTariffBookHd> lstEntity = BusinessLayer.GetvTariffBookHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BookID DESC");
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

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            bool result = true;
            //string filterExpression = string.Format("BookID = {0} AND (ItemID NOT IN (SELECT ItemID FROM ItemTariff WHERE BookID = {0}) OR IsRevised = 1)", hdnID.Value);
            string filterExpression = string.Format("BookID = {0} AND ItemID NOT IN (SELECT ItemID FROM ItemTariff WHERE BookID = {0})", hdnID.Value);
            List<vTariffBookDt> lstTariffBookDt = BusinessLayer.GetvTariffBookDtList(filterExpression);
            List<TariffBookDtCost> lstTariffBookDtCost = BusinessLayer.GetTariffBookDtCostList(filterExpression);

            IDbContext ctx = DbFactory.Configure(true);
            TariffBookHdDao tariffBookHdDao = new TariffBookHdDao(ctx);
            ItemTariffDao itemTariffDao = new ItemTariffDao(ctx);
            ItemTariffCostDao itemTariffCostDao = new ItemTariffCostDao(ctx);
            try
            {
                TariffBookHd tariffBookHd = tariffBookHdDao.Get(Convert.ToInt32(hdnID.Value));

                foreach (vTariffBookDt tariffBookDt in lstTariffBookDt)
                {
                    ItemTariff itemTariff = new ItemTariff();
                    itemTariff.HealthcareID = tariffBookHd.HealthcareID;
                    itemTariff.BookID = tariffBookDt.BookID;
                    itemTariff.ItemID = tariffBookDt.ItemID;
                    itemTariff.ClassID = tariffBookDt.ClassID;
                    itemTariff.GCItemType = tariffBookDt.GCItemType;
                    itemTariff.GCTariffScheme = tariffBookDt.GCTariffScheme;
                    itemTariff.Tariff = tariffBookDt.ApprovedTariff;
                    itemTariff.TariffComp1 = tariffBookDt.ApprovedTariffComp1;
                    itemTariff.TariffComp2 = tariffBookDt.ApprovedTariffComp2;
                    itemTariff.TariffComp3 = tariffBookDt.ApprovedTariffComp3;
                    itemTariff.StartingDate = tariffBookHd.StartingDate;
                    itemTariff.CreatedBy = AppSession.UserLogin.UserID;

                    itemTariffDao.Insert(itemTariff);

                    itemTariff.ID = BusinessLayer.GetItemTariffMaxID(ctx);

                    TariffBookDtCost tariffBookDtCost = lstTariffBookDtCost.FirstOrDefault(p => p.ClassID == tariffBookDt.ClassID && p.ItemID == tariffBookDt.ItemID);
                    ItemTariffCost itemTariffCost = new ItemTariffCost();
                    itemTariffCost.ID = itemTariff.ID;
                    itemTariffCost.ItemID = itemTariff.ItemID;
                    itemTariffCost.ClassID = itemTariff.ClassID;
                    itemTariffCost.CurrentBurden = tariffBookDtCost.CurrentBurden;
                    itemTariffCost.CurrentLabor = tariffBookDtCost.CurrentLabor;
                    itemTariffCost.CurrentMaterial = tariffBookDtCost.CurrentMaterial;
                    itemTariffCost.CurrentOverhead = tariffBookDtCost.CurrentOverhead;
                    itemTariffCost.CurrentSubContract = tariffBookDtCost.CurrentSubContract;

                    itemTariffCost.PreviousBurden = tariffBookDtCost.PreviousBurden;
                    itemTariffCost.PreviousLabor = tariffBookDtCost.PreviousLabor;
                    itemTariffCost.PreviousMaterial = tariffBookDtCost.PreviousMaterial;
                    itemTariffCost.PreviousOverhead = tariffBookDtCost.PreviousOverhead;
                    itemTariffCost.PreviousSubContract = tariffBookDtCost.PreviousSubContract;
                    itemTariffCost.LastUpdatedBy = AppSession.UserLogin.UserID;

                    itemTariffCostDao.Insert(itemTariffCost);
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