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
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ProcessTariffBookList : BasePageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.PROCESS_TARIFF_BOOK;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            BindGridView(1, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Healthcare" };
            fieldListValue = new string[] { "HealthcareName" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND IsDeleted = 0", Constant.TransactionStatus.APPROVED);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTariffBookHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTariffBookHd> lstEntity = BusinessLayer.GetvTariffBookHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTariffBookHd entity = e.Row.DataItem as vTariffBookHd;
                TextBox txtEffectiveDate = e.Row.FindControl("txtEffectiveDate") as TextBox;
                txtEffectiveDate.Text = entity.StartingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            if (type == "processtariffbook")
            {
                bool result = true;
                string filterExpression = String.Format("BookID IN ({0})", hdnParamBookID.Value);

                List<int> listBookID = hdnParamBookID.Value.Trim().Split(',').Select(n => int.Parse(n)).ToList();
                List<DateTime> listEffectiveDate = hdnParamEffectiveDate.Value.Trim().Split(',').Select(n => Helper.GetDatePickerValue(n)).ToList();

                List<vTariffBookDt> lstTariffBookDt = BusinessLayer.GetvTariffBookDtList(filterExpression);
                List<TariffBookDtCost> lstTariffBookDtCost = BusinessLayer.GetTariffBookDtCostList(filterExpression);
                List<TariffBookHd> lstTariffBookHd = BusinessLayer.GetTariffBookHdList(filterExpression);

                IDbContext ctx = DbFactory.Configure(true);
                ItemTariffDao itemTariffDao = new ItemTariffDao(ctx);
                ItemTariffCostDao itemTariffCostDao = new ItemTariffCostDao(ctx);
                TariffBookHdDao tariffBookHdDao = new TariffBookHdDao(ctx);
                try
                {
                    foreach (vTariffBookDt tariffBookDt in lstTariffBookDt)
                    {
                        int index = listBookID.IndexOf(tariffBookDt.BookID);

                        ItemTariff itemTariff = new ItemTariff();
                        itemTariff.HealthcareID = lstTariffBookHd.FirstOrDefault(p => p.BookID == tariffBookDt.BookID).HealthcareID;
                        itemTariff.BookID = tariffBookDt.BookID;
                        itemTariff.ItemID = tariffBookDt.ItemID;
                        itemTariff.ClassID = tariffBookDt.ClassID;
                        itemTariff.GCItemType = tariffBookDt.GCItemType;
                        itemTariff.GCTariffScheme = tariffBookDt.GCTariffScheme;
                        itemTariff.Tariff = tariffBookDt.ApprovedTariff;
                        itemTariff.TariffComp1 = tariffBookDt.ApprovedTariffComp1;
                        itemTariff.TariffComp2 = tariffBookDt.ApprovedTariffComp2;
                        itemTariff.TariffComp3 = tariffBookDt.ApprovedTariffComp3;
                        itemTariff.StartingDate = listEffectiveDate[index];
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
                    foreach (TariffBookHd tariffBookHd in lstTariffBookHd)
                    {
                        tariffBookHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        tariffBookHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        tariffBookHdDao.Update(tariffBookHd);
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
            return true;
        }
    }
}