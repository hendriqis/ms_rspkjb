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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class FADepreciationEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override void InitializeDataControl(string param)
        {
            hdnFixedAssetID.Value = param;

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FADEPRECIATION_FROM_APPROVE_FAACCEPTANCE);
            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnIsProcessDepreciationFromFAAcceptance.Value = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.FADEPRECIATION_FROM_APPROVE_FAACCEPTANCE).FirstOrDefault().ParameterValue;

            if (hdnIsProcessDepreciationFromFAAcceptance.Value == "1")
            {
                trButtonProcess.Style.Add("display", "none");
            }
            else
            {
                trButtonProcess.Style.Remove("display");
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        protected void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        { 
            String filterExpression = String.Format("FixedAssetID = {0}",hdnFixedAssetID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFADepreciationProcessRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFADepreciationProcess> entity = BusinessLayer.GetvFADepreciationProcessList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdPopupView.DataSource = entity;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e) 
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                if (e.Parameter == "process")
                {
                    ProsesFADepreciation(ref result);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool ProsesFADepreciation(ref String errMessage) 
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure();
            FADepreciationDao entityDao = new FADepreciationDao(ctx);
            try
            {
                BusinessLayer.GenerateFADepreciation(Convert.ToInt32(hdnFixedAssetID.Value), AppSession.UserLogin.UserID, ctx);
                ctx.CommitTransaction();
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

    }
}