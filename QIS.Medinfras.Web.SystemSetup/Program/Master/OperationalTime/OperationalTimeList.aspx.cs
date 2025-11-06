using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class OperationalTimeList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.OPERATIONAL_TIME_HOUR;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetOperationalTimeRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;
            GetSettingParameter();
            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Operational Time Code", "Operational Time Name" };
            fieldListValue = new string[] { "OperationalTimeCode", "OperationalTimeName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetOperationalTimeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<OperationalTime> lstEntity = BusinessLayer.GetOperationalTimeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Program/Master/OperationalTime/OperationalTimeEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/OperationalTime/OperationalTimeEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                OperationalTime entity = BusinessLayer.GetOperationalTime(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateOperationalTime(entity);
                BridgingToMedinfrasMobileApps(entity, entity.OperationalTimeID, "003");
                return true;
            }
            return false;
        }

        private void BridgingToMedinfrasMobileApps(OperationalTime oOperationalTime, int operationalTimeID, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (oOperationalTime != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnOperationalTimeMasterChanged(oOperationalTime, operationalTimeID, eventType);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}