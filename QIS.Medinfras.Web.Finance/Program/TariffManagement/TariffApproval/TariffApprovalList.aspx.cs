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
    public partial class TariffApprovalList : BasePageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TARIFF_APPROVAL;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
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
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND IsDeleted = 0", Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTariffBookHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTariffBookHd> lstEntity = BusinessLayer.GetvTariffBookHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Program/TariffManagement/CreateTariffBook/CreateTariffBookEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/TariffManagement/CreateTariffBook/CreateTariffBookEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                TariffBookHd entity = BusinessLayer.GetTariffBookHd(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTariffBookHd(entity);
                return true;
            }
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            if (type == "approve")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                TariffBookHdDao tariffHdDao = new TariffBookHdDao(ctx);
                TariffBookDtDao tariffDtDao = new TariffBookDtDao(ctx);
                try
                {
                    string filterExpressionTariffBookHd = String.Format("BookID IN ({0})", hdnParam.Value);
                    string filterExpressionTariffBookDt = String.Format("BookID IN ({0}) AND IsApproved = 0", hdnParam.Value);

                    List<TariffBookHd> lstTariffBookHd = BusinessLayer.GetTariffBookHdList(filterExpressionTariffBookHd);
                    foreach (TariffBookHd tariffHd in lstTariffBookHd)
                    {
                        tariffHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        tariffHd.ApprovedDate = DateTime.Now;
                        tariffHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        tariffHdDao.Update(tariffHd);

                        List<TariffBookDt> lstTariffBookDt = BusinessLayer.GetTariffBookDtList(filterExpressionTariffBookDt, ctx);
                        foreach (TariffBookDt tariffDt in lstTariffBookDt)
                        {
                            tariffDt.IsApproved = true;
                            tariffDt.ApprovedTariff = tariffDt.ProposedTariff;
                            tariffDt.ApprovedTariffComp1 = tariffDt.ProposedTariffComp1;
                            tariffDt.ApprovedTariffComp2 = tariffDt.ProposedTariffComp2;
                            tariffDt.ApprovedTariffComp3 = tariffDt.ProposedTariffComp3;
                            tariffDt.ApprovedBaseTariff = tariffDt.BaseTariff;
                            tariffDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            tariffDtDao.Update(tariffDt);
                        }
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
            else
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                TariffBookHdDao entityDao = new TariffBookHdDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int bookID = Convert.ToInt32(param);

                        TariffBookHd entity = entityDao.Get(bookID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.RevisionNo++;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
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
}