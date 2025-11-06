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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ProposeTariffBookList : BasePageList
    {
        protected int PageCount = 1;
        List<vUser> lstUser = null;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.PROPOSE_TARIFF_BOOK;
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
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND IsDeleted = 0", Constant.TransactionStatus.OPEN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTariffBookHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            lstUser = BusinessLayer.GetvUserList("IsDeleted = 0 ORDER BY UserName");

            List<vTariffBookHd> lstEntity = BusinessLayer.GetvTariffBookHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTariffBookHd entity = e.Row.DataItem as vTariffBookHd;
                ASPxComboBox cboApprovedBy = e.Row.FindControl("cboApprovedBy") as ASPxComboBox;
                Methods.SetComboBoxField<vUser>(cboApprovedBy, lstUser, "UserName", "UserID");
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
                IDbContext ctx = DbFactory.Configure(true);
                TariffBookHdDao entityDao = new TariffBookHdDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split('|');
                    foreach (string param in listParam)
                    {
                        string[] data = param.Split(';');
                        int bookID = Convert.ToInt32(data[0]);
                        string approvedBy = data[1];

                        TariffBookHd entity = entityDao.Get(bookID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ApprovedBy = Convert.ToInt32(approvedBy);
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
            return true;
        }
    }
}