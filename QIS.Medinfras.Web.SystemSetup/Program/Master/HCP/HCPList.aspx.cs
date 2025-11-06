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
    public partial class HCPList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.HEALTHCARE_PROFESSIONAL;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvParamedicMasterRowIndex(filterExpression, keyValue, "ParamedicName ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;
            GetSettingParameter();
            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "ParamedicName", "ParamedicCode", "ParamedicMasterType", "SpecialtyName" };
            fieldListValue = new string[] { "ParamedicName", "ParamedicCode", "ParamedicMasterType", "SpecialtyName" };
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
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ParamedicName ASC");
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
            url = ResolveUrl("~/Program/Master/HCP/HCPEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/HCP/HCPEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                ParamedicMaster entity = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateParamedicMaster(entity);
                BridgingToMedinfrasMobileApps(entity, entity.ParamedicID, "003");
                return true;
            }
            return false;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //List<vServiceUnitParamedic> lstHSU = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND IsDeleted = 0 AND DepartmentID IN ('{1}','{2}')", hdnExpandID.Value, Constant.Facility.EMERGENCY, Constant.Facility.OUTPATIENT));
            List<vServiceUnitParamedic> lstHSU = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND IsDeleted = 0", hdnExpandID.Value),int.MaxValue,1,"ServiceUnitName");
            grdDetail.DataSource = lstHSU;
            grdDetail.DataBind();
        }

        private void BridgingToMedinfrasMobileApps(ParamedicMaster oParamedicMaster, int paramedicID, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (oParamedicMaster != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnParamedicMasterChanged(oParamedicMaster, paramedicID, eventType);
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